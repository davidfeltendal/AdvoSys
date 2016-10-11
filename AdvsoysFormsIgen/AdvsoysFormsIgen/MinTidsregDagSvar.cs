using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot(ElementName = "MinTidsregDag", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class MinTidsregDagSvar
    {
        [XmlArray("Poster")]
        [XmlArrayItem(typeof(Post))]
        public Post[] Poster { get; set; }
    }
}