using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot("IdParam", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class HentMinTidsregParam
    {
        [XmlElement("id")]
        public string Id
        {
            get { return "APPTESTVEJLE,xxx,Android,1.0,1.0"; }
            set { }
        }
    }
}