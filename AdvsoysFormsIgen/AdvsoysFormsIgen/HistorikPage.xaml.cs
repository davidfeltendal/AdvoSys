using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdvsoysFormsIgen
{
    public partial class HistorikPage
    {
        private readonly DateTime dato;

        public HistorikPage(DateTime dato)
        {
            this.dato = dato;
            InitializeComponent();

            if (Device.OS == TargetPlatform.WinPhone)
            {
                HistorikListView.GroupHeaderTemplate = Resources["WinPhoneHeaderTemplate"] as DataTemplate;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await HentPoster();
            MessagingCenter.Subscribe<PostViewModel>(this, "Refresh", async _ => await HentPoster());
            MessagingCenter.Subscribe<PostViewModel>(this, "SletFailed", async _ => await DisplayAlert("Der gik noget galt!", "Det var ikke muligt at slette posten. Prøv igen senere.", "OK"));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<PostViewModel>(this, "Refresh");
            MessagingCenter.Unsubscribe<PostViewModel>(this, "SletFailed");
        }

        private async Task HentPoster()
        {
            try
            {
                IsBusy = true;
                var svar = await AdvosysKlient.HentMinTidsregDagAsync(dato);
                var poster = svar.Poster
                                 .OrderByDescending(p => p.FraKlokken)
                                 .Select(p => new PostViewModel(dato, p))
                                 .GroupBy(g => g.Dato)
                                 .Select(g => new Group<DateTime, PostViewModel>(g.Key, g))
                                 .ToList();
                HistorikListView.ItemsSource = poster;

                if (poster.Count == 0)
                {
                    ShowEmptyLabel("Der findes ingen tidregistreringer for denne dato.");
                }
                else
                {
                    EmptyLabel.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                ShowEmptyLabel("Der kunne ikke hentes tidsregistreringer for denne dato: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowEmptyLabel(string message)
        {
            EmptyLabel.Text = message;
            EmptyLabel.IsVisible = true;
        }

        private void HistorikListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (HistorikListView.SelectedItem != null)
            {
                HistorikListView.SelectedItem = null;
            }
        }

        private async void HistorikListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var postViewModel = (PostViewModel) e.Item;
            var page = new RegistrerTidPage(postViewModel.Post);
            await Navigation.PushAsync(page, true);
        }

        private async void NyTidsregistrering_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrerTidPage(dato));
        }

        private async void HistorikListView_OnRefreshing(object sender, EventArgs e)
        {
            try
            {
                await HentPoster();
            }
            finally
            {
                HistorikListView.IsRefreshing = false;
            }
        }
    }
}