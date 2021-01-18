namespace NFe.ConvertTxt
{
    /// <summary>
    /// Dest
    /// </summary>
    public class Dest
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
        public enderDest enderDest;
        public string IE;
        public string IM;
        public string ISUF;
        public string email;
        public TpcnindIEDest indIEDest;

        /// <summary>
        /// NFC-e
        /// </summary>
        public string idEstrangeiro;

        public Dest()
        {
            idEstrangeiro = CNPJ = CPF = string.Empty;
            indIEDest = TpcnindIEDest.inContribuinte;
            enderDest = new enderDest();
        }
    }

    /// <summary>
    /// enderDest
    /// </summary>
    public class enderDest
    {
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public int cMun;
        public string xMun;
        public string UF;
        public int CEP;
        public int cPais;
        public string xPais;
        public string fone;

        public enderDest()
        {
            fone = nro = UF = xBairro = xCpl = xLgr = xMun = xPais = "";
        }
    }

}
