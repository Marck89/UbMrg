using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal static class FileAttMgr
  {
    const int NumProgrOffset = 18;

    /*  Legge il progressivo da file (se avvio == true) e apre un file di attivita`  */
    internal static void MGR_ApriFileAtt(UB_DatiBase _UB_DatiBase, bool Avvio)
    {
      String funcName = "MGR_ApriFileAtt()";
      String msgLog = "";

      String modoAvvio = Avvio ? "1" : "0";

      msgLog = funcName + " STARTS Avvio=" + modoAvvio;
      LogTrace.Write(LogType.LOG_MGR, Severity.LOG_INFO, msgLog);

      /* Prima controllo che non ci sia un file gia` presente, nel qual caso prendo il suo
      ** progressivo, aggiorno i contatori, lo chiudo e lo archivio */
      if (FileUtils.UCB_FileExists(DirsNames.FILE_ATTIVITA_MGR))
      {
        ushort progrAtt = ReadProgrAttMgr();
        if (progrAtt > 0)
        {
          _UB_DatiBase.m_ProgrAtt = progrAtt;
          msgLog = funcName + " reason=\"Trovato file Attivita' aperto\""
                 + ", Nomefile=\"" + DirsNames.FILE_ATTIVITA_MGR + "\""
                 + ", ModoAvvio=%d, ProgressivoAttivita=" + progrAtt.ToString();
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_NOTICE, msgLog);

          /* Scrivo gli allarmi attivi su file Activity */
          // DA FARE  MGR_ScriviAllarmiAttiviSuFileAtt(_UB_DatiBase);

          /* Inserisco il record dei contatori di movimento e l'uptime prima dello spegnimento anomalo */
          /* DA FARE FAtt_ContatoriManutenzione(
                       _UB_DatiBase.statoDisp.Dispositivo[0].Dati_Dispositivo.Tipo,
                        _UB_DatiBase.statoDisp.Dispositivo[0].Dati_Dispositivo.Sottotipo,
                        _UB_DatiBase.descrTipoMGR.Serial_Number, 0L/* /, 0L);
*/
          /* Signature, rinomino e sposto */
          // DA FARE  MGR_ChiudiFileAtt(_UB_DatiBase);

          /* Se il file era presente, sicuramente c'e` stato uno spegnimento improvviso quindi sul
             file del progressivo (progrAttMGR) c'e` un valore non corretto. Il valore giusto
             l'ho appena preso dal file, quindi devo forzare avvio = false */
          Avvio = false;
        }
      }
    }

    private static ushort ReadProgrAttMgr()
    {
      String funcName = "ReadProgrAttMgr()";
      String msgLog = "";

      ushort progrAtt = 0;

      FileStream fs = null;
      try
      {
        fs = new FileStream(DirsNames.FILE_ATTIVITA_MGR, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        /* Il numero progressivo e` all'offset 18 ed e` su 2 byte */
        long pos = br.BaseStream.Seek(NumProgrOffset, SeekOrigin.Begin);
        if (pos != NumProgrOffset)
        {
          progrAtt = 1;
          msgLog = funcName + " reason=\"Errore 'lseek' su file\""
                + ", Nomefile=\"" + DirsNames.FILE_ATTIVITA_MGR + "\""
                + ", errno=" + Porting.GetErrNoStr() 
                + ", ProgressivoAttivita=" + progrAtt.ToString();
          LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
        }
        else
        {
          if ((br.BaseStream.Length - br.BaseStream.Position) < 2)
          {
            progrAtt = 1;
            msgLog = funcName + " reason=\"Errore 'read' su file\""
                  + ", Nomefile=\"" + DirsNames.FILE_ATTIVITA_MGR + "\""
                  + ", errno=" + Porting.GetErrNoStr()
                  + ", ProgressivoAttivita=" + progrAtt.ToString();
            LogTrace.Write(LogType.LOG_MGR, Severity.LOG_ERR, msgLog);
          }
          else
          {
            ushort progAscii = br.ReadUInt16();

            /* Devo swappare i byte */
            progrAtt = Utils.InvertoShort(progAscii);
          }
        }
        fs.Close();
      }
      catch
      {
        progrAtt = 0;
      }
      return progrAtt;
    }
  }
}
