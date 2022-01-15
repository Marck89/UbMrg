using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Struttura contenente i dati relativi ai dispositivi di bordo. Questi dati sono quelli "reali",
     mentre quelli ricevuti dal CD (che possono essere diversi da quelli reali) sono nella struttura
     UB_DatiEsterni_st */

  internal class UB_DatiBase
  {
    /* Questi 4 sono nel file Config/fileUBid		  */
    internal SBME_Operator m_Operatore;		  /* ID ATM/FS/FNM	*/
    internal SBME_DeviceClass m_Classe;     /* UB/CU/					*/

    internal uint m_IdVeicolo;		/* Univoco veicolo					*/
    internal byte m_Vettore;	    /* Vettore che eroga il servizio */

    internal int m_nCNV;		    	/* Numero di CNV gestite dall'UB */

    internal ushort m_ProgrAtt;	  /* Progressivo del file di attivita' UB	*/

    internal UB_DatiSync m_UB_DatiSync;

    /* Questa struttura ricevuta dal CD contiene le versioni SW dei dispositivi
       a bordo raggruppate per tipo/sottotipo */
    internal XConfig m_VerTipo;			/* Versione SW per ogni (tipo,sottotipo)		*/
    internal CdxMsgSendStatusT m_StatoDisp;			/* Stato completo dispositivo da inviare a CD	*/

    internal CdxMsgDeviceDataT m_DescrTipoMGR;			/* Tipo/Sottotipo/SW major/SW minor CU			*/

    internal Geografia m_InfoLinea;				/* Informazioni in uso alle CNV					*/

    internal short m_AggiornaOrario;				/* Settato dopo uno scambio dati col CDD		*/

    internal int m_UltimoContattoCDD;		/* Istante (in centesimi dall'avvio) dell'ultimo scambio dati				*/
    internal int m_TempoSpegnimento;	  	/* Istante (in centesimi dall'avvio) nel quale spegnersi definitivamente	*/

    internal UB_DatiBase()
    {
      m_UB_DatiSync = new UB_DatiSync();
      m_VerTipo = new XConfig();
      m_StatoDisp = new CdxMsgSendStatusT();
      m_DescrTipoMGR = new CdxMsgDeviceDataT();
      m_InfoLinea = new Geografia();
      Clear();
    }

    internal void Clear()
    {
      m_Operatore = SBME_Operator.SBME_OP_UNDEFINED;
      m_Classe =  SBME_DeviceClass.SBME_CLASS_NULL;
      m_IdVeicolo = 0;
      m_Vettore = 0;
      m_nCNV = 0;
      m_ProgrAtt = 0;
      m_UB_DatiSync.Clear();

      m_VerTipo.Clear();
      m_StatoDisp.Clear();
      m_DescrTipoMGR.Clear();

      m_InfoLinea.Clear();

      m_AggiornaOrario = 0;
      m_UltimoContattoCDD = 0;
      m_TempoSpegnimento = 0;
    }

    internal void Init(ushort SerialNum)
    {
      /* Informazioni sul tipo UB manager */

      m_DescrTipoMGR.Init(SerialNum);

      m_StatoDisp.Init(SerialNum);


      /* Questo serve per non scaricare dal CDX il software 
       * per il manager che sto gia' utilizzando     */
      m_VerTipo.Init();

      m_UltimoContattoCDD = 0;

      m_TempoSpegnimento = 0;	/* Occhio perche` questo segna l'ora in cui l'UB si spegnera` definitivamente */
    }

    /* Questa macro calcola il numero sociale del veicolo dato il suo ID */
    internal static uint NUMERO_SOCIALE(uint IdVeicolo)
    {
      const int tag = 8192;
      if (IdVeicolo > tag) return IdVeicolo - tag;
      return IdVeicolo;
    }

    private bool LeggiServizioFromFile()
    {
      String funcName = "LeggiServizioFromFile()";
      String msgLog = "";

      bool rst = false;

      String datasStr = "";
      StreamReader sr = null;
      try
      {
        sr = new StreamReader(DirsNames.NOME_FILE_IDENTIFICAZIONE);

        /* Leggo il file in una volta sola */
        datasStr = sr.ReadToEnd();
        sr.Close();
      }
      catch (Exception)
      {
        msgLog = funcName
               + " reason=\"File con informazioni di identificazione veicolo non trovato\""
               + ", Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
               + ", errno=" + Porting.GetErrNoStr()
               + ", OldIdVeicolo=" + m_IdVeicolo.ToString()
               + ", OldNumeroSociale=" + NUMERO_SOCIALE(m_IdVeicolo);
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
      }
      if (sr != null) sr.Close();

      if (datasStr != null)
      {
        string[] items = datasStr.Split('\n');
        int value;
        for (int i = 0; i < items.Length; i++ )
        {
          int.TryParse(items[i].Trim(), out value);
          if (i == 0) m_Operatore = (SBME_Operator)value;
          else if (i == 1) m_Classe    =  (SBME_DeviceClass)value;
          else if (i == 2) m_IdVeicolo = (uint)value;
          else if (i == 3) m_Vettore   = (byte)value;
        }

        msgLog = funcName
               + " reason=\"Informazioni di identificazione veicolo lette con successo da file\""
               + ", Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
               + ", Operatore=" + ((int)m_Operatore).ToString()
               + ", Classe=" + ((int)m_Classe).ToString()
               + ", IdVeicolo=" + m_IdVeicolo.ToString()
               + ", NumeroSociale=" + NUMERO_SOCIALE(m_IdVeicolo);
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        rst = true;
      }
      return rst;
    }

    private void WaitIdentification(UbMgr _Ubmgr)
    {
      String funcName = "WaitIdentification()";
      String msgLog = "";

      /* Non esco finche` non ho l'Id veicolo oppure mi spengono */

      RccDati rccDati = new RccDati();
      rccDati.m_IdVeicolo = 0;

      int count = 0;
      while (rccDati.m_IdVeicolo == 0) 
      {
        /* Leggo i dati base dall'RCC */
        RccApi.GetStato(rccDati);

        /* Test per lo spegnimento */
        bool rst = IntAlim.UB_intAlim();
        if ( rst == true)
        {
          /* Accendo tutti i led */
          LedSrv.Set(LedState.LED_SHUTDOWN);

          TimeUtils.SospendiInCentesimi(50);

          _Ubmgr.m_OmiMgr.OMI_Finalize();
          _Ubmgr.m_RccMgr.RccSvr_End();

          TimeUtils.SospendiInCentesimi(50);

          /* Termino diagnostica su RCC */
          Diag.Diag_End();

          LedSrv.End();

          TimeUtils.SospendiInCentesimi(50);

          /* Log del tempo di esecuzione */
          TimeEsecuzioneUCB.TimerEsecuzioneUCB(CmdTimer.STOP_TIME);

          if (FileUtils.UCB_FileExists(DirsNames.DIR_PID + "UB.pid"))
          {
            FileUtils.unlink(DirsNames.DIR_PID + "UB.pid");
          }

          msgLog = funcName +  "Spegnimento della macchina";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);

          /* Spengo CNV */
          _Ubmgr.UCB_SpegniCNV();

          _Ubmgr.UB_Terminate(ExitCode.UCB_EC_SUCCESSFUL);
        }

        TimeUtils.SospendiInCentesimi(100); /* Aspetto un secondo alla volta, tanto non posso fare altro */

        if ((count % 300) == 0)
        {
          /* Faccio un log ogni 5 minuti */
          msgLog = funcName + " reason=\"Veicolo non identificato"
                +", attendo le informazioni di identificazione da QB\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        }
        count++;
      }

      /* Metto i dati base nella struttura principale */
      m_Operatore  = SBME_Operator.SBME_OP_ATM;
      m_Classe     = SBME_DeviceClass.SBME_CLASS_UB;
      m_IdVeicolo  = rccDati.m_IdVeicolo;
      m_Vettore    = 0;

      msgLog = funcName + " reason=\"Veicolo identificato tramite QB, salvo i dati nel file\""
             + ", Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
             + ", Operatore=" + ((int)m_Operatore).ToString()
             + ", Classe=" + ((int)m_Classe).ToString()
             + ", IdVeicolo=" + m_IdVeicolo.ToString()
             + ", Vettore=" + m_Vettore.ToString();
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
    }


    /* Leggo il file di configurazione di base */
    internal bool UB_LeggiServizio(UbMgr _Ubmgr)
    {
      String funcName = "UB_LeggiServizio()";
      String msgLog = "";

      msgLog = funcName + " STARTS";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      bool rst = LeggiServizioFromFile();

      /** Se il file non esisteva oppure l'idVeicolo e' zero, aspetto il dato dall'RCC 
      ** e lo scrivo in un nuovo file di servizio   */

      if ( rst == false || m_IdVeicolo == 0)
      {
        msgLog = funcName + " reason=\"Veicolo non identificato"
             + ", attendo le informazioni di identificazione da QB\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);

        LedSrv.Set(LedState.LED_NON_IDENTIFICATO);

        WaitIdentification(_Ubmgr);

        /* Ora che ho le info di base, le metto su file per i prossimi riavvii */
        UB_ScriviServizio();
      }

      /* Mi salvo l'ID Veicolo */
      Globals.m_StatoUcb.m_IdVeicolo = m_IdVeicolo;

      /* Se il vettore e` zero, vuol dire che
        1) e` il primo avvio in assoluto
        2) qualcuno mi ha tolto il file di identificazione

         Nel secondo caso cancello tutti i file in Current e New per aspettare
         di avere il vettore durante un contatto col CDD
      */
      if (m_Vettore == 0 && VersionSw.MGR_CheckConfigFile(VersionType.ENTRAMBE) == false)
      {
        /* Ripulisco la configurazione */
        VersionSw.MGR_RimuoviConfig();
      }

      msgLog = funcName + " ENDS, rc=0";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      return true;
    }

    private bool UB_ScriviServizio()
    {
      String funcName = "UB_LeggiServizio()";
      String msgLog = "";

      msgLog = funcName + " ENDS, rc=0";
      msgLog = funcName + " STARTS"
             + " Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
             + ", Operatore=" + ((int)m_Operatore).ToString()
             + ", Classe=" + ((int)m_Classe).ToString()
             + ", IdVeicolo=" + m_IdVeicolo.ToString()
             + ", Vettore=" + m_Vettore.ToString();
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      StreamWriter sw = null;
      try
      {
        sw = new StreamWriter(DirsNames.NOME_FILE_IDENTIFICAZIONE);
      }
      catch (Exception)
      {
        msgLog = funcName + " FAILED reason=\"Non riesco ad aprire il file\""
               + ", Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
               + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);  
        return false;
      }

      bool rst = false;

      if ( sw != null)
      {
        /* Genero una stringa che scrivero' sul file. 
         * Mantenere questo formato, e` quello usato dalla LeggiServizio! */
        String strDati = "";
        strDati = ((int)m_Operatore).ToString() + "\n"
                + ((int)m_Classe).ToString() + "\n"
                + m_IdVeicolo.ToString() + "\n"
                + m_Vettore.ToString() + "\n";

        try
        {
          sw.Write(strDati);
          sw.Close();
          FileUtils.UCB_DiskFlush();

          msgLog = funcName + " ENDS rc=0";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);
          rst = true;
        }
        catch (Exception)
        {
          msgLog = funcName + " FAILED reason=\"Non riesco a scrivere su file\""
                 + ", Nomefile=\"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
                 + ", ToWrite =" + strDati.Length + ", Written =-1"
                 + ", errno =" + Porting.GetErrNoStr() + ", rc =-1";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
          sw.Close();
          FileUtils.unlink(DirsNames.NOME_FILE_IDENTIFICAZIONE);
        }
      }
      return rst;
    }

    internal void UB_ScriviVettore(UbMgr _Ubmgr)
  {
      UB_DatiBase ub_DatiBaseAux = new UB_DatiBase();
      ub_DatiBaseAux.Clear();

      String funcName = "UB_ScriviVettore()";
      String msgLog = "";

      msgLog = funcName + " reason=\"Lettura dati di servizio\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      /* Leggo il file fileUBid */
      ub_DatiBaseAux.UB_LeggiServizio(_Ubmgr);

      msgLog = funcName + " reason=\"Su file \"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
            + " il vettore vale \"" + ub_DatiBaseAux.m_Vettore.ToString() + "\""
            + ", dal CDX ho \"" + m_Vettore.ToString() + "\" (in memoria)\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      /* Se il vettore nel file e` diverso da quello in memoria (letto da CDX) */
      if (ub_DatiBaseAux.m_Vettore != m_Vettore) 
      {
        /* Se il valore attualmente in memoria (da CDX quando ho fatto il rientro) e` corretto */
        /* quindi per verificarne la correttezza del valore basta vedere che sia diverso da 0 */
        if (m_Vettore != 0)
        {
          msgLog = funcName 
                 + " reason=\"Trovato su file \"" + DirsNames.NOME_FILE_IDENTIFICAZIONE + "\""
                 + " un vettore diverso da quello letto da CDX"
                 + ", lo imposto al nuovo valore \"" + m_Vettore.ToString() + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);

          /* Imposto il nuovo valore del vettore */
          ub_DatiBaseAux.m_Vettore = m_Vettore;

          /* Scrivo il file */
          ub_DatiBaseAux.UB_ScriviServizio();
        }
      }
    }
  }
}
