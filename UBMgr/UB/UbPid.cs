using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  internal class UbId
  {
    internal static bool WritePId(int Pid)
    {
      String funcName = "WritePId";
      String msgLog = "";

      msgLog = funcName + " reason=\"Scrittura file con PID applicazione\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      String fileName = DirsNames.DIR_PID + "UB.pid";

      StreamWriter sw = null;
      try
      {
        sw = new StreamWriter(fileName);
      }
      catch (Exception)
      {
        msgLog = funcName
               + " FAILED reason=\"Fallita creazione file con PID\""
               + ", Nomefile=\"" + fileName + "\""
               + ", errno=" + Porting.GetErrNoStr() + "rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }

      if (sw == null) return false;

      bool rst = false;
      try
      {
        sw.WriteLine(Pid.ToString());
        rst = true;
      }
      catch (Exception)
      {
        msgLog = funcName
               + " FAILED reason=\"Fallita scrittura file con PID\""
               + ", Nomefile=\"" + fileName + "\""
               + ", errno=" + Porting.GetErrNoStr() + "rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      sw.Close();
      return rst;
    }


    internal static String ReadPId()
    {
      String pid = "";
      String fileName = DirsNames.DIR_PID + "UB.pid";

      if (FileUtils.UCB_FileExists(fileName))
      {
        StreamReader sr = null;
        try
        {
          sr = new StreamReader(fileName);

          /* Leggo il pid */
          pid = sr.ReadLine();
        }
        catch (Exception)
        {
        }
        if ( sr != null ) sr.Close();
      }
      return pid;
    }

    internal static int ReadSerialNum()
    {
      String funcName = "ReadSerialNum";
      String msgLog = "";

      msgLog = funcName + " reason=\"Lettura file con numero seriale UCB\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);


      String numStr = "";
      int serialNum = -1;

      String fileName = DirsNames.UCB_SERIAL_PATH;

      if (FileUtils.UCB_FileExists(fileName))
      {
        StreamReader sr = null;
        try
        {
          sr = new StreamReader(fileName);

            /* Leggo il id */
          numStr = sr.ReadLine();
        }
        catch (Exception)
        {
          msgLog = funcName
                 + " FAILED reason=\"Fallita lettura file con numero seriale UCB\""
                 + ", NomeFile=\"" + fileName + "\", SerialNumber=\"" + serialNum + "\""
                 + ", errno=" + Porting.GetErrNoStr() + ", rc=-1";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
        }
        if (sr != null) sr.Close();
      }

      if (numStr.Length < 5)
      {
        msgLog = funcName
               + " FAILED reason=\"Formato numero seriale UCB non valido"
               + " (lunghezza minima 5 caratteri)\""
               + ", NomeFile=\"" + fileName + "\", SerialNumber=\"" + numStr + "\""
               + ", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      else 
      {
        int.TryParse(numStr, out serialNum);

        if (serialNum == 0)
        {
          msgLog = funcName + " reason=\"Numero seriale UCB non configurato\""
                 + ", SerialNumber=\"" + numStr + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        }
        else
        {
          msgLog = funcName + " reason=\"Numero seriale UCB configurato correttamente\""
                 + ", SerialNumber=\"" + numStr + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        }
      }
      return serialNum;
    }

  }
}
