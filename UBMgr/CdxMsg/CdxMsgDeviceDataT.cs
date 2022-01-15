using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Record Tipo Dispositivo con serial number */
  internal class CdxMsgDeviceDataT
  {
    internal SBME_DeviceType m_Tipo;
    internal byte m_Sottotipo;
    internal UInt16 m_Serial_Number;
    internal byte m_Versione_Software_Major;
    internal byte m_Versione_Software_Minor;

    internal CdxMsgDeviceDataT()
    {
      Clear();
    }

    internal void Clear()
    {
      m_Tipo = 0;
      m_Sottotipo = 0;
      m_Serial_Number = 0;
      m_Versione_Software_Major = 0;
      m_Versione_Software_Minor = 0;
    }

    internal void Init(ushort SerialNum)
    {
      m_Tipo = SBME_DeviceType.SBME_TYPE_UCB;
      m_Sottotipo = 1;
      m_Versione_Software_Major = VersionSw.VERSIONE_MAJOR;
      m_Versione_Software_Minor = VersionSw.VERSIONE_MINOR;
      m_Serial_Number = SerialNum;
    }
  }
}
