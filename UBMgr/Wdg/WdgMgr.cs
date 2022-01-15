using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  internal class WdgMgr
  {
    bool m_Running = false;

    internal bool Start()
    {
      String funcName = "Start()";
      String msgLog = "";

      if (m_Running == true)
      {
        msgLog = funcName + " reason=\"Thread gestione ResetWD trovato gia' avviato\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        return true;
      }

      msgLog = funcName + " reason=\"Lancio thread 'ResetWD'\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      bool rst = Porting.UCB_CreateThread(ThreadId.THREAD_WDG, new ThreadStart(Main), ThreadPriority.Normal);

      if (rst == false)
      {
        msgLog = funcName + " FAILED reason=\"Fallita creazione del thread 'ResetWD_Thread'\", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }

      return rst;
    }

    internal void Main()
    {
      while ( true)
      {
        Thread.Sleep(5000);
      }
      // XXXXXX DA FARE
      // UCB_CreateThread(&gvListaThreadHandle[THREAD_WDG], ResetWD_Thread, (void*)&ThresetExitCode);
    }

    }
}
