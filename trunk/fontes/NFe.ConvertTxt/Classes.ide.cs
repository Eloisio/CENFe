using NFe.Components;
using System;
using System.Collections.Generic;

namespace NFe.ConvertTxt
{

    /// Ide

    public class Ide
    {
        public int cUF { get; set; }
        public int cNF { get; set; }
        public string natOp { get; set; }
        public TpcnIndicadorPagamento indPag { get; set; }
        public TpcnMod mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
        public DateTime dEmi { get; set; }
        public DateTime dSaiEnt { get; set; }
        public DateTime hSaiEnt { get; set; }
        public TpcnTipoNFe tpNF { get; set; }
        public int cMunFG { get; set; }
        public List<NFref> NFref { get; set; }
        public TpcnTipoImpressao tpImp { get; set; }
        public TipoEmissao tpEmis { get; set; }
        public int cDV { get; set; }
        public TipoAmbiente tpAmb { get; set; }
        public TpcnFinalidadeNFe finNFe { get; set; }
        public TpcnProcessoEmissao procEmi { get; set; }
        public string verProc { get; set; }
        public string dhCont { get; set; }  //mudado para 'string' pq na versao 3 (NFC-e) deve ter o time-zone
        public string xJust { get; set; }


        /// NFC-e

        public string dhEmi { get; set; }
        public string dhSaiEnt { get; set; }
        public TpcnDestinoOperacao idDest { get; set; }
        public TpcnConsumidorFinal indFinal { get; set; }
        public TpcnPresencaComprador indPres { get; set; }

        public Ide()
        {
            dhEmi =
                dhSaiEnt =
                dhCont = string.Empty;

            idDest = TpcnDestinoOperacao.doInterna;
            indFinal = TpcnConsumidorFinal.cfConsumidorFinal;
            indPres = TpcnPresencaComprador.pcPresencial;
            tpAmb = TipoAmbiente.taHomologacao;
            tpEmis = TipoEmissao.teNormal;
            tpImp = TpcnTipoImpressao.tiRetrato;
            procEmi = TpcnProcessoEmissao.peAplicativoContribuinte;
            NFref = new List<NFref>();
        }
    }


    /// NFref

    public class NFref
    {
        public string refNFe { get; set; }
        public string refCTe { get; set; }
        public refNF refNF { get; set; }
        public refNFP refNFP { get; set; }
        public refECF refECF { get; set; }

        public NFref()
        {
            refNF = null;
            refNFP = null;
            refECF = null;
            refCTe = refNFe = string.Empty;
        }
        public NFref(string refNFe, string refCTe)
        {
            this.refNFe = refNFe;
            this.refCTe = refCTe;
        }
    }


    /// RefECF

    public class refECF
    {
        public string mod { get; set; }
        public int nECF { get; set; }
        public int nCOO { get; set; }
    }


    /// RefNF

    public class refNF
    {
        public int cUF { get; set; }
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
    }


    /// RefNFP

    public class refNFP
    {
        public int cUF { get; set; }
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string IE { get; set; }
        public string mod { get; set; }
        public int serie { get; set; }
        public int nNF { get; set; }
        public refNFP()
        {
            CNPJ = CPF = string.Empty;
        }
    }
}
