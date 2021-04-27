using System.Collections.Generic;

namespace NFe.ConvertTxt
{
     
    /// Transp
     
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
