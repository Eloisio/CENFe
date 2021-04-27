using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NFe.Components
{
    #region SubPastas da pasta de enviados

     
    /// SubPastas da pasta de XML´s enviados para os webservices
     
    public enum PastaEnviados
    {
        EmProcessamento,
        Autorizados,
        Denegados
    }

    #endregion SubPastas da pasta de enviados
    /// Serviços executados pelo Aplicativo
    public enum Servicos
    {
        #region NFe

         
        /// Consulta status serviço NFe
         
        NFeConsultaStatusServico,

         
        /// Somente converter TXT da NFe para XML de NFe
         
        NFeConverterTXTparaXML,

         
        /// Envia os lotes de NFe para os webservices
         
        NFeEnviarLote,

         
        /// Envia XML de Inutilização da NFe
         
        NFeInutilizarNumeros,

         
        /// Assinar e montar lote de uma NFe
         
        NFeMontarLoteUma,

         
        /// Consulta situação da NFe
         
        NFePedidoConsultaSituacao,

         
        /// Consulta recibo do lote nfe
         
        NFePedidoSituacaoLote,

        #region Eventos NFe

         
        /// Enviar XML Evento - Cancelamento
         
        EventoCancelamento,

         
        /// Enviar XML Evento - Carta de Correção
         
        EventoCCe,

         
        /// Enviar um evento de EPEC
         
        EventoEPEC,

         
        /// Enviar um evento de manifestacao
         
        EventoManifestacaoDest,

         
        /// Enviar XML de Evento NFe
         
        EventoRecepcao,

        #endregion Eventos NFe

         
        /// Assinar e validar um XML de NFe no envio em Lote
         
        NFeAssinarValidarEnvioEmLote,

         
        /// Monta chave de acesso
         
        NFeGerarChave,

         
        /// Assinar e montar lote de várias NFe
         
        NFeMontarLoteVarias,

        #endregion NFe
        #region CTe

         
        /// Assinar e validar um XML de CTe no envio em Lote
         
        CTeAssinarValidarEnvioEmLote,

         
        /// Consulta Status Serviço CTe
         
        CTeConsultaStatusServico,

         
        /// Envia os lotes de CTe para os webservices
         
        CTeEnviarLote,

         
        /// Envia XML de Inutilização da CTe
         
        CTeInutilizarNumeros,

         
        /// Montar lote de um CTe
         
        CTeMontarLoteUm,

         
        /// Assinar e montar lote de várias CTe
         
        CTeMontarLoteVarios,

         
        /// Consulta situação da CTe
         
        CTePedidoConsultaSituacao,

         
        /// Consulta recibo do lote CTe
         
        CTePedidoSituacaoLote,

         
        /// Enviar XML Evento CTe
         
        CTeRecepcaoEvento,

         
        /// Enviar XML de distribuição de DFe de interesses de autores (CTe)
         
        CTeDistribuicaoDFe,

         
        /// Enviar XML de CTe modelo 67
         
        CteRecepcaoOS,

        #endregion CTe
        #region NFSe


        /// Cancelar NFS-e

        [Description("Cancelar NFS-e")]
        NFSeCancelar,

         
        /// Consultar NFS-e por Data
         
        [Description("Consultar NFS-e por Data")]
        NFSeConsultar,

         
        /// Consultar lote RPS
         
        [Description("ConsultarLoteRPS")]
        NFSeConsultarLoteRps,

         
        /// Consultar NFS-e por RPS
         
        [Description("Consultar NFS-e por RPS")]
        NFSeConsultarPorRps,

         
        /// Consultar Situação do lote RPS NFS-e
         
        [Description("Consultar Situação do lote RPS NFS-e")]
        NFSeConsultarSituacaoLoteRps,

         
        /// Consultar a URL de visualização da NFSe
         
        [Description("Consultar a URL de Visualização da NFS-e")]
        NFSeConsultarURL,

         
        /// Consultar a URL de visualização da NFSe
         
        [Description("Consultar a URL de Visualização da NFS-e com a Série")]
        NFSeConsultarURLSerie,

         
        /// Enviar Lote RPS NFS-e
         
        [Description("Enviar Lote RPS NFS-e ")]
        NFSeRecepcionarLoteRps,

         
        /// Enviar Lote RPS NFS-e de forma sincrona
        /// Criado inicialmente para ser utilizado para o padrão BHIss, pois é necessario utilizar a recepção de lote das duas formas.
         
        [Description("Enviar Lote RPS NFS-e Sincrono")]
        NFSeRecepcionarLoteRpsSincrono,

         
        /// Enviar Lote RPS NFS-e de forma sincrona
        /// Criado inicialmente para ser utilizado para o padrão BHIss, pois é necessario utilizar a recepção de lote das duas formas.
         
        [Description("Enviar Lote RPS NFS-e Sincrono")]
        NFSeGerarNfse,

         
        /// Consulta da imagem de uma NFS-e em formato PNG
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
         
        [Description("Consulta da imagem de uma NFS-e em formato PNG")]
        NFSeConsultarNFSePNG,

         
        /// Consulta da imagem de uma NFS-e em formato
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
         
        [Description("Inutilização de uma NFS-e")]
        NFSeInutilizarNFSe,

         
        /// Consulta da imagem de uma NFS-e em formato PDF
        /// Criado inicialmente para ser utilizado para o padrão INFISC para a Prefeitura de Caxias do Sul - RS
         
        [Description("Consulta da imagem de uma NFS-e em formato PDF")]
        NFSeConsultarNFSePDF,

         
        /// Baixar o XML da NFSe
         
        [Description("Obter o XML da NFS-e")]
        NFSeObterNotaFiscal,

         
        /// Consulta Sequencia do Lote da Nota RPS
         
        [Description("Consulta Sequencia do Lote da Nota RPS")]
        NFSeConsultaSequenciaLoteNotaRPS,

         
        /// Substituir NFS-e
         
        [Description("Substituir NFS-e")]
        NFSeSubstituirNfse,

        #endregion NFSe
        #region CFSe

         
        /// Enviar Lote CFS-e
         
        [Description("Enviar Lote CFS-e")]
        RecepcionarLoteCfse,

         
        /// Enviar Lote CFS-e Sincrono
         
        [Description("Enviar Lote CFS-e")]
        RecepcionarLoteCfseSincrono,

         
        /// Cancelar CFS-e
         
        [Description("Enviar Cancelamento CFS-e")]
        CancelarCfse,

         
        /// Consultar Lote CFS-e
         
        [Description("Enviar consulta do lote CFS-e")]
        ConsultarLoteCfse,

         
        /// Consultar CFS-e
         
        [Description("Enviar consulta do CFS-e")]
        ConsultarCfse,

         
        /// Configurar/Ativar Terminal CFS-e
         
        [Description("Enviar XML de configuração/ativação de terminal CFS-e")]
        ConfigurarTerminalCfse,

         
        /// Informar terminal CFS-e em manutenção
         
        [Description("Enviar XML para informar que o terminal de CFS-e está em manutenção")]
        EnviarInformeManutencaoCfse,

         
        /// Informar data sem movimento de CFS-e
         
        [Description("Enviar XML para informar que não teve movimento de CFS-e no dia")]
        InformeTrasmissaoSemMovimentoCfse,

         
        /// Consulta dados cadastro terminal CFS-e
         
        [Description("Enviar XML para consultar dados cadastros terminal CFS-e")]
        ConsultarDadosCadastroCfse,

        #endregion CFSe
        #region MDFe

         
        /// Assinar e validar um XML de MDFe no envio em Lote
         
        MDFeAssinarValidarEnvioEmLote,

         
        /// Consulta MDFe nao encerrados
         
        MDFeConsultaNaoEncerrado,

         
        /// Consulta Status Serviço MDFe
         
        MDFeConsultaStatusServico,

         
        /// Envia os lotes de MDFe para os webservices
         
        MDFeEnviarLote,

         
        /// Montar lote de um MDFe
         
        MDFeMontarLoteUm,

         
        /// Assinar e montar lote de várias MDFe
         
        MDFeMontarLoteVarios,

         
        /// Consulta situação da MDFe
         
        MDFePedidoConsultaSituacao,

         
        /// Consulta recibo do lote MDFe
         
        MDFePedidoSituacaoLote,

         
        /// Enviar XML Evento MDFe
         
        MDFeRecepcaoEvento,

        #endregion MDFe
        #region SAT/CFe

         
        /// Consultar SAT
         
        SATConsultar,

        SATExtrairLogs,
        SATConsultarStatusOperacional,
        SATTesteFimAFim,
        SATTrocarCodigoDeAtivacao,
        SATEnviarDadosVenda,
        SATConverterNFCe,
        SATCancelarUltimaVenda,
        SATConfigurarInterfaceDeRede,
        SATAssociarAssinatura,
        SATAtivar,
        SATBloquear,
        SATDesbloquear,
        SATConsultarNumeroSessao,

        #endregion SAT/CFe
        #region Serviços em comum NFe, CTe, MDFe e NFSe

         
        /// Valida e envia o XML de pedido de Consulta do Cadastro do Contribuinte para o webservice
         
        ConsultaCadastroContribuinte,

         
        /// Efetua verificações nas notas em processamento para evitar algumas falhas e perder retornos de autorização de notas
         
        EmProcessamento,

         
        /// Somente assinar e validar o XML
         
        AssinarValidar,

        #endregion Serviços em comum NFe, CTe, MDFe e NFSe
        CENFeConsultaInformacoes,
        CENFeAlterarConfiguracoes,
        CENFeLimpezaTemporario,
        CENFeConsultaGeral,
        CENFeConsultaCertificados,
        CENFeUpdate,
        WSExiste,
        DANFEImpressao,
        DANFEImpressao_Contingencia,
        DANFERelatorio,
        LMCAutorizacao,
        DFeEnviar,
        RecepcaoLoteReinf,
        ConsultarLoteReinf,
        RecepcaoLoteeSocial,
        ConsultarLoteeSocial,
        Nulo
    }
    #region TipoAplicativo

    public enum TipoAplicativo
    {
         
        /// Aplicativo ou serviços para processamento dos XMLs da NF-e e NFC-e
         
        ///
        [Description("NF-e e NFC-e")]
        Nfe = 0,

         
        /// Aplicativo ou serviços para processamento dos XMLs do CT-e
         
        [Description("CT-e")]
        Cte = 1,

         
        /// Aplicativo ou servicos para processamento dos XMLs da NFS-e
         
        [Description("NFS-e")]
        Nfse = 2,

         
        /// Aplicativo ou serviços para processamento dos XMLs do MDF-e
         
        [Description("MDF-e")]
        MDFe = 3,

         
        /// Aplicativo ou serviços para processamento dos XMLs da NFC-e
         
        [Description("NFC-e")]
        NFCe = 4,

         
        /// Aplicativo ou serviços para processamento dos XMLs do SAT
         
        [Description("SAT")]
        SAT = 5,

         
        /// Aplicativo ou serviços para processamento dos XMLs do EFD Reinf
         
        [Description("EFD Reinf")]
        EFDReinf = 6,

         
        /// Aplicativo ou serviços para processamento dos XMLs do eSocial
         
        [Description("eSocial")]
        eSocial = 7,

         
        /// Aplicativo ou seviços para processamento dos XMLs de EFD Reinf e eSocial
         
        [Description("EFD Reinf e eSocial")]
        EFDReinfeSocial = 8,

#if _fw46

        [Description("NF-e, NFC-e, CT-e, MDF-e, EFD Reinf e eSocial")]
        Todos = 10,

#else
        [Description("NF-e, NFC-e, CT-e, MDF-e")]
        Todos = 10,
#endif

        [Description("")]
        Nulo = 100
    }

    #endregion TipoAplicativo
    #region Padrão NFSe

    public enum PadroesNFSe
    {
         
        /// Não Identificado
         
        [Description("Não identificado")]
        NaoIdentificado,

         
        /// Padrão GINFES
         
        [Description("GINFES")]
        GINFES,

         
        /// Padrão da BETHA Sistemas
         
        [Description("BETHA")]
        BETHA,

         
        /// Padrão da BETHA versão 2.02
         
        [Description("BETHA 2.02")]
        BETHA202,

         
        /// Padrão da THEMA Informática
         
        [Description("THEMA")]
        THEMA,

         
        /// Padrão da prefeitura de Salvador-BA
         
        [Description("Salvador-BA")]
        SALVADOR_BA,

         
        /// Padrão da prefeitura de Canoas-RS
         
        [Description("Canoas-RS")]
        CANOAS_RS,

         
        /// Padrão da ISS Net / NotaControl
         
        [Description("ISS Net")]
        ISSNET,

         
        /// Padrão ISS On-Line/Assessor Público
        /// Padrão da prefeitura de Barra Mansa-RJ
         
        [Description("ISS On-line/Assessor Público")]
        ISSONLINE_ASSESSORPUBLICO,

         
        /// Padrão da prefeitura de Blumenau-SC
         
        [Description("Blumenau-SC")]
        BLUMENAU_SC,

         
        /// Padrão da prefeitura de Juiz de Fora-MG
         
        [Description("BHISS")]
        BHISS,

         
        /// Padrao GIF
        /// Prefeitura de Campo Bom-RS
         
        [Description("GIF/Infisc")]
        GIF,

         
        /// Padrão IPM
        /// <para>Prefeitura de Campo Mourão.</para>
         
        [Description("IPM")]
        IPM,

         
        /// Padrão DUETO
        /// Prefeitura de Nova Santa Rita - RS
         
        [Description("Dueto")]
        DUETO,

         
        /// Padrão WEB ISS
        /// Prefeitura de Feira de Santana - BA
         
        [Description("Web ISS")]
        WEBISS,

         
        /// Padrão WEB ISS
        /// Versão do XML 2.02
         
        [Description("Web ISS")]
        WEBISS_202,

         
        /// Padrão Nota Fiscal Eletrônica Paulistana -
        /// Prefeitura São Paulo - SP
         
        [Description("Paulistana")]
        PAULISTANA,

         
        /// Padrão Nota Fiscal Eletrônica Porto Velhense
        /// Prefeitura de Porto Velho - RO
         
        [Description("Portovelhense")]
        PORTOVELHENSE,

         
        /// Padrão Nota Fiscal Eletrônica da PRONIN (GovBR)
        /// Prefeitura de Mirassol - SP
         
        [Description("Pronin")]
        PRONIN,

         
        /// Padrão Nota Fiscal Eletrônica ISS-ONline da 4R Sistemas
        /// Prefeitura de Governador Valadares - SP
         
        [Description("ISS On-Line/4R")]
        ISSONLINE4R,

         
        /// Padrão Nota Fiscal eletrônica DSF
        /// Prefeitura de Campinas - SP
        /// Prefeitura de Campo Grande - MS
         
        [Description("DSF")]
        DSF,

         
        /// Padrão Tecno Sistemas
        /// Prefeitura de Portão - RS
         
        [Description("Tecno Sistemas")]
        TECNOSISTEMAS,

         
        /// Padrão System-PRO
        /// Prefeitura de Erechim - RS
         
        [Description("System-Pro")]
        SYSTEMPRO,

         
        /// Preifetura de Macaé - RJ
         
        [Description("Tiplan")]
        TIPLAN,

         
        /// Preifetura de Niterói - RJ
         
        [Description("Tiplan")]
        TIPLAN_203,

         
        /// Prefeitura do Rio de Janeiro - RJ
         
        [Description("Carioca")]
        CARIOCA,

         
        /// Prefeitura de Bauru - SP
         
        [Description("SigCorp/SigISS")]
        SIGCORP_SIGISS,

         
        /// Padrão SmaraPD
        /// Prefeitura de Sertãozinho - SP
         
        [Description("SmaraPD")]
        SMARAPD,

         
        /// Padrão Fiorilli
        /// Prefeitura de Taquara - SP
         
        [Description("Fiorilli")]
        FIORILLI,

         
        /// Padrão Fintel
        /// Prefeitura de Ponta Grossa - PR
         
        [Description("Fintel")]
        FINTEL,

         
        /// Padrão ISSWEB
        /// Prefeitura de Mairipora - SP
         
        [Description("ISSWeb")]
        ISSWEB,

         
        /// Padrão SimplIss
        /// Prefeitura de Piracicaba - SP
         
        [Description("SimplIss")]
        SIMPLISS,

         
        /// Padrão Conam
        /// Prefeitura de Varginha - MG
         
        [Description("CONAM")]
        CONAM,

         
        /// Padrão Rlz Informatica
        /// Prefeitura de Santa Fé do Sul - PR
         
        [Description("Rlz Informática")]
        RLZ_INFORMATICA,

         
        /// Padrão E-Governe
        /// Prefeitura de Curitiba - PR
         
        [Description("E-Governe")]
        EGOVERNE,

         
        /// Padrão E-Governe
        /// Prefeitura de Osasco - SP
         
        [Description("EGoverne ISS")]
        EGOVERNEISS,

         
        /// Padrão E&L
        /// Prefeitura de Simões Filho - BA
         
        [Description("E&L")]
        EL,

         
        /// Padrao GOV-Digital
        /// Prefeitura de Divinopolis-MG
         
        [Description("Gov-Digital")]
        GOVDIGITAL,

         
        /// Padrão Equiplano
        /// Prefeitura de Toledo - PR
         
        [Description("Equiplano")]
        EQUIPLANO,

         
        /// Padrão Prodata
        /// Prefeitura de Itumbiara - GO
         
        [Description("Prodata")]
        PRODATA,

         
        /// Padrão VVISS
        /// Prefeitura de Vila Velha - ES
         
        [Description("VVISS")]
        VVISS,

         
        /// Padrão FISSLEX
        /// Prefeitura Sinop - MT
         
        [Description("FISSLEX")]
        FISSLEX,

         
        /// Padrão EloTech
        /// Prefeitura de Quatro Barras - PR
         
        [Description("EloTech")]
        ELOTECH,

         
        /// Padrão MGM
        /// Prefeitura de Penapolis - SP
         
        [Description("MGM")]
        MGM,

         
        /// Padrão Natalense
        /// Prefeitura de Natal - RN
         
        [Description("Natalense")]
        NATALENSE,

         
        /// Padrão Consist
        /// Prefeitura de Patos de Minas - MG
         
        [Description("Consist")]
        CONSIST,

         
        /// Padrão de Goiania
         
        [Description("Goiania")]
        GOIANIA,

         
        /// Padrão Nota Inteligente
        /// Prefeitura de Claudio - MG
         
        [Description("Nota Inteligente")]
        NOTAINTELIGENTE,

         
        /// Padrão Freire Informática
        /// Prefeitura de Alagoinhas - BA
         
        [Description("Freire Informática")]
        FREIRE_INFORMATICA,

         
        /// Padrão Memory
        /// Prefeitura de Ponte Nova - MG
         
        [Description("Memory")]
        MEMORY,

         
        /// Prefeitura de Camaçari - BA
         
        [Description("Camaçari-BA")]
        CAMACARI_BA,

         
        /// Padrão N&A Informática
         
        [Description("N&A Informática")]
        NA_INFORMATICA,

         
        /// PAdrão ABACO
        /// Prefeitura de Rondonópolis - MT
         
        [Description("ABACO")]
        ABACO,

         
        /// Padrão Metrópolis
         
        [Description("Metrópolis")]
        METROPOLIS,

         
        /// Padrão Actcon
         
        [Description("Portal Fácil/Actcon")]
        PORTALFACIL_ACTCON,

         
        /// Padrão Actcon
         
        [Description("Portal Fácil/Actcon")]
        PORTALFACIL_ACTCON_202,

         
        /// Padrão Pública
         
        [Description("Pública")]
        PUBLICA,

         
        /// Padrão BSIT-BR
         
        [Description("BSIT-BR")]
        BSITBR,

         
        /// Padrão ABASE Sistemas
         
        [Description("ABASE")]
        ABASE,

         
        /// Lexsom
         
        [Description("LEXSOM")]
        LEXSOM,

         
        /// SH3
         
        [Description("SH3")]
        SH3,

         
        /// Coplan
         
        [Description("Coplan")]
        COPLAN,

         
        /// Super Nova
         
        [Description("Super Nova")]
        SUPERNOVA,

         
        /// DBSELLER
         
        [Description("DBSELLER")]
        DBSELLER,

         
        /// Prefeitura de Maringá-PR
         
        [Description("MARINGA_PR")]
        MARINGA_PR,

         
        /// Padrão utilizado pela prefeitura de Bauru-SP
         
        [Description("BAURU_SP")]
        BAURU_SP,

         
        /// Padrão utilizado pela prefeitura de Jaboatão dos Guararapes-PE
         
        [Description("TINUS")]
        TINUS,

         
        /// Padrão utilizado pela prefeitura de Eusébio-CE
         
        [Description("INTERSOL")]
        INTERSOL,

         
        /// Padrão utilizado pela prefeitura de Florianópolis-SC
         
        [Description("SOFTPLAN")]
        SOFTPLAN,

         
        /// Padrão utilizado pela prefeitura de Manaus-AM
         
        [Description("MANAUS_AM")]
        MANAUS_AM,

         
        /// Padrão utilizado pela prefeitura de Joinville-SC
         
        JOINVILLE_SC,

         
        /// Padrão utilizado pela prefeitura de Pelotas-RS
         
        [Description("AVMB/ASTEN")]
        AVMB_ASTEN,

         
        /// Padrão utilizado pela prefeitura de Lorena-SP
         
        [Description("EMBRAS")]
        EMBRAS,

         
        /// Padrão utilizado pela prefeitura Paragominas-PA
         
        [Description("Desenvolve Cidade")]
        DESENVOLVECIDADE,

         
        /// Padrão utilizada pela prefeitura de Vitória-ES
         
        [Description("VITORIA_ES")]
        VITORIA_ES,

         
        /// Padrão utilizada pela prefeitura de Nova Friburgo-RJ
         
        [Description("MODERNIZACAO_PUBLICA")]
        MODERNIZACAO_PUBLICA,
         
        /// Padrão utilizado pelo município de Montes Claros-MG
         
        [Description("e-Receita")]
        E_RECEITA,

         
        /// Padrão utilizado pelo município de Amargosa-BA
         
        [Description("ADM_SISTEMAS")]
        ADM_SISTEMAS,

         
        /// Padrão utilizado pela prefeitura de Paulistana-PE
         
        [Description("PUBLIC_SOFT")]
        PUBLIC_SOFT

        ///***ATENÇÃO***
        ///o nome deste enum tem que coincidir com o nome da url, pq faço um "IndexOf" deste enum para pegar o padrao
    }

    #endregion Padrão NFSe
    #region Classe dos tipos de ambiente da NFe

     
    /// Tipo de ambiente
     
    public enum TipoAmbiente
    {
        [Description("Produção")]
        taProducao = 1,

        [Description("Homologação")]
        taHomologacao = 2
    }

    #endregion Classe dos tipos de ambiente da NFe
    /// Regime tributação ISSQN
    public enum RegTribISSQN
    {
        [Description("1 - Micro Empresa Municipal")]
        MicroEmpresaMunicipal = 1,

        [Description("2 - Estimativa")]
        Estimativa = 2,

        [Description("3 - Sociedade de Profissionais")]
        SociedadeDeProfissionais = 3,

        [Description("4 - Cooperativa")]
        Cooperativa = 4,

        [Description("5 - Micro Empresário Individual (MEI)")]
        MicroEmpresarioIndividual = 5
    }
    /// Informa se o Desconto sobre
    /// subtotal deve ser rateado entre
    /// os itens sujeitos à tributação pelo ISSQN.
    public enum IndRatISSQN
    {
        [Description("Sim")]
        S,

        [Description("Não")]
        N
    }
    #region TipoEmissao

     
    /// TipoEmissao
     
    public enum TipoEmissao
    {
        [Description("")]
        teNone = 0,

        [Description("Normal")]
        teNormal = 1,

        [Description("Contingência com FS")]
        teFS = 2,

        [Description("Contingência com EPEC")]
        teEPEC = 4,

        [Description("Contingência com FS-DA")]
        teFSDA = 5,

        [Description("Contingência com SVC-AN")]
        teSVCAN = 6,

        [Description("Contingência com SVC-RS")]
        teSVCRS = 7,

        [Description("Contingência com SVC-SP")]
        teSVCSP = 8,

        [Description("Contingência Off-Line (NFC-e)")]
        teOffLine = 9
    }

    #endregion TipoEmissao
    #region Erros Padrões

    public enum ErroPadrao
    {
        ErroNaoDetectado = 0,
        FalhaInternet = 1,
        FalhaEnvioXmlWS = 2,
        CertificadoVencido = 3,
        FalhaEnvioXmlNFeWS = 5,
        CertificadoNaoEncontrado = 6
    }

    #endregion Erros Padrões
    #region EnumHelper

    /*
ComboBox combo = new ComboBox();
combo.DataSource = EnumHelper.ToList(typeof(SimpleEnum));
combo.DisplayMember = "Value";
combo.ValueMember = "Key";

        foreach (string value in Enum.GetNames(typeof(Model.TipoCampanhaSituacao)))
        {
            Model.TipoCampanhaSituacao stausEnum = (Model.TipoCampanhaSituacao)Enum.Parse(typeof(Model.TipoCampanhaSituacao), value);
            Console.WriteLine(" Description: " + value+"  "+ Model.EnumHelper.GetDescription(stausEnum));
        }

 */

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class AttributeTipoAplicacao : Attribute
    {
        private TipoAplicativo aplicacao;

        public TipoAplicativo Aplicacao
        {
            get
            {
                return aplicacao;
            }
        }

        public AttributeTipoAplicacao(TipoAplicativo aplicacao)
            : base()
        {
            this.aplicacao = aplicacao;
        }
    }

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        public string Description
        {
            get
            {
                return description;
            }
        }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }

     
    /// Classe com metodos para serem utilizadas nos Enuns
     
    public static class EnumHelper
    {
         
        /// Retorna a description do enum
         
        /// <param name="value">Enum para buscar a description</param>
        /// <returns>Retorna a description do enun</returns>
        /*public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }*/

        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

         
        /// Gets the <see cref="DescriptionAttribute"/> of an <see cref="Enum"/> type value.
         
        /// <param name="value">The <see cref="Enum"/> type value.</param>
        /// <returns>A string containing the text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                return GetEnumItemDescription(value);
                //DescriptionAttribute[] dattributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                //if (dattributes != null && dattributes.Length > 0)
                //description = dattributes[0].Description;
            }
            return description;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetEnumItemDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

         
        ///  Converts the <see cref="Enum"/> type to an <see cref="IList"/> compatible object.
         
        /// <param name="type">The <see cref="Enum"/> type.</param>
        /// <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        public static IList ToList(Type type, bool returnInt, bool excluibrancos)
        {
            return ToList(type, returnInt, excluibrancos, "");
        }

        public static IList ToList(Type type, bool returnInt, bool excluibrancos, string eliminar)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                string _descr = GetDescription(value);
                if (excluibrancos && string.IsNullOrEmpty(_descr)) continue;

                if (eliminar.IndexOf(Convert.ToInt32(value).ToString()) != -1) continue;

                if (returnInt)
                    list.Add(new KeyValuePair<int, string>(Convert.ToInt32(value), _descr));
                else
                    list.Add(new KeyValuePair<Enum, string>(value, _descr));
            }

            return list;
        }

        public static IList ToStrings(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(GetDescription(value));
            }

            return list;
        }
    }

    #endregion EnumHelper
}