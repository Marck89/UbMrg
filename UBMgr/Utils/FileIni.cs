using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal class FileIni
  {
    StreamReader m_Sr = null;

    internal bool StartFileIni(String NomeFileIni)
    {
      bool esito = false;

      try
      {
        EndFileIni();
        m_Sr = new StreamReader(NomeFileIni);
        esito = true;
      }
      catch (Exception)
      {
        m_Sr = null;
        esito = false;
      }
      return esito;
    }

    internal void EndFileIni()
    {
      if (m_Sr == null) return;
      m_Sr.Close();
      m_Sr = null;
    }

    private String ReadValString( String NomeVar )
    {
      if (String.IsNullOrEmpty(NomeVar) == true) return null;
      if (m_Sr == null) return null;

      bool found = false;
      String value = "";

      // Posizionamento a inizio file
      m_Sr.BaseStream.Seek(0, SeekOrigin.Begin);

      while (found == false)
      {
        string text = m_Sr.ReadLine();
        if ( text == null) break;

        int index = text.IndexOf('=');
        if ( index != -1)
        {
          String keyword = text.Substring(0, index);
          keyword = keyword.Trim();
          if ( String.Compare(keyword, NomeVar, true) == 0)
          {
            found = true;   /* Trovata ! */
            value = text.Substring(index + 1);
            value = value.Trim();
          }
        }
      }
      return value;
    }

    internal String GetFileIniString(String NomeVar, String DefVal)
    {
      String value = ReadValString(NomeVar);

      // Se KO copia stringa di default
      if (value == null)
      {
        if (DefVal != null) value = DefVal;
        else value = "";
      }
      return value;
    }

    internal int GetFileIniInt(String NomeVar, int DefVal)
    {
      int value = DefVal;
      String valueStr = ReadValString(NomeVar);

      // Se KO copia stringa di default
      if (valueStr != null)
      {
        bool rst = int.TryParse(valueStr, out value);
        if ( rst == false) value = DefVal;
      }
      return value;
    }

  }
}
