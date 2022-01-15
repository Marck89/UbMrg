using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal enum DiagState
  {
    NOTDEF = 0,
    NOCONF = 1,
    CONFIG = 2,
  }

  internal static class Diag
  {
    internal static int m_Time = 0;
    internal static short m_Mgr_VersionMinor = 0;
    internal static short m_Mgr_VersionMajor = 0;

    internal static short m_CnvE_VersionMinor = 0;
    internal static short m_CnvE_VersionMajor = 0;

    internal static short m_CnvME_VersionMinor = 0;
    internal static short m_CnvME_VersionMajor = 0;

    internal static short m_NFP_VersionMinor = 0;
    internal static short m_NFP_VersionMajor = 0;

    internal static DiagState m_Status = DiagState.NOTDEF;

    internal static Mutex m_Mutex = new Mutex();

    static Diag()
    {
    }

    internal static void Diag_Init(short VersionMajor, short VersionMinor)
    {
      String funzName = "Diag_Init()";
      String msgLog = "";
      msgLog = funzName + " reason=\"Inizializzazionione Diagnostica\""
            + ", Major=" + VersionMajor.ToString() + ", Minor =" + VersionMinor.ToString();
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      m_Mgr_VersionMajor = VersionMajor;
      m_Mgr_VersionMinor = VersionMinor;

      m_Mutex.CreateMutex();

      m_Time = TimeUtils.TempoInMillesimi();

      m_Status = DiagState.NOTDEF;
    }

    internal static void Diag_End()
    {
      m_Mutex.MutexLock();

      m_Status = DiagState.NOTDEF;

      m_Mutex.MutexUnlock();
    }
  }
}
