using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  /* Stati possibili WLAN */
  internal enum WLanState
  {
    SBME_WLAN_DISCONNECTED = 0x00,
    SBME_WLAN_CONNECTING = 0x01,
    SBME_WLAN_CONNECTED = 0x02,
    SBME_WLAN_DISCONNECTING = 0x03,
  }

/********************************/
/*		Return code defines		*/
/********************************/
  internal enum WLanCode
  {
    WLD_OK = 0,
    WLD_BAD_ARGUMENT = -1,
    WLD_INTERNAL_ERROR = -2,
    WLD_FILE_NOT_FOUND = -3,
    WLD_PROCESS_TERMINATED = -4,
    WLD_INVALID_OPERATION = -5,
    WLD_TIMEOUT = -6,
    WLD_ERROR_REPORTED_BY_SCRIPT = -7,
  }

  /* OpCode */
  internal enum WLanOpCode
  {
    WLAN_SETUP_IP_FISSO = 0,
    WLAN_SETUP_DHCP = 1,
    WLAN_SWITCH_OFF = 2,
    APPL_REGISTER = 3,
    APPL_UNREGISTER = 4,
  }

  /* IDUser */
  internal enum WLanIdUser
  {
    SBME = 0,
    SPAM = 1,
  }

  class WLan
  {
    internal static bool ApplRegisterMgr(WLanOpCode OpCode, WLanIdUser IdUser, 
                                         int RetryCount)
    {
      String msgLog = "";
      String funcName = "ApplRegisterMgr()";

       WLanCode rc = WLanCode.WLD_OK;
      bool registered = false;
      while ( RetryCount > 0 && registered == false)
      {
        rc = BusInterface.ApplRegister( OpCode, IdUser);
        if (rc != WLanCode.WLD_OK)
        {
          RetryCount--;

          msgLog = funcName 
                 + " reason=\"Registrazione applicazione " + IdUser.ToString()
                 + " al WLAN Daemon fallita, ci riprovo tra 1 secondo\""
                 + ", RetryCount=" + RetryCount.ToString();
          LogTrace.Write(LogType.LOG_WLAN, Severity.LOG_WARNING, msgLog); 

          Thread.Sleep(1000);

          continue;
        }

        msgLog = funcName 
               + " reason=\"Registrazione applicazione " + OpCode.ToString()
               + " - " + IdUser.ToString()
               + " al WLAN Daemon avvenuta con successo\"";
        LogTrace.Write(LogType.LOG_WLAN, Severity.LOG_NOTICE, msgLog);

        registered = true;

        break;
      }
      if ( registered == false )
      {
        msgLog = funcName + " FAILED reason=\"" + OpCode.ToString()
               + " - applicazione " + IdUser.ToString() + " al WLAN Daemon fallita\", rc=" + rc.ToString();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }
      else
      {
        msgLog = funcName + " reason=\"" + OpCode.ToString()
               + " - applicazione " + IdUser.ToString() + " al WLAN Daemon avvenuta con successo\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
      }

      return registered;
    }
  }
}
