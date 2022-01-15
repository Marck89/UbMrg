using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Record Invio Stato Operativo */
  class CdxMsgEquipmentStatusT
  {
    internal CdxMsgDeviceIdT m_Identificatore;
    internal CdxMsgDeviceDataT m_Dati_Dispositivo;
    internal CdxStatoOperativo m_Stato_Operativita;
    internal CdxStatoDiagnostico m_Stato_Diagnostico;
    internal CdxModalitaOperativa m_Modalita_Operativa;
    internal UInt16 m_Moduli;
    internal byte[] m_Reserved;      /* Riservato per uso futuro */


    internal byte m_Numero_Contatori;
    internal CdxMsgCounterT[] m_Contatore;  /* NB: Il numero di elementi validi e' definito da 'Numero_Contatori' */

    internal byte m_Numero_Allarmi;
    internal CdxMsgDetAlmT[] m_Allarme;     /* NB: Il numero di elementi validi e' definito da 'Numero_Allarmi' */

    internal CdxMsgEquipmentStatusT()
    {
      m_Identificatore = new CdxMsgDeviceIdT();
      m_Dati_Dispositivo = new CdxMsgDeviceDataT();

      m_Reserved = new byte[CdxMsg.CDXMSG_DEFSPARELEN-2];
      m_Contatore = new CdxMsgCounterT[CdxMsg.CDXMSG_MAXCNT];
      m_Allarme = new CdxMsgDetAlmT[CdxMsg.CDXMSG_MAXALM];


      for (int i = 0; i < m_Contatore.Length; i++)
      {
        m_Contatore[i] = new CdxMsgCounterT();
      }

      for (int i = 0; i < m_Allarme.Length; i++)
      {
        m_Allarme[i] = new CdxMsgDetAlmT();
      }


      Clear();
    }

    internal void Clear()
    {
      m_Identificatore.Clear();
      m_Dati_Dispositivo.Clear();

      m_Stato_Operativita = 0;
      m_Stato_Diagnostico = 0;
      m_Modalita_Operativa = 0;
      m_Moduli = 0;

      for (int i = 0; i < m_Reserved.Length; i++)
      {
        m_Reserved[i] = 0;
      }

      m_Numero_Contatori = 0;
      for (int i = 0; i < m_Contatore.Length; i++)
      {
        m_Contatore[i].Clear();
      }

      m_Numero_Allarmi = 0;
      for (int i = 0; i < m_Allarme.Length; i++)
      {
        m_Allarme[i].Clear();
      }
    }

    internal void Init(ushort SerialNum)
    {
      m_Dati_Dispositivo.m_Tipo = SBME_DeviceType.SBME_TYPE_UCB;
      m_Dati_Dispositivo.m_Sottotipo = 1;
      m_Dati_Dispositivo.m_Versione_Software_Major = VersionSw.VERSIONE_MAJOR;
      m_Dati_Dispositivo.m_Versione_Software_Minor = VersionSw.VERSIONE_MINOR;
      m_Dati_Dispositivo.m_Serial_Number = SerialNum;
    }

    internal void DiagInitStates()
    {
      /* Una UB non e` mai Fuori Servizio Al piu` vado in degradato se l'RCC non mi parla */
      m_Stato_Operativita = CdxStatoOperativo.CDXMSG_OPERATIVO;
      m_Stato_Diagnostico = CdxStatoDiagnostico.CDXMSG_FUNZIONANTE;
      m_Modalita_Operativa = CdxModalitaOperativa.CDXMSG_INIZIALIZZAZIONE;
    }
  }
}
