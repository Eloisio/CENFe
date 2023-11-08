using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.SAT.Conversao;
using NFe.Settings;
using System.IO;
using System.Xml;

namespace NFe.SAT.Servico.Envio
{
     
    /// Classe responsável pela conversão da NFCe para CFe para envio pelo SAT
     
    public class ConverterSAT : ServicoBase
    {
         
        /// Dados da empresa
         
        Empresa DadosEmpresa = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Caminho do arquivo XML convertido
         
        private string ArquivoConvertido { get; set; }


         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConverterSAT(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            ArquivoConvertido = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                             Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConverterSAT).EnvioXML) +
                                                                                  Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML);

            ConverterNFCe conversao = new ConverterNFCe(ArquivoXML, DadosEmpresa, ArquivoConvertido);
            conversao.ConverterSAT();
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlEnvio, Path.GetFileName(ArquivoConvertido));
            File.Move(ArquivoConvertido, result);
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
