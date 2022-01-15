using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  internal class OmiMgr
  {
    internal bool m_Running = false;

    internal bool Start()
    {
      String funcName = "Start()";
      String msgLog;

      if (m_Running == true)
      {
        msgLog = funcName + " reason=\"Thread comunicazione con SPA trovato gia' avviato\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        return true;
      }

      msgLog = funcName + " reason=\"Avvio thread di comunicazione con SPAM\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      bool rst = Porting.UCB_CreateThread(ThreadId.THREAD_OMI, new ThreadStart(Main), ThreadPriority.Normal);

      if (rst == false)
      {
        msgLog = funcName + " Fallita reason=\"Fallita la creazione del thread di comunicazione con SPAM\""
               + ", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      else
      {
        msgLog = funcName + " reason=\"Thread di comunicazione con SPAM avviato\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);
      }


      return rst;
    }

    internal void Main()
    {
      // XXXXXX DA FARE
      // UCB_CreateThread(&gvListaThreadHandle[THREAD_WDG], ResetWD_Thread, (void*)&ThresetExitCode);
    }


    internal void OMI_Finalize()
    {
      // XXXXXX DA FARE
      throw new NotImplementedException();
    }
  }
}
