using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot("TidsregSletParam", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class TidsregSletParam
    {
        [XmlElement("id")]
        public virtual string Id
        {
            get { return "APPTESTVEJLE,xxx,Android,1.0,1.0"; }
            set { }
        }

        [XmlElement("postid")]
        public string PostId { get; set; }
    }
}