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
     
    public class DesbloquearSAT : ServicoBase
    {
         
        /// Dados da empresa
         
        private Empresa DadosEmpresa = null;

         
        /// Resposta do equipamento sat
         
        private Servicos.Retorno.DesbloquearSATResponse DesbloquearSATRetorno = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public DesbloquearSAT(string arquivoXML, Empresa dadosEmpresa)
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
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.DesbloquearSAT();
            DesbloquearSATRetorno = new Servicos.Retorno.DesbloquearSATResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.DesbloquearSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.DesbloquearSAT).RetornoXML);

            File.WriteAllText(result, DesbloquearSATRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}