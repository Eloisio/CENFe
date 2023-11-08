namespace NFe.ConvertTxt
{
     
    /// Det
     
    public class Det
    {
        public Prod Prod;
        public Imposto Imposto;
        public impostoDevol impostoDevol;
        public string infAdProd;

        public Det()
        {
            Prod = new Prod();
            Imposto = new Imposto();
            impostoDevol = new impostoDevol();
        }
    }

}
