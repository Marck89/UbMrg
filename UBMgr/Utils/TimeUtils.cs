using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sbme
{
  internal class TimeUtils
  {
    internal static DateTime ConvertFromUnixTimestamp(int Timestamp)
    {
      if (Timestamp < 0) return DateTime.MinValue;
      DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(Timestamp);
      return dateTime.ToLocalTime();
    }

    internal static int ConvertToUnixTimestamp(DateTime _DateTime)
    {
      if (_DateTime == DateTime.MinValue) return 0;

      DateTime dtL = new DateTime(1970, 1, 1);
      DateTime dateTimeL = _DateTime.ToUniversalTime();
      TimeSpan span = dateTimeL.Subtract(dtL);
      return (int)span.TotalSeconds;
    }

    internal static int GetTime()
    {
      return ConvertToUnixTimestamp(DateTime.Now);
    }

    /* Restituisce un tempo dall'avvio dall'avvio in *millesimi* di secondo */
    internal static int TempoInMillesimi()
    {
      int result = Environment.TickCount & Int32.MaxValue;
      return result;
    }

    /* Restituisce un tempo dall'avvio in *centesimi* di secondo */
    internal static int TempoInCentesimi()
    {
      int result = TempoInMillesimi() / 10;
      return result;
    }

    /* Restituisce un tempo dall'avvio in minuti */
    internal static int TempoInMinuti()
    {
      long dummy = TempoInCentesimi();

      return ((int)(dummy / 6000)); /* 6000 centesimi = 1 minuto */
    }

    /* Funzioni per gestire Data e Ora */
    internal static String DecodeTime(int TotSecondi)
    {
      String str = "";

      int totMinuti, secondi, minuti, ore;

      totMinuti = TotSecondi / 60;
      secondi = (TotSecondi % 60);
      minuti = (totMinuti % 60);
      ore = totMinuti / 60;

      str = ore.ToString("00") + ":" + minuti.ToString("00") + ":" + secondi.ToString("00");
      return str;
    }

    internal static String DecodeDateTime(int TotSecondi)
    {
      String str = "";
      DateTime dateTime = ConvertFromUnixTimestamp(TotSecondi);
      if (dateTime != DateTime.MinValue)
      {
        str = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
      }
      else
      {
        str = "Error";
      }

      return str;
    }

    /* Sospende il processo per il tempo richiesto. Sostituisce la sleep e usleep */
    internal static bool SospendiInCentesimi(int Centesimi)
    {
      return SospendiInMillesimi(Centesimi * 10);
    }

    /* Sospende il processo per il tempo richiesto. Sostituisce la sleep e usleep */
    internal static bool SospendiInMillesimi(int Millesimi)
    {
      if (Millesimi <= 0) return false;
      Thread.Sleep(Millesimi);
      return true;
    }

  }
}
