using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
    /* In UB_AVVIATA i messaggi dipendono dai dati che arrivano dal pannello RCC ("Attesa linea",...) */
  enum RccState
  {
    UB_NONE        = 0x00,
    UB_IN_AVVIO    = 0x01,	/* "In avvio"		*/
    UB_NO_CONFIG   = 0x02,	/* "No config."		*/
    UB_AVVIATA     = 0x03,	/* vari				*/
    UB_IN_CONFIG   = 0x04,	/* "In config."		*/
    UB_SPEGNIMENTO = 0x05,	/* "Spegnimento"	*/
    UB_WIRELESS_KO = 0x06,	/* "Wireless KO"	*/
  }

  /* Significato della "direzione" ricevuta da RCC */
  enum RccDirectionCode
  {
    RCC_DIR_ANDATA    = 0,	/* Direzione di "andata" */
    RCC_DIR_RITORNO   = 1,	/* Direzione di "ritorno" */
    RCC_DIR_DIAG      = 4,	/* Per la diagnostica */
    RCC_DIR_ECO_ON    = 5,	/* Abilita giornata ecologica */
    RCC_DIR_ECO_OFF   = 6,	/* Disabilita giornata ecologica (solo se impostata da autista) */
    RCC_DIR_STOP_CNV  = 7,	/* Blocco convalide */
    RCC_DIR_DEFAULT   = 9,	/* Direzione usata nei depositi e servizi urbani */
  }

  internal class RccMgr
  {
    internal bool m_Running = false;
    internal RccState m_StatoUBaRCC = RccState.UB_NONE;

    internal bool Start()
    {
      String funcName = "Start()";
      String msgLog;

      if ( m_Running == true ) 
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

    /* Usata dal manager per informare l'RCC del suo stato. Non e'
       necessario bloccare perche' l'RCC lo usa in sola lettura */
    internal void SetStato(RccState StatoUB)
    {
      m_StatoUBaRCC = StatoUB;
    }

    internal void RccSvr_End()
    {
      /// XXXXX DA FARE
      throw new NotImplementedException();
    }
  }
}
