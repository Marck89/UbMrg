using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Record Allarmi di Dettaglio (Invio Stato Operativo) */
  internal class CdxMsgDetAlmT
  {
    internal byte m_Tipo_Modulo;
    internal byte m_Sottotipo_Modulo;
    internal UInt32 m_Codice;

    internal CdxMsgDetAlmT()
    {
      Clear();
    }

    internal void Clear()
    {
      m_Tipo_Modulo = 0;
      m_Sottotipo_Modulo = 0;
      m_Codice = 0;
    }
  }
}
