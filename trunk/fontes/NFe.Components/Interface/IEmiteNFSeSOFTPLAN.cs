namespace NFe.Components
{

    /// Interface para a emissão de NFSe do padrão SOFTPLAN

    public interface IEmiteNFSeSOFTPLAN : IEmiteNFSe
    {
        string ClientID { get; set; }
        string ClientSecret { get; set; }
        string URLAPIBase { get; }
        string GerarToken();
    }
}