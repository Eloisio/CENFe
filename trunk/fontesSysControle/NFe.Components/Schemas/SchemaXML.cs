using System.Collections.Generic;
using System.Linq;

namespace NFe.Components
{
     
    /// Classe responsável por definir uma lista dos arquivos de SCHEMAS para validação dos XMLs
     
    public class SchemaXML
    {
         
        /// Informações dos schemas para validação dos XML
         
        public static Dictionary<string, InfSchema> InfSchemas = new Dictionary<string, InfSchema>();

         
        /// O Maior ID que tem na lista
         
        public static int MaxID { get { return InfSchemas.Count; } }

         
        /// Cria várias listas com as TAG´s de identificação dos XML´s e seus Schemas
         
        /// <date>31/07/2009</date>
        /// <by>Wandrey Mundin Ferreira</by>
        public static void CriarListaIDXML()
        {
            InfSchemas.Clear();

            ///
            /// le todas as classes no projeto para ler as definicoes dos schemas
            ///
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            var xx = ass.GetTypes().Where(p => p.IsClass && (p.Name.StartsWith("SchemaXML_") || p.Name.StartsWith("SchemaXMLNFSe_")));
            foreach (var h1 in xx)
            {
                //Console.WriteLine(h1 + " -> ");
                h1.InvokeMember("CriarListaIDXML", System.Reflection.BindingFlags.InvokeMethod, null, null, null);
            }
        }
    }

    public class InfSchema
    {
         
        /// TAG do XML que identifica qual XML é
         
        public string Tag { get; set; }

         
        /// Identificador único numérico do XML
         
        public int ID { get; set; }

         
        /// Breve descrição do arquivo XML
         
        public string Descricao { get; set; }

         
        /// Nome do arquivo de schema para validar o XML
         
        public string ArquivoXSD { get; set; }

         
        /// Nome da tag do XML que será assinada
         
        public string TagAssinatura { get; set; }

         
        /// Nome da tag que tem o atributo ID
         
        public string TagAtributoId { get; set; }

         
        /// Nome da tag de lote do XML que será assinada
         
        public string TagLoteAssinatura { get; set; }

         
        /// Nome da tag de lote que tem o atributo ID
         
        public string TagLoteAtributoId { get; set; }

         
        /// Nome da tag do XML que será assinada (uma segunda tag que tem que ser assinada ex. SubstituirNfse Pelotas-RS)
         
        public string TagAssinatura0 { get; set; }
         
        /// Nome da tag que tem o atributo ID que será assinada, faz consunto com a TagAssinatura0
         
        public string TagAtributoId0 { get; set; }

         
        /// URL do schema de cada XML
         
        public string TargetNameSpace { get; set; }
    }
}