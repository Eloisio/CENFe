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

    /// Classe responsável pela associação da assinatura no SAT

    public class AssociarAssinatura : ServicoBase
    {

        /// Dados do XML

        private XmlDocument Document = new XmlDocument();


        /// Dados da empresa

        private Empresa DadosEmpresa = null;


        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial".

        private string CNPJValue = null;


        /// Assinatura digital conjunto "CNPJ Software House" + "CNPJ do estabelecimento comercial".

        private string AssinaturaCNPJs = null;


        /// Resposta do equipamento sat

        private Servicos.Retorno.AssociarAssinaturaResponse AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse();


        /// Nome do arquivo XML que esta sendo manipulado

        public override string ArquivoXML { get; set; }


        /// Construtor com serialização

        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public AssociarAssinatura(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            Document.Load(fs);
            fs.Close();
            fs.Dispose();

            CNPJValue = GetValueXML(Document, "AssociarAssinatura", "CNPJvalue");
            AssinaturaCNPJs = GetValueXML(Document, "AssociarAssinatura", "assinaturaCNPJs");

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }


        /// Comunicar com o equipamento SAT

        public override void Enviar()
        {
            string resposta = Sat.CancelarUltimaVenda(CNPJValue, AssinaturaCNPJs);
            AssociarAssinaturaRetorno = new Servicos.Retorno.AssociarAssinaturaResponse(resposta);
        }


        /// Salva o XML de Retorno

        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.AssociarAssinaturaSAT).RetornoXML);

            File.WriteAllText(result, AssociarAssinaturaRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}