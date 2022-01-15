using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  internal class FileUtils
  {
    private const int m_PaddingSize = 4;
    private static byte[] m_PaddingByte = null;

    static FileUtils()
    {
      m_PaddingByte = new byte[FileUtils.m_PaddingSize];
      for (int i = 0; i < m_PaddingByte.Length; i++)
      {
        m_PaddingByte[i] = 0;
      }
    }

    internal static int GetPaddingNrBytes(long Position)
    {
      int nPad = (m_PaddingSize - (int)(Position % m_PaddingSize)) % m_PaddingSize;
      return nPad;
    }

    internal static int ReadPadding(BinaryReader br)
    {
      int nPad = GetPaddingNrBytes(br.BaseStream.Position);
      br.ReadBytes( nPad);
      return nPad;
    }

    internal static int WritePadding(BinaryWriter bw)
    {
      int nPad = GetPaddingNrBytes( bw.BaseStream.Position );
      bw.Write( m_PaddingByte, 0, nPad );
      return nPad;
    }

    internal static bool unlink(String FileName)
    {
      return UCB_DeleteFile(FileName);
    }

    internal static bool UCB_DeleteFile(String FileName)
    {
      bool ret = false;
      try
      {
        if (File.Exists(FileName))
        {
          File.Delete(FileName);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_DeleteFile() reason=\"Fallita cancellazione file\""
               + ", Filename=\"" + FileName + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }


    internal static bool UCB_FileExists(String Filename)
    {
      bool ret = false;
      try
      {
        ret = File.Exists(Filename);
      }
      catch (Exception)
      {
      }

      return (ret);
    }

    internal static bool UCB_DirectoryExists(String Foldername)
    {
      bool ret = false;
      try
      {
        ret = Directory.Exists(Foldername);
      }
      catch (Exception)
      {
      }

      return (ret);
    }

    internal static void UCB_DiskFlush()
    {
    }

    internal static bool UCB_CreateDirectory(String Foldername)
    {
      String funcName = "UCB_CreateDirectory()";
      String logMsg = "";

      bool ret = false;
      if (UCB_DirectoryExists(Foldername) == true)
      {
        logMsg = funcName + " reason=\"Chiesta la creazione di una Directory esistente\""
               + ", Foldername=\"" + Foldername + "\"";
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = true;
      }

      try
      {
        Directory.CreateDirectory(Foldername);
        ret = true;
      }
      catch (Exception)
      {
        logMsg = funcName + " reason=\"Impossibile creare la Directory\""
               + ", Foldername=\"" + Foldername + "\", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }

    internal static bool UCB_RemoveDirectory(String Foldername)
    {
      bool ret = false;
      try
      {
        if (UCB_DirectoryExists(Foldername))
        {
          Directory.Delete(Foldername);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_RemoveDirectory() reason=\"Impossibile rimuovere la Directory\""
               + ", Foldername=\"" + Foldername + "\", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }

    internal static bool UCB_RenameDirectory(String SourceDirname, String DestDirname)
    {
      bool ret = false;
      try
      {
        if (UCB_DirectoryExists(SourceDirname))
        {
          Directory.Move(SourceDirname, DestDirname);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_RenameDirectory() reason=\"Fallito cambio nome directory\""
               + ", SourceDirname=\"" + SourceDirname + "\", DestDirname=\"" + DestDirname + "\""
               + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }

    internal static bool UCB_Rename(String SourceFilename, String DestFilename)
    {
      bool ret = false;
      try
      {
        if (UCB_FileExists(SourceFilename))
        {
          File.Copy(SourceFilename, DestFilename, true);
          UCB_DeleteFile(SourceFilename);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_Rename() reason=\"Fallito cambio nome\""
               + ", SourceFilename=\"" + SourceFilename + "\", DestFilename=\"" + DestFilename + "\""
               + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }

    internal static bool UCB_MoveFile(String SourceFilename, String DestFilename)
    {
      bool ret = false;
      try
      {
        if (UCB_FileExists(SourceFilename))
        {
          File.Copy( SourceFilename, DestFilename, true);
          UCB_DeleteFile(SourceFilename);
          ret = true;
        }
      }
      catch (Exception)
      {
        ret = false;
        if (SourceFilename == DirsNames.LOG_FILE_NAME) return ret;

        String logMsg = "";
        logMsg = "UCB_MoveFile() reason=\"Fallito spostamento file\""
               + ", SourceFilename=\"" + SourceFilename + "\", DestFilename=\"" + DestFilename + "\""
               + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
      }
      return ret;
    }

    internal static bool UCB_CopyFile(String SourceFilename, String DestFilename)
    {
      bool ret = false;
      try
      {
        if (UCB_FileExists(SourceFilename))
        {
          File.Copy(SourceFilename, DestFilename, true);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_CopyFile() reason=\"Fallita copia file\""
               + ", SourceFilename=\"" + SourceFilename + "\", DestFilename=\"" + DestFilename + "\""
               + ", errno=" + Porting.GetErrNoStr();
        LogTrace.Write(0, Severity.LOG_WARNING, logMsg);
        ret = false;
      }
      return ret;
    }

    internal static long GetFileSize(string p)
    {
      long size = -1;

      try
      {
        FileInfo fi = new System.IO.FileInfo(DirsNames.LOG_FILE_NAME);
        if (fi != null)
        {
          size = fi.Length;
        }
      }
      catch ( Exception )
      {
      }
      return size;
    }



    /* Rimuove i file in una directory. Se oreMax e` 0, li cancello tutti. Se e` > 0, cancello solo quelli
       piu` "vecchi" di <oreMax>, discendendo nelle sottodirectory se <ricorsivo> e` TRUE */
    internal static bool CancellaFile(bool ricorsivo, String FolderName, int oreMax)
    {
      String funcName = "CancellaFile()";
      String msgLog = "";

      msgLog = funcName + " STARTS NomeDirectory=\"" + FolderName + "\", OreMax=" + oreMax.ToString();
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      int countTot = 0;
      int countCancellati = 0;
      int errorOccur = 0;

      int scadenza = (int)oreMax * 3600;	      /* Converto in secondi */
      int time = UCB.UCB_GetSystemTime();				/* Calcolo il tempo per fare la differenza */
      DateTime tempoAttuale = TimeUtils.ConvertFromUnixTimestamp(time);

      try
      {
        DirectoryInfo di = new DirectoryInfo(FolderName);
        if (di == null) return true;

        msgLog = funcName + " reason=\"Rimuovo i file dalla directory\""
               + ", NomeDirectory=\"" + FolderName;
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

        FileInfo[] files = di.GetFiles();

        for (int i = 0; i < files.Length; i++)
        {
          String fullName = files[i].FullName;
          String fileName = files[i].Name;

          /* Salto le directory '.' e '..' */
          if (fileName == "." || fileName == "..") continue;

          /*  M.F. 6/12/2004 */
          /* Non considero i file di log quando svuoto una dir */
          /* Fatto per non buttar via i log in seguito a svuotadir su ramdisk */
          if (fileName == "ucb_log.txt" || fileName == "ucb_old_log.txt" || fileName == "DiffTime")
          {
            continue;
          }

          if (Directory.Exists(fullName))
          {
            if (ricorsivo)	/* ... ci entro solo se non sono gia` "annidato" */
            {
              // SE ancello la cartella perche chiamare la cancella file?
              /* Entro nella sottodirectory, mettendo a FALSE ricorsivo, cosi` scendo solo di un livello (per sicurezza) */
              CancellaFile(false, fullName, oreMax);

              /* Rimuovo la directory (sicuramente annidata) */
              UCB_RemoveDirectory(fullName);
            }
          }
          else if (File.Exists(fullName))
          {
            countTot++;
            TimeSpan span = tempoAttuale - files[i].LastWriteTime;
            if (oreMax == 0 || span.TotalHours > oreMax)
            {
              if (unlink(fullName) == false)
              {
                msgLog = funcName + " Fallita cancellazione di " + fullName
                       + " errno=" + Porting.GetErrNoStr();
                LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
                errorOccur++;
              }
              else
              {
                msgLog = funcName + " reason=\"File cancellato\" NomeFile=" + fullName
                        + " oreMax=" + oreMax.ToString();
                LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

                countCancellati++;
              }
            }
          }
        }
      }
      catch (Exception)
      {
        msgLog = funcName + " Fallito lettura files in folderName=\"" + FolderName + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
      }

      if (oreMax > 0 && countTot > 0)
      {
        msgLog = funcName + " reason=\"Cancellati " + countCancellati.ToString()
              + " file vecchi su " + countTot.ToString() + "\""
              + ", NomeDirectory=\"" + FolderName + "\"";
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);
      }

      if (errorOccur != 0)
      {
        return false;
      }

      msgLog = funcName + " ENDS NomeDirectory=\"" + FolderName + "\" rc=0";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);
      return true;
    }



    /* Usa la CancellaFile per svuotare il contenuto di una directory */
    /* Fa un controllo sul nome della directory, consentendo la cancellazione solamente
        delle directory consentite! */
    internal static bool SvuotaDir(string FolderName, bool ancheDir, int OreMax)
    {
      String funzName = "SvuotaDir()";
      String msgLog = "";
      bool rc = true;

      msgLog = funzName + " STARTS NomeDirectory=\"" + FolderName + "\", CancellaDir=" + ancheDir.ToString();
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      /* Evito nomi "pericolosi" che potrebbero avere effetti indesiderati */
      int n = FolderName.IndexOf("Config");
      if (FolderName == "" || FolderName == "." || n != -1)
      {
        msgLog = funzName + " FAILED reason=\"Operazione non ammessa su questa Directory\""
               + ", NomeDirectory=\"" + FolderName + "\", CancellaDir=" + ancheDir.ToString();
        LogTrace.Write(LogType.LOG_UB, Severity.LOG_ERR, msgLog);
        return false;
      }

      CancellaFile(true, FolderName, OreMax);

      if (ancheDir)
      {
        rc = UCB_RemoveDirectory(FolderName);
        if (rc == false)
        {
          msgLog = funzName + " FAILED reason=\"Fallita rimozione Directory\""
                 + ", NomeDirectory=\"" + FolderName + "\""
                 + ", CancellaDir=" + ancheDir.ToString();
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_WARNING, msgLog);
        }
        else
        {
          msgLog = funzName + " reason=\"Rimozione directory avvenuta con successo\""
                 + ", NomeDirectory=\"" + FolderName + "\"";
          LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);
        }
      }

      msgLog = funzName + "  ENDS NomeDirectory=\"" + FolderName + "\", rc=0";
      LogTrace.Write(LogType.LOG_UB, Severity.LOG_INFO, msgLog);

      return rc;
    }

    internal static void ScriviPID(String NomeThread, int Pid)
    {
    }
  }
}

