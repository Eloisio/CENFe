namespace NFe.Components
{
    public class Smf
    {
        #region Objetos para semaforos
         
        /// Objeto para auxiliar no semaforo do fluxonfe
         
        public static SmfAuxiliar Fluxo = new SmfAuxiliar();
         
        /// Objeto para auxiliar no semaforo do carregamento das webproxy's
         
        public static SmfAuxiliar WebProxy = new SmfAuxiliar();
         
        /// Objeto para auxiliar no semaforo do controle do número de lote
         
        public static SmfAuxiliar NumLote = new SmfAuxiliar();
        #endregion
    }

    public class SmfAuxiliar
    {
    }
}
