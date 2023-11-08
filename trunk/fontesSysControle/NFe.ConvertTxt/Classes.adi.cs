using System;
using System.Collections.Generic;

namespace NFe.ConvertTxt
{
     
    /// Adi
     
    public class Adi
    {
        public int nAdicao;
        public int nSeqAdi;
        public string cFabricante;
        public double vDescDI;
        public string nDraw;
    }

     
    /// DI
     
    public class DI
    {
        public string nDI;
        public DateTime dDI;
        public string xLocDesemb;
        public string UFDesemb;
        public DateTime dDesemb;
        public string cExportador;
        public TpcnTipoViaTransp tpViaTransp;
        public double vAFRMM;
        public TpcnTipoIntermedio tpIntermedio;
        public string CNPJ;
        public string UFTerceiro;
        public List<Adi> adi;

        public DI()
        {
            adi = new List<Adi>();
        }
    }
}
