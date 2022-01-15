using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  class Utils
  {

    /* Converto un numero su 2 byte */
    internal static ushort InvertoShort(ushort prima)
    {
      ushort low = (ushort)((prima >> 8) & 0x00FF);
      ushort hi  = (ushort)((prima << 8) & 0xFF00);
      return (ushort)(low | hi);
    }
  }
}
