using System;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.Metropolis
{
    public class Metropolis : MetropolisBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public Metropolis(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string proxyserver, string proxyuser, string proxypass, X509Certificate2 certificado)
            : base(tpAmb, pastaRetorno, codMun, proxyserver, proxyuser, proxypass, certificado)
        { }
        #endregion
    }
}
