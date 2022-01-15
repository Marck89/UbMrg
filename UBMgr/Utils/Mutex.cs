using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  internal class Mutex
  {
    Semaphore m_Semaphore = null;

    internal bool CreateMutex()
    {
      if (m_Semaphore != null) return true;
      m_Semaphore = new Semaphore(1, 1);
   	  return true;
    }

    internal bool DestroyMutex()
    {
      m_Semaphore = null;
      return true;
    }

    internal bool MutexLock()
    {
      return MutexLock(-1);
    }

    internal bool MutexLock(int milliSecondsTimeout )
    {
      if (m_Semaphore == null) return false;
      return m_Semaphore.WaitOne(milliSecondsTimeout);
    }

    internal bool MutexUnlock()
    {
      if (m_Semaphore == null) return false;
      m_Semaphore.Release();
    	return true;
    }
  }
}
