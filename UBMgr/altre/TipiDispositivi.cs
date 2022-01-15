using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  internal enum DispositivoId
  {
    UCB = 10,
    UB = 11,
    CNV_ME = 63,
    CNV_E = 69,
  }

  internal class Dispositivo
  {
    internal DispositivoId m_Id;
    internal String m_Descrizione;
    internal Dispositivo(DispositivoId Id, String Descrizione)
    {
      m_Id = Id;
      m_Descrizione = Descrizione;
    }
  }

  internal static class TipiDispositivi
  {
    static List<Dispositivo> m_Names = new List<Dispositivo>();

    static TipiDispositivi()
    {
      m_Names.Add(new Dispositivo(DispositivoId.UCB, "UCB"));
      m_Names.Add(new Dispositivo(DispositivoId.UB, "UB"));
      m_Names.Add(new Dispositivo(DispositivoId.CNV_ME, "CNV_ME"));
      m_Names.Add(new Dispositivo(DispositivoId.CNV_E, "CNV_E"));
    }

    internal static String GetNomeTipoDispositivo(int Id)
    {
      if (Id == 0)
      {
        return ("Non valorizzato");
      }

      Dispositivo item = m_Names.Find(x => (int)x.m_Id == Id);
      if (item != null) return item.m_Descrizione;

      /* Not found */
      return "Sconosciuto";
    }
  }
}
