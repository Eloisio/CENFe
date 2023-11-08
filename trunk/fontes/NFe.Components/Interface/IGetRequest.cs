namespace NFe.Components.Interface
{
    public interface IGetRequest : IRequest
    {

        /// Faz o Get e retorna uma string como resultado

        /// <param name="url">url base para utilizar dentro do get</param>
        /// <returns></returns>
        string GetForm(string url);
    }
}