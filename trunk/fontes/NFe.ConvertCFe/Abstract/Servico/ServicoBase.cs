using NFe.SAT.Contract;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NFe.SAT.Abstract.Servico
{

    /// Classe abstrata para comunicação com as DLLs

    public abstract class ServicoBase : IServico
    {

        /// Marca do fabricante do SAT

        public Unimake.SAT.Enuns.Fabricante Marca { get; set; }


        /// Código de ativação do SAT

        public string CodigoAtivacao { get; set; }


        /// Arquivo XML carregado no construtor

        public abstract string ArquivoXML { get; set; }


        /// Define o objeto de comunicação SAT

        Unimake.SAT.SAT _Sat = null;


        /// Objeto de comunicação com o SAT

        protected Unimake.SAT.SAT Sat
        {
            get
            {
                if (_Sat == null)
                    _Sat = new Unimake.SAT.SAT(Marca, CodigoAtivacao);

                return _Sat;
            }
        }


        /// Método responsável em transmitir a comunicação com o SAT

        /// <returns></returns>
        public abstract void Enviar();


        /// Método responsável por salvar o retorno do SAT

        /// <returns>caminho do arquivo de resposta</returns>
        public abstract string SaveResponse();


        /// Deserializar o objeto para string

        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <returns></returns>
        public T DeserializarObjeto<T>()
            where T : new()
        {
            T envio = new T();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = envio.GetType().Name;

            XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);
            FileStream stream = File.Open(ArquivoXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(stream);
            envio = (T)serializer.Deserialize(reader);
            reader.Close();
            stream.Close();
            return envio;
        }


        /// Retorna o valor do documento a partir da raiz

        /// <param name="document">XML carregado</param>
        /// <param name="groupTag">nome do nó</param>
        /// <param name="nameTag">nome da tag</param>
        /// <returns></returns>
        public string GetValueXML(XmlDocument document, string groupTag, string nameTag)
        {
            string value = "";
            XmlNodeList nodes = document.GetElementsByTagName(groupTag);
            XmlNode node = nodes[0];

            foreach (XmlNode n in node)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.Equals(nameTag))
                    {
                        value = n.InnerText;
                        break;
                    }
                }
            }

            return value;
        }
    }
}
