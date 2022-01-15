using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  class MacAddress
  {
    private const int m_MacAddressSize = 18; /* 00:00:00:00:00:00\n */

    private static bool ReadAddress(string FileName, string Address)
    {
      string msgLog = "";
      string funcName = "ReadAddress()";
      bool rst = false;

      Address = "";
      FileStream fd = null;
      try
      {
        fd = new FileStream(FileName, FileMode.Open);
        BinaryReader br = new BinaryReader(fd);

        byte[] bytes = br.ReadBytes(m_MacAddressSize);
        Address = bytes.ToString();
        if ( bytes.Length != m_MacAddressSize ) /* 00:00:00:00:00:00\n */
        {
          msgLog = funcName + " reason=\"Formato Mac Address non valido\""
                 + ", Filename=\"" + FileName + "\", MacAddress=\"" + Address + "\"";
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_WARNING, msgLog);

          /* Vado avanti */
        }
        fd.Close();
        rst = true;
      }
      catch (Exception)
      {
        msgLog = funcName + " reason=\"Errore in apertura file\""
               + ", Filename=\"" + FileName
               + "\", errno=" + Porting.GetErrNoStr() + ", rc=0";
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);

        rst = false;
      }
      return rst;
    }


    private static bool WriteAddress(string FileName, string Address)
    {
      string msgLog = "";
      string funcName = "WriteAddress()";
      bool rst = false;

      Address = "";
      FileStream fd = null;
      try
      {
        fd = new FileStream(FileName, FileMode.OpenOrCreate);
        BinaryWriter bw = new BinaryWriter(fd);

        try
        {
          bw.Write(Address);
        }
        catch (Exception)
        {
          msgLog = funcName + " reason=\"Errore in in scrittura file\""
                 + ", Filename=\"" + FileName 
                 + "\", errno=" + Porting.GetErrNoStr() + ", rc=-1";
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);

          fd.Close();

          FileUtils.unlink(FileName); /* Non lascio zombies */
          rst = false;
        }

        fd.Close();
        rst = true;
      }
      catch (Exception)
      {
        msgLog = funcName + " reason=\"Errore in apertura file\""
               + ", Filename=\"" + FileName + "\", errno=" + Porting.GetErrNoStr() + ", rc=0";
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);

        rst = false;
      }
      return rst;
    }

    internal static bool CheckMacAddress()
    {
      String funcName = "CheckMacAddress()";
      String msgLog = "";

      String currentMacAddress = "";
      String lastMacAddress = "";

      if (FileUtils.UCB_FileExists(DirsNames.UCB_CURRENT_MAC_ADDRESS) == false)
      {
        msgLog = funcName + " FAILED reason=\"File non trovato\""
               + ", Filename=\"" + DirsNames.UCB_CURRENT_MAC_ADDRESS + "\"";
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
        return false;
      }

      /* Lettura MAC Address corrente */
      bool rst = ReadAddress(DirsNames.UCB_CURRENT_MAC_ADDRESS, currentMacAddress);

        /* Lettura MAC Address salvato */
      if (FileUtils.UCB_FileExists(DirsNames.UCB_LAST_MAC_ADDRESS))
      {
        rst = ReadAddress(DirsNames.UCB_LAST_MAC_ADDRESS, lastMacAddress);
        if (rst == false) return false;
      }
      else
      {
        /* Salvo il MAC Address corrente */
        rst = WriteAddress(DirsNames.UCB_LAST_MAC_ADDRESS, currentMacAddress);
        if (rst == false) return false;

        lastMacAddress = currentMacAddress;
      }

        /* Levo il return */
      currentMacAddress.TrimEnd('\n');
      lastMacAddress.TrimEnd('\n');
      if ( currentMacAddress != lastMacAddress ) 
      {
        /* MAC Address variato */
        msgLog = funcName + " FAILED reason=\"Il MAC Address della UCB e' variato"
               + ", la Compact Flash e' stata spostata su una UCB differente\""
               + ", LastMacAddress=\"" + lastMacAddress + "\""
               + ", CurrentMacAddress=\"" + currentMacAddress + "\", rc=-1";
        LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
        return false;
      }

      msgLog = funcName + " reason=\"Il MAC Address della UCB non e' variato\""
             + ", LastMacAddress=\"" + lastMacAddress + "\""
             + ", CurrentMacAddress=\"" + currentMacAddress + "\", rc=0";
      LogTrace.Write(LogType.LOG_MGR, Severity.LOG_NOTICE, msgLog);

      return true;
    }

    internal static void Check()
    {
      if (CheckMacAddress() == false)
      {
        /* Non mi devo avviare (per ora non lo faccio) */
        LogTrace.Write( LogType.LOG_UB, Severity.LOG_ERR, "SBME() reason=\"MAC Address variato\"");

        /* Scrivo un allarme */
        Globals.m_Alarms.m_AllarmeMacAddressVariato = true;

        /* Vedere quando bloccare l'applicativo */
      }
    }
  }
}
