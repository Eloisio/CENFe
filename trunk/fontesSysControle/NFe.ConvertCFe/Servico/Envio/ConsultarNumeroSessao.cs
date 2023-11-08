using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System;
using System.IO;
using System.Xml;
using Unimake.SAT.Utility;
using EnunsSAT = Unimake.SAT.Enuns;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
     
    /// Classe responsável pela comunicação com o SAT
     
    public class ConsultarNumeroSessao : ServicoBase
    {
         
        /// Dados da empresa
         
        private Empresa DadosEmpresa = null;

         
        /// Dados do envio do XML
         
        private string ConsultarNumeroSessaoEnvio = null;

         
        /// Numero da sessão que será consultada
         
        private int NumeroSessao = 0;

         
        /// Resposta do equipamento sat
         
        private Servicos.Retorno.ConsultarNumeroSessaoResponse ConsultarRetorno = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public ConsultarNumeroSessao(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            ConsultarNumeroSessaoEnvio = doc.InnerXml;
            NumeroSessao = Convert.ToInt32(GetValueXML(doc, "ConsultarNumeroSessao", "NumeroSessao"));
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.ConsultarNumeroSessao(NumeroSessao);
            ConsultarRetorno = new Servicos.Retorno.ConsultarNumeroSessaoResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarNumeroSessaoSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.ConsultarNumeroSessaoSAT).RetornoXML);

            File.WriteAllText(result, ConsultarRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}