using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMEMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        async void LoginClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        async void RegisterClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}