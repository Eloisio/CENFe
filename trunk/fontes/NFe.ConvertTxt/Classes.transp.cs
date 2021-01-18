using System.Collections.Generic;

namespace NFe.ConvertTxt
{
    /// <summary>
    /// Transp
    /// </summary>
    public class Transp
    {
        public TpcnModalidadeFrete modFrete;
        public Transporta Transporta;
        public retTransp retTransp;
        public veicTransp veicTransp;
        public List<Vol> Vol;
        public List<Reboque> Reboque;
        public string vagao;
        public string balsa;

        public Transp()
        {
            Vol = new List<Vol>();
            Reboque = new List<Reboque>();
        }
    }
}
