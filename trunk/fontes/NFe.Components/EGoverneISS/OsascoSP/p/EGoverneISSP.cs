using NFe.Components.Abstract;
using NFe.Components.br.com.nfeosasco.www.p;
using System;

namespace NFe.Components.EGoverneISS.OsascoSP.p
{
    public class EGoverneISSP : EmiteNFSeBase
    {
        #region Propriedades
        NotaFiscalEletronicaServico Request = new NotaFiscalEletronicaServico();


        /// Namespace utilizada para deserialização do objeto

        public override string NameSpaces
        {
            get
            {
                return "http://schemas.datacontract.org/2004/07/Eissnfe.Negocio.WebServices.Mensagem";
            }
        }
        #endregion

        #region Construtores
        public EGoverneISSP(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            ProxyUser = proxyuser;
            ProxyPass = proxypass;
            ProxyServer = proxyserver;

            DefinirProxy<NotaFiscalEletronicaServico>(Request);
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            EmissaoNotaFiscalRequest notaFiscal = DeserializarObjeto<EmissaoNotaFiscalRequest>(file);

            notaFiscal.NotaFiscal.Homologacao = tpAmb == TipoAmbiente.taHomologacao ? true : false;

            string result = SerializarObjeto(Request.Emitir(notaFiscal));

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            CancelamentoNotaFiscalRequest notaFiscal = DeserializarObjeto<CancelamentoNotaFiscalRequest>(file);
            notaFiscal.Homologacao = tpAmb == TipoAmbiente.taHomologacao ? true : false;

            string result = SerializarObjeto(Request.Cancelar(notaFiscal));

            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarNfse(string file)
        {
            throw new NotImplementedException();
        }

        public override void ConsultarNfsePorRps(string file)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
