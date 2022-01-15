using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
/******************************************************
 * Strutture che definiscono la geografia per una CNV *
 ******************************************************/
  internal class Geografia
  {
    internal byte m_Modo;		      /* Nominale / Degradato								*/
    internal ushort m_Linea;		  /* Numero linea										*/
    internal ushort m_Zona;		    /* Id zona (localita`?)								*/
    internal byte m_Trasporto;	  /* Metro/Bus/Passante...							*/
    internal byte m_NumCorsa;	    /* Numero corsa univoco in giornata ATM - 15/04/03	*/
    internal byte m_Direzione;	  /* 0/1 = normale, 8 = validazioni bloccate			*/
    internal byte m_Ecologica;	  /* Giornata Ecologica (da NFP ecodays.bin)			*/
    internal String m_NomeLocalita;		/* Nome della localita` in ascii					*/

    internal Geografia()
    {
      Clear();
    }

    internal void Clear()
    {
      m_Modo = 0;
      m_Linea = 0;
      m_Zona = 0;
      m_Trasporto = 0;
      m_NumCorsa = 0;
      m_Direzione = 0;
      m_Ecologica = 0;
      m_NomeLocalita = "";
    }
  }
}
