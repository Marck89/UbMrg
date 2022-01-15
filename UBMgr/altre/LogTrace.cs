using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Sbme
{
  /* Per i log su file */
  enum LogType
  {
    LOG_UB = 0,
    LOG_MGR = 2,
    LOG_CNV = 3,
    LOG_CDX = 4,
    LOG_RCC = 5,
    LOG_WLAN = 8,
    LOG_CNVOP = 10,
    LOG_UTIL = 14,		/* 08/02/2005 - Util (varie)	*/
    LOG_SUA95 = 15,		/* 25/01/2007 - Log sul protocollo delle CNV	*/
  }

  internal static class LogTrace
  {
#if !IS_WINDOWS
    internal const int MAX_LOG_FILE_SIZE = 	716800;	/* 700 Kbyte */
#else
    internal const int MAX_LOG_FILE_SIZE = 	1048576;	/* 1 Mbyte */
#endif

    internal static Mutex m_LogFileMutex = null;

    static LogTrace()
    {
      m_LogFileMutex = new Mutex();
      m_LogFileMutex.CreateMutex();
    }

    internal static int Write(LogType LogHandle, Severity _Severity, String LogMessage)
    {
      return Write(LogHandle, _Severity, LogMessage, 2);
    }

  internal static int Write(LogType LogHandle, Severity _Severity, String LogMessage,
                              int SkipFrames)
    {
      if ( (int)_Severity > (int)Globals.m_SeverityThreashold)
      {
        return 0;
      }

      StackFrame stFr = new StackFrame(SkipFrames, true);
      String sourceName = stFr.GetFileName();
      int lineNumber  = stFr.GetFileLineNumber();
      if (sourceName == null) 
          sourceName = " No file";
 
      String shortSourceName = sourceName;
      char[] any = new char[] { '\\', '/' };
      int index = sourceName.LastIndexOfAny(any);
      if (index != -1) shortSourceName = sourceName.Substring(index + 1);

      String severityStr = SeverityUtils.GetString(_Severity);

      LoggingOnConsole(_Severity, severityStr, LogMessage);


      /*
      **  ============== Scrittura su file di LOG =====================================
      */

      m_LogFileMutex.MutexLock();

      int ret = ControlSize();
      if ( ret < 0 )
      {
        m_LogFileMutex.MutexUnlock();
  			return -1;
      }

      int TimeInSec = UCB.UCB_GetSbmeTime();
      DateTime dateTime = TimeUtils.ConvertFromUnixTimestamp(TimeInSec);

        /* costruzione della riga di log */
      String logString = "";
      if ( dateTime == DateTime.MinValue ) 
      {
        logString = "00/00 00:00:00";
	    }
      else  
      {
        logString = dateTime.ToString("MM/dd HH:mm:ss");
      }
      logString += " " + Globals.m_ApplicationName.PadRight(8, ' ')
                +  " " + severityStr 
                + " [" + Globals.m_MainThreadPid.ToString() 
                + "@" + shortSourceName + ":" + lineNumber.ToString() + "]"
                + " ("+ Porting.THREAD_SELF() + ")"
                + " " + LogMessage;

      ret = 0;
      StreamWriter sw = null;
      try 
      {
        sw = new StreamWriter(DirsNames.LOG_FILE_NAME, true);
      	/* Scrittura su file */
        sw.WriteLine( logString );
      }
      catch 
      {
        ret = -1;
      }
      if ( sw != null ) sw.Close();

  	  /* Unlock del file di log */
      m_LogFileMutex.MutexUnlock();

      return ret;

    }

    private static void LoggingOnConsole(Severity _Severity, String SeverityStr, String LogMessage)
    {
      if ((int)_Severity >= (int)Severity.LOG_INFO) return;

      String msg = "";
      switch (_Severity)
      {
        case Severity.LOG_EMERG:					/* 0 */
        case Severity.LOG_ALERT:					/* 1 */
        case Severity.LOG_CRIT:					  /* 2 */
        case Severity.LOG_ERR:					  /* 3 */
        case Severity.LOG_WARNING:				/* 4 */
          msg = "SBME[" + SeverityStr + "] " + LogMessage;
          ConsoleColor color = Console.ForegroundColor;
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(msg);
          Console.ForegroundColor = color;
          break;

        default:
          msg = "SBME[" + SeverityStr + "] " + LogMessage;
          Console.WriteLine(msg);
          break;
      }
    }

    private static int ControlSize()
    {
      int ret = 0;
      long size = FileUtils.GetFileSize(DirsNames.LOG_FILE_NAME);
      if (size > MAX_LOG_FILE_SIZE)
      {
        /* Swap log file */
        if (!FileUtils.UCB_MoveFile(DirsNames.LOG_FILE_NAME, 
                                    DirsNames.LOG_FILE_NAME_OLD))
        {
          ret = -1;
        }
      }
      return ret;
    }

  }
}
