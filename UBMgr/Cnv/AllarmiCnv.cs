using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/* 
** Struttura simile a T_Sua095EvtAlarme. Ridefinita perche' gli eventi mi arrivano
** come buffer da analizzare, quindi se devo lavorarci almeno lo faccio con i nomi
** in italiano. Questi allarmi mi arrivano dalla CNV sia come eventi che come stato 
*/
  internal class AllarmeCnv
  {
    internal UInt16 m_CodSottoinsieme = 0;
    internal UInt16 m_CodGuasto = 0;
    internal char m_StDispositivo = ' ';
    internal UInt16 m_Gravita = 0;

    internal AllarmeCnv()
    {
      Clear();
    }

    internal void Clear()
    {
      m_CodSottoinsieme = 0;
      m_CodGuasto = 0;
      m_StDispositivo = ' ';
      m_Gravita = 0;
    }
  }

  internal class AllarmiCnv
  {
    internal const int MAX_ALLARMI_CNV = 64;

    internal uint m_SerialeCnv = 0;					 /* Ultimo numero seriale noto per CNV */
    internal int m_Count = 0;						     /* Numero di allarmi attivi */
    internal AllarmeCnv[] m_Allarmi = null;  /* Allarmi attivi al momento */

    internal AllarmiCnv()
    {
      m_Allarmi = new AllarmeCnv[MAX_ALLARMI_CNV];
      for (int i = 0; i < MAX_ALLARMI_CNV; i++)
      {
        m_Allarmi[i] = new AllarmeCnv();
      }
    }

    internal void Clear()
    {
      for (int i = 0; i < MAX_ALLARMI_CNV; i++)
      {
        m_Allarmi[i].Clear();
      }
    }
  }
}
