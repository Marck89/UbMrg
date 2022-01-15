using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Stato operativo delle CNV da passare all'UBmanager	*/
  internal enum CNV_Oper
  {
    CNV_MANCANTE = 0,	/* CNV non risponde - 27/01/2004										*/
    CNV_OFF,			/* Stato iniziale e in aggiornamento SW/NFP (CNV_IN_CONFIG)				*/
    CNV_IDENTIFICATA,	/* CNV identificate con SW da aggiornare								*/
    CNV_SVUOTO,			/* CNV in condizione di flush delle attivita` a seguito di una chiusura	*/
    CNV_CHIUSA,			/* CNV identificate ed aggiornate in attesa di apertura servizio		*/
    CNV_APERTA,			/* CNV in servizio (nominale o degradato)								*/
  };

  /* Stato diagnostico delle CNV da passare all'UBmanager */
  internal enum CNV_Diag
  {
    CNV_GUASTA = 0,	/* allarme "fatale" dalla CNV	*/
    CNV_FERMA,		/* Stato iniziale				*/
    CNV_IN_CONFIG,	/* In configurazione			*/
    CNV_OPERATIVA,	/* Abilitata al servizio		*/
  };

  internal enum CNV_SwType
  {
    CNV_SW_TYPE_SCONOSCIUTO = 0,
    CNV_SW_TYPE_BOOT_LOADER,
    CNV_SW_TYPE_SOFTWARE_CNV,
  };

  internal enum CNV_StatoContatto
  {
    CNV_STATO_CONTATTO_SCONOSCIUTO = -1,
    CNV_STATO_CONTATTO_NON_RAGGIUNGIBILE,
    CNV_STATO_CONTATTO_RAGGIUNGIBILE,
  };

  internal enum CNV_StatoServizio
  {
    CNV_STATO_SERVIZIO_SCONOSCIUTO = -1,
    CNV_STATO_SERVIZIO_CHIUSO,
    CNV_STATO_SERVIZIO_APERTO_NORMALE,
    CNV_STATO_SERVIZIO_APERTO_DEGRADATO,
  };

  internal class Cnvs
  {
    /* Numero massimo di Thread che gestiscono le CNV */
    internal const int MAX_THR_CNV = 2;

    /* Numero massimo di CNV per thread */
    internal const int MAX_CNV = 6;

    private StatoCnv[] m_Stato = null;/* Stato corrente CNV */
    private StatoCnv[] m_StatoPrec = null;		/* Stato precedente CNV */
    private AllarmiCnv[] m_AllarmiAttivi = null;		/* Allarmi attivi sulle CNV */

    internal Cnvs()
    {
      m_Stato = new StatoCnv[MAX_CNV];
      m_StatoPrec = new StatoCnv[MAX_CNV];
      m_AllarmiAttivi = new AllarmiCnv[MAX_CNV];

      for (int i = 0; i < MAX_CNV; i++)
      {
        m_Stato[i] = new StatoCnv();
        m_StatoPrec[i] = new StatoCnv();
        m_AllarmiAttivi[i] = new AllarmiCnv();
      }
    }

    internal void ClearStato()
    {
      for (int i = 0; i < MAX_CNV; i++)
      {
        m_Stato[i].Clear();
      }
    }

    internal void ClearStatoPrec()
    {
      for (int i = 0; i < MAX_CNV; i++)
      {
        m_StatoPrec[i].Clear();
      }
    }

    internal void MGR_InitCnvStatus( bool PrimaInizializzazione )
    {
      int timeNow = UCB.UCB_GetSbmeTime();

      for (int i = 0; i < MAX_CNV; i++)
      {
        m_Stato[i].Init(PrimaInizializzazione, timeNow);
        m_StatoPrec[i].Clear();
        m_AllarmiAttivi[i].Clear();
      }
    }


    internal static String StatoContattoToStr(CNV_StatoContatto StatoContatto)
    {
      String str = "";
      switch (StatoContatto)
      {
        case CNV_StatoContatto.CNV_STATO_CONTATTO_SCONOSCIUTO:
          str = "Sconosciuto";
          break;

        case CNV_StatoContatto.CNV_STATO_CONTATTO_NON_RAGGIUNGIBILE:
          str = "Non raggiungibile";
          break;

        case CNV_StatoContatto.CNV_STATO_CONTATTO_RAGGIUNGIBILE:
          str = "Raggiungibile";
          break;

        default:
          str = "??? Non in elenco ???";
          break;
      }
      return str;
    }

    internal static String StatoServizioToStr(CNV_StatoServizio StatoServizio)
    {
      String str = "";
      switch (StatoServizio)
      {
        case CNV_StatoServizio.CNV_STATO_SERVIZIO_SCONOSCIUTO:
          str = "Sconosciuto";
          break;

        case CNV_StatoServizio.CNV_STATO_SERVIZIO_CHIUSO:
          str = "Chiuso";
          break;

        case CNV_StatoServizio.CNV_STATO_SERVIZIO_APERTO_NORMALE:
          str = "Aperto normale";
          break;

        case CNV_StatoServizio.CNV_STATO_SERVIZIO_APERTO_DEGRADATO:
          str = "Aperto degradato";
          break;

        default:
          str = "??? Non in elenco ???";
          break;
      }
      return str;
    }

  }
}
