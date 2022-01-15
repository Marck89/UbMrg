using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Record Contatori (Invio Stato Operativo) */
  internal class CdxMsgCounterT
  {
    internal UInt32 m_Valore;

    internal CdxMsgCounterT()
    {
      Clear();
    }

    internal void Clear()
    {
      m_Valore = 0;
    }
  }
}
