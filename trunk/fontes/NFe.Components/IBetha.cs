using System.Net;
using System.Xml;

namespace NFe.Components
{
    public interface IBetha
    {
        IWebProxy Proxy { get; set; }

        string CancelarNfse(XmlNode xml, int tpAmb);
        string ConsultarLoteRps(XmlNode xml, int tpAmb);
        string ConsultarNfse(XmlNode xml, int tpAmb);
        string ConsultarNfsePorRps(XmlNode xml, int tpAmb);
        string ConsultarSituacaoLoteRps(XmlNode xml, int tpAmb);
        string RecepcionarLoteRps(XmlNode xml, int tpAmb);
    }
}
