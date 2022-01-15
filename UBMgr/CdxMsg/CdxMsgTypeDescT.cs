using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Record Tipo Dispositivo */
  internal class CdxMsgTypeDescT
  {
    internal byte m_Tipo;
    internal byte m_Sottotipo;
    internal byte m_Versione_Software_Major;
    internal byte m_Versione_Software_Minor;
    internal byte[] m_Reserved; /* Riservato per uso futuro */


    internal CdxMsgTypeDescT()
    {
      m_Reserved = new byte[CdxMsg.CDXMSG_DEFSPARELEN];
      Clear();
    }

    internal void Clear()
    {
      m_Tipo = 0;
      m_Sottotipo = 0;
      m_Versione_Software_Major = 0;
      m_Versione_Software_Minor = 0;

      for (int i = 0; i < m_Reserved.Length; i++)
      {
        m_Reserved[i] = 0;
      }
    }

    internal void Init()
    {
      m_Tipo = (byte)SBME_DeviceType.SBME_TYPE_UCB;
      m_Sottotipo = 1;
      m_Versione_Software_Major = VersionSw.VERSIONE_MAJOR;
      m_Versione_Software_Minor = VersionSw.VERSIONE_MINOR;
    }
  }
}
