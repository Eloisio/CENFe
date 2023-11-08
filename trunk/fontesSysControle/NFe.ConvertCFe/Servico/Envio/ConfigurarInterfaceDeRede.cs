using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Unimake.SAT.Utility;
using EnunsSAT = Unimake.SAT.Enuns;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
     
    /// Classe responsável pela comunicação com o SAT
     
    public class ConfigurarInterfaceDeRede : ServicoBase
    {
         
        /// Dados da empresa
         
        private Empresa DadosEmpresa = null;

         
        /// Dados do envio do XML
         
        private string ConfigurarInterfaceDeRedeEnvio = null;

         
        /// Resposta do equipamento sat
         
        private Servicos.Retorno.ConfigurarInterfaceDeRedeResponse ConfigurarInterfaceDeRedeRetorno = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConfigurarInterfaceDeRede(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ConfigurarInterfaceDeRedeEnvio = doc.InnerXml;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.ConfigurarInterfaceDeRede(ConfigurarInterfaceDeRedeEnvio);
            ConfigurarInterfaceDeRedeRetorno = new Servicos.Retorno.ConfigurarInterfaceDeRedeResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConfigurarInterfaceDeRedeSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConfigurarInterfaceDeRedeSAT).RetornoXML);

            File.WriteAllText(result, ConfigurarInterfaceDeRedeRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}