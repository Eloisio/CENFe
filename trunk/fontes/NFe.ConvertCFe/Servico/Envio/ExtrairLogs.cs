using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{

    /// Classe responsável pela comunicação com o SAT

    public class ExtrairLogs : ServicoBase
    {

        /// Dados da empresa

        private Empresa DadosEmpresa = null;


        /// Dados do envio do XML

        private Servicos.Envio.ExtrairLogs ExtrairLogsEnvio = new Servicos.Envio.ExtrairLogs();


        /// Resposta do equipamento sat

        private Servicos.Retorno.ExtrairLogsResponse ExtrairLogsRetorno = null;


        /// Nome do arquivo XML que esta sendo manipulado

        public override string ArquivoXML { get; set; }


        /// Construtor com serialização

        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ExtrairLogs(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ExtrairLogsEnvio = DeserializarObjeto<Servicos.Envio.ExtrairLogs>();
            Marca = ExtrairLogsEnvio.Marca;
            CodigoAtivacao = ExtrairLogsEnvio.CodigoAtivacao;
        }


        /// Comunicar com o equipamento SAT

        public override void Enviar()
        {
            string resposta = Sat.ExtrairLogs();
            ExtrairLogsRetorno = new Servicos.Retorno.ExtrairLogsResponse(resposta);
        }


        /// Salva o XML de Retorno

        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                                         Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarSAT).EnvioXML) +
                                                                              Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarSAT).RetornoXML);
            File.WriteAllText(result, ExtrairLogsRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}