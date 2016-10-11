using System;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;

namespace AdvsoysFormsIgen
{
    public partial class RegistrerTidPage
    {
        private readonly RegistrerTidViewModel viewModel;
        private readonly SagslistePage sagslistePage;
        private readonly AktivitetslistePage aktivitetslistePage;

        private RegistrerTidPage()
        {
            Title = "Registrér tid";
            InitializeComponent();

            sagslistePage = new SagslistePage();
            sagslistePage.ItemTapped += SagslistePageOnItemTapped;

            aktivitetslistePage = new AktivitetslistePage();
            aktivitetslistePage.ItemTapped += AktivitetslistePageOnItemTapped;
        }

        public RegistrerTidPage(DateTime dato)
            : this()
        {
            BindingContext = viewModel = new RegistrerTidViewModel(dato)
            {
                Navigation = new ViewModelNavigation(Navigation),
            };
        }

        public RegistrerTidPage(Post post)
            : this()
        {
            BindingContext = viewModel = new RegistrerTidViewModel(post)
            {
                Navigation = new ViewModelNavigation(Navigation)
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<RegistrerTidViewModel, Exception>(this, "TidsregistreringFejlede",
                async (vm, ex) =>
                {
                    await DisplayAlert("Der gik noget galt!",
                            "Tidsregistreringen fejlede - vi har gemt din tidsregistrering, der bliver afsendt, så snart der er en internetforbindelse. " + ex.Message, "OK");
                    await Navigation.PopAsync();
                });
            await viewModel.HentSagerOgAktiviteter();
        }

        private async void SagslistePageOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PopAsync();
            viewModel.SelectedSag = e.Item as Sag;
        }

        private async void AktivitetslistePageOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PopAsync();
            viewModel.SelectedAktivitet = e.Item as Aktivitet;
        }

        private async void VælgSagCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(sagslistePage);
        }

        private async void VælgAktivitetCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(aktivitetslistePage);
        }

        private void VælgDatoCell_OnTapped(object sender, EventArgs e)
        {
            if (DatoPicker.IsEnabled)
            {
                DatoPicker.Focus();
            }
        }

        private void VælgTidspunktCell_OnTapped(object sender, EventArgs e)
        {
            TidspunktPicker.Focus();
        }

        private void VælgTidsforbrugCell_OnTapped(object sender, EventArgs e)
        {
            TidsforbrugPicker.Focus();
        }
    }
}