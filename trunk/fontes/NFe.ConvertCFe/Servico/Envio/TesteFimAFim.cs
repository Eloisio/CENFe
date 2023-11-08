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

    /// Classe responsável pelo teste de comunicação com o SAT

    public class TesteFimAFim : ServicoBase
    {

        /// Dados da empresa

        private Empresa DadosEmpresa = null;


        /// Dados do envio do XML

        private string TesteFimAFimEnvio = null;


        /// Resposta do equipamento sat

        private Servicos.Retorno.TesteFimAFimResponse TesteFimAFimRetorno = new Servicos.Retorno.TesteFimAFimResponse();


        /// Nome do arquivo XML que esta sendo manipulado

        public override string ArquivoXML { get; set; }


        /// Construtor com serialização

        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public TesteFimAFim(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
            TesteFimAFimEnvio = doc.InnerXml;
        }


        /// Comunicar com o equipamento SAT

        public override void Enviar()
        {
            string resposta = Sat.TesteFimAFim(TesteFimAFimEnvio);
            TesteFimAFimRetorno = new Servicos.Retorno.TesteFimAFimResponse(resposta);
        }


        /// Salva o XML de Retorno

        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarSAT).RetornoXML);

            File.WriteAllText(result, TesteFimAFimRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}