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
     
    /// Classe responsável pela ativação do SAT
     
    public class AtivarSAT : ServicoBase
    {
         
        /// Dados do XML
         
        private XmlDocument Document = new XmlDocument();

         
        /// Dados da empresa
         
        private Empresa DadosEmpresa = null;

         
        /// Identificador do tipo de Certificado, descritos na tabela 15
         
        private int SubComando = 0;

         
        /// CNPJ do contribuinte, somente números
         
        private string CNPJ = null;

         
        /// Código do Estado da Federação, segundo tabela do IBGE, onde o SAT será ativado
         
        private int CodigoUF = 0;

         
        /// Resposta do equipamento sat
         
        private Servicos.Retorno.AtivarSATResponse AtivarSATRetorno = new Servicos.Retorno.AtivarSATResponse();

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public AtivarSAT(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            Document.Load(fs);
            fs.Close();
            fs.Dispose();

            SubComando = Convert.ToInt32(GetValueXML(Document, "AtivarSAT", "SubComando"));
            CNPJ = GetValueXML(Document, "AtivarSAT", "CNPJ");
            CodigoUF = Convert.ToInt32(GetValueXML(Document, "AtivarSAT", "CodigoUF"));

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            Marca = UConvert.ToEnum<EnunsSAT.Fabricante>(DadosEmpresa.MarcaSAT);
            CodigoAtivacao = DadosEmpresa.CodigoAtivacaoSAT;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.AtivarSAT(SubComando, CNPJ, CodigoUF);
            AtivarSATRetorno = new Servicos.Retorno.AtivarSATResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.AtivarSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.AtivarSAT).RetornoXML);

            File.WriteAllText(result, AtivarSATRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}