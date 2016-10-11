using System;
using Xamarin.Forms;

namespace AdvsoysFormsIgen
{
    public partial class HubPage
    {
        public HubPage()
        {
            InitializeComponent();
        }

        private void HubPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Title = CurrentPage.Title;
            }
        }
    }
}