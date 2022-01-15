using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  internal class StatoUCB
  {
    private const int m_RecordSize = 1676;
    private const int m_RecordHeaderSize = 8;
    private const int m_RecordEndSize = 2;

    internal String m_RecordHeader = "";
    internal uint  m_RecordVersion;

    internal int m_DataOraInizioMisurazione;
    internal int m_DataOraFineMisurazione;

    internal uint m_IdVeicolo;

    internal uint m_SerialNumber;

    internal uint m_SoftwareMajor;
    internal uint m_SoftwareMinor;

    internal uint m_CnvCount;
    internal StatoCnv[] m_StatoCnv = null;

    internal String m_RecordEnd = "";

    internal StatoUCB()
    {
      m_StatoCnv = new StatoCnv[Cnvs.MAX_CNV];

      for (int i = 0; i < Cnvs.MAX_CNV; i++)
      {
        m_StatoCnv[i] = new StatoCnv();
      }
      Clear();
    }

    internal void Clear()
    {
      m_RecordHeader = "";

      m_DataOraInizioMisurazione = 0;
      m_DataOraFineMisurazione = 0;

      m_IdVeicolo = 0;

      m_SerialNumber = 0;

      m_SoftwareMajor = 0;
      m_SoftwareMinor = 0;

      m_CnvCount = 0;

      for (int i = 0; i < m_RecordEnd.Length; i++)
      {
        m_StatoCnv[i].Clear();
      }

      m_RecordEnd = "";
    }

    internal void Init()
    {
      Clear();

      m_RecordHeader = "UCBSTATS";
      m_RecordEnd = "ZZ";

      m_RecordVersion = 1; // 1.0

      m_SoftwareMajor = VersionSw.VERSIONE_MAJOR;
      m_SoftwareMinor = VersionSw.VERSIONE_MINOR;

      m_DataOraInizioMisurazione = UCB.UCB_GetSbmeTime();

      m_DataOraFineMisurazione = 0;
    }

    internal bool Write(FileStream fdOut)
    {
      bool rst = false;
      try
      {       
        BinaryWriter bw = new BinaryWriter(fdOut);
        int nPad = FileUtils.WritePadding(bw);

        byte[] bytes = null;

        String str = m_RecordHeader.PadLeft( m_RecordHeaderSize, '\0');
        bytes = Encoding.UTF8.GetBytes(str);
        bw.Write(bytes);
        nPad = FileUtils.WritePadding(bw);

        bw.Write( m_RecordVersion);
        bw.Write( m_DataOraInizioMisurazione);
        bw.Write( m_DataOraFineMisurazione);
        bw.Write( m_IdVeicolo);
        bw.Write( m_SerialNumber);
        bw.Write( m_SoftwareMajor);
        bw.Write( m_SoftwareMinor);
        bw.Write( m_CnvCount);

        for (int i = 0; i < m_StatoCnv.Length; i++)
        {
          rst = m_StatoCnv[i].Write( bw);
          if (rst == false) break;
        }
        if (rst)
        {
          str = m_RecordEnd.PadLeft(m_RecordEndSize, '\0');
          bytes = Encoding.UTF8.GetBytes(str);
          bw.Write(bytes);

          nPad = FileUtils.WritePadding(bw);

          rst = true;
        }

      }
      catch (Exception)
      {
        rst = false;
      }
      return rst;
    }

    internal bool Read(FileStream fdIn)
    {
      bool rst = false;

      try
      {
        if (fdIn.Position >= fdIn.Length) return false;

        BinaryReader br = new BinaryReader(fdIn);

        int nPad = FileUtils.ReadPadding(br);

        byte[] bytes = br.ReadBytes(m_RecordHeaderSize);
        m_RecordHeader = Encoding.UTF8.GetString(bytes);
        nPad = FileUtils.ReadPadding(br);

        m_RecordVersion = br.ReadUInt32();
        m_DataOraInizioMisurazione = br.ReadInt32();
        m_DataOraFineMisurazione = br.ReadInt32();

        m_IdVeicolo = br.ReadUInt32();

        m_SerialNumber = br.ReadUInt32();

        m_SoftwareMajor = br.ReadUInt32();

        m_SoftwareMinor = br.ReadUInt32();

        m_CnvCount = br.ReadUInt32();

        for (int i = 0; i < m_StatoCnv.Length; i++)
        {
          rst = m_StatoCnv[i].Read(br);
          if (rst == false) break;
        }
        if (rst)
        {
          bytes = br.ReadBytes(m_RecordEndSize);
          m_RecordEnd = Encoding.UTF8.GetString(bytes);

          nPad = FileUtils.ReadPadding(br);

          rst = true;
        }
      }
      catch (Exception)
      {
        rst = false;
      }
      return rst;
    }

    internal static void MGR_PackUcbStatistics()
    {
      String msgLog = "";
      String funcName = "MGR_PackUcbStatistics()";

      String statisticsTmpPath = "";

      int recTotal = 0;
      int recCopied = 0;

      /* Faccio pulizia sul file delle statistiche (elimino le vecchie) */
      FileStream fdIn = null;
      try
      {
        fdIn = new FileStream(DirsNames.UCB_STAT_DAT_FILE_NAME, FileMode.Open);

        int timeNow = UCB.UCB_GetSbmeTime();
        int sevenDaysSec = 7 * 24 * 3600; /* Secondi in una settimana */

        statisticsTmpPath = DirsNames.UCB_LOG_RAMDISK_PATH + "Statistics.tmp";

        msgLog = funcName + " reason=\"Pulizia su file statistiche UCB\""
               + ", Filename=\"" + DirsNames.UCB_STAT_DAT_FILE_NAME + "\""
               + ", TmpFilename=\"" + statisticsTmpPath + "\"";
        LogTrace.Write( LogType.LOG_UB, Severity.LOG_NOTICE, msgLog);

        FileStream fdOut = null;
        try
        {
          StatoUCB statoUcb = new StatoUCB();
          fdOut = new FileStream(statisticsTmpPath, FileMode.OpenOrCreate);
          while (true)
          {
            bool rst = statoUcb.Read(fdIn);
            if (rst)
            {
              int elapsedTime = timeNow - statoUcb.m_DataOraFineMisurazione;

              msgLog = funcName + " reason=\"Letto un record\"\""
                     + ", ElapsedTime=" + elapsedTime.ToString();
              LogTrace.Write(0, Severity.LOG_DEBUG, msgLog);

              recTotal++;

              if (elapsedTime <= sevenDaysSec)
              {
                /* Questo record è meno vecchio di una settimana,
                 * lo tengo ed eventualmente lo converto (TO DO) */
                rst = statoUcb.Write(fdOut);
                if (rst == false)
                {
                  msgLog = funcName + " reason=\"Errore in scrittura file\""
                         + ", Filename=\"" + statisticsTmpPath + "\""
                         + ", StructSize=" + StatoUCB.m_RecordSize
                         + ", errno=" + Porting.GetErrNo();
                  LogTrace.Write(0, Severity.LOG_ERR, msgLog);
                  break;
                }

                msgLog = funcName + " reason=\"Scritto un record\""
                       + ", ElapsedTime=" + elapsedTime.ToString();
                LogTrace.Write(0, Severity.LOG_INFO, msgLog);

                recCopied++;
              }
            }
            else
            {
              break; // End of the loop
            }
          }
          fdOut.Close();
        }
        catch (Exception)
        {
          msgLog = funcName + " reason=\"Errore in apertura file\""
                 + ", Filename=\"" + DirsNames.UCB_STAT_DAT_FILE_NAME + "\""
                 + ", errno=" + Porting.GetErrNoStr();
          LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
        }
        fdIn.Close();

        /* Copio il file indietro */
        if (!FileUtils.UCB_CopyFile(statisticsTmpPath, DirsNames.UCB_STAT_DAT_FILE_NAME))
        {
          msgLog = funcName + " reason=\"Errore in copia file\""
                 + ", SourceFilename=\"" + statisticsTmpPath + "\""
                 + ", DestFilename=\"" + DirsNames.UCB_STAT_DAT_FILE_NAME + "\""
                 + ", errno=" + Porting.GetErrNoStr();
          LogTrace.Write(0, Severity.LOG_ERR, msgLog);
        }

        /* Elimino il file temporaneo */
        FileUtils.unlink(statisticsTmpPath);

        msgLog = funcName + " reason=\"File statistiche elaborato\","
                 + ", SourceFilename=\"" + statisticsTmpPath + "\""
                 + ", RecordLetti=" + recTotal.ToString()
                 + ", RecordCopiati=" + recCopied;
        LogTrace.Write(0, Severity.LOG_NOTICE, msgLog);
      }
      catch (Exception )
      {
        msgLog = funcName + " reason=\"File statistiche UCB non trovato\""
               + ", Filename=\"" + DirsNames.UCB_STAT_DAT_FILE_NAME + "\"";
        LogTrace.Write(0, Severity.LOG_WARNING, msgLog);
      }
    }
  }
}
