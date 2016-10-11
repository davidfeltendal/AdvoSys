using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot("HentSagslisteParamV2", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class HentSagslisteParamV2
    {
        [XmlElement("egne")]
        public int Egne
        {
            get { return 1; }
            set { }
        }

        [XmlElement("filter")]
        public string Filter { get; set; }

        [XmlElement("id")]
        public string Id
        {
            get { return "APPTESTVEJLE,xxx,Android,1.0,1.0"; }
            set { }
        }

        [XmlElement("max")]
        public int Max { get; set; }
    }
}