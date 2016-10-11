using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlType("TSKode")]
    public class Aktivitet
    {
        [XmlElement("Kode")]
        public string Kode { get; set; }

        [XmlElement("Tekst")]
        public string Tekst { get; set; }

        public string Formatted
        {
            get { return ToString(); }
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Tekst, Kode);
        }

    }
}