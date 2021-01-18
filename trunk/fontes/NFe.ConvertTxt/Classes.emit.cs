namespace NFe.ConvertTxt
{
    /// <summary>
    /// Emit
    /// </summary>
    public class Emit
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public enderEmit enderEmit { get; set; }
        public string IE { get; set; }
        public string IEST { get; set; }
        public string IM { get; set; }
        public string CNAE { get; set; }
        public TpcnCRT CRT { get; set; }

        public Emit()
        {
            CNPJ = CPF = string.Empty;
            CRT = TpcnCRT.crtRegimeNormal;
            enderEmit = new enderEmit();
        }
    }

    /// <summary>
    /// enderEmit
    /// </summary>
    public class enderEmit
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
        public int cMun { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
        public int CEP { get; set; }
        public int cPais { get; set; }
        public string xPais { get; set; }
        public string fone { get; set; }

        public enderEmit()
        {
            fone = nro = UF = xBairro = xCpl = xLgr = xMun = xPais = "";
        }
    }
}
