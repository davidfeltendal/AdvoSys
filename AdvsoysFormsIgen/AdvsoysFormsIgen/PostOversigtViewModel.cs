using System;

namespace AdvsoysFormsIgen
{
    public class PostOversigtViewModel : IEquatable<PostOversigtViewModel>
    {
        private readonly Post post;

        public PostOversigtViewModel(Post post)
        {
            this.post = post;
        }

        public DateTime Dato
        {
            get { return post.Dato; }
        }

        public string Text
        {
            get { return Dato.ToString("D"); }
        }

        public string Detail
        {
            get
            {
                if (string.IsNullOrEmpty(post.ForbrugtValue) && string.IsNullOrEmpty(post.DebiteretValue))
                {
                    return string.Empty;
                }

                return string.Format(
                    "{0} ({1} debiteret)",
                    post.ForbrugtValue,
                    post.DebiteretValue);
            }
        }

        public bool Equals(PostOversigtViewModel other)
        {
            return post.Equals(other.post);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PostOversigtViewModel);
        }

        public override int GetHashCode()
        {
            return post.GetHashCode();
        }
    }
}