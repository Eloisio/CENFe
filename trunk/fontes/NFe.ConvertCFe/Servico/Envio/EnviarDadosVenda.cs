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

    /// Classe responsável pelo envio da venda pelo SAT

    public class EnviarDadosVenda : ServicoBase
    {

        /// Dados da empresa

        private Empresa DadosEmpresa = null;


        /// Dados do envio do XML

        private string DadosVendaEnvio = null;


        /// Resposta do equipamento sat

        private Servicos.Retorno.EnviarDadosVendaResponse DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse();


        /// Nome do arquivo XML que esta sendo manipulado

        public override string ArquivoXML { get; set; }


        /// Construtor com serialização

        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public EnviarDadosVenda(string arquivoXML, Empresa dadosEmpresa)
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
            DadosVendaEnvio = doc.InnerXml;
        }


        /// Comunicar com o equipamento SAT

        public override void Enviar()
        {
            string resposta = Sat.EnviarDadosVenda(DadosVendaEnvio);
            DadosVendaRetorno = new Servicos.Retorno.EnviarDadosVendaResponse(resposta);
        }


        /// Salva o XML de Retorno

        public override string SaveResponse()
        {
            string xml = string.Empty;
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).RetornoXML);

            File.WriteAllText(result, DadosVendaRetorno.ToXML());

            if (DadosVendaRetorno.CodigoMensagem.Equals("06000"))
            {
                xml = Path.Combine(DadosEmpresa.PastaXmlRetorno, DadosVendaRetorno.ChaveConsulta.Substring(3) + Propriedade.Extensao(Propriedade.TipoEnvio.EnviarDadosVendaSAT).EnvioXML);
                File.WriteAllText(xml, DadosVendaRetorno.ArquivoCFe);
            }

            File.Delete(ArquivoXML);

            return xml;
        }
    }
}