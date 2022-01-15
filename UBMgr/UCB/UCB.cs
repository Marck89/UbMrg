using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  /* Codici allarmi del Manager (CU e UB) */
  internal enum UcbAlarmsCode
  {
    UCB_ALLARME_RCC_SCONNESSO = 10,
    UCB_ALLARME_PERDITA_ORARIO = 11,  /* Srg 09/06/2003 */
    UCB_ALLARME_ORA_GPS_DISALLINEATA = 12,  /* MF 30/09/2006 */
    UCB_ALLARME_RESTART_DOPO_SPEGNIMENTO_ANOMALO = 13,  /* MF 30/09/2006 */
    UCB_ALLARME_MAC_ADDRESS_VARIATO = 14, /* MF 30/09/2006 */
    UCB_ALLARME_CNV_CON_SERVIZIO_ANOMALO = 15,  /* RD 27/03/2007 */
    UCB_ALLARME_CNV_CON_TIPO_ERRATO = 16, /* RD 27/03/2007 */
    UCB_ALLARME_CNV_SENZA_SOFTWARE = 17,  /* RD 27/03/2007 */
    UCB_ALLARME_CNV_PROBLEMI_DI_CONNESSIONE = 18, /* RD 27/03/2007 */

    UCB_ALLARME_CONFIGURAZIONE_INATTESA = 24, /* Srg 22/03/2004 */
    UCB_ALLARME_SCARICO_FILE = 25,  /* Srg 22/03/2004 */
    UCB_ALLARME_INVIO_FILE = 26,  /* Srg 22/03/2004 */
    UCB_ALLARME_NFP_ERRATO = 27,  /* Srg 23/03/2004 (NON USATO IN UCB) */

    UCB_ALLARME_CNV0_SCONNESSA = 131,
    UCB_ALLARME_CNV1_SCONNESSA = 132,
    UCB_ALLARME_CNV2_SCONNESSA = 133,
    UCB_ALLARME_CNV3_SCONNESSA = 134,
    UCB_ALLARME_CNV4_SCONNESSA = 135,
    UCB_ALLARME_CNV5_SCONNESSA = 136,
  }


  internal class UCB
  {
    /* Legge il file per notificare il cambio dell'ora  */
    internal static int UCB_ReadDeltaTime(ref int DeltaTime)
    {
      String funcName = "UCB_ReadDeltaTime()";
      String logMsg = "";

      DeltaTime = 0; /* Non devo variare l'ora */

      int rc = -1;
      if (FileUtils.UCB_FileExists(DirsNames.WLAN_DIFFTIME_PATH))
      {
        try
        {
          StreamReader sr = new StreamReader(DirsNames.WLAN_DIFFTIME_PATH);

          /* Leggo dal file il Delta Time tra CDD e UCB */
          String st = sr.ReadLine();
          sr.Close();

          if (String.IsNullOrEmpty(st))
          {
            /* Read error */
            logMsg = funcName + " reason=\"Errore durante la lettura del file\""
                   + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                   + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
            LogTrace.Write(0, Severity.LOG_ERR, logMsg);
            rc = -1;
          }
          else
          {
            int value = -1;
            bool rst = int.TryParse(st, out value);
            if (rst == false)
            {
              /* Invalid format */
              logMsg = funcName + " reason=\"Invalid format\""
                     + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                     + ", Value=\"" + st + "\", rc=-1";
              LogTrace.Write(0, Severity.LOG_ERR, logMsg);
              rc = -1;
            }
            else
            {
              DeltaTime = value;
              rc = 0;
            }
          }
        }
        catch (Exception)
        {
          logMsg = funcName + " reason=\"Errore durante l'apertura del file\""
                 + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                 + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
          LogTrace.Write(0, Severity.LOG_ERR, logMsg);

          /* File con l'ora non trovato */
          rc = 0;
        }
      }

      return rc;
    }

    internal static int UCB_WriteDeltaTime(int DeltaTime)
    {
      String funcName = "UCB_WriteDeltaTime()";
      String logMsg = "";

      /* Cancello il vecchio valore */
      FileUtils.unlink(DirsNames.WLAN_DIFFTIME_PATH);

      int rc = -1;
      try
      {
        StreamWriter sw = new StreamWriter(DirsNames.WLAN_DIFFTIME_PATH);
        try
        {
          /* Scrivo su file il Delta Time tra CDD e UCB */
          String st = DeltaTime.ToString();
          sw.WriteLine(st);
          rc = 0;
        }
        catch (Exception)
        {
          rc = -1;
          /* Write error */
          logMsg = funcName + " reason=\"Errore durante la scrittura del file\""
                 + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                 + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
          LogTrace.Write(0, Severity.LOG_ERR, logMsg);
        }
        sw.Close();
        if (rc == -1)
        {
          FileUtils.unlink(DirsNames.WLAN_DIFFTIME_PATH);
        }
        else
        {
          FileUtils.UCB_DiskFlush(); /* Voglio essere sicuro che il dato sia stato scritto */

          /* Verifico quanto appena scritto */
          int currentDeltaTime = 0;
          rc = UCB_ReadDeltaTime(ref currentDeltaTime);
          if (rc != 0 || DeltaTime != currentDeltaTime)
          {
            rc = -1;
            logMsg = funcName + " reason=\"Errore durante la scrittura del dato di cambio orario\""
                   + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                   + ", DeltaTimeScritto=" + DeltaTime.ToString() + ", DeltaTimeLetto=" + currentDeltaTime.ToString()
                   + ", rc=-1";

            LogTrace.Write(0, Severity.LOG_ERR, logMsg);
          }
          else
          {
            rc = 0;
            logMsg = funcName + " reason=\"Dato di sincronizzazione del RTC scritto su file con successo\""
                   + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
                   + ", DeltaTimeScritto=" + DeltaTime.ToString() + ", DeltaTimeLetto=" + currentDeltaTime.ToString()
                   + ", rc=0";

            LogTrace.Write(0, Severity.LOG_ERR, logMsg);
          }
        }
      }
      catch (Exception)
      {
        rc = -1;
        /* Non riesco ad aprire il file */
        logMsg = funcName + " reason=\"Errore durante l'apertura del file\""
               + ", Filename=\"" + DirsNames.WLAN_DIFFTIME_PATH + "\""
               + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
        LogTrace.Write(0, Severity.LOG_ERR, logMsg);
      }
      return rc;
    }

    /* Restituisce l'ora di sistema corretta con la differenza tra UCB e CDD
       deve essere utilizzata per ogni orario di "tipo SBME" */
    internal static int UCB_GetSbmeTime()
    {
      const int SBME_MINIMUM_TIME = 0;

      int UcbTime;
      int MinTimeValue = SBME_MINIMUM_TIME;

      UcbTime = TimeUtils.GetTime();
      if (UcbTime < MinTimeValue)
      {
        String msg = "UCB_GetSbmeTime()";
        msg += " FAILED reason=\"Fallita richiesta data di sistema\""
              + ", UcbTime=" + UcbTime + ", MinTimeValue=" + MinTimeValue;
        LogTrace.Write(0, Severity.LOG_ERR, msg);

        return 0;
      }

      UcbTime += Globals.m_UcbDeltaTime;

      return UcbTime;
    }

    /* Restituisce l'ora di sistema NON corretta con la differenza tra UCB e CDD
    ** deve essere utilizzata per calcolare i delta sui tempi trascorsi (ogni volta
    ** che si devono confrontare due orari) */
    internal static int UCB_GetSystemTime()
    {
      int SBME_MINIMUM_TIME = 0;
      int UcbTime = TimeUtils.GetTime();
      if (UcbTime < SBME_MINIMUM_TIME)
      {
        String msgLog = "UCB_GetSystemTime()";
        msgLog += " FAILED reason=\"Fallita richiesta data di sistema\","
               + " UcbTime=" + UcbTime.ToString() + " MinTimeValue=" + SBME_MINIMUM_TIME.ToString();
        LogTrace.Write(0, Severity.LOG_ERR, msgLog);

        return (0);
      }
      return UcbTime;
    }

    internal static bool CreateDirectoryMgr(String FolderName)
    {
      String funcName = "CreateDirectory()";
      String msgLog = "";

      bool rst = true;

      if (!FileUtils.UCB_DirectoryExists(FolderName))
      {
        if (!FileUtils.UCB_CreateDirectory(FolderName))
        {
          msgLog = funcName + " FAILED reason=\"Fallita creazione Directory\""
                + ", NomeDirectory=\"" + FolderName + "\""
                + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
          rst = false;
        }
        else
        {
          msgLog = funcName + " reason=\"Directory creata con successo\""
                + ", NomeDirectory=\"" + FolderName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);
        }
      }
      return rst;
    }

    static internal String CodiceAllarmeMgrToStr(UcbAlarmsCode CodiceAllarme)
    {
      string str = "";
      switch (CodiceAllarme)
      {
        case UcbAlarmsCode.UCB_ALLARME_RCC_SCONNESSO:
          str = "Sconnessione prolungata QB";
          break;

        case UcbAlarmsCode.UCB_ALLARME_PERDITA_ORARIO:
          str = "Perdita orario";
          break;

        case UcbAlarmsCode.UCB_ALLARME_ORA_GPS_DISALLINEATA:
          str = "Ora SBME non allineata con GPS";
          break;

        case UcbAlarmsCode.UCB_ALLARME_RESTART_DOPO_SPEGNIMENTO_ANOMALO:
          str = "Restart dopo spegnimento anomalo";
          break;

        case UcbAlarmsCode.UCB_ALLARME_MAC_ADDRESS_VARIATO:
          str = "Mac address variato";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV_CON_SERVIZIO_ANOMALO:
          str = "Servizio anomalo CNV";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV_CON_TIPO_ERRATO:
          str = "CNV con tipo errato";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV_SENZA_SOFTWARE:
          str = "Mancanza software su CNV";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV_PROBLEMI_DI_CONNESSIONE:
          str = "CNV con problemi di connessione";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV0_SCONNESSA:
          str = "Perdita contatto CNV0";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV1_SCONNESSA:
          str = "Perdita contatto CNV1";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV2_SCONNESSA:
          str = "Perdita contatto CNV2";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV3_SCONNESSA:
          str = "Perdita contatto CNV3";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV4_SCONNESSA:
          str = "Perdita contatto CNV4";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CNV5_SCONNESSA:
          str = "Perdita contatto CNV5";
          break;

        case UcbAlarmsCode.UCB_ALLARME_CONFIGURAZIONE_INATTESA:
          str = "Configurazione non attesa";
          break;

        case UcbAlarmsCode.UCB_ALLARME_SCARICO_FILE:
          str = "Errore su scarico file";
          break;

        case UcbAlarmsCode.UCB_ALLARME_INVIO_FILE:
          str = "Errore su invio file";
          break;

        default:
          str = "??? Non in elenco ???";
          break;
      }
      return str;
    }

  }
}

