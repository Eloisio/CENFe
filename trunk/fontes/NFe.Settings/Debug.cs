using System;

namespace NFe.Settings
{
#if DEBUG

    /// Classe para mensagem em modo debug, que serão exibidas no output

    public sealed class Debug
    {
        #region Constantes

        /// Categoria para o modo debug

        public const string Category = "UniNFe";
        #endregion


        /// Escreve um log no Output

        /// <param name="message">mensagem a ser escrita</param>
        public static void WriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0:HH:mm:ss.fff}=> {1}", DateTime.Now, message), Category);
        }
    }

#endif
}
