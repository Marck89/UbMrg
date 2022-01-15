using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Identificativo apparato */
  internal class CdxMsgDeviceIdT
  {
    internal byte m_Id_Operatore;
    internal byte m_Id_Classe;
    internal UInt16 m_Id_Numerico;

    internal CdxMsgDeviceIdT()
    {
      Clear();
    }

    internal void Clear()
    {
      m_Id_Operatore = 0;
      m_Id_Classe = 0;
      m_Id_Numerico = 0;
    }
  }
}
