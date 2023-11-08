namespace NFe.ConvertTxt
{

    /// pag - NFC-e

    public class pag
    {
        public TpcnIndicadorPagamento indPag = TpcnIndicadorPagamento.ipOutras;
        public TpcnFormaPagamento tPag = TpcnFormaPagamento.fpOutro;
        public double vPag;
        public string CNPJ;
        public TpcnBandeiraCartao tBand;
        public string cAut;
        public int tpIntegra;
        public double vTroco;

        public pag()
        {
            cAut = CNPJ = string.Empty;
        }
    }

}
