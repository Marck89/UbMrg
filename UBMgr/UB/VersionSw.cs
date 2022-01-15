using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Per il controllo di configFile() */
  internal enum VersionType
  {
    NUOVA = 1,
    CORRENTE = 2,
    ENTRAMBE = 3
  }

  internal static class VersionSw
  {
    internal const int VERSIONE_MAJOR = 0;
    internal const int VERSIONE_MINOR = 65;
    internal const int VERSIONE_FILEATTIVITA = 2;
    internal const int NUMERO_COMPILAZIONE = 1;

    static int m_LastWarningTime = 0;

    internal static int GetVersionLong()
    {
      return (VERSIONE_MAJOR * 100 + VERSIONE_MINOR);
    }

    /* Controllo l'esistenza di almeno una configurazione SW per i dispositivi */
    /* In Current c'e' quella "stabile", in NEW la cerco solo la prima volta quando
       non ho ancora creato la Current */
    internal static bool MGR_CheckConfigFile(VersionType quale)
    {
      string funcName = "MGR_CheckConfigFile()";
      String msgLog = "";
      String fileName = "";

      bool res = false;
      if (quale == VersionType.CORRENTE)
      {
        fileName = DirsNames.DIR_SW_CORRENTE + "verFile";
        res = FileUtils.UCB_FileExists(fileName);
        if (res == false)
        {
          msgLog = funcName + " reason=\"Configurazione CORRENTE NON trovata\""
            + ", Nomefile=\"" + fileName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        }
        else
        {
          msgLog = funcName + " reason=\"Configurazione CORRENTE trovata\""
                 + ", Nomefile=\"" + fileName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        }
        return (res);
      }

      if (quale == VersionType.NUOVA)
      {
        fileName = DirsNames.DIR_SW_NUOVO + "verFile";
        res = FileUtils.UCB_FileExists(fileName);
        if (res == false)
        {
          msgLog = funcName + " reason=\"Nessuna NUOVA configurazione presente su HDD"
                 + ", Nomefile=\"" + fileName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        }
        else
        {
          msgLog = funcName + " reason=\"NUOVA configurazione trovata su HDD\""
                 + ", Nomefile=\"" + fileName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        }
        return (res);
      }

      if (quale == VersionType.ENTRAMBE)
      {
        String fileNameCur = DirsNames.DIR_SW_CORRENTE + "verFile";
        String fileNameNew = DirsNames.DIR_SW_NUOVO + "verFile"; ;
        bool res1 = FileUtils.UCB_FileExists(fileNameCur);
        bool res2 = FileUtils.UCB_FileExists(fileNameNew);

        res = (res1 || res2);
        if (res == false)
        {
          int TimeNow = UCB.UCB_GetSystemTime();

          /* Faccio un Log di Warning ogni 5 minuti massimo */
          if (m_LastWarningTime == 0 || (TimeNow - m_LastWarningTime) > 300)
          {
            msgLog = funcName + " reason=\"NESSUNA configurazione trovata\""
                   + ", NomefileCorrente=\"" + fileNameCur + "\""
                   + ", NomefileNuovo=\"" + fileNameNew + "\"";
            LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
            m_LastWarningTime = TimeNow;
          }
        }
        else
        {
          if (res1 == true)
          {
            msgLog = funcName + " reason=\"Configurazione CORRENTE trovata\""
                   + ", Nomefile=\"" + fileNameCur + "\"";
            LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
          }

          if (res2 == true)
          {
            msgLog = funcName + " reason=\"Configurazione NUOVA trovata\""
                   + ", Nomefile=\"" + fileNameCur + "\"";
            LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
          }
        }

        return res;
      }

      return false;
    }

    internal static void CheckAtTheStart()
    {
      string funcName = "CheckAtTheStart()";
      String msgLog = "";

      /* Per sicurezza: se non c'e` il verFile in New/ e/o Current/, pulisco le directory */
      msgLog = funcName + " reason=\"Verifica presenza di Nuova Configurazione alla partenza\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      if (MGR_CheckConfigFile(VersionType.NUOVA) == false)
      {
        FileUtils.SvuotaDir(DirsNames.DIR_SW_NUOVO, false, 0);
      }

      msgLog = funcName + " reason=\"Verifica presenza di Configurazione Corrente alla partenza\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

      if (MGR_CheckConfigFile(VersionType.CORRENTE) == false)
      {
        FileUtils.SvuotaDir(DirsNames.DIR_SW_CORRENTE, false, 0);
      }
    }

    internal static void MGR_RimuoviConfig()
    {
      string funcName = "MGR_RimuoviConfig()";
      String msgLog = "";

      msgLog = funcName + " reason=\"Elimino configurazione Current e New\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);

      /* Current */
      FileUtils.SvuotaDir(DirsNames.DIR_SW_CORRENTE, false, 0);

      /* New */
      FileUtils.SvuotaDir(DirsNames.DIR_SW_NUOVO, false, 0);
    }

  }
}
