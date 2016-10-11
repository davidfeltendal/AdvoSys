using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AdvsoysFormsIgen
{
    public class AdvosysKlient
    {
        private static readonly Uri BaseAddress = new Uri("https://uwa.unik.dk/Mobil/AdvosysService/");

        public static async Task<MinTidsregSvar> HentMinTidregAsync()
        {
            using (var http = new HttpClient { BaseAddress = BaseAddress })
            {
                var relativeUri = new Uri("HentMinTidsreg", UriKind.Relative);
                var parameter = new HentMinTidsregParam();
                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content))
                {
                    var deserializer = new XmlSerializer(typeof(MinTidsregSvar));
                    var svar = (MinTidsregSvar) deserializer.Deserialize(await response.Content.ReadAsStreamAsync());
                    return svar;
                }
            }
        }

        public static async Task<MinTidsregDagSvar> HentMinTidsregDagAsync(DateTimeOffset dato)
        {
            using (var http = new HttpClient {BaseAddress = BaseAddress})
            {
                var relativeUri = new Uri("HentMinTidsregDag", UriKind.Relative);
                var parameter = new HentMinTidsregDagParam(dato);
                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content))
                {
                    var deserializer = new XmlSerializer(typeof (MinTidsregDagSvar));
                    var svar = (MinTidsregDagSvar) deserializer.Deserialize(await response.Content.ReadAsStreamAsync());

                    foreach (var post in svar.Poster)
                    {
                        post.DatoValue = dato.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    }

                    return svar;
                }
            }
        }

        public static async Task<HentSaglisteOgKoderV2Svar> HentSagslisteOgKoderAsync(Koordinat? koordinat)
        {
            using (var http = new HttpClient { BaseAddress = BaseAddress })
            {
                var relativeUri = new Uri("HentSagslisteOgKoderV2", UriKind.Relative);
                var parameter = new HentSagslisteParamV2
                {
                    Max = 200,
                    Filter = koordinat.HasValue
                        ? string.Format("GEO {0} {1}", koordinat.Value.Breddegrad.ToString(CultureInfo.InvariantCulture), koordinat.Value.Længdegrad.ToString(CultureInfo.InvariantCulture))
                        : string.Empty
                };
                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content).ConfigureAwait(false))
                {
                    xml = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var deserializer = new XmlSerializer(typeof (HentSaglisteOgKoderV2Svar));
                    var svar = (HentSaglisteOgKoderV2Svar) deserializer.Deserialize(await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
                    return svar;
                }
            }
        }

        public static async Task RegistrerTidAsync(Sag sag, Aktivitet aktivitet, DateTimeOffset datoOgTid, int antalMinutter, string beskrivelse)
        {
            using (var http = new HttpClient { BaseAddress = BaseAddress })
            {
                var relativeUri = new Uri("TidsregistrerV2", UriKind.Relative);
                var parameter = new TidsregistrerParamV2
                {
                    Aktkode = aktivitet.Kode,
                    Beskrivelse = beskrivelse,
                    Dato = datoOgTid.ToString("yyyyMMdd"),
                    FraKlokken = datoOgTid.ToString("HH:mm"),
                    AntalMinutter = antalMinutter.ToString(),
                    Sagsnr = sag.Nummer
                };
                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public static async Task RetTidsregistreringAsync(string postId, Sag sag, Aktivitet aktivitet, DateTimeOffset datoOgTid, int antalMinutter, string beskrivelse)
        {
            using (var http = new HttpClient { BaseAddress = BaseAddress })
            {
                var relativeUri = new Uri("TidsregRet", UriKind.Relative);
                var parameter = new TidsregRetParam
                {
                    Aktkode = aktivitet.Kode,
                    Beskrivelse = beskrivelse,
                    FraKlokken = datoOgTid.ToString("HH:mm"),
                    AntalMinutter = antalMinutter.ToString(),
                    PostId = postId,
                    Sagsnr = sag.Nummer
                };

                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public static async Task SletTidsregistreringAsync(string postId)
        {
            using (var http = new HttpClient { BaseAddress = BaseAddress })
            {
                var relativeUri = new Uri("TidsregSlet", UriKind.Relative);
                var parameter = new TidsregSletParam {PostId = postId};
                var xml = Serialize(parameter);
                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (var response = await http.PostAsync(relativeUri, content))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private static string Serialize<T>(T t)
        {
            if (t == null)
            {
                return string.Empty;
            }
            
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var textWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                var type = t.GetType();
                new XmlSerializer(type).Serialize(xmlWriter, t);
                return textWriter.ToString();
            }
        }
    }
}