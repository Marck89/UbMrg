using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal class Concentratore
  {
    internal String m_QueueManagerName;
    internal String m_NomeDeposito;

    internal Concentratore(String QueueManagerName, String NomeDeposito)
    {
      m_QueueManagerName = QueueManagerName;
      m_NomeDeposito = NomeDeposito;
    }
  }

  internal static class Concentratori
  {
    static List<Concentratore> m_Names = new List<Concentratore>();

    static Concentratori()
    {
      m_Names.Add(new Concentratore("AD00001CDX__001", "BAGGIO"));
      m_Names.Add(new Concentratore("AD00002CDX__001", "MOLISE"));
      m_Names.Add(new Concentratore("AD00003CDX__001", "LEONCAVALLO"));
      m_Names.Add(new Concentratore("AD00004CDX__001", "GIAMBELLINO"));
      m_Names.Add(new Concentratore("AD00005CDX__001", "MESSINA"));
      m_Names.Add(new Concentratore("AD00006CDX__001", "NOVARA"));
      m_Names.Add(new Concentratore("AD00007CDX__001", "TICINESE"));
      m_Names.Add(new Concentratore("AD00008CDX__001", "SALMINI"));
      m_Names.Add(new Concentratore("AD00009CDX__001", "SARCA"));
      m_Names.Add(new Concentratore("AD00010CDX__001", "PALMANOVA"));
      m_Names.Add(new Concentratore("AD00011CDX__001", "PRECOTTO"));
      m_Names.Add(new Concentratore("AD00021CDX__001", "ABBIATEGRASSO"));
      m_Names.Add(new Concentratore("AD00022CDX__001", "CARATE"));
      m_Names.Add(new Concentratore("AD00023CDX__001", "CORSICO"));
      m_Names.Add(new Concentratore("AD00024CDX__001", "CUGGIONO"));
      m_Names.Add(new Concentratore("AD00025CDX__001", "DESIO"));
      m_Names.Add(new Concentratore("AD00026CDX__001", "MAGENTA"));
      m_Names.Add(new Concentratore("AD00027CDX__001", "MONZA"));
      m_Names.Add(new Concentratore("AD00028CDX__001", "TREZZO"));
      m_Names.Add(new Concentratore("AD00029CDX__001", "VAREDO"));
    }

    internal static String GetNomeDeposito(String QueueManagerName)
    {
      Concentratore item = m_Names.Find(x => x.m_QueueManagerName == QueueManagerName);
      if (item != null) return item.m_NomeDeposito;

      /* Not found */
      return "Sconosciuto";
    }
  }
}
