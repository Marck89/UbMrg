using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{

  /* NOTA:
    Quando si creano stringhe contenenti nomi di directory, aggiungere *sempre* in coda
    il carattere '/'. Questo perche' quando tali nomi verranno utilizzati sara' implicito che
    tale carattere esista.

    Esempi:
    sprintf(dirSW, DIR_SW_CORRENTE"%03d_%03d/", CNV_Versione.tipo, CNV_Versione.sottotipo);
    CNVAPI_UpdateNfpCNV(idCNVSRV, CNVcount, DIR_SW_CORRENTE"NFP/");
  */

  class DirsNames
  {
#if !IS_WINDOWS
    internal const String DIR_TEMP = "/tmp/";
    internal const String DIR_HOME = "/home/sbme/";
    internal const String DIR_RAMDISK = DIR_HOME + "ramdisk/";
    internal const String DIR_DATA = DIR_HOME + "data/";
    internal const String DIR_BASE = DIR_HOME + "UBmgr/";
    internal const String DIR_SW_CORRENTE = "Current/";
    internal const String DIR_NFP_CORRENTI = "Current/NFP/";
    internal const String DIR_SW_NUOVO = "New/";
    internal const String DIR_ATTIVITA = "Attivita/";
    internal const String DIR_ATTIVITA_CORR = "Attivita/InCorso/";
    internal const String DIR_ATTIVITA_NUOVE = "Attivita/Nuove/";
    internal const String DIR_ATTIVITA_VECCHIE = "Attivita/Vecchie/";
    internal const String DIR_CONFIGURAZIONE = "Config/";
    internal const String DIR_CERTIFICATI = "Certificati/";
    internal const String DIR_TRIANGLEDATAFILE = DIR_BASE + DIR_NFP_CORRENTI;

    internal const String DIR_PID = DIR_RAMDISK + "Pid/";

#else
#if CLIENT
    internal const String DIR_TEMP = "/Temp/";
    internal const String DIR_HOME = "/FlashDisk/SBME/";
    internal const String DIR_RAMDISK = "/ramdisk/";
#else
    internal const String DIR_TEMP = "/Sbme/home/Temp/";
    internal const String DIR_HOME = "/Sbme/home/Sbme/";
    internal const String DIR_RAMDISK = "/Sbme/home/Sbme/ramdisk/";
#endif

    internal const String DIR_DATA = DIR_HOME + "data/";
    internal const String DIR_BASE = DIR_HOME + "UBmgr/";
    internal const String DIR_SW_CORRENTE = DIR_BASE + "Current/";
    internal const String DIR_NFP_CORRENTI = DIR_BASE + "Current/NFP/";
    internal const String DIR_SW_NUOVO = DIR_BASE + "New/";
    internal const String DIR_ATTIVITA = DIR_BASE + "Attivita/";
    internal const String DIR_ATTIVITA_CORR = DIR_BASE + "Attivita/InCorso/";
    internal const String DIR_ATTIVITA_NUOVE = DIR_BASE + "Attivita/Nuove/";
    internal const String DIR_ATTIVITA_VECCHIE = DIR_BASE + "Attivita/Vecchie/";
    internal const String DIR_CONFIGURAZIONE = DIR_BASE + "Config/";
    internal const String DIR_CERTIFICATI = DIR_BASE + "Certificati/";
    internal const String DIR_TRIANGLEDATAFILE = DIR_NFP_CORRENTI;

    internal const String DIR_PID = DIR_RAMDISK;
#endif

    internal const String WLAN_DIFFTIME_PATH = DIR_RAMDISK + "DiffTime"; /* File per notificare il cambio dell'ora */

    internal const String UCB_SERIAL_PATH = DIR_BASE + "Config/UBserial";

    internal const String UCB_LOG_RAMDISK_PATH = DIR_RAMDISK;
    internal const String UCB_LOG_DATA_PATH = DIR_DATA;

    internal const String UCB_UPGRADE_SHELL_SCRIPT = DIR_HOME + "upgrade";
    internal const String UCB_UPGRADETMP_SHELL_SCRIPT = DIR_HOME + "upgrade.tmp";

    internal const String UCB_SBME_UPGRADE_SCRIPT = DIR_DATA + "SbmeUpgrade.sh";

    internal const String UCB_CHECK_FS_SCRIPT = DIR_DATA + "FileSystemCheckOte.sh";
    internal const String UCB_CHECK_FS_SCRIPT_TMP = DIR_DATA + "FileSystemCheckOte.tmp";

    internal const String UCB_DIRTY_FLAG_FILENAME = UCB_LOG_DATA_PATH + "SBME_DF";

    internal const String UCB_CURRENT_MAC_ADDRESS = DIR_TEMP + "MAC-ADDR";
    internal const String UCB_LAST_MAC_ADDRESS = DIR_DATA + "LAST-MAC-ADDR";

    internal const String UCB_STAT_DAT_FILE_NAME = UCB_LOG_DATA_PATH + "cnv_statistics.dat";
    internal const String UCB_STAT_CSV_FILE_NAME = UCB_LOG_RAMDISK_PATH + "cnv_statistics.csv";

    internal const String UCB_FILE_NAME_AUTOLOC_DATA = DIR_TRIANGLEDATAFILE + "triangle.bin";
    internal const String UCB_FILE_NAME_AUTOLOC_PAR = DIR_TRIANGLEDATAFILE + "autoloc.bin";

    internal const String UCB_FILE_NAME_ECO_NFP = DIR_RAMDISK + "ecoNFP";
    internal const String UCB_FILE_NAME_ECO_RCC = DIR_RAMDISK + "ecoRCC";

    internal const String UCB_FILE_NAME_RIAVVIO_SW = DIR_CONFIGURAZIONE + "RiavvioSW";
    internal const String UCB_FILE_NAME_CONTATTO_DEP = DIR_CONFIGURAZIONE + "dataContattoDP";

    internal const String LOG_FILE_NAME = UCB_LOG_RAMDISK_PATH + "ucb_log.txt";
    internal const String LOG_FILE_NAME_OLD = UCB_LOG_DATA_PATH + "ucb_old_log.txt";

    internal const String FILE_ATTIVITA_MGR = DIR_ATTIVITA_CORR + "MGR_attivita.tmp";


    internal const String DOWNLOAD_FILE_LIST_NAME = "UCB_download_list.txt";


    /* Sul server remoto */
    internal const String DIR_FTP_SERVER = "/SBME/";



    /* Nomi file */
    internal const String NOME_FILE_IDENTIFICAZIONE = DIR_CONFIGURAZIONE + "fileUBid";
    internal const String NOME_FILE_NUMERO_CORSA = DIR_CONFIGURAZIONE + "numCorsa";
    internal const String NOME_FILE_PROGR_ATTIV_MGR = DIR_CONFIGURAZIONE + "progrAttMGR";
    internal const String NOME_FILE_VERS_CORR_UCB = DIR_RAMDISK + "MGRswFile";


  }
}
