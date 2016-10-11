using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlType("Sag")]
    public class Sag
    {
        [XmlElement("Nr")]
        public string Nummer { get; set; }

        [XmlElement("Navn")]
        public string Navn { get; set; }

        [XmlElement("Adresse")]
        public string Adresse { get; set; }

        [XmlElement("Postnr")]
        public string Postnr { get; set; }

        [XmlElement("Vedr")]
        public string Vedrørende { get; set; }

        public string Formatted
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return Vedrørende;
        }
    }
}