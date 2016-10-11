using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    [XmlType("Post")]
    public class Post : IComparable<Post>, IEquatable<Post>
    {
        [XmlElement("PostId")]
        public string Id { get; set; }

        [XmlElement("Sag")]
        public string Sagsnummer { get; set; }

        [XmlElement("Aktivitet")]
        public string Aktivitetskode { get; set; }

        [XmlElement("Tekst")]
        public string Tekst { get; set; }

        [XmlElement("FraKlokken")]
        public string FraKlokkenValue { get; set; }

        [XmlElement("Dato")]
        public string DatoValue { get; set; }

        [XmlElement("Dato")]
        public DateTime Dato
        {
            get
            {
                DateTime dato;
                if (DateTime.TryParseExact(DatoValue, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dato))
                {
                    return dato;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        [XmlIgnore]
        public DateTime FraKlokken
        {
            get
            {
                DateTime fraKlokken;
                if (DateTime.TryParseExact(FraKlokkenValue, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None,  out fraKlokken))
                {
                    return fraKlokken;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        [XmlElement("Forbrugt")]
        public string ForbrugtValue { get; set; }

        [XmlIgnore]
        public TimeSpan Forbrugt
        {
            get
            {
                TimeSpan forbrugt;
                if (TimeSpan.TryParseExact(ForbrugtValue, "g", CultureInfo.InvariantCulture, out forbrugt))
                {
                    return forbrugt;
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }

        [XmlElement("Deb")]
        public string DebiteretValue { get; set; }

        [XmlIgnore]
        public TimeSpan Debiteret
        {
            get
            {
                TimeSpan debiteret;
                if (TimeSpan.TryParseExact(DebiteretValue, "g", CultureInfo.InvariantCulture, out debiteret))
                {
                    return debiteret;
                }
                else
                {
                    return Forbrugt;
                }
            }
        }

        [XmlElement("Rediger")]
        public string Rediger { get; set; }

        [XmlIgnore]
        public bool KanRedigeres
        {
            get { return Rediger == "Ja"; }
        }

        public int CompareTo(Post other)
        {
            return FraKlokken.CompareTo(other.FraKlokken);
        }

        public override string ToString()
        {
            return Tekst;
        }

        public bool Equals(Post other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Post);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}