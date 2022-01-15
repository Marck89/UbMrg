using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Header (generico per tutti i messaggi) */
  internal class CdxMsgHeaderT
  {
    internal CdxMsgDeviceIdT m_Identificatore;
    internal UInt16 m_Tipo_Messaggio;

    internal CdxMsgHeaderT()
    {
      m_Identificatore = new CdxMsgDeviceIdT();
      Clear();
    }

    internal void Clear()
    {
      m_Identificatore.Clear();
      m_Tipo_Messaggio = 0;
    }
  }
}
