using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal class CdxMsgSendStatusT
  {
     /* Invio Stato Operativo (coda STATUS) */
    internal CdxMsgHeaderT	m_Header;
		internal byte	 m_Versione_Parametri;
    internal byte m_Numero_Dispositivi;
  	internal CdxMsgEquipmentStatusT[]		m_Dispositivo;
														/* NB: Il numero di elementi validi e' definito da 'Numero_Dispositivi' */

    internal CdxMsgSendStatusT()
    {
      m_Header = new CdxMsgHeaderT();
      m_Dispositivo = new CdxMsgEquipmentStatusT[CdxMsg.CDXMSG_MAXOBJ];
      for (int i = 0; i < m_Dispositivo.Length; i++)
      {
        m_Dispositivo[i] = new CdxMsgEquipmentStatusT();
      }

      Clear();
    }

    internal void Clear()
    {
      m_Header.Clear();
      m_Versione_Parametri = 0;
      m_Numero_Dispositivi = 0;
      for ( int i = 0; i < m_Dispositivo.Length; i ++)
      {
        m_Dispositivo[i].Clear();
      }
    }

    internal void Init(ushort SerialNum)
    {
      m_Numero_Dispositivi = 1;
      m_Dispositivo[0].Init(SerialNum);
    }

    internal void DiagInitStates()
    {
      m_Dispositivo[0].DiagInitStates();
    }
  }
}
