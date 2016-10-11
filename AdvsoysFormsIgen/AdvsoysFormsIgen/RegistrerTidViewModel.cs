using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public class RegistrerTidViewModel : ViewModel
    {
        private readonly Command gemCommand;
        private readonly string id;
        private readonly ISimpleCache cache;

        private string beskrivelse;
        private Sag selectedSag;
        private Aktivitet selectedAktivitet;
        private DateTime dato;
        private TimeSpan fraKlokken;
        private TimeSpan forbrugt;
        private bool kanÆndreDato;

        private RegistrerTidViewModel()
        {
            gemCommand = new Command(async () => await Gem(), KanGemme);
            cache = Resolver.Resolve<ISimpleCache>();
        }

        public RegistrerTidViewModel(DateTime dato)
            : this()
        {
            Dato = dato;
            Tidspunkt = DateTime.Now.TimeOfDay;
            Tidsforbrug = TimeSpan.FromHours(1);
            kanÆndreDato = true;
        }

        public RegistrerTidViewModel(Post post)
            : this()
        {
            id = post.Id;
            Beskrivelse = post.Tekst;
            Dato = post.Dato;
            Tidspunkt = post.FraKlokken.TimeOfDay;
            Tidsforbrug = post.Forbrugt;
            kanÆndreDato = false;

            if (cache != null)
            {
                var sager = cache.Get<List<Sag>>("Sager") ?? new List<Sag>();
                var aktiviteter = cache.Get<List<Aktivitet>>("Aktiviteter") ?? new List<Aktivitet>();
                SelectedSag = sager.Find(sag => sag.Nummer == post.Sagsnummer);
                SelectedAktivitet = aktiviteter.Find(akt => akt.Kode == post.Aktivitetskode);
            }
        }

        public bool KanÆndreDato
        {
            get { return kanÆndreDato; }
            set
            {
                if (kanÆndreDato == value)
                {
                    return;
                }

                kanÆndreDato = value;
                NotifyPropertyChanged(() => KanÆndreDato);
            }
        }

        public async Task HentSagerOgAktiviteter()
        {
            try
            {
                var svar = await AdvosysKlient.HentSagslisteOgKoderAsync(null);
                cache.Set("Sager", svar.Sager.ToList());
                cache.Set("Aktiviteter", svar.Aktiviteter.ToList());
            }
            catch (Exception)
            {
            }
        }

        public bool ErIDag
        {
            get { return DateTime.Today.Date == dato.Date; }
        }

        public Sag SelectedSag
        {
            get { return selectedSag; }
            set
            {
                if (selectedSag == value)
                {
                    return;
                }

                selectedSag = value;
                NotifyPropertyChanged(() => SelectedSag);
                gemCommand.ChangeCanExecute();
            }
        }

        public Aktivitet SelectedAktivitet
        {
            get { return selectedAktivitet; }
            set
            {
                if (selectedAktivitet == value)
                {
                    return;
                }

                selectedAktivitet = value;
                NotifyPropertyChanged(() => SelectedAktivitet);
                gemCommand.ChangeCanExecute();
            }
        }

        public string Beskrivelse
        {
            get { return beskrivelse; }
            set
            {
                if (beskrivelse == value)
                {
                    return;
                }

                beskrivelse = value;
                NotifyPropertyChanged(() => Beskrivelse);
            }
        }

        public DateTime Dato
        {
            get { return dato; }
            set
            {
                if (dato == value)
                {
                    return;
                }

                dato = value;
                NotifyPropertyChanged(() => Dato);
            }
        }

        public TimeSpan Tidspunkt
        {
            get { return fraKlokken; }
            set
            {
                if (fraKlokken == value)
                {
                    return;
                }

                fraKlokken = value;
                NotifyPropertyChanged(() => Tidspunkt);
            }
        }

        public TimeSpan Tidsforbrug
        {
            get { return forbrugt; }
            set
            {
                if (forbrugt == value)
                {
                    return;
                }

                // Rund op til nærmeste 5. minut.
                forbrugt = TimeSpan.FromMinutes(5 * Math.Ceiling(value.TotalMinutes / 5));
                NotifyPropertyChanged(() => Tidsforbrug);
            }
        }

        public ICommand GemCommand
        {
            get { return gemCommand; }
        }

        private bool KanGemme()
        {
            return SelectedSag != null &&
                   SelectedAktivitet != null;
        }

        public async Task Gem()
        {
            if (IsBusy)
            {
                return;
            }

            var sag = SelectedSag;
            var aktivitet = SelectedAktivitet;
            var datoOgTid = new DateTimeOffset(dato.Date).Add(fraKlokken);
            var antalMinutter = (int) Tidsforbrug.TotalMinutes; // Rund op til nærmeste femte minut eller hvordan det var.
            var beskrivelse = Beskrivelse ?? string.Empty;

            try
            {
                IsBusy = true;

                if (id == null)
                {
                    await AdvosysKlient.RegistrerTidAsync(sag, aktivitet, datoOgTid, antalMinutter, beskrivelse);

                    // Send besked til achievement system.
                    MessagingCenter.Send(this, "TidRegistreret");
                }
                else
                {
                    await
                        AdvosysKlient.RetTidsregistreringAsync(id, sag, aktivitet, datoOgTid, antalMinutter, beskrivelse);
                }

                await Navigation.PopAsync();
            }
            catch (HttpRequestException ex)
            {
                MessagingCenter.Send(this, "TidsregistreringFejlede", ex);

                var tidsregistreringer = cache.Get<List<TidsregistreringCache>>(TidsregistreringCache.Key);

                if (!tidsregistreringer.Any(t =>
                    t.Beskrivelse == Beskrivelse && t.Sag.Nummer == SelectedSag.Nummer &&
                    t.Aktivitet.Kode == SelectedAktivitet.Kode))
                {
                    tidsregistreringer.Add(new TidsregistreringCache
                    {
                        Sag = SelectedSag,
                        Aktivitet = SelectedAktivitet,
                        Beskrivelse = Beskrivelse,
                        Dato = dato.Date,
                        Forbrugt = Tidsforbrug,
                        FraKlokken = DateTime.MinValue.Add(Tidspunkt),
                        Id = id
                    });
                    cache.Replace(TidsregistreringCache.Key, tidsregistreringer);
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, "TidsregistreringFejlede", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}