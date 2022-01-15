using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  /* Stati Applicativi per piattaforma AVM */
  internal enum SbmeStatus
  {
    STARTING = 0x01,
    STARTED = 0x02,
    OPERATIVE = 0x03,
    STOPPING = 0x04,
    STOPPED = 0x05,
    STARTFAILED = 0x06,
  }

  /* Enumerations referring to SBME devices */
  internal enum SBME_DeviceClass
  {
    /* 	CC				 */
    SBME_CLASS_NULL = 0,
    SBME_CLASS_SSBME = 1,
    SBME_CLASS_SG = 2,
    SBME_CLASS_CLI = 3,
    SBME_CLASS_SS = 4,
    SBME_CLASS_SM = 5,
    SBME_CLASS_CA = 6,
    SBME_CLASS_UP = 7,

    /* 	TLCN				 */
    SBME_CLASS_WMN = 8,
    SBME_CLASS_HUB = 9,
    SBME_CLASS_RHUB = 10,

    /* 	Impianto			 */
    SBME_CLASS_CDX = 11,
    SBME_CLASS_UCC = 12,

    /* 	ApparatiPerif.Vendita	 */
    SBME_CLASS_DSD = 31,
    SBME_CLASS_DSDE = 32,
    SBME_CLASS_CPPK = 33,
    SBME_CLASS_DAB = 34,
    SBME_CLASS_MAT = 35,
    SBME_CLASS_CAPK = 36,

    /* 	ApparatiPerif.Parcheggio */
    SBME_CLASS_PIPK = 37,
    SBME_CLASS_PUPK = 38,

    /* 	ApparatiPerif.Monetel	 */
    SBME_CLASS_RDE = 40,
    SBME_CLASS_LP = 41,

    /* 	SupportoConvalida		 */
    /* 	Nota: SBME_CLASS_UB comprende anche i device di tipo UCB */
    SBME_CLASS_UB = 71,
    SBME_CLASS_CU = 72,
    SBME_CLASS_CUS = 73,

    /* 	Convalida			 */
    SBME_CLASS_CNV = 74,
    SBME_CLASS_CNVG = 76,

    /* 	CCD				 */
    SBME_CLASS_EDMPC = 201,
    SBME_CLASS_CODMCT = 202,
    SBME_CLASS_MTDMCS = 203,
    SBME_CLASS_ETSCPC = 204,
    SBME_CLASS_ETSCCT = 205,
    SBME_CLASS_ETSCPR = 206,
    SBME_CLASS_CSC = 207,
    SBME_CLASS_MAILER = 208,
    SBME_CLASS_PGD = 210,
    SBME_CLASS_ARCSRV = 211,
    SBME_CLASS_ARCCLI = 213
  }

  internal enum SBME_DeviceType
  {
    SBME_TYPE_NULL = 0,

    /* 	Impianto			 */
    /* 	CC				 */
    SBME_TYPE_UP = 5,
    SBME_TYPE_CDX = 6,

    /* 	SupportoConvalida		 */
    SBME_TYPE_UCB = 10,
    SBME_TYPE_UB = 11,
    SBME_TYPE_CU = 12,
    SBME_TYPE_CUS = 13,

    /* 	Convalida			 */
    /* Attenzione ! la rev. 0.3 corregge precedente scambio tra ME_GATE e ME_DISABLED */
    SBME_TYPE_CNV_ME_BUS = 63,
    SBME_TYPE_CNV_ME_GATE = 66,
    SBME_TYPE_CNV_E_BUS = 69,
    SBME_TYPE_CSC = 71,

    /* 	ApparatiPerif.Monetel - RDE	 */
    SBME_TYPE_RDE = 107,

    /* 	#Vendita			 */
    SBME_TYPE_DSD = 125,
    SBME_TYPE_DSDE = 126,
    SBME_TYPE_CPPK = 127,
    SBME_TYPE_DAB = 140,
    SBME_TYPE_MAT = 141,
    SBME_TYPE_CAPK = 142,

    /* 	ApparatiPerif.Parcheggio */
    SBME_TYPE_PIPK = 143,
    SBME_TYPE_PUPK = 144,
    SBME_TYPE_PIUPK = 145,

    /* 	ApparatiPerif.Monetel - LP	 */
    SBME_TYPE_LP = 185,

    /* 		some new types added in rev 0.3 */
    SBME_TYPE_CODMCT = 195,
    SBME_TYPE_MTDMCS = 196,
    SBME_TYPE_EDMPC = 197,
    SBME_TYPE_ETSCPC = 198,
    SBME_TYPE_ETSCPR = 199,
    SBME_TYPE_MAILER = 200,
    SBME_TYPE_ETSCCT = 201,
    SBME_TYPE_ARCSRV = 203,
    SBME_TYPE_ARCCLI = 204,
    SBME_TYPE_PGD = 205,
    SBME_TYPE_ARCOCR = 206,

    /* 	Component parts of parking devices  */
    SBME_TYPE_DTSC = 208,
    SBME_TYPE_MCSC = 209,
    SBME_TYPE_MDT1 = 210,
    SBME_TYPE_MDT2 = 211,
    SBME_TYPE_BPK1 = 212,
    SBME_TYPE_BPK2 = 213,
    SBME_TYPE_BAP = 214,
    SBME_TYPE_MCRT1 = 215,
    SBME_TYPE_MCRT2 = 216,

    /* 	Component parts of TVM machines	 */
    SBME_TYPE_MTM = 220,
    SBME_TYPE_MBNA = 221,
    SBME_TYPE_MEDM = 222,
    SBME_TYPE_MSG = 223,
    SBME_TYPE_VPOS = 224,
    SBME_TYPE_CTU = 225,
    SBME_TYPE_MAV = 226,
    SBME_TYPE_MLP = 227,
    SBME_TYPE_MALM = 228,
    SBME_TYPE_MPS2 = 229,
    SBME_TYPE_MTPP = 230,
    SBME_TYPE_MPCI = 231,

    /* 	Component parts of BOM machines	 */
    SBME_TYPE_MTDE = 236,
    SBME_TYPE_MTDM = 237,
    SBME_TYPE_POS = 238,
    SBME_TYPE_S_TSC = 239
  }


  enum SBME_Operator
  {
    SBME_OP_UNDEFINED = 0,
    SBME_OP_ATM = 1,
    SBME_OP_FNME = 2,
    SBME_OP_TRENITALIA = 3,
    SBME_OP_INTEGRATO = 4    /* Note: SBME_OP_INTEGRATO is used only for SSBME device */
  }


  /*  Stato delle CNV  **	0 SPENTE  **	1 ACCESE  */
  internal enum CnvStatus
  {
    On = 0,
    Off = 1
  }

  internal class Sbme
  {
    /* Special values used in various sections */
    internal const int SBME_UNKNOWN_LOC = 0;		/* 	Special value used when a localita is not known	 */
    internal const int SBME_RING_0_BUS_LOC = 999;		/* 	Special value for the only "bus" localita in ring zero */
    internal const int SBME_UNKNOWN_SEMIZONE = 0;		/* 	Special value used when a semizone is not known  */
    internal const int SBME_RING_0_SEMIZONE = 1;     	/* 	Special value for the only semizone defined in ring zero */
    internal const int SBME_UNKNOWN_ROUTE = 0;		/* 	Special value used when a route number is not known  */
    internal const int SBME_ROUTE_ON_RAIL = 1000;		/* 	Special value for route id used for railways */
    internal const int SBME_UNKNOWN_USER = 0;		/* 	Special value used for documents that do not belong to a definite user  */
    internal const int SBME_UNKNOWN_RUN = 0;		/* 	Special value used when a run number is not known  */
    internal const int SBME_UNKNOWN_GRPLINES = 0;		/* 	Special value used when a "group of lines" identifer is not known  */
    internal const int SBME_UNDEF_RUNINDEX = 0;		/* 	Value used for RunIndex when it is undefined/unknown	 */
    internal const int SBME_INFINITE_BIG_JRS = 1023;		/* 	Special value used when no limit is put on the number of journeys	 */
    internal const int SBME_UNKNOWN_SHC_MODEL = 0;		/* 	Value for "Unknown short card model"	 */
    internal const int SBME_VEC_UNKNOWN = 0;		/* 	Value for "Unknown vector"	 */
    internal const int SBME_UNKNOWN_AGENTID = 0;		/* 	Value for "Unknown agent"	 */
    internal const int SBME_STRONG_PARKPRIO = 12;		/* 	strong priority for parking only contracts (#P***, #SPEX)	 */
    internal const int SBME_WEAK_PARKPRIO = 13;		/* 	weak priority for parking only contracts (#P***, #SPEX)	 */
    internal const int SBME_PARK_GLOB_AREA = 1;		/* 	ID of parking area caontaining all parkings	 */
  }

}
