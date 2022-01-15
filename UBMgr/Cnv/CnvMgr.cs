using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  class CnvMgr
  {
    internal bool m_Running = false;

    internal Mutex m_StatusMutex = new Mutex();
    internal Mutex m_CommandMutex = new Mutex();

    internal CnvMgr()
    {
    }

    /* Inizializzazione delle strutture per i thread CNV */
    internal void Init()
    {
      String funcName = "Init()";
      String msgLog;

        /* Inizializzo le variabili per l'accesso esclusivo ai dati delle CNV */
      m_StatusMutex.CreateMutex();
      m_CommandMutex.CreateMutex();
      m_Running = false;

      msgLog = funcName + " reason=\"Mutex gestione CNV creati\". Running=FALSE";
      LogTrace.Write(LogType.LOG_CNV, Severity.LOG_INFO, msgLog);
    }

    internal bool Start()
    {
      String funcName = "Start()";
      String msgLog;

      if (m_Running == true)
      {
        msgLog = funcName + " reason=\"Thread gestione QB trovato gia' avviato\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        return true;
      }

      msgLog = funcName + " reason=\"Avvio thread gestione QB\"";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_DEBUG, msgLog);

      bool rst = Porting.UCB_CreateThread(ThreadId.THREAD_OMI, new ThreadStart(Main), ThreadPriority.Normal);

      if (rst == false)
      {
        msgLog = funcName + " reason=\"Fallita la creazione del thread di gestione QB\""
              + ", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      return rst;
    }

    internal void Main()
    {
      // XXXXXX DA FARE
      // UCB_CreateThread(&gvListaThreadHandle[THREAD_WDG], ResetWD_Thread, (void*)&ThresetExitCode);
    }
  }
}
