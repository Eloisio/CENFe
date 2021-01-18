namespace NFe.Components
{
    public class Municipio
    {
        public int CodigoMunicipio { get; set; }
        public string UF { get; set; }
        public string Nome { get; set; }
        public string PadraoStr { get; set; }
        public PadroesNFSe Padrao { get; set; }
        public TipoEmissao svc { get; set; }

        public Municipio() { }
        public Municipio(int _cod, string _uf, string _nome, PadroesNFSe _padrao)
        {
            Nome = _nome;
            if (!_nome.Trim().EndsWith(" - " + _uf))
                Nome = _nome.Trim() + " - " + _uf;
            CodigoMunicipio = _cod;
            Padrao = _padrao;
            PadraoStr = _padrao.ToString();
            UF = _uf;
        }
        public override string ToString()
        {
            return UF + " - " + Nome;
        }
    }
}
