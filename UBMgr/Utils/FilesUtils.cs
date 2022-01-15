using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  internal class FilesUtils
  {
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
      String funzName = "UCB_CreateDirectory()";
      String logMsg = "";

      bool ret = false;
      if (UCB_DirectoryExists(Foldername) == true)
      {
        logMsg = funzName + " reason=\"Chiesta la creazione di una Directory esistente\""
               + ", Foldername=\"" + Foldername + "\"";
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
        ret = true;
      }

      try
      {
        Directory.CreateDirectory(Foldername);
        ret = true;
      }
      catch (Exception)
      {
        logMsg = funzName + " reason=\"Impossibile creare la Directory\""
               + ", Foldername=\"" + Foldername + "\", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
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
               + ", Foldername=\"" + Foldername + "\", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
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
               + ", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
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
          File.Move(SourceFilename, DestFilename);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_Rename() reason=\"Fallito cambio nome\""
               + ", SourceFilename=\"" + SourceFilename + "\", DestFilename=\"" + DestFilename + "\""
               + ", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
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
          File.Move(SourceFilename, DestFilename);
          ret = true;
        }
      }
      catch (Exception)
      {
        String logMsg = "";
        logMsg = "UCB_MoveFile() reason=\"Fallito spostamento file\""
               + ", SourceFilename=\"" + SourceFilename + "\", DestFilename=\"" + DestFilename + "\""
               + ", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
        ret = false;
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
               + ", errno=" + Porting.ERRNO.ToString();
        LogTrace.Write(0, Porting.GetLine(), Porting.GetFile(), Severity.LOG_WARNING, logMsg);
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
  }
}

