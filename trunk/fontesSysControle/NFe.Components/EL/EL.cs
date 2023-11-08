using System;

namespace NFe.Components.EL
{
    public class EL : ELBase
    {
        public override string NameSpaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Construtures
        public EL(TipoAmbiente tpAmb, string pastaRetorno, int codMun, string usuarioWs, string senhaWs, string usuarioProxy, string senhaProxy, string domainProxy)
            : base(tpAmb, pastaRetorno, codMun, usuarioWs, senhaWs, usuarioProxy, senhaProxy, domainProxy)
        { }
        #endregion
    }
}
