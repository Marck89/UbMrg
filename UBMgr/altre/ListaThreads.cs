using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
     /* Definizione stato thread */
  enum ThreadState
  {
    THR_OK	         =	0x00,
    THR_RUNNING	     =	0x01,
    THR_IN_AVVIO	   =	0x02,
    THR_NOT_RUNNING  =	0x03,
    THR_CMD_IN_CORSO =	0x04	/* per il thread CNV (e CDD?) */
  }

  enum ThreadId 
  { 
    THREAD_CNV1	=	 0,
//    THREAD_CNV2	=	 1,
    THREAD_CDD	=	 2,
    THREAD_RCC	=	 3,
//    THREAD_PAN	=	 4,
//    THREAD_COM =	 5,
//    THREAD_DIG_IN	 = 6,
//    THREAD_DIG_OUT	 7,
    THREAD_LED		= 8,
    THREAD_WDG		= 9,
//    THREAD_DIAG_CNV =	10,
//    THREAD_DIAG_LNK =	11,
    THREAD_OMI =		12
  }


  internal class ListaThreads
  {
    private const int NUM_MAX_THREAD	= 13;
    private Thread[] m_Thread = new Thread[NUM_MAX_THREAD];		/* Vettore contenente l'id dei thread avviati */

    internal ListaThreads()
    {
      Clear();
    }

    internal void Clear()
    {
      for (int i = 0; i < NUM_MAX_THREAD; i++)
      {
        m_Thread[i] = null;
      }
    }

    internal void Set(ThreadId ThreadId, Thread Thr)
    {
      m_Thread[(int)ThreadId] = Thr;
    }

    internal void Clear(ThreadId ThreadId, Thread Thr)
    {
      m_Thread[(int)ThreadId] = null;
    }

  }
}
