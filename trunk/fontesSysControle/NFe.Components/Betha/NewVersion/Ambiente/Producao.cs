using NFe.Components.Abstract;
using NFe.Components.br.com.betha.egov.p;
using System;
using System.IO;
using System.Net;
using System.Xml;

namespace NFe.Components.Betha.NewVersion.Ambiente
{
    public class Producao : EmiteNFSeBase, IAmbiente
    {
        #region Propriedades
         
        /// Objeto de conexão com o Webservice
         
        private NfseWSService Service = new NfseWSService();

         
        /// Cabeçalho do soap
         
        private string CabecMsg = "<cabecalho xmlns=\"http://www.betha.com.br/e-nota-contribuinte-ws\" versao=\"2.02\"><versaoDados>2.02</versaoDados></cabecalho>";

         
        /// XML em string
         
        private string XmlString = "";

         
        /// Namespace utilizada para deserialização do objeto
         
        public override string NameSpaces
        {
            get
            {
                return "http://www.betha.com.br/e-nota-contribuinte-ws";
            }
        }
        #endregion

        #region Construtores
         
        /// Construtor
         
        /// <param name="tpAmb"></param>
        /// <param name="pastaRetorno"></param>
        /// <param name="usuario"></param>
        /// <param name="senhaWs"></param>
        /// <param name="proxyuser"></param>
        /// <param name="proxypass"></param>
        /// <param name="proxyserver"></param>
        public Producao(TipoAmbiente tpAmb, string pastaRetorno, string usuario, string senhaWs, string proxyuser, string proxypass, string proxyserver)
            : base(tpAmb, pastaRetorno)
        {
            if (!string.IsNullOrEmpty(proxyuser))
            {
                NetworkCredential credentials = new System.Net.NetworkCredential(proxyuser, proxypass, proxyserver);
                WebRequest.DefaultWebProxy.Credentials = credentials;

                Service.Proxy = WebRequest.DefaultWebProxy;
                Service.Proxy.Credentials = new NetworkCredential(proxyuser, proxypass);
                Service.Credentials = new NetworkCredential(proxyuser, proxypass);
            }
        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(file);

            RecepcionarLoteRps envio = new RecepcionarLoteRps
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = xml.InnerXml
            };

            string result = Service.RecepcionarLoteRps(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public void EmiteNFSincrono(string file)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(file);

            RecepcionarLoteRpsSincrono envio = new RecepcionarLoteRpsSincrono
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = xml.InnerXml
            };

            string result = Service.RecepcionarLoteRpsSincrono(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.EnvLoteRps).RetornoXML);
        }

        public override void CancelarNfse(string file)
        {
            ReadXML(file);

            CancelarNfse envio = new CancelarNfse
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = XmlString
            };

            string result = Service.CancelarNfse(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedCanNFSe).RetornoXML);
        }

        public override void ConsultarLoteRps(string file)
        {
            ReadXML(file);

            ConsultarLoteRps envio = new ConsultarLoteRps
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = XmlString
            };

            string result = Service.ConsultarLoteRps(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedLoteRps).RetornoXML);

        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            ReadXML(file);

            ConsultarNfseFaixa envio = new ConsultarNfseFaixa
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = XmlString
            };

            string result = Service.ConsultarNfseFaixa(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSe).RetornoXML);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            ReadXML(file);

            ConsultarNfsePorRps envio = new ConsultarNfsePorRps
            {
                nfseCabecMsg = CabecMsg,
                nfseDadosMsg = XmlString
            };

            string result = Service.ConsultarNfsePorRps(envio).@return;
            GerarRetorno(file, result, Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).EnvioXML,
                                       Propriedade.Extensao(Propriedade.TipoEnvio.PedSitNFSeRps).RetornoXML);
        }

        private void ReadXML(string file)
        {
            StreamReader SR = null;
            try
            {
                SR = File.OpenText(file);
                XmlString = SR.ReadToEnd();

            }
            catch (Exception)
            {
                throw new Exceptions.ProblemaLeituraXML(file);
            }
            finally
            {
                SR.Close();
                SR = null;
            }
        }
        #endregion


    }
}
