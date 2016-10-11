using System;
using Newtonsoft.Json;

namespace AdvsoysFormsIgen
{
    public class TidsregistreringCache
    {
        public const string Key = "TidsregistreringerCache";

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public Sag Sag { get; set; }

        [JsonProperty]
        public Aktivitet Aktivitet { get; set; }

        [JsonProperty]
        public string Beskrivelse { get; set; }

        [JsonProperty]
        public DateTime Dato { get; set; }

        [JsonProperty]
        public DateTime FraKlokken { get; set; }

        [JsonProperty]
        public TimeSpan Forbrugt { get; set; }

        public bool ErNy
        { 
            get { return Id == null; }
        }
    }
}