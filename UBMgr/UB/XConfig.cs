using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
// Descrizione configurazione del sistema
  internal class XConfig
  {
    internal byte m_Versione_Parametri;
    internal byte m_Numero_Tipi;
    internal CdxMsgTypeDescT[] m_Tipo;	// NB: Il numero di tipi validi 												// e' definito da 'Numero_Tipi'

    internal XConfig()
    {
      m_Tipo = new CdxMsgTypeDescT[CdxMsg.CDXMSG_MAXTYP];
      for ( int i = 0; i < m_Tipo.Length; i++ )
      {
        m_Tipo[i] = new CdxMsgTypeDescT();
      }
      Clear();
    }

    internal void Clear()
    {
      m_Versione_Parametri = 0;
      m_Numero_Tipi = 0;
      for (int i = 0; i < m_Tipo.Length; i++)
      {
        m_Tipo[i].Clear();
      }
    }

    internal void Init()
    {
      m_Numero_Tipi = 1;
      m_Tipo[0].Init();
    }
  }
}
