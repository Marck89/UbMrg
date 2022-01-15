using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Dettaglio dello stato di funzionamento (ex stato diagnostico)    */
  /* Valori comunicati dagli apparati                                 */
  enum CdxStatoDiagnostico
  {
    CDXMSG_FUNZIONANTE = 0x0001,		/* Regolare					*/
    CDXMSG_DEGRADATO = 0x0002,		/* Avaria parziale          */
    CDXMSG_GUASTO = 0x0008,		/* Avaria totale            */
    CDXMSG_NON_RAGGIUNGIBILE = 0x0010,
  }

  /* Modalita' operativa */
  /* NB: Non e' detto che tutti gli apparati debbano inviare tutti gli stati.     */
  /* Vale quanto descritto "Specifica funzionale della Supervisione".	            */

  /* Comune a (quasi) tutti */
  enum CdxModalitaOperativa
  {
    CDXMSG_NORMALE = 0x0001,
    CDXMSG_MANUTENZIONE = 0x0002,
    CDXMSG_INIZIALIZZAZIONE = 0x0004,
  }

  /* Stato di funzionamento (ex stato operativo)                      */
  enum CdxStatoOperativo
  {
    CDXMSG_OPERATIVO = 0x0001,
    CDXMSG_FUORI_SERVIZIO = 0x0002,
  }

  class CdxMsg
  {
    internal const int CDXMSG_IDLEN = 16;
    internal const int CDXMSG_DEFSPARELEN = 16;
    internal const int CDXMSG_LENORAAPP = 6;
    internal const int CDXMSG_LENCDXDESCR = 50;

    internal const int CDXMSG_MAXCNT = 8;
    internal const int CDXMSG_MAXALM = 32;
    internal const int CDXMSG_MAXOBJ = 20;
    internal const int CDXMSG_MAXTYP = 40;


  }
}
