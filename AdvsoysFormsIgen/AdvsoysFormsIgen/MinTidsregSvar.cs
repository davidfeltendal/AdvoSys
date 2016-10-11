using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot(ElementName = "MinTidsreg", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class MinTidsregSvar
    {
        [XmlArray("Poster")]
        [XmlArrayItem(typeof(Post))]
        public Post[] Poster { get; set; }
    }
}