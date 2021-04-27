using NFe.Components;
using NFe.SAT.Abstract.Servico;
using NFe.Settings;
using System.IO;
using System.Xml;
using Servicos = Unimake.SAT.Servico;

namespace NFe.SAT.Servico.Envio
{
     
    /// Classe responsável pela comunicação com o SAT
     
    public class TrocarCodigoDeAtivacao : ServicoBase
    {
         
        /// Dados da empresa
         
        Empresa DadosEmpresa = null;

         
        /// Dados do envio do XML
         
        Servicos.Envio.TrocarCodigoDeAtivacao TrocarCodigoDeAtivacaoEnvio = new Servicos.Envio.TrocarCodigoDeAtivacao();

         
        /// Resposta do equipamento sat
         
        Servicos.Retorno.TrocarCodigoDeAtivacaoResponse TrocarCodigoDeAtivacaoRetorno = null;

         
        /// Nome do arquivo XML que esta sendo manipulado
         
        public override string ArquivoXML { get; set; }

         
        /// Construtor com serialização
         
        /// <param name="arquivoXML">arquivo a ser lido</param>
        /// <param name="dadosEmpresa">dados da empresa</param>
        public TrocarCodigoDeAtivacao(string arquivoXML, Empresa dadosEmpresa)
        {
            FileStream fs = new FileStream(arquivoXML, FileMode.Open, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(fs);
            fs.Close();
            fs.Dispose();

            DadosEmpresa = dadosEmpresa;
            ArquivoXML = arquivoXML;
            TrocarCodigoDeAtivacaoEnvio = DeserializarObjeto<Servicos.Envio.TrocarCodigoDeAtivacao>();
            Marca = TrocarCodigoDeAtivacaoEnvio.Marca;
            CodigoAtivacao = TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoAtual;
        }

         
        /// Comunicar com o equipamento SAT
         
        public override void Enviar()
        {
            string resposta = Sat.TrocarCodigoDeAtivacao(TrocarCodigoDeAtivacaoEnvio.Opcao, TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoNovo, TrocarCodigoDeAtivacaoEnvio.CodigoAtivacaoNovo);
            TrocarCodigoDeAtivacaoRetorno = new Servicos.Retorno.TrocarCodigoDeAtivacaoResponse(resposta);
        }

         
        /// Salva o XML de Retorno
         
        public override string SaveResponse()
        {
            string result = Path.Combine(DadosEmpresa.PastaXmlRetorno,
                Functions.ExtrairNomeArq(ArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.TrocarCodigoDeAtivacaoSAT).EnvioXML) +
                Propriedade.Extensao(Propriedade.TipoEnvio.TrocarCodigoDeAtivacaoSAT).RetornoXML);

            File.WriteAllText(result, TrocarCodigoDeAtivacaoRetorno.ToXML());
            File.Delete(ArquivoXML);

            return result;
        }
    }
}
