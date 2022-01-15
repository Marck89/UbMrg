using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Return codes */
  enum UcbRcCode
  {
    UCB_RC_OK = 0,	/* Successo */
    UCB_RC_HW_FAILURE = 1,	/* Fallimento per un errore HW */
    UCB_RC_OS_FAILURE = 2,	/* Fallimento per errore dovuto a una chiamata di sistema */
    UCB_RC_BAD_ARGUMENT = 3,	/* I parametri passati alla funzione non sono validi */
    UCB_RC_INTERNAL_ERR = 4,	/* Errore interno alla libreria */
    UCB_RC_INVALID_OP = 5,	/* La funzione non può essere eseguita in questo contesto */
    UCB_RC_SERIAL_READTIMEOUT = 6, /* Timeout nella lettura di dati da seriale */
  }


  /* Macro relative ai PIN */
  enum PinState
  {
    UCB_KEY_STATUS = 0,	/* Contatto azionato dall'autista */
    UCB_RELE_UCB = 1,	/* Contatto per accensione/spegnimento UCB */
    UCB_RELE_CNV = 2,	/* Contatto per accensione/spegnimento */
  }

  /* convalidatrici  Stato  del contatto */
  enum ContattoStato
  {
    UCB_OPEN = 0,	/* contatto aperto */
    UCB_CLOSE = 1,	/* contatto chiuso */
  }

  internal class BusInterface
  {

    internal static WLanCode ApplRegister(WLanOpCode OpCode, WLanIdUser IdUser)
    {
      return WLanCode.WLD_OK;
    }
 
    internal static UcbRcCode ucb_init()
    {
      String funcName = "ucb_init()";
      String msgLog = "";

      UcbRcCode code = UcbRcCode.UCB_RC_OK;
      if (code != UcbRcCode.UCB_RC_OK)
      {
        msgLog = funcName + " FAILED reason=\"Errore nell'inizializzazione della libreria libucb\", rc=-1";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      return code;
    }

    internal static UcbRcCode ucb_end()
    {
      UcbRcCode code = UcbRcCode.UCB_RC_OK;
      return code;
    }

    internal static UcbRcCode ucb_setstatus_elsag(PinState id, ContattoStato value)
    {

      // XXXXXXXXXXXXX DA FARE
      UcbRcCode code = UcbRcCode.UCB_RC_OK;
      return code;
    }

  }
}
