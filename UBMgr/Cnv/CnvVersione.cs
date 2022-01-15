using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* Parametri SW per ogni convalidatrice */
  internal class CnvVersione
  {
    internal UInt16 m_Tipo = 0;
    internal UInt16 m_Sottotipo = 0;
    internal UInt16 m_Sw_type = 0;	/* 0: Sconosciuto, 1 : Boot Loader, 2 : Software CNV */
    internal UInt16 m_Major = 0;
    internal UInt16 m_Minor = 0;
    internal UInt16 m_Nfp = 0;			/* versione file parametri */
    internal int m_Seriale = 0;
  }
}
