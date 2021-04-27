using System;
using System.Collections.Generic;

namespace NFe.ConvertTxt
{
     
    /// Cobr
     
    public class Cobr
    {
        public Fat Fat;
        public List<Dup> Dup;

        public Cobr()
        {
            Dup = new List<Dup>();
        }
    }

     
    /// Dup
     
    public class Dup
    {
        public string nDup;
        public DateTime dVenc;
        public double vDup;
    }

     
    /// Fat
     
    public struct Fat
    {
        public string nFat;
        public double vOrig;
        public double vDesc;
        public double vLiq;
    }

}
