using System.Collections.Generic;

namespace NFe.ConvertTxt
{
     
    /// Vol
     
    public class Vol
    {
        public int qVol;
        public string esp;
        public string marca;
        public string nVol;
        public double pesoL;
        public double pesoB;
        public List<Lacres> Lacres;

        public Vol()
        {
            Lacres = new List<Lacres>();
        }
    }
}
