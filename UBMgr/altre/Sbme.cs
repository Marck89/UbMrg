using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Stati Applicativi per piattaforma AVM */
  internal enum SbmeStatus
  {
    STARTING    = 0x01,
    STARTED     = 0x02,
    OPERATIVE   = 0x03,
    STOPPING    = 0x04,
    STOPPED     = 0x05,
    STARTFAILED = 0x06,
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
