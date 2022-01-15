using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sbme
{
  /* Lo stato applicativo è una evoluzione (sempre crescente) da SCONOSCIUTO a OPERATIVA */
  enum CNV_StatoApplicativo
  {
    CNV_STATO_APPLICATIVO_SCONOSCIUTO = -1,
    CNV_STATO_APPLICATIVO_ASSENTE,
    CNV_STATO_APPLICATIVO_CONTATTATA,
    CNV_STATO_APPLICATIVO_IDENTIFICATA,
    CNV_STATO_APPLICATIVO_MANCANZA_SOFTWARE,
    CNV_STATO_APPLICATIVO_INVIO_SOFTWARE,
    CNV_STATO_APPLICATIVO_MANCANZA_NFP,
    CNV_STATO_APPLICATIVO_INVIO_FAMILLE,
    CNV_STATO_APPLICATIVO_INVIO_NFP,
    CNV_STATO_APPLICATIVO_CONFIGURATA,
    CNV_STATO_APPLICATIVO_OPERATIVA,
    CNV_STATO_APPLICATIVO_FLUSH,
  };


  internal class StatoCnv
  {
    private const int m_DummyVar17Size = 33;
    private const int m_DummyVar18Size = 33;

    internal int m_DataOraInizioMisurazione;
    internal int m_DataOraFineMisurazione;

    /* Dati di configurazione */
    internal uint m_IdOperatore;
    internal uint m_IdClasse;
    internal uint m_IdNumerico;
    internal uint m_SerialNumber;
    internal uint m_Tipo;
    internal uint m_Sottotipo;
    internal uint m_TipoSoftware;
    internal uint m_SoftwareMajor;
    internal uint m_SoftwareMinor;

    internal int m_StatoApplicativo;							/* N.B. Lo stato applicativo è una evoluzione (sempre crescente) da SCONOSCIUTO a OPERATIVA */
    internal int m_StatoDiagnostico;							/* Notificato al CDD */
    internal int m_CodiceUltimoGuasto;

    /* Dati su Raggiungibilita */
    internal int m_StatoUltimoContatto;
    internal int m_NumeroCambiStatoContatto;
    internal int m_NumeroPassaggiInStatoConnesso;
    internal int m_NumeroPassaggiInStatoSconnesso;
    internal int m_IstanteUltimoCambioStatoContatto;			/* Data/ora */
    internal int m_DurataTotaleTempoSconnessione;				/* In secondi */
    internal int m_DurataTotaleTempoConnessione;				/* In secondi */
    internal int m_NumeroFermiProlungati;						/* "Fermo prolungato": Se rimane ferma per più di 180 sec */
    internal bool m_AttualmenteInFermoProlungato;				/* Dice se adesso è in "Fermo prolungato" (180 sec) */
    internal int m_NumeroFermiBrevi;							/* "Fermo breve": Se rimane ferma per più di 90 sec */
    internal bool m_AttualmenteInFermoBreve;					/* Dice se adesso è in "Fermo beve" (90 sec) */

    /* Dati su stato Servizio */
    internal int m_StatoUltimoServizio;
    internal int m_IstanteUltimoCambioStatoServizio;			/* Data/ora */
    internal int m_IstanteComandoEntrataInServizio;			/* Data/ora */
    internal int m_RitardoMassimoEntrataInServizio;			/* In secondi */
    internal int m_RitardoMinimoEntrataInServizio;				/* In secondi */
    internal int m_DurataTotaleTempoServizioApertoNormale;		/* In secondi */
    internal int m_DurataTotaleTempoServizioApertoDegradato;	/* In secondi */
    internal int m_DurataTotaleTempoServizioChiuso;			/* In secondi */
    internal int m_DurataTotaleTempoServizioSconosciuto;		/* In secondi */
    internal int m_NumeroApertureServizioAssolute;
    internal int m_NumeroChiusureServizio;
    internal int m_NumeroEventiConvalida;

    /* Dati generici */
    internal int m_PeriodoQuarantenaTentativiConnessione;
    internal uint m_UltimoSerialNumberConosciuto;

    /* Variabili per uso futuro */
    internal uint m_DummyVar01;									/* Uso futuro */
    internal uint m_DummyVar02;									/* Uso futuro */
    internal uint m_DummyVar03;									/* Uso futuro */
    internal uint m_DummyVar04;									/* Uso futuro */
    internal uint m_DummyVar05;									/* Uso futuro */
    internal int m_NumeroApertureServizioRelative;
    internal int m_DummyVar07;									/* Uso futuro */
    internal int m_DummyVar08;									/* Uso futuro */
    internal int m_DummyVar09;									/* Uso futuro */
    internal int m_DummyVar10;									/* Uso futuro */
    internal int m_DummyVar11;									/* Uso futuro */
    internal bool m_PresenteServizioPrecedente;
    internal bool m_DummyVar13;									/* Uso futuro */
    internal bool m_DummyVar14;									/* Uso futuro */
    internal bool m_DummyVar15;									/* Uso futuro */
    internal bool m_DummyVar16;									/* Uso futuro */
    internal String m_DummyVar17 = "";			    /* Uso futuro */
    internal String m_DummyVar18 = "";	 	   	  /* Uso futuro */
      
    internal StatoCnv()
    {
      Clear();
    }

    internal void Clear()
    {
      m_DataOraInizioMisurazione = 0;
      m_DataOraFineMisurazione = 0;

      /* Dati di configurazione */
      m_IdOperatore = 0;
      m_IdClasse = 0;
      m_IdNumerico = 0;
      m_SerialNumber = 0;
      m_Tipo = 0;
      m_Sottotipo = 0;
      m_TipoSoftware = 0;
      m_SoftwareMajor = 0;
      m_SoftwareMinor = 0;

      m_StatoApplicativo = 0;
      m_StatoDiagnostico = 0;
      m_CodiceUltimoGuasto = 0;

      /* Dati su Raggiungibilita */
      m_StatoUltimoContatto = 0;
      m_NumeroCambiStatoContatto = 0;
      m_NumeroPassaggiInStatoConnesso = 0;
      m_NumeroPassaggiInStatoSconnesso = 0;
      m_IstanteUltimoCambioStatoContatto = 0;
      m_DurataTotaleTempoSconnessione = 0;
      m_DurataTotaleTempoConnessione = 0;
      m_NumeroFermiProlungati = 0;
      m_AttualmenteInFermoProlungato = false;
      m_NumeroFermiBrevi = 0;
      m_AttualmenteInFermoBreve = false;

      /* Dati su stato Servizio */
      m_StatoUltimoServizio = 0;
      m_IstanteUltimoCambioStatoServizio = 0;
      m_IstanteComandoEntrataInServizio = 0;
      m_RitardoMassimoEntrataInServizio = 0;
      m_RitardoMinimoEntrataInServizio = 0;
      m_DurataTotaleTempoServizioApertoNormale = 0;
      m_DurataTotaleTempoServizioApertoDegradato = 0;
      m_DurataTotaleTempoServizioChiuso = 0;
      m_DurataTotaleTempoServizioSconosciuto = 0;
      m_NumeroApertureServizioAssolute = 0;
      m_NumeroChiusureServizio = 0;
      m_NumeroEventiConvalida = 0;

      /* Dati generici */
      m_PeriodoQuarantenaTentativiConnessione = 0;
      m_UltimoSerialNumberConosciuto = 0;

      /* Variabili per uso futuro */
      m_DummyVar01 = 0;
      m_DummyVar02 = 0;
      m_DummyVar03 = 0;
      m_DummyVar04 = 0;
      m_DummyVar05 = 0;
      m_NumeroApertureServizioRelative = 0;
      m_DummyVar07 = 0;
      m_DummyVar08 = 0;
      m_DummyVar09 = 0;
      m_DummyVar10 = 0;
      m_DummyVar11 = 0;
      m_PresenteServizioPrecedente = false;
      m_DummyVar13 = false;
      m_DummyVar14 = false;
      m_DummyVar15 = false;
      m_DummyVar16 = false;
      m_DummyVar17 = "";
      m_DummyVar18 = "";
    }

    internal bool Write(BinaryWriter bw)
    {
      bool rst = false;

      try
      {
        int nPad = FileUtils.WritePadding(bw);

        bw.Write( m_DataOraInizioMisurazione);
        bw.Write( m_DataOraFineMisurazione);
        bw.Write( m_IdOperatore);
        bw.Write( m_IdClasse);
        bw.Write( m_IdNumerico);
        bw.Write( m_SerialNumber);
        bw.Write( m_Tipo);
        bw.Write( m_Sottotipo);
        bw.Write(m_TipoSoftware);
        bw.Write(m_SoftwareMajor);
        bw.Write( m_SoftwareMinor);
        bw.Write( m_StatoApplicativo);
        bw.Write( m_StatoDiagnostico);
        bw.Write( m_CodiceUltimoGuasto);
        bw.Write( m_NumeroCambiStatoContatto);
        bw.Write( m_NumeroPassaggiInStatoConnesso);
        bw.Write( m_NumeroPassaggiInStatoSconnesso);
        bw.Write( m_IstanteUltimoCambioStatoContatto);
        bw.Write( m_DurataTotaleTempoSconnessione);
        bw.Write( m_DurataTotaleTempoConnessione);
        bw.Write( m_NumeroFermiProlungati);
        bw.Write( m_AttualmenteInFermoProlungato);
        FileUtils.WritePadding(bw);
        bw.Write( m_NumeroFermiBrevi);
        bw.Write( m_AttualmenteInFermoBreve);
        FileUtils.WritePadding(bw);

        bw.Write( m_StatoUltimoServizio);
        bw.Write( m_IstanteUltimoCambioStatoServizio);
        bw.Write( m_IstanteComandoEntrataInServizio);
        bw.Write( m_RitardoMassimoEntrataInServizio);
        bw.Write( m_RitardoMinimoEntrataInServizio);
        bw.Write( m_DurataTotaleTempoServizioApertoNormale);
        bw.Write( m_DurataTotaleTempoServizioApertoDegradato);
        bw.Write( m_DurataTotaleTempoServizioChiuso);
        bw.Write( m_DurataTotaleTempoServizioSconosciuto);
        bw.Write( m_NumeroApertureServizioAssolute);
        bw.Write( m_NumeroChiusureServizio);
        bw.Write( m_NumeroEventiConvalida);
        bw.Write( m_PeriodoQuarantenaTentativiConnessione);
        bw.Write( m_UltimoSerialNumberConosciuto);
        bw.Write( m_DummyVar01);
        bw.Write( m_DummyVar02);
        bw.Write( m_DummyVar03);
        bw.Write( m_DummyVar04);
        bw.Write( m_DummyVar05);
        bw.Write( m_NumeroApertureServizioRelative);
        bw.Write( m_DummyVar07);
        bw.Write( m_DummyVar08);
        bw.Write( m_DummyVar09);
        bw.Write( m_DummyVar10);
        bw.Write( m_DummyVar11);
        bw.Write( m_PresenteServizioPrecedente);
        bw.Write( m_DummyVar13);
        bw.Write( m_DummyVar14);
        bw.Write( m_DummyVar15);
        bw.Write( m_DummyVar16);

        String str = m_DummyVar17.PadLeft(m_DummyVar17Size, '\0');
        bw.Write( str);

        str = m_DummyVar18.PadLeft(m_DummyVar18Size, '\0');
        bw.Write(str);

        nPad = FileUtils.WritePadding(bw);

        rst = true;

      }
      catch (Exception)
      {
        rst = false;
      }
      return rst;
    }

    internal bool Read(BinaryReader br)
    {
      bool rst = false;
      try
      {
        int nPad = FileUtils.ReadPadding(br);

        byte[] bytes = null;

        m_DataOraInizioMisurazione = br.ReadInt32();
        m_DataOraFineMisurazione = br.ReadInt32();

        m_IdOperatore = br.ReadUInt32();
        m_IdClasse = br.ReadUInt32();
        m_IdNumerico = br.ReadUInt32();
        m_SerialNumber = br.ReadUInt32();
        m_Tipo = br.ReadUInt32();
        m_Sottotipo = br.ReadUInt32();
        m_TipoSoftware = br.ReadUInt32();
        m_SoftwareMajor = br.ReadUInt32();
        m_SoftwareMinor = br.ReadUInt32();
        m_StatoApplicativo = br.ReadInt32();

        m_StatoDiagnostico = br.ReadInt32();
        m_CodiceUltimoGuasto = br.ReadInt32();

        m_StatoUltimoContatto = br.ReadInt32();
        m_NumeroCambiStatoContatto = br.ReadInt32();
        m_NumeroPassaggiInStatoConnesso = br.ReadInt32();
        m_NumeroPassaggiInStatoSconnesso = br.ReadInt32();
        m_IstanteUltimoCambioStatoContatto = br.ReadInt32();
        m_DurataTotaleTempoSconnessione = br.ReadInt32();
        m_DurataTotaleTempoConnessione = br.ReadInt32();
        m_NumeroFermiProlungati = br.ReadInt32();
        m_AttualmenteInFermoProlungato = br.ReadBoolean();
        FileUtils.ReadPadding(br);
        m_NumeroFermiBrevi = br.ReadInt32();
        m_AttualmenteInFermoBreve = br.ReadBoolean();
        FileUtils.ReadPadding(br);

        m_StatoUltimoServizio = br.ReadInt32();
        m_IstanteUltimoCambioStatoServizio = br.ReadInt32();
        m_IstanteComandoEntrataInServizio = br.ReadInt32();
        m_RitardoMassimoEntrataInServizio = br.ReadInt32();
        m_RitardoMinimoEntrataInServizio = br.ReadInt32();
        m_DurataTotaleTempoServizioApertoNormale = br.ReadInt32();
        m_DurataTotaleTempoServizioApertoDegradato = br.ReadInt32();
        m_DurataTotaleTempoServizioChiuso = br.ReadInt32();
        m_DurataTotaleTempoServizioSconosciuto = br.ReadInt32();
        m_NumeroApertureServizioAssolute = br.ReadInt32();
        m_NumeroChiusureServizio = br.ReadInt32();
        m_NumeroEventiConvalida = br.ReadInt32();

        m_PeriodoQuarantenaTentativiConnessione = br.ReadInt32();
        m_UltimoSerialNumberConosciuto = br.ReadUInt32();

        m_DummyVar01 = br.ReadUInt32();
        m_DummyVar02 = br.ReadUInt32();
        m_DummyVar03 = br.ReadUInt32();
        m_DummyVar04 = br.ReadUInt32();
        m_DummyVar05 = br.ReadUInt32();
        m_NumeroApertureServizioRelative = br.ReadInt32();
        m_DummyVar07 = br.ReadInt32();
        m_DummyVar08 = br.ReadInt32();
        m_DummyVar09 = br.ReadInt32();
        m_DummyVar10 = br.ReadInt32();
        m_DummyVar11 = br.ReadInt32();
        m_PresenteServizioPrecedente = br.ReadBoolean();
        m_DummyVar13 = br.ReadBoolean();
        m_DummyVar14 = br.ReadBoolean();
        m_DummyVar15 = br.ReadBoolean();
        m_DummyVar16 = br.ReadBoolean();

        bytes = br.ReadBytes(m_DummyVar17Size);
        m_DummyVar17 = Encoding.UTF8.GetString(bytes);

        bytes = br.ReadBytes(m_DummyVar18Size);
        m_DummyVar18 = Encoding.UTF8.GetString(bytes);

        nPad = FileUtils.ReadPadding( br);

        rst = true;
      }
      catch (Exception)
      {
        rst = false;
      }
      return rst;
    }

    internal void Init(bool PrimaInizializzazione, int TimeNow)
    {
      m_DataOraInizioMisurazione = TimeNow;

      if (PrimaInizializzazione == true)
      {
        /* Questo viene eseguito solo all'avvio applicativo */

        m_SerialNumber = 0;
        m_Tipo = 0;
        m_Sottotipo = 0;
        m_UltimoSerialNumberConosciuto = 0;
      }

      m_StatoUltimoContatto = (int)CNV_StatoContatto.CNV_STATO_CONTATTO_SCONOSCIUTO;
      m_NumeroCambiStatoContatto = 0;
      m_IstanteUltimoCambioStatoContatto = TimeNow;
      m_NumeroPassaggiInStatoSconnesso = 0;
      m_NumeroPassaggiInStatoConnesso = 0;
      m_DurataTotaleTempoSconnessione = 0;
      m_DurataTotaleTempoConnessione = 0;
      m_NumeroFermiProlungati = 0;
      m_AttualmenteInFermoProlungato = false;
      m_NumeroFermiBrevi = 0;
      m_AttualmenteInFermoBreve = false;

      m_StatoUltimoServizio = (int)CNV_StatoServizio.CNV_STATO_SERVIZIO_SCONOSCIUTO;
      m_NumeroApertureServizioAssolute = 0;
      m_NumeroApertureServizioRelative = 0;
      m_NumeroChiusureServizio = 0;
      m_IstanteUltimoCambioStatoServizio = TimeNow;
      m_DurataTotaleTempoServizioSconosciuto = 0;
      m_DurataTotaleTempoServizioApertoNormale = 0;
      m_DurataTotaleTempoServizioApertoDegradato = 0;
      m_DurataTotaleTempoServizioChiuso = 0;

      m_IstanteComandoEntrataInServizio = TimeNow;
      m_RitardoMassimoEntrataInServizio = 0;
      m_RitardoMinimoEntrataInServizio = 24 * 3600; /* Un giorno */

      m_NumeroEventiConvalida = 0;

      m_PresenteServizioPrecedente = true;	/* Normalmente assumo : presente */
      m_PeriodoQuarantenaTentativiConnessione = 0;

      m_StatoApplicativo = (int)CNV_StatoApplicativo.CNV_STATO_APPLICATIVO_SCONOSCIUTO;
      m_StatoDiagnostico = (int)CdxStatoDiagnostico.CDXMSG_NON_RAGGIUNGIBILE;

      m_CodiceUltimoGuasto = 0;
	
    }

  }
}
