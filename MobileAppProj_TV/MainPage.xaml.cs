using MobileAppProj_TV.ViewsPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppProj_TV
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnLogIn_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["Name"] = txtName.Text;
            Navigation.PushAsync(new TermHomePage());
        }
    }
}
