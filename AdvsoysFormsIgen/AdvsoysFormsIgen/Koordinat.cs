namespace AdvsoysFormsIgen
{
    public struct Koordinat
    {
        private readonly double breddegrad;
        private readonly double længdegrad;

        public Koordinat(double breddegrad, double længdegrad)
        {
            this.breddegrad = breddegrad;
            this.længdegrad = længdegrad;
        }

        public double Breddegrad
        {
            get { return breddegrad; }
        }

        public double Længdegrad
        {
            get { return længdegrad; }
        }
    }
}