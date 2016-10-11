using System;
using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlRoot("HentMinTidsregDagParam", Namespace = "http://schemas.datacontract.org/2004/07/WcfRestAdvosys")]
    public class HentMinTidsregDagParam
    {
        public HentMinTidsregDagParam()
        {
        }

        public HentMinTidsregDagParam(DateTimeOffset dato)
        {
            SetDato(dato);
        }

        [XmlElement("dato")]
        public string Dato { get; set; }

        [XmlElement("id")]
        public string Id
        {
            get { return "APPTESTVEJLE,xxx,Android,1.0,1.0"; }
            set { }
        }
        
        public void SetDato(DateTimeOffset value)
        {
            Dato = value.LocalDateTime.ToString("yyyyMMdd");
        }
    }
}