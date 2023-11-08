using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
     
    /// Classe responsável pela comunicação com o SAT
     
    public class ConsultarStatusOperacional : ServicoBase
    {
         
        /// Dados da empresa
         
        private Empresa DadosEmpresa = null;

         
        /// Dados do envio do XML
         
        private Servicos.Envio.ConsultarStatusOperacional ConsultarStatusOperacionalEnvio = new Servicos.Envio.ConsultarStatusOperacional();

         
        /// Resposta do equipamento sat
         
        private Servicos.Retorno.ConsultarStatusOperacionalResponse ConsultarStatusOperacionalRetorno = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConsultarStatusOperacional(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ConsultarStatusOperacionalEnvio = DeserializarObjeto<Servicos.Envio.ConsultarStatusOperacional>();
            Marca = ConsultarStatusOperacionalEnvio.Marca;
            CodigoAtivacao = ConsultarStatusOperacionalEnvio.CodigoAtivacao;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.ConsultarStatusOperacional();
            ConsultarStatusOperacionalRetorno = new Servicos.Retorno.ConsultarStatusOperacionalResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarStatusOperacionalSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarStatusOperacionalSAT).RetornoXML);

            File.WriteAllText(result, ConsultarStatusOperacionalRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}