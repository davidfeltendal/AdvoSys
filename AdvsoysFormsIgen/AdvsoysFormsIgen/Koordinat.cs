namespace AdvsoysFormsIgen
{
    public struct Koordinat
    {
        private readonly double breddegrad;
        private readonly double l�ngdegrad;

        public Koordinat(double breddegrad, double l�ngdegrad)
        {
            this.breddegrad = breddegrad;
            this.l�ngdegrad = l�ngdegrad;
        }

        public double Breddegrad
        {
            get { return breddegrad; }
        }

        public double L�ngdegrad
        {
            get { return l�ngdegrad; }
        }
    }
}