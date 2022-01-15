using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal static class Globals
  {
    /* impostabile da manutentore tramite oppurtuno codice diagnostico da QB */
    internal static Severity m_SeverityThreashold = Severity.LOG_NOTICE;

    /* Abilitazione del dump dl protocollo su seriale CNV */
    internal static bool m_Stp403DumpProtocol;
    internal static int m_Stp403CnvAddress;
    internal static int m_Stp403Parity;
    internal static bool m_DumpGpsData;

    internal static bool m_RS485UseDriverRTS; /* faccio un reset dell'RTS via software */

    /* Questa variabile serve per bloccare il Thread delle CNV 
     * (lasciarlo inattivo) in modo tale da non mettere in servizio le CNV */
    internal static bool m_BloccoThreadCNV;

    /* Causa l'errore sull'invio del file "FAMILLE.BIN" */
    internal static bool m_ErronOnFamilleBin;

    /* Contatore minuti di uptime (per manutenzione) */
    internal static int m_ContatoreMinutiUptime;

    /* Contatore in minuti (run-time) da avvio UCB (BOOT) */
    internal static int m_ContatoreMinutiRuntime;

    /* Data-ora (start-time) da startup UCB (Avvio o riavvio) */
    internal static long m_StartupTime;

    /* Pid associato al Thread principale usato per fare i Log */
    internal static int m_MainThreadPid;

    /* UCB Application name */
    internal static String m_ApplicationName; /* Modificata in fase di avvio */

    /* Ultima linea con la quale è stato aperto il servizio alle CNV (richiesta apertura) */
    internal static int m_CnvLastValidLine;

    /* Stato Applicativo necessario per piattaforma AVM */
    internal static SbmeStatus m_SbmeApplStatus;

    internal static Alarms m_Alarms;
    internal static QB m_QB;

    internal static Cnvs m_Cnvs;
    internal static ApparatiGenerico m_ApparatiGenerico;

    internal static StatoUCB m_StatoUcb;

    /* Dice una linea è autolocalizzata */
    internal static bool	m_LineaAutolocalizzata;		
    /* Dice se l'autolocalizzazione è stata attivata manualmente */
    internal static bool	m_UsaAutolocalizzazioneManuale;	

    /* Variabile globale con la stringa del LAC (GPS) 
     * ricevuta da SPAM ogni secondo */
    internal static String m_GpsFromSpam;

    /* Per informare il main della localita` dal GPS */
    /* Località dedotta dal sistema di autolocalizzazione */
    internal static int m_LocalityFromGPS;

    internal static ListaThreads m_ListaThread;


    /* Differenza in secondi fra le date del CDX e della UCB (variabile globale) */
    internal static int m_UcbDeltaTime;

    static Globals()
    {
      m_SeverityThreashold = Severity.LOG_NOTICE;

      m_Stp403DumpProtocol = false;
      m_Stp403CnvAddress = 0;
      m_Stp403Parity = 0;
      m_DumpGpsData = false;

      m_RS485UseDriverRTS = false;

      m_BloccoThreadCNV = false;

      m_ErronOnFamilleBin = false;

      m_ContatoreMinutiUptime = 0;

      m_ContatoreMinutiRuntime = 0;

      m_StartupTime = 0;

      m_MainThreadPid = 0;

      m_ApplicationName = "UCB_XXXX"; /* Modificata in fase di avvio */

      m_CnvLastValidLine = Sbme.SBME_UNKNOWN_ROUTE;

      m_SbmeApplStatus = SbmeStatus.STARTING;

      m_Alarms = new Alarms();
      m_QB = new QB();

      m_Cnvs = new Cnvs();
      m_ApparatiGenerico = new ApparatiGenerico();

      m_StatoUcb = new StatoUCB();

      m_LineaAutolocalizzata = false;
      m_UsaAutolocalizzazioneManuale = false;

      m_GpsFromSpam = "";

      m_LocalityFromGPS = 0;

      m_ListaThread = new ListaThreads();

      m_UcbDeltaTime = 0;

    }

    internal static void Init()
    {
      /* Dati statistici CNV */
      Globals.m_Cnvs.ClearStato();
      Globals.m_Cnvs.ClearStatoPrec();

      /* Lista dei Thread Handle */
      Globals.m_ListaThread.Clear();

      Globals.m_ContatoreMinutiRuntime = TimeUtils.TempoInMinuti(); /* Minuti dal boot dell'UCB */

      /* Usato per fare i Log altrimenti Linux mi da' un Pid diverso per ogni Thread */
      Globals.m_MainThreadPid = Porting.GETPID();

      /* Creo un Application name da usare per i Log */
      Globals.m_ApplicationName = "UCB_" + VersionSw.VERSIONE_MAJOR.ToString("00")
                                         + VersionSw.VERSIONE_MINOR.ToString("00");

      Globals.m_GpsFromSpam = ""; 
    }
  }
}
