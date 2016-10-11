using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdvsoysFormsIgen
{
    public class PostViewModel : IComparable<PostViewModel>
    {
        private readonly DateTime dato;
        private readonly Post post;
        private readonly Command sletCommand;

        public PostViewModel(DateTime dato, Post post)
        {
            this.dato = dato;
            this.post = post;
            sletCommand = new Command(Slet);
        }

        public ICommand SletCommand
        {
            get { return sletCommand; }
        }

        public DateTime Dato
        {
            get { return dato; }
        }

        public Post Post
        {
            get { return post; }
        }

        public string Text
        {
            get { return string.Format("{0}: {1}", post.Aktivitetskode, post.Tekst); }
        }

        public string Detail
        {
            get
            {
                var fraKlokken = post.FraKlokken;
                var tilKlokken = fraKlokken.Add(post.Forbrugt);
                return string.Format("{0} - {1}", fraKlokken.ToString("t"), tilKlokken.ToString("t"));
            }
        }

        public int CompareTo(PostViewModel other)
        {
            return Post.CompareTo(other.Post);
        }

        private async void Slet()
        {
            try
            {
                await AdvosysKlient.SletTidsregistreringAsync(post.Id);
                MessagingCenter.Send(this, "Refresh");
            }
            catch (Exception)
            {
                MessagingCenter.Send(this, "SletFailed");
            }
        }
    }
}