using System;

namespace AdvsoysFormsIgen
{
    public class AdvosysEventArgs : EventArgs
    {
        private readonly Exception error;

        public AdvosysEventArgs()
        {
        }

        public AdvosysEventArgs(Exception error)
        {
            this.error = error;
        }

        public Exception Error
        {
            get { return error; }
        }
    }

    public class AdvosysEventArgs<T> : AdvosysEventArgs
    {
        private readonly T svar;

        public AdvosysEventArgs(T svar)
        {
            this.svar = svar;
        }

        public AdvosysEventArgs(Exception error)
            : base(error)
        {
        }

        public T Svar
        {
            get { return svar; }
        }
    }
}