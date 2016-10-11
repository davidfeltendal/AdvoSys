using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace AdvsoysFormsIgen
{
    public class Achievement : INotifyPropertyChanged
    {
        public const string Key = "Achievements";
        private bool _opnået;

        public Achievement(int id, string ikon, string beskrivelse)
        {
            Id = id;
            Ikon = ikon;
            Beskrivelse = beskrivelse;
        }

        public Achievement()
        {
        }

        public string Beskrivelse { get; set; }
        public int Id { get; set; }
        public string Ikon { get; set; }
        public DateTimeOffset? DatoOpnået { get; set; }

        [JsonIgnore]
        public string IkonForRealz
        {
            get
            {
                if (Opnået)
                {
                    return Ikon;
                }
                else
                {
                    var fileName = Path.GetFileNameWithoutExtension(Ikon);
                    return fileName + "_gray.png";
                }
            }
        }

        public bool Opnået
        {
            get { return _opnået; }
            set
            {
                if (_opnået == value)
                {
                    return;
                }

                _opnået = value;
                DatoOpnået = DateTimeOffset.Now;
                OnPropertyChanged();
                OnPropertyChanged("DatoOpnået");
                OnPropertyChanged("IkonForRealz");
            }
        }

        [JsonIgnore]
        public string DatoOpnåetString
        {
            get
            {
                if (DatoOpnået.HasValue)
                {
                    return DatoOpnået.Value.ToString("f");
                }

                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}