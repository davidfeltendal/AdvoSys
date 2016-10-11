using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public partial class MainPage
    {
        private bool notLoaded;

        public MainPage()
        {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS)
            {
                ToolbarItems.Add(new ToolbarItem("Dato", "history@2x.png", () => IsPresented = !IsPresented, ToolbarItemOrder.Primary, 0));
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var cache = Resolver.Resolve<ISimpleCache>();

            if (cache != null)
            {
                var cachedPosts = cache.Get<List<TidsregistreringCache>>(TidsregistreringCache.Key);
                var kø = new Queue<TidsregistreringCache>(cachedPosts);

                while (kø.Count > 0)
                {
                    var post = kø.Dequeue();

                    try
                    {
                        if (post.ErNy)
                        {
                            await AdvosysKlient.RegistrerTidAsync(
                                post.Sag,
                                post.Aktivitet,
                                post.Dato.Date.Add(post.FraKlokken.TimeOfDay),
                                (int)post.Forbrugt.TotalMinutes,
                                post.Beskrivelse);

                            // Send besked til achievement system.
                            MessagingCenter.Send(this, "TidRegistreret");
                        }
                        else
                        {
                            await AdvosysKlient.RetTidsregistreringAsync(
                                post.Id,
                                post.Sag,
                                post.Aktivitet,
                                post.Dato.Date.Add(post.FraKlokken.TimeOfDay),
                                (int) post.Forbrugt.TotalMinutes,
                                post.Beskrivelse);
                        }
                    }
                    catch
                    {
                        kø.Enqueue(post);
                        break;
                    }
                }

                cache.Replace(TidsregistreringCache.Key, kø.ToList());

                if (cachedPosts.Count > 0 && kø.Count == 0)
                {
                    await DisplayAlert("Gemte tidsregistreringer", "De gemte tidsregistreringer blev afsendt.", "OK");
                }
            }

            if (notLoaded == false)
            {
                await HentPostOversigt();
                notLoaded = true;
            }
        }

        private async Task HentPostOversigt()
        {
            Exception exception = null;

            try
            {
                Master.IsBusy = true;
                var svar = await AdvosysKlient.HentMinTidregAsync();
                var poster = svar.Poster
                                 .Select(p => new PostOversigtViewModel(p))
                                 .ToList();
                OversigtListView.ItemsSource = poster;
                OversigtListView.SelectedItem = poster.FirstOrDefault();
            }
            catch (WebException hrex)
            {
                Detail = new HistorikPage(DateTime.Today);
                IsPresented = false;
                exception = hrex;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                Master.IsBusy = false;
            }

            if (exception != null)
            {
                await DisplayAlert("Advosys", "Der gik noget galt: " + exception.Message, "OK");
            }
        }

        private void OversigtListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var kvp = (PostOversigtViewModel) e.SelectedItem;
            Detail = new HistorikPage(kvp.Dato);
            IsPresented = false;
        }

        private async void OversigtListView_OnRefreshing(object sender, EventArgs e)
        {   
            await HentPostOversigt();
            OversigtListView.IsRefreshing = false;
        }
    }
}