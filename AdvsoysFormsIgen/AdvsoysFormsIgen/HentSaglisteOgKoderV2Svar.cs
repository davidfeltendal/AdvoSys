using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot(ElementName = "SagsListeOgKoder", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class HentSaglisteOgKoderV2Svar
    {
        [XmlArray("Sager")]
        [XmlArrayItem(typeof(Sag))]
        public Sag[] Sager { get; set; }

        [XmlArray("Timesagskoder")]
        [XmlArrayItem(typeof(Aktivitet))]
        public Aktivitet[] Aktiviteter { get; set; }
    }


}