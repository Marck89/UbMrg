using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Allarmi Applicativi da spedire al QB */
  enum AlarmsQB
  {
    DATA_NON_VALIDA   = 0x00000001,
    ID_VEICOLO_ERRATO = 0x00000002
  }

  class Alarms
  {
    /* Questa maschera di BIT indica quali allarmi (da inviare al QB) sono attivi */
    internal uint m_AllarmiAttiviVersoQB = 0x00000000;

    /* Elenco allarmi rilevati da UCB che vanno segnalati al CDD */
    internal bool m_AllarmeMacAddressVariato = false;
    internal bool m_AllarmePerditaOrario = false;
    internal bool m_AllarmePartenzaAnomala = false;
    internal bool m_AllarmeDisallineamentoOrarioGps = false;
    internal bool m_AllarmeCnvConServizioAnomalo = false;
    internal bool m_AllarmeCnvConTipoErrato = false;
    internal bool m_AllarmeCnvSenzaSoftware = false;
    internal bool m_AllarmeCnvConProblemiDiConnessione = false;


     /*  Gestione allarmi Applicativi verso il QB */
    internal void AttivaAllarmeVersoQB(AlarmsQB ID_Allarme, bool Stato)
    {
      if (Stato == true)
      {
        m_AllarmiAttiviVersoQB |= (uint)ID_Allarme; // Attivazione allarme
      }
      else
      {
        m_AllarmiAttiviVersoQB &= (uint)(~ID_Allarme); // Disattivazione allarme
      }
    }

  }
}
