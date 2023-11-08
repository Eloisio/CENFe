using System;

namespace NFe.Components.Exceptions
{

    /// Serviço não disponível

    public class ServicoInexistenteException : Exception
    {
        public override string Message
        {
            get
            {
                return "Serviço não disponível ou não existe.";
            }
        }
    }


    /// Serviço Indisponível para homologação
    /// Renan - 27/02/2014

    public class ServicoInexistenteHomologacaoException : Exception
    {

        /// Servico que esta indisponivel para Homologacao

        private string _Service = "";


        /// Construtor

        /// <param name="service">nome do servico que esta indisponivel para o ambiente</param>
        public ServicoInexistenteHomologacaoException(Servicos service)
        {
            _Service = EnumHelper.GetDescription(service);
        }
        public override string Message
        {
            get
            {
                return String.Format("Serviço {0} não está disponível para o Ambiente de Homologação.", _Service);
            }
        }
    }

    public class ProblemaLeituraXML : Exception
    {
        private string Arquivo = "";

        public ProblemaLeituraXML(string arquivo)
        {
            Arquivo = arquivo;

        }
        public override string Message
        {
            get
            {
                return String.Format("Não foi possivel abrir/ler o arquivo {0}.", Arquivo);
            }
        }

    }


    /// Se ocorrer alguma falha na execução do UniNFe vai gerar esta exceção

    public class ProblemaExecucaoUniNFe : Exception
    {
        private string Mensagem = "";

        public ProblemaExecucaoUniNFe(string mensagem)
        {
            Mensagem = mensagem;
        }

        public override string Message
        {
            get
            {
                return Mensagem;
            }
        }
    }


    /// Se já tiver algum UniNFe executando vai gerar esta exceção

    public class AppJaExecutando : Exception
    {
        private string Mensagem = "";

        public AppJaExecutando(string mensagem)
        {
            Mensagem = mensagem;
        }

        public override string Message
        {
            get
            {
                return Mensagem;
            }
        }

    }


    /// Lançado quando um município não funciona com o .NET Framework 3.5

    public class MunicipioSemSuporteAoNETFramework35Exception : Exception
    {

        /// Mensagem que é exibida ao usuário

        public override string Message
        {
            get
            {
                return "Este município não funciona com a versão do UniNFe com .NET Framework 3.5. Dessa forma, instale a versão do UniNFe com .NET Framework 4.6.2 que consta no site da Unimake.";
            }
        }
    }
}
