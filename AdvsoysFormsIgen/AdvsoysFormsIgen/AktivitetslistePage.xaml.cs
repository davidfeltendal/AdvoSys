using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public partial class AktivitetslistePage
    {
        private const string Key = "Aktiviteter";
        private readonly ISimpleCache cache;
        private readonly ObservableCollection<Aktivitet> aktiviteter;

        public AktivitetslistePage()
        {
            cache = Resolver.Resolve<ISimpleCache>();
            aktiviteter = new ObservableCollection<Aktivitet>();
            InitializeComponent();
        }

        public event EventHandler<ItemTappedEventArgs> ItemTapped
        {
            add { AktiviteterListView.ItemTapped += value; }
            remove { AktiviteterListView.ItemTapped -= value; }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            AktiviteterListView.ItemsSource = aktiviteter;
            HentFraCache();
            await Refresh();
        }

        private void HentFraCache()
        {
            var a = cache.Get<List<Aktivitet>>(Key);
            aktiviteter.Clear();
            foreach (var aktivitet in a)
            {
                aktiviteter.Add(aktivitet);
            }
        }

        private async Task Refresh()
        {
            Exception exception = null;

            try
            {
                IsBusy = true;
                SearchBar.IsEnabled = false;

                var svar = await AdvosysKlient.HentSagslisteOgKoderAsync(null);
                cache.Set(Key, svar.Aktiviteter.ToList());
                FejlLabel.IsVisible = false;
            }
            catch (WebException)
            {
                FejlLabel.IsVisible = true;
            }
            catch
            {
            }
            finally
            {
                SearchBar.IsEnabled = true;
                IsBusy = false;
                AktiviteterListView.IsRefreshing = false;
            }

            HentFraCache();
            FiltrerAktivitetsliste();
        }

        private void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            FiltrerAktivitetsliste();
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrerAktivitetsliste();
        }

        private void FiltrerAktivitetsliste()
        {
            var tekst = SearchBar.Text;

            if (tekst == null)
            {
                tekst = string.Empty;
            }

            tekst = tekst.ToLower();
            AktiviteterListView.ItemsSource = aktiviteter.Where(s => s.Tekst.ToLower().Contains(tekst) || s.Kode.ToLower().Contains(tekst)).ToList();
        }

        private async void AktiviteterListView_OnRefreshing(object sender, EventArgs e)
        {
            await Refresh();
        }
    }
}