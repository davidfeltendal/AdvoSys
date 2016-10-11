using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot("TidsregRetParam", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class TidsregRetParam
    {
        [XmlElement("aktkode", Order = 0)]
        public string Aktkode { get; set; }

        [XmlElement("beskrivelse", Order = 1)]
        public string Beskrivelse { get; set; }

        [XmlElement("fraKlokken", Order = 2)]
        public string FraKlokken { get; set; }

        [XmlElement("id", Order = 3)]
        public string Id
        {
            get { return "APPTESTVEJLE,xxx,Android,1.0,1.0"; }
            set { }
        }

        [XmlElement("minutter", Order = 4)]
        public string AntalMinutter { get; set; }

        [XmlElement("postid", Order = 5)]
        public string PostId { get; set; }

        [XmlElement("sagsnr", Order = 6)]
        public string Sagsnr { get; set; }
    }
}