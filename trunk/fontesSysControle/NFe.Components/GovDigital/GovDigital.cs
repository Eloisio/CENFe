using System;
using System.Security.Cryptography.X509Certificates;

namespace NFe.Components.GovDigital
{
    public class GovDigital : GovDigitalBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtores
        public GovDigital(TipoAmbiente tpAmb, string pastaRetorno, X509Certificate certificate, int codMun, string proxyUser, string proxyPass, string proxyServer)
            : base(tpAmb, pastaRetorno, certificate, codMun, proxyUser, proxyPass, proxyServer)
        { }
        #endregion
    }
}
