using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sbme
{
  internal class Porting
  {
    internal static int GETPID()
    {
      Process currentProcess = Process.GetCurrentProcess();
      if ( currentProcess == null ) return 0;
      return currentProcess.Id % 65535;
    }

    internal static int THREAD_SELF()
    {
      return Thread.CurrentThread.ManagedThreadId;
    }

    internal static int GetErrNo()
    {
      return 0;
    }

    internal static String GetErrNoStr()
    {
      return GetErrNo().ToString();
    }

    internal static bool UCB_CreateThread(ThreadId ThrId, ThreadStart ThrStart, ThreadPriority Priority)
    {
      bool rst = false;
      try
      {
        Thread thr = new Thread(ThrStart);
        Globals.m_ListaThread.Set(ThrId, thr);
        if ( thr != null )
        {
          thr.Priority = Priority;
          thr.Start();
          rst = true;
        }
      }
      catch (Exception)
      {
        rst = false;
      }

      if (rst == false)
      {
        String msgLog = "Start()";
        msgLog += " FAILED reason=\"Fallita creazione del thread\", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      return rst;
    }

    internal static bool UCB_InstanceAlreadyRunning(String UbPidStr)
    {
      int pid = -1;
      int.TryParse( UbPidStr, out pid);
      if ( pid <= 0 ) return false;

      Process process = null;
      try
      {
        process = Process.GetProcessById(pid);
      }
      catch (Exception )
      {
      }

      bool rst = false;
      if ( process != null )
      {
        String funcName = "UCB_InstanceAlreadyRunning()";
        String msgLog = "";
        msgLog = funcName + " reason=\"Processo SBME gia' in esecuzione\""
               + ", OldPid=\"" + pid.ToString() + " %s\"";

        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
        rst = true; // Instance already running
      }
      return rst;
    }

  }
}
