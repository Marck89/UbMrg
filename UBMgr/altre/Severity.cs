using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  enum Severity
  {
    LOG_EMERG    =  0,       /* system is unusable */
	  LOG_ALERT    =  1,       /* action must be taken immediately */
	  LOG_CRIT     =  2,       /* critical conditions */
	  LOG_ERR      =  3,       /* error conditions */
	  LOG_WARNING  =  4,       /* warning conditions */
	  LOG_NOTICE   =  5,       /* normal but significant condition */
	  LOG_INFO     =  6,       /* informational */
	  LOG_DEBUG    =  7       /* debug-level messages */
  }



  class SeverityUtils
  {
    internal static Severity GetSeverity(int StartUpSeverity)
    {
      // XXXXX ERRORE il valiore numerico da argv non corrispondono ai valori dell'enum
      Severity severity = Severity.LOG_ERR;
           
      switch (StartUpSeverity)
      {
        case 0:
          severity = Severity.LOG_ERR;
          break;

        case 1:
          severity = Severity.LOG_WARNING;
          break;

        case 2:
          severity = Severity.LOG_NOTICE;
          break;

        case 3:
          severity = Severity.LOG_INFO;
          break;

        case 4:
          severity = Severity.LOG_DEBUG;
          break;

        default:
          severity = Severity.LOG_ERR;
          break;
      }
      return severity;
    }

    internal static String GetString(Severity Severity)
    {
      string severityStr = "";

      switch (Severity)
      {
        case Severity.LOG_EMERG:					/* 0 */
        case Severity.LOG_ALERT:					/* 1 */
        case Severity.LOG_CRIT:					/* 2 */
        case Severity.LOG_ERR:				  	/* 3 */
          severityStr = "ERR";
          break;

        case Severity.LOG_WARNING:				/* 4 */
          severityStr = "WRN";
          break;

        case Severity.LOG_NOTICE:				/* 5 */
          severityStr = "LOG";
          break;

        case Severity.LOG_INFO:					/* 6 */
          severityStr = "TRC";
          break;

        case Severity.LOG_DEBUG:					/* 7 */
          severityStr = "DBG";
          break;

        default:
          severityStr = "???";
          break;
      }
      return severityStr;
    }
  }
}
