using NFe.Settings;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace NFe.Certificado
{
    public class CertificadoProviders
    {
        #region Propriedades

        /// Propriedade que identifica se foi possível identificar um provider valido

        public bool ProviderIdentificado { get; private set; }

        /// Dados referente ao provider valido

        public CertProviders ProviderValido { get; private set; }

        /// Propriedade referente ao certificado que será testado

        public bool IsA3 { get; private set; }

        private CertificadoDigital oCertificado = new CertificadoDigital();
        private X509Certificate2 Certificado { get; set; }
        private List<CertProviders> ProvidersIdentificados { get; set; }
        private string TempFile { get; set; }
        private int CodEmp { get; set; }
        private string PIN { get; set; }
        #endregion

        #region Construtores
        public CertificadoProviders(X509Certificate2 certificado,
                                    string folderTemp,
                                    int codEmp,
                                    string pin)
        {
            Certificado = certificado;
            TempFile = folderTemp + "\\Temp\\Simulacao.xml";
            CodEmp = codEmp;
            PIN = pin;
            IsA3 = Certificado.IsA3();
            ProviderIdentificado = false;

            if (!Directory.Exists(folderTemp + "\\temp"))
                Directory.CreateDirectory(Path.GetFullPath(folderTemp + "\\temp"));
        }
        #endregion

        #region Metodos

        /// Define a propriedade com o provider que deverá ser sugerida ao usuario

        private void GetProviderValido()
        {
            for (int i = 0; i < ProvidersIdentificados.Count; i++)
            {
                if (TestarProvider(ProvidersIdentificados[i]))
                {
                    ProviderValido = ProvidersIdentificados[i];
                    ProviderIdentificado = true;
                    break;
                }
            }
        }


        /// Realiza efetivamente o teste assinando o XML de simualação

        /// <param name="provider">Objeto com as informaçoes do provider que será testado</param>
        /// <returns></returns>
        public bool TestarProvider(CertProviders provider)
        {
            bool result = false;

            SimularXML(provider);
            AssinaturaDigital assinatura = new AssinaturaDigital();
            assinatura.TesteCertificado = true;
            result = assinatura.TestarProviderCertificado(TempFile,
                "SimulacaoProvider",
                "Provider",
                Certificado,
                CodEmp,
                PIN);

            ApagarXMLTeste();

            return result;
        }


        /// Apagar arquivo XML de teste para assinatura

        private void ApagarXMLTeste()
        {
            if (File.Exists(TempFile))
                File.Delete(TempFile);
        }


        /// Gera um XML para realizar a simulação do teste da assinatura

        /// <param name="provider">Objeto CertProviders que contem o provider que será testado</param>
        /// <author>Renan Borges</author>
        private void SimularXML(CertProviders provider)
        {
            Empresas.CriarPasta(true);

            ApagarXMLTeste();

            XElement docElement = new XElement("SimulacaoProvider");
            XElement eProvider = new XElement("Provider");

            eProvider.Add(new XElement("Nome", provider.NameKey));
            eProvider.Add(new XElement("Type", provider.Type));

            docElement.Add(eProvider);

            docElement.Save(TempFile);
        }


        /// Busca uma lista de providers que podem ser utilizados pelo certificado

        /// <author>Renan Borges</author>
        public void GetProviders()
        {
            ProvidersIdentificados = oCertificado.GetListProviders();
        }


        /// Busca os types dos providers encontrados

        /// <author>Renan Borges</author>
        public void GetProvidersType()
        {
            for (int i = 0; i < ProvidersIdentificados.Count; i++)
            {
                ProvidersIdentificados[i].Type = oCertificado.GetInfoProvider(ProvidersIdentificados[i].NameKey.ToString()).Type;
            }
        }
        #endregion
    }
}
