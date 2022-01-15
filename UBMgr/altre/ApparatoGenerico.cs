using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /*  Stati Apparato Generici  */
  internal enum UCB_StatoApparatoGenerico
  {
    UCB_STATO_APPARATO_SCONOSCIUTO = 0,		/* 0 */
    UCB_STATO_APPARATO_NON_DISPONIBILE,		/* 1 */
    UCB_STATO_APPARATO_DISPONIBILE,			  /* 2 */
    UCB_STATO_APPARATO_FINE_MONITORAGGIO,	/* 3 */
  };

  internal enum UCB_IdApparatoGenerico
  {
    UCB_ID_APPARATO_QUADRO_BORDO,			 /* 0 */
    UCB_ID_APPARATO_CANALE_SPAM,			 /* 1 */
    UCB_ID_APPARATO_LOCALIZZAZIONE,		 /* 2 */
    UCB_ID_APPARATO_GPS,					     /* 3 */
    UCB_ID_APPARATO_ULTIMO_APPARATO,	 /* Lasciare questa macro sempre per ultima */
  };



  internal class StatoApparatoGenerico
  {
    /* Stato per apparati generici */

    internal uint m_IdApparatoGenerico;						  	/* Dice che tipo di apparatio sto monitorizzando */
    internal uint m_StatoCorrente;							     	/* (Sconosciuto,Disponibile,NonDisponibile) */
    internal uint m_NumeroCambiStato;						    	/* Mi dice quante volte ha cambiato stato */
    internal long m_IstanteUltimoCambioStato;					/* Data/ora */
    internal uint m_DurataTotaleTempoSconosciutoSec;			/* In secondi */
    internal uint m_DurataTotaleTempoDisponibileSec;			/* In secondi */
    internal uint m_DurataTotaleTempoNonDisponibileSec;			/* In secondi */
    internal uint m_DurataParametroProlungatoSec;				/* Durata in secondi per il periodo di disponibilità prolungata */
    internal uint m_NumeroNonDisponibiliProlungati;				/* "NonDisponibili Prolungati": Se rimane NonDisponibile per più di 180 sec */
    internal bool m_AttualmenteInNonDisponibileProlungato;		/* Dice se adesso è in "NonDisponibile prolungato" (180 sec) */
    internal uint m_DurataParametroBreveSec;					/* Durata in secondi per il periodo di disponibilità brave */
    internal uint m_NumeroNonDisponibiliBrevi;					/* "NonDisponibili brevi": Se rimane NonDisponibili per più di 90 sec */
    internal bool m_AttualmenteInNonDisponibileBreve;			/* Dice se adesso è in "NonDisponibile breve" (90 sec) */

    internal StatoApparatoGenerico()
    {
      Clear();
    }

    internal void Clear()
    {
      m_IdApparatoGenerico = 0;
      m_StatoCorrente = 0;
      m_NumeroCambiStato = 0;
      m_IstanteUltimoCambioStato = 0;
      m_DurataTotaleTempoSconosciutoSec = 0;
      m_DurataTotaleTempoDisponibileSec = 0;
      m_DurataTotaleTempoNonDisponibileSec = 0;
      m_DurataParametroProlungatoSec = 0;
      m_NumeroNonDisponibiliProlungati = 0;
      m_AttualmenteInNonDisponibileProlungato = false;
      m_DurataParametroBreveSec = 0;
      m_NumeroNonDisponibiliBrevi = 0;
      m_AttualmenteInNonDisponibileBreve = false;
    }

    internal void Init(int Id)
    {
      m_IdApparatoGenerico = (uint)Id;
      m_IstanteUltimoCambioStato = Globals.m_StatoUcb.m_DataOraInizioMisurazione;

      switch (Id)
      {
        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_QUADRO_BORDO:
          m_DurataParametroBreveSec = 60;			/* Un minuto */
          m_DurataParametroProlungatoSec = 600;		/* Dopo 10 minuti la CNV vanno in degradato (Richiesta Bedogna 12 Lug 2007) */
          break;

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_GPS:
          m_DurataParametroBreveSec = 60;			/* 60 Sec */
          m_DurataParametroProlungatoSec = 300;		/* Cinque minuti */
          break;

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_LOCALIZZAZIONE:
          m_DurataParametroBreveSec = 25;			/* 25 Sec */
          m_DurataParametroProlungatoSec = 300;		/* Cinque minuti */
          break;

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_CANALE_SPAM:
          m_DurataParametroBreveSec = 25;			/* 25 Sec */
          m_DurataParametroProlungatoSec = 300;		/* Cinque minuti */
          break;

        default:
          m_DurataParametroBreveSec = 60;			/* Un minuto */
          m_DurataParametroProlungatoSec = 300;		/* Cinque minuti */
          break;
      }
    }

    internal static String MGR_StatoApparatoToStr(int StatoCorrente)
    {
      switch (StatoCorrente)
      {
        case (int)UCB_StatoApparatoGenerico.UCB_STATO_APPARATO_SCONOSCIUTO:
          return ("Sconosciuto");

        case (int)UCB_StatoApparatoGenerico.UCB_STATO_APPARATO_NON_DISPONIBILE:
          return ("Non disponibile");

        case (int)UCB_StatoApparatoGenerico.UCB_STATO_APPARATO_DISPONIBILE:
          return ("Disponibile");

        case (int)UCB_StatoApparatoGenerico.UCB_STATO_APPARATO_FINE_MONITORAGGIO:
          return ("Fine monitoraggio");

        default:
          return ("Errore");
      }
    }



    internal static String MGR_IdStatoApparatoToStr( int IdApparato)
    {
      switch (IdApparato)
      {
        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_QUADRO_BORDO:
          return ("Contatto Quadro di Bordo");

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_CANALE_SPAM:
          return ("Contatto con SPAM");

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_LOCALIZZAZIONE:
          return ("Localizzazione Inerziale Veicolo");

        case (int)UCB_IdApparatoGenerico.UCB_ID_APPARATO_GPS:
          return ("Localizzazione GPS");

        default:
          return ("Errore");
      }
    } 
  }


  internal class ApparatiGenerico
  {
    internal const int MAX_APPARATI_GENERICI = 16;

    internal StatoApparatoGenerico[] m_Stato = null;

    internal ApparatiGenerico()
    {
      m_Stato = new StatoApparatoGenerico[MAX_APPARATI_GENERICI];

      for (int i = 0; i < MAX_APPARATI_GENERICI; i++)
      {
        m_Stato[i] = new StatoApparatoGenerico();
      }
    }

    internal void Clear()
    {
      for (int i = 0; i < MAX_APPARATI_GENERICI; i++)
      {
        m_Stato[i].Clear();
      }
    }

    internal void Init()
    {
      Clear();

      for (int i = 0; i < MAX_APPARATI_GENERICI; i++)
      {
        m_Stato[i].Init(i);
      }

    }
  }

}
