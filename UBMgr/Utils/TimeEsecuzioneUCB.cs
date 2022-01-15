using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Per il calcolo del tempo di esecuzione */
  enum CmdTimer
  {
      START_TIME = 0,
      STOP_TIME = 1,
      STOP_ERR_TIME = 2,
      GET_TIME = 3
  }

  class TimeEsecuzioneUCB
  {
    static int m_StartTime;

    static TimeEsecuzioneUCB()
    {
      m_StartTime = 0;
    }

    /* Scrive un log con il tempo di esecuzione. Chiamata in uscita (anche su errore) dal manager */
    internal static int TimerEsecuzioneUCB(CmdTimer ComandoTimer)
    {
      String msgLog = "";
      String funcName = "TimerEsecuzioneUCB()";

      int stopTime = TimeUtils.TempoInCentesimi();

      if (ComandoTimer == CmdTimer.START_TIME)
      {
        m_StartTime = stopTime;	/* Salvo l'ora di start */

        msgLog = funcName + " reason=\"Avvio timer funzionamento UCB\""
               + ", TempoEsecuzione=\"00:00:00\", TotMinuti=00";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

        return 0;
      }

      /* Calcolo il tempo di esecuzione */
      int totSecondi = (stopTime - m_StartTime) / 100;
      int totMinuti = totSecondi / 60;

      String timeStr = TimeUtils.DecodeTime(totSecondi);
      String timeEsecuzione = "TempoEsecuzione=\"" + timeStr + "\", TotMinuti=" + totMinuti.ToString();

      if (ComandoTimer == CmdTimer.STOP_TIME)
      {
        msgLog = funcName + " reason=\"Arrestato timer funzionamento UCB\", " + timeEsecuzione;
        LogTrace.Write(0, Severity.LOG_NOTICE, msgLog);
      }
      else if (ComandoTimer == CmdTimer.STOP_ERR_TIME)
      {
        msgLog = funcName + " reason=\"Arrestato timer funzionamento UCB (crash)\", " + timeEsecuzione;
        LogTrace.Write(0, Severity.LOG_ERR, msgLog);
      }
      else
      {
        msgLog = funcName + " reason=\"Letto valore timer funzionamento UCB\", " + timeEsecuzione;
        LogTrace.Write(0, Severity.LOG_DEBUG, msgLog);
      }
      return (totSecondi); /* Secondi passati dall'ultima chiamata 'START_TIME' */
    }

  }
}
