using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Sbme
{
   /* Exit codes per UBMgr */
  enum ExitCode
  { 
    UCB_EC_SUCCESSFUL	       = 0,	/* Arresto normale */
    UCB_EC_CONFIG_ERROR	     = 1,	  /* Errore grave in configurazione */
    UCB_EC_INTERNAL_ERROR	   = 2,	  /* Errore grave, es. Segmentation Fault */
    UCB_EC_INVALID_ARGUMENT	 = 3,	  /* Argomento non valido passato sulla argument line */
  }

  class UbMgr
  {
    internal bool m_EmulaErroreDataNonValida = false;

    internal bool m_UCB_Registered = false;
    internal bool m_UCBLibInitialized = false;

    internal bool m_DownloadSWInCorso = false;

    internal WLanState m_UCB_WLanStatus = WLanState.SBME_WLAN_DISCONNECTED;
    internal bool m_UCB_WLanIsBusy = false;

    internal UB_DatiBase m_UB_DatiBase = new UB_DatiBase();

    /*  Stato delle CNV  */
    internal CnvStatus m_CNVStatus = CnvStatus.Off; /*Di default CNV Spente */

      /* Ttread  */
    internal WdgMgr m_WdgMgr = new WdgMgr();
    internal OmiMgr m_OmiMgr = new OmiMgr();
    internal RccMgr m_RccMgr = new RccMgr();
    internal CnvMgr m_CnvMgr = new CnvMgr();

//    internal int m_Stato = 0;

    internal  UbMgr()
    {
    }

    /* Read para,eter From command line */
    private void ReadCommandLine(string[] Args)
    {
      PrintUsage();

      String what = "@(#)UBmgr (SBME UCB Application) v0065 (C)Selex-ES S.p.A. 2013";

      String msg = "";

      CultureInfo ci = CultureInfo.InvariantCulture;

      for (int i = 0; i < Args.Length; i++)
      {
        String st = Args[i];
        if (String.IsNullOrEmpty(st)) continue;

        if (st.Length < 2)
        {
          msg = "\nInvalid argument '" + st + "\n";
          Console.WriteLine(msg);

          PrintUsage();

          Environment.Exit((int)ExitCode.UCB_EC_INVALID_ARGUMENT);
        }

        String paramType = st.Substring(0, 2);

        if (st[0] != '-')
        {
          msg = "\nInvalid argument '" + st + "\n";
          Console.WriteLine(msg);

          PrintUsage();

          Environment.Exit((int)ExitCode.UCB_EC_INVALID_ARGUMENT);
        }

        /* Verifico l'argomento passato */
        if (st == "-v") /* Richiesta what string */
        {
          msg = "\n" + what + "\n";
          Console.WriteLine(msg);

          Environment.Exit((int)ExitCode.UCB_EC_SUCCESSFUL);
        }
        else if (st == "-norts") /* Non eseguo il Reset dell'RTS (lo lascio fare al driver) */
        {
          msg = "\nATTENZIONE: Richiesta inibizione Reset segnale RTS!\n";
          ConsoleColor color = Console.ForegroundColor;
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(msg);
          Console.ForegroundColor = color;

          Globals.m_RS485UseDriverRTS = true;
        }
        else if (st == "-stp403dump")
        {
          Globals.m_Stp403DumpProtocol = true;
        }
        else if (st == "-nocnv")
        {
          Globals.m_BloccoThreadCNV = true; /* Forzatura blocco Thread gestione CNV */
        }
        else if (st == "-gpsdump")
        {
          Globals.m_DumpGpsData = true;
        }
        else if (st == "-famille")
        {
          Globals.m_ErronOnFamilleBin = true;
        }
        else if (st == "-invaliddate")
        {
          m_EmulaErroreDataNonValida = true;
        }
        else if (paramType == "-d") /* Delta time */
        {
          int.TryParse(st.Substring(2), NumberStyles.HexNumber, ci, out Globals.m_UcbDeltaTime);
        }
        else if (paramType == "-s") /* Log Severity */
        {
          int startUpSeverity = 0;
          int.TryParse(st.Substring(2), out startUpSeverity);
          Globals.m_SeverityThreashold = SeverityUtils.GetSeverity(startUpSeverity);
        }
      }
    }

    private static void PrintUsage()
    {
      string codeBase = Assembly.GetExecutingAssembly().CodeBase;
      string appName = Path.GetFileName(codeBase);

      String msg = "Use: " + appName;
      msg += " [-v]"
          + " [-norts]"
          + " [-stp403dump]"
          + " [-nocnv]"
          + " [-gpsdump]"
          + " [-famille]"
          + " [-invaliddate]"
          + " [-d<delta hex>]"
          + " [-s<severity>]";

      Console.WriteLine(msg);
    }

    internal void Init()
    {
      String funcName = "Init()";
      String msgLog = "";

      Globals.Init();

      /* Provo a leggere il file /ramdisk/DiffTime */
      /* Se il file non esiste o fallisce la lettura, UcbDeltaTime resta uguale a prima */
      UCB.UCB_ReadDeltaTime( ref Globals.m_UcbDeltaTime);

    	 /* ORA VALIDA : Solo da questo momento in poi ha senso chiamare "UCB_GetSbmeTime()	*/
      Globals.m_StartupTime = UCB.UCB_GetSbmeTime();

     	/* Le inizializzo all'ora attuale per evitare di lasciarle a 0 che potrebbe essere rischioso */
	    Globals.m_QB.m_Stop_OffLine_Time = Globals.m_StartupTime;
  	  Globals.m_QB.m_Start_OffLine_Time = Globals.m_StartupTime;

 	      /*  LOG ATTIVI : Solo da questo momento ha senso fare dei nuovi LOG */
    	UB_LoadLogFile(); /* Sposto i file di log dal disco a ramdisk */

#if !IS_WINDOWS
   	   /* Armo la signal() per intercettare un segmentation fault */
   	 signal(SIGSEGV, SignalHandler);
#endif

      WriteStartOnLog();

      /* Print SYS INFO */
      MGR_GetSystemInfo();

      /* BACKUP LOG FILE */
      UB_StoreLogFile(true);

      if (Globals.m_BloccoThreadCNV == true)
      {
        msgLog = funcName + " reason=\"Thread di gestione CNV BLOCCATO da programma (lasciato inattivo)\"";
        LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
      }

      /* Azzero il valore di "Ultima Linea Utilizzata" */
      Globals.m_CnvLastValidLine = Sbme.SBME_UNKNOWN_ROUTE;

      /* Inizializzo la struttura globale di stato della UCB */
      MGR_InitUcbStatus();

      /* Inizializzo la struttura globale di stato delle CNV */
      MGR_InitCnvStatus(true);

      UB_StoreLogFile(true);

      if (Globals.m_Stp403DumpProtocol == true)
      {
        msgLog = funcName + " reason=\"Abilitato il dump del protocollo STP403 (a TRACE)\"";
        LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
      }

      if (Globals.m_ErronOnFamilleBin == true)
      {
        msgLog = funcName + " reason=\"Abilitato errore su file FAMILLE.BIN\"";
        LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
      }

      /* Elimino i dati statistici troppo vecchi */
      MGR_PackUcbStatistics();
    }

    private void SetAllCNVIcon(int CnvCount, int IconID)
    {
    }

    private void MGR_InitCnvStatus(bool PrimaInizializzazione)
    {
      Globals.m_Cnvs.MGR_InitCnvStatus(PrimaInizializzazione);
    }

    private void MGR_PackUcbStatistics()
    {
      StatoUCB.MGR_PackUcbStatistics();
    }

    private void MGR_InitUcbStatus()
    {
      Globals.m_StatoUcb.Init();
      Globals.m_ApparatiGenerico.Init();
    }

    internal static void UB_LoadLogFile()
    {
      if (!FileUtils.UCB_FileExists(DirsNames.UCB_LOG_RAMDISK_PATH + "ucb_log.txt"))
      {
        /* Se non ho il file in RAMDISK allora lo ricarico dal disco (se esiste) */
        if (FileUtils.UCB_FileExists(DirsNames.UCB_LOG_DATA_PATH + "ucb_log.txt"))
        {
          /* Sposto il file da disco a ramdisk */
          FileUtils.UCB_CopyFile(DirsNames.UCB_LOG_DATA_PATH + "ucb_log.txt", DirsNames.UCB_LOG_RAMDISK_PATH + "ucb_log.txt");
        }
      }
    }

    internal static void UB_StoreLogFile(bool BackupMode)
    {
      if (BackupMode == true)
      {
        LogTrace.Write(0, Severity.LOG_NOTICE, "<Log backup>");
      }

      if (FileUtils.UCB_FileExists(DirsNames.UCB_LOG_RAMDISK_PATH + "ucb_log.txt"))
      {
        /* Sposto il file da ramdisk a disco */
        FileUtils.UCB_CopyFile(DirsNames.UCB_LOG_RAMDISK_PATH + "ucb_log.txt", 
                               DirsNames.UCB_LOG_DATA_PATH + "ucb_log.txt");
      }
    }

    internal static void WriteStartOnLog()
    {
      /* ======================================== */
      /* Da questo momento i log sono disponibili */
      /* ======================================== */
      String msg = "";
      msg = "===========================================";
      LogTrace.Write(0, Severity.LOG_NOTICE, msg);
      msg = "         S B M E    I N    A V V I O       ";
      LogTrace.Write(0, Severity.LOG_NOTICE, msg);
      msg = "===========================================";
      LogTrace.Write(0, Severity.LOG_NOTICE, msg);

      msg = "SBME() STARTING PROCESS"
          + ", Versione=" + VersionSw.VERSIONE_MAJOR.ToString("00") + "." + VersionSw.VERSIONE_MINOR.ToString("00")
          + " (compilazione " + VersionSw.NUMERO_COMPILAZIONE + ")"
          + ", BootTimeMin=" + Globals.m_ContatoreMinutiRuntime.ToString()
          + ", Thread_Id=" + Porting.THREAD_SELF().ToString();
      LogTrace.Write(0, Severity.LOG_NOTICE, msg);
    }

    internal static void MGR_GetSystemInfo()
    {
      //// DA FARE
    }

    /* Controllo se DATA<2004 solo su UCB Linux, gli apparati AVM escono dalla fabbrica tutti con data 2000 */
    internal void ControlDate2004()
    {
      int systemDate = UCB.UCB_GetSystemTime();
      DateTime dateTime = TimeUtils.ConvertFromUnixTimestamp(systemDate);

      if (m_EmulaErroreDataNonValida == true ||
          (dateTime != DateTime.MinValue && dateTime.Year <= 2004))
      {
        String funcName = "ControlDate";
        String msgLog = funcName;
        msgLog = funcName + " reason=\"Errore nella data di sistema: Data troppo vecchia (<=2004)"
               + ", BLOCCO LE CONVALIDATRICI\", Data=" + dateTime.ToString("HH:mm-dd/MM/yyyy");
        LogTrace.Write(0, Severity.LOG_ERR, msgLog);


        /* Blocco le CNV */
        Globals.m_BloccoThreadCNV = true;

        /* Devo segnalare l'Allarme */
        Globals.m_Alarms.m_AllarmePerditaOrario = true;

        Globals.m_Alarms.AttivaAllarmeVersoQB(AlarmsQB.DATA_NON_VALIDA, true);
      }
    }

    internal void InitEx()
    {
      MacAddress.Check();

      /* Avvio il timer che misura il tempo di esecuzione */
      TimeEsecuzioneUCB.TimerEsecuzioneUCB(CmdTimer.START_TIME);

      /* M.F. Non mi sono ancora registrato al Demone */
      m_UCB_Registered = false;

      /* M.F. La WLAN e' spenta alla partenza */
      m_UCB_WLanIsBusy = false;

      DirtyFlag.Check();

      ControlDate2004();

      /* BACKUP LOG FILE */ 
      UB_StoreLogFile(true);
    }

    internal void InitLocalDatas()
    {
      string funcName = "InitLocalDatas()";
      String msgLog = "";

      InitEx();

      msgLog = funcName + " reason=\"Inizializzazione dati interni SBME\"";
      LogTrace.Write(0, Severity.LOG_INFO, msgLog);

      bool rst = InizializzazioneSBME(m_UB_DatiBase);

      if (rst == false)
      {
        /* Errore grave */
        msgLog = funcName + " reason=\"Errore grave durante la fase di inizializzazione\"";
        LogTrace.Write(0, Severity.LOG_ERR, msgLog);

        if (m_UCB_Registered == true)
        {
          /* Mi deregistro dal WLAN Daemon */
          rst = WLan.ApplRegisterMgr(WLanOpCode.APPL_UNREGISTER, WLanIdUser.SBME, 1);
          m_UCB_Registered = false;
        }

        /* Salvo i log su Disco */
        UB_StoreLogFile(false);

        /* Termino l'applicazione */
        UB_Terminate(ExitCode.UCB_EC_CONFIG_ERROR);
      }

      VersionSw.CheckAtTheStart();

      /* Per la diagnostica... */
      Diag.Diag_Init(VersionSw.VERSIONE_MAJOR, VersionSw.VERSIONE_MINOR);

      /* BACKUP LOG FILE */
      UB_StoreLogFile(true);

      msgLog = funcName + " reason=\"Lettura dati di servizio\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      /* Leggo le informazioni di base, se non le trovo nel file, aspetto l'RCC */
      m_UB_DatiBase.UB_LeggiServizio(this);
      if (rst == false)
      {
        msgLog = funcName + " reason=\"Lettura dati di servizio fallita\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
      }

      /* Lettura parametro attesa spegnimento (in minuti), default 60 (1 ora) */
      int attesaSpegnimento = LeggiAttesaSpegnimento();

 // DA FARE XXXX     MGR_ApriFileAtt(&UB_DatiBase, true);

      FileUtils.ScriviPID("UBmgr", 0);

      /* Se questo e' un riavvio dopo un aggiornamento del SW dell'UB, 
       * devo andare direttamente all'aggiornamento delle CNV ed in seguito spegnermi  */

      if (MGR_TestPrimoRiavvio() == true) /* Il test cancella il file di flag, se esiste */
      {
        msgLog = funcName + " reason=\"Riavvio dopo sostituzione software UCB\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

     // DA FARAE   UB_AggiornamentoCNVPrimoRiavvio(&UB_DatiBase);
      }

      /* Per la diagnostica */
      m_UB_DatiBase.m_StatoDisp.DiagInitStates();
    }


    /* Test e cancellazione del file di controllo per il primo riavvio
     * dopo l'aggiornamento del SW del manager */
    internal bool MGR_TestPrimoRiavvio()
    {
      bool rst = false;
      String funcName = "MGR_TestPrimoRiavvio()";
      String msgLog = "";

      if (FileUtils.UCB_FileExists(DirsNames.UCB_FILE_NAME_RIAVVIO_SW))
      {
        msgLog = funcName + " reason=\"Trovato file con dati per la gestione del riavvio\""
               + ", Filename=\"" + DirsNames.UCB_FILE_NAME_RIAVVIO_SW + "\"";
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_NOTICE, msgLog);

        try
        {
          StreamReader sr = new StreamReader(DirsNames.UCB_FILE_NAME_RIAVVIO_SW);
          String VersioneCorrente = sr.ReadLine();
          sr.Close();

          /* 16/10/2003 Srg
           * Controllo su cancellazione eseguibile. Poiche` sarebbe grave rimanere senza
             l'eseguibile attualmente in uso (impossibile ripartire in automatico) faccio
             un controllo se la versione che vado a cancellare sia diversa da quella in  uso */

          /* Pesco gli ultimi 4 byte di ZZmgrxxyy dove c'e` la versione */
          int checkVer = 0;
          String tmp = VersioneCorrente.Substring(VersioneCorrente.Length - 4);
          int.TryParse(tmp, out checkVer);
          int verSw = VersionSw.GetVersionLong();
          if (checkVer != verSw)
          {
            /* Rimuovo il vecchio eseguibile */
            msgLog = funcName + " reason=\"Rimuovo vecchio SW " + VersioneCorrente + "\"";
            LogTrace.Write(LogType.LOG_MGR, Severity.LOG_NOTICE, msgLog);

            if (FileUtils.unlink(VersioneCorrente) == false)
            {
              msgLog = funcName + " reason=\"Errore rimozione SW " + VersioneCorrente + "\"";
              LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
            }
            FileUtils.unlink(DirsNames.UCB_FILE_NAME_RIAVVIO_SW);
            rst = true;
          }
        }
        catch
        {
          msgLog = funcName + " reason=\"Errore apertura file con dati per la gestione del riavvio\""
                 + ", Filename=\"" + DirsNames.UCB_FILE_NAME_RIAVVIO_SW + "\""
                 + ", errno=" + Porting.GetErrNoStr();
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
        }
      }
      return rst;
    }

    /* Verifica dati auto-localizzazione */
    internal void VerificaAutoLlocalizzazione()
    {
      String funcName = "VerificaAutoLlocalizzazione()";
      String msgLog = "";

      if (FileUtils.UCB_FileExists(DirsNames.UCB_FILE_NAME_AUTOLOC_PAR))
      {
        msgLog = funcName + " reason=\"File con Parametri per Auto-Localizzazione PRESENTE\""
               + ", Pathname=\"" + DirsNames.UCB_FILE_NAME_AUTOLOC_PAR + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
      }
      else
      {
        msgLog = funcName + " reason =\"File con Parametri per Auto-Localizzazione NON PRESENTE\""
               + ", Pathname=\"" + DirsNames.UCB_FILE_NAME_AUTOLOC_PAR + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
      }

      if (FileUtils.UCB_FileExists(DirsNames.UCB_FILE_NAME_AUTOLOC_DATA))
			{
        msgLog = funcName + " reason=\"File con Dati per Auto-Localizzazione PRESENTE\""
               + ", Pathname=\"" + DirsNames.UCB_FILE_NAME_AUTOLOC_DATA + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
			}
			else
			{
        msgLog = funcName + " reason=\"File con Dati per Auto-Localizzazione NON PRESENTE\""
               + ", Pathname=\"" + DirsNames.UCB_FILE_NAME_AUTOLOC_DATA + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
			}
		}

    /* Tempo atteso fra intAlim giu` e lo spegnimento effettivo in mancanza di contatto deposito */
    internal int LeggiAttesaSpegnimento()
    {
      String funcName = "LeggiAttesaSpegnimento()";
      String msgLog = "";

      String fileName = DirsNames.DIR_CONFIGURAZIONE + "UBconfig";

      msgLog = funcName + " reason=\"Lettura file di configurazione\""
            + ", Nomefile=\"" + fileName + "\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      int attesaSpegnimento = 0;

      msgLog = funcName + " reason=\"Lettura file di configurazione\""
            + ", Nomefile=\"" + fileName + "\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      /* Lettura parametro attesa spegnimento (in minuti), default 60 (1 ora) */
      FileIni fileIni = new FileIni();
      bool rst = fileIni.StartFileIni(fileName);
      if (rst == false)
      {
        attesaSpegnimento = 60;

        msgLog = funcName + " reason=\"Fallita la lettura file di configurazione\""
               + ", Nomefile=\"" + fileName + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
      }
      else
      {
        attesaSpegnimento = fileIni.GetFileIniInt("attesaSpegnimento", 60);

        /* Chiudo il file di configurazione */
        fileIni.EndFileIni();
      }

      msgLog = funcName + " reason=\"Tempo di attesa prima dello spegimento e' di"
             + attesaSpegnimento.ToString() + " minuti \"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      return attesaSpegnimento;
    }

    private bool InizializzazioneSBME(UB_DatiBase _UB_DatiBase)
    {
      String funcName = "InizializzazioneSBME()";
      String msgLog = "";

      msgLog = funcName + " START";
      LogTrace.Write( LogType.LOG_UB, Severity.LOG_DEBUG, msgLog );

      /* Inizializzo la libreria libucb */
      if (BusInterface.ucb_init() != UcbRcCode.UCB_RC_OK)
      {
        return false;
      }
      m_UCBLibInitialized = true; /* UCB Lib initialized */

      /* Avvio thread per reset WD */
      bool rc = m_WdgMgr.Start();
      if (rc == false) 
      {
        return false;
      }

      _UB_DatiBase.Clear();

      msgLog = funcName + " reason=\"Verifico se ci sono altre istanze di SBME in esecuzione\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      String ubPid = UbId.ReadPId();
      if ( ubPid != "" ) 
      {
        if ( Porting.UCB_InstanceAlreadyRunning(ubPid))
        {
          msgLog = funcName + " FAILED reason=\"Processo SBME gia' in esecuzione\""
                + ", OldPid=\"" + ubPid + "\", rc=-1";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
          return false;
        }
        /* Se il file UB.pid esiste ma il processo no, 
         * verra' cancellato sulla creazione del nuovo */
      }

      /* Inizializzo la struttura con gli ID dei thread */
// XXXXX ERRORE forse va messo prima della creazione del thread XXXXXX
      Globals.m_ListaThread.Clear();

      /* Nome localita' di partenza di default 
       * (per evitare di stampare una stringa non inizializzata) */
      _UB_DatiBase.m_InfoLinea.m_NomeLocalita = "NoLoc";

      msgLog = funcName + " reason=\"Inizializzato il nome della localita' di default\""
             + ", NomeLocalita=\"" + _UB_DatiBase.m_InfoLinea.m_NomeLocalita + "\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);


      /* Accendo le CNV */
      if (UCB_AccendiCNV() == false)
      {
        msgLog = funcName + " FAILED reason=\"Fallita accensione CNV\", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
        return false;
      }

      msgLog = funcName + " reason=\"Rele' di accensione delle CNV su impostato ON\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      /* Registrazione WLD (attendo 1 secondo prima di provare) */
      Thread.Sleep(1000);

      int retryCount = 10;
      m_UCB_Registered = WLan.ApplRegisterMgr(WLanOpCode.APPL_REGISTER, 
                                              WLanIdUser.SBME, retryCount);
      if (m_UCB_Registered == false)
      {
        UCB_SpegniCNV();    /* Spengo CNV */
        return false;
      }

      msgLog = funcName + " reason=\"Inizializzazione strutture CNV\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      /* Inizializzo le strutture comuni per i thread CNV (flags e mutex) senza avviare i thread */
      m_CnvMgr.Init();

      /* Controllo l'esistenza delle directory di lavoro. Se mancano, le creo */
      bool rst = CreateDirectories();
      if ( rst == false ) 
      {
        UCB_SpegniCNV();       /* Spengo CNV */
        return false;
      }
        /* lettura numero seriale UCB */
      int serialNum = UbId.ReadSerialNum();
      if ( serialNum < 0 ) 
      {
        UCB_SpegniCNV();   /* Spengo CNV */
        return false;
      }
      Globals.m_StatoUcb.m_SerialNumber = (uint)serialNum;

      /*  Creo un file contenente il pid del processo */
      int pid = Porting.GETPID(); 
      rst = UbId.WritePId(pid);
      if ( rst == false)
      {
        UCB_SpegniCNV();   /* Spengo CNV */
        return false;
      }
      FileUtils.UCB_DiskFlush();


      /* Pulisco il ramdisk da eventuali file lasciati da precedenti esecuzioni */
      msgLog = funcName + " reason=\"Pulizia RamDisk\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      FileUtils.SvuotaDir(DirsNames.DIR_RAMDISK, false, 0);

       /* Avvio thread gestione QB */
      rc = m_RccMgr.Start();
      if (rc == false)
      {
        UCB_SpegniCNV();   /* Spengo CNV */
        return false;
      }

      m_RccMgr.SetStato( RccState.UB_IN_AVVIO);

      rc = m_OmiMgr.Start();
      if (rc == false)
      {
        /* Non lo considero, per ora, come bloccante */
        // return false;
      }


      _UB_DatiBase.Init((ushort)serialNum);

      msgLog = funcName + "  ENDS rc=0";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      return true;
    }

    internal bool UCB_SpegniCNV()
    {
      String funcName = "UCB_SpegniCNV()";
      String msgLog = "";

      UcbRcCode rc = BusInterface.ucb_setstatus_elsag(PinState.UCB_RELE_CNV, ContattoStato.UCB_OPEN);
      if (rc != UcbRcCode.UCB_RC_OK)
      {
        msgLog = funcName + " FAILED reason=\"Spegnimento CNV fallito\""
               + ", ucb_setstatus_elsag() returns " + rc.ToString();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
        return false;
      }

      msgLog = funcName + " reason=\"CNV spente\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      m_CNVStatus = CnvStatus.Off; /* CNV Spente */

      msgLog = funcName + " reason=\"Rele' di accensione delle CNV su impostato OFF\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      return true;
    }

    internal bool UCB_AccendiCNV()
    {
      String funcName = "UCB_AccendiCNV()";
      String msgLog = "";

      UcbRcCode rc = BusInterface.ucb_setstatus_elsag(PinState.UCB_RELE_CNV, ContattoStato.UCB_CLOSE);
      if (rc != UcbRcCode.UCB_RC_OK)
      {
        msgLog = funcName + " FAILED reason=\"Accensione CNV fallita\""
               + ", ucb_setstatus_elsag() returns " + rc.ToString();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
        return false;
      }

      msgLog = funcName + " reason=\"CNV accese\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      m_CNVStatus = CnvStatus.On; /* CNV Accese */

      return true;
    }

    private bool CreateDirectories()
    {
      /* Controllo l'esistenza delle directory di lavoro. Se mancano, le creo */

      String funcName = "CreateDirectories()";
      String msgLog = "";

      msgLog = funcName + " reason=\"Verifica esistenza directory di lavoro\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      bool rst = UCB.CreateDirectoryMgr(DirsNames.DIR_SW_CORRENTE);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_SW_NUOVO);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_ATTIVITA);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_ATTIVITA_CORR);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_ATTIVITA_NUOVE);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_ATTIVITA_VECCHIE);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_CONFIGURAZIONE);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_CERTIFICATI);
      if (rst) rst = UCB.CreateDirectoryMgr(DirsNames.DIR_PID);

      return rst;
    }

    internal void UB_Terminate(ExitCode exitCode)
    {
      // XXXXXXXXXXXXXXXXXXXXX DA FARE
      throw new NotImplementedException();
    }

    internal void Run(string[] Args)
    {
      ReadCommandLine(Args);

      Init();

      try
      {
        InitLocalDatas();
      }
      catch (Exception )
      {
          
      }
    }
  }
}
