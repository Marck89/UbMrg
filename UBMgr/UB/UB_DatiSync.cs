using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal  class UB_DatiSync
  {
    /* Questa e' una copia della prima parte della struttura XDATISYNC ricevuta dal CD.
       Viene popolata ogni volta che ricevo un sincronismo dal CD. L'ho svincolata dalla
       seconda parte (quella sui dispositivi) perche' quest'ultima e' utilizzata solo in
       coppia ad uno scaricamento */
    internal ushort m_PeriodoInvioStato;
    internal ushort m_PeriodoSync;
    internal byte   m_ScaricoAttivita;
    internal ushort m_TipoConcentratore;
    internal byte   m_IdVettore;
    internal ushort m_IdLinea;
    internal byte   m_IdTrasporto;
    internal ushort m_IdLocalita;

    internal UB_DatiSync()
    {
      Clear();
    }

    internal void Clear()
    {
      m_PeriodoInvioStato = 0;
      m_PeriodoSync = 0;
      m_ScaricoAttivita = 0;
      m_TipoConcentratore = 0;
      m_IdVettore = 0;
      m_IdLinea = 0;
      m_IdTrasporto = 0;
      m_IdLocalita = 0;
    }
  }
}
