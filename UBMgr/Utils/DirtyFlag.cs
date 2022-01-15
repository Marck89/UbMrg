using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  class DirtyFlag
  {
    internal static int ReadLastShutdownTime()
    {
      int LastShutdownTime = -1;

      String funcName = "ReadLastShutdownTime()";
      String msgLog = "";

      String str = "";

      /* Leggo la presenza del dirty flag */
      try
      {
        StreamReader fp = new StreamReader(DirsNames.UCB_DIRTY_FLAG_FILENAME);
          /* Se ho il df leggo l'ora SBME (sincronozzata con CDX) 
             scritta poco prima che l'applicativo SBME terminasse l'esecuzione */

        /* Leggo la Data/Ora ultimo arresto */
        str = fp.ReadLine();
        bool rst = int.TryParse(str, out LastShutdownTime);
        if (rst == false) LastShutdownTime = 0;

        /* Chiudo il file del Dirty Flag */
        fp.Close();
      }
      catch
      {
        msgLog = funcName + " reason=\"Rilevata una procedura di shutdown non corretta (o prima partenza)\""
               + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      return LastShutdownTime;
    }

    internal static void Check()
    {
      String funcName = "Check()";
      String msgLog = "";

        /* Leggo la Data/Ora ultimo arresto */
      int LastShutdownTime = ReadLastShutdownTime();
      if ( LastShutdownTime == -1 )
      {
        Globals.m_Alarms.m_AllarmePartenzaAnomala = true;
        return; 
      }

      if (LastShutdownTime > 0)
      {
        int CurrentTime = UCB.UCB_GetSbmeTime();
        String StDateTime1 = TimeUtils.DecodeDateTime(LastShutdownTime);
        String StDateTime2 = TimeUtils.DecodeDateTime(CurrentTime);

        /* Se l'ora di sistema (che dovrebbe essere stata aggiornata poco prima dello spegnimento della UCB)
        ** fosse arretrata rispetto a quella nel df vuol dire che ho avuto un problema sull'aggiornamento
        ** dell'RTC */
        if (LastShutdownTime > CurrentTime)
        {
          String StDateTime3 = TimeUtils.DecodeTime(LastShutdownTime - CurrentTime);
          msgLog = funcName + "reason=\"Rilevata anomalia sul Real Time Clock\""
                + ", LastShutdownTime=\"" + StDateTime1 + "\""
                + ", CurrentTime=\"" + StDateTime2 + "\""
                + ", Differenza=\"" + StDateTime3 + "\"";
          LogTrace.Write(0, Severity.LOG_ERR, msgLog);
          Globals.m_Alarms.m_AllarmePerditaOrario = true;
        }
        else
        {
          String StDateTime3 = TimeUtils.DecodeTime(CurrentTime - LastShutdownTime);
          msgLog = funcName + " reason=\"Informazioni su ultimo arresto UCB\""
                + ", LastShutdownTime=\"" + StDateTime1 + "\""
                + ", CurrentTime=\"" + StDateTime2 + "\""
                + ", TempoInattivitaUCB=\"" + StDateTime3 + "\"";
          StDateTime3 = TimeUtils.DecodeTime(CurrentTime - LastShutdownTime);

          LogTrace.Write(0, Severity.LOG_NOTICE, msgLog);
        }
      }

      /* Elimino il Dirty Flag */
      bool rst = FileUtils.unlink(DirsNames.UCB_DIRTY_FLAG_FILENAME);
      if (rst == false)
      {
        msgLog = funcName + " reason=\"Errore nella cancellazione del dirty flag\""
               + ", Nomefile=\"" + DirsNames.UCB_DIRTY_FLAG_FILENAME + "\""
               + ", errno=" + Porting.GetErrNoStr();

        LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
      }
    }

    internal static void SaveDirtyFlag()
    {
      String funcName = "SaveDirtyFlag()";
      String msgLog = "";

      bool rst = false;

      int timeinSec = UCB.UCB_GetSbmeTime();
      StreamWriter sw = null;
      try
      {
        sw = new StreamWriter( DirsNames.UCB_DIRTY_FLAG_FILENAME);
        sw.WriteLine(timeinSec.ToString());  /* Data/Ora di scrittura del Dirty Flag */
        sw.Close();

        FileUtils.UCB_DiskFlush(); /* Voglio essere sicuro che il dato sia stato scritto */

        rst = FileUtils.UCB_FileExists(DirsNames.UCB_DIRTY_FLAG_FILENAME);
        if ( rst == false)
        {
          msgLog = funcName + " FAILED, reason=\"Non riesco a creare il dirty flag file\""
                + ", Filename=\"" + DirsNames.UCB_DIRTY_FLAG_FILENAME + "\""
                + ", errno=" + Porting.GetErrNoStr();
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
        }
        else
        {
          msgLog = funcName + " reason=\"Dirty flag file creato\""
                + ", Filename=\"" + DirsNames.UCB_DIRTY_FLAG_FILENAME + "\""
                + ", TimeInSec=" + timeinSec.ToString();
        }
      }
      catch
      {
        msgLog = funcName + " FAILED, reason=\"Non riesco a creare il dirty flag file\""
              + ", Filename=\"" + DirsNames.UCB_DIRTY_FLAG_FILENAME + "\""
              + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
      }
    }
  }
}

