using PMEMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace PMEMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public class LoginAccount
        {
            private string Username;
            private string Password;

            public LoginAccount(string username, string password)
            {
                this.Username = username;
                this.Password = password;
            }

            public string username { get => Username; set => Username = value; }
            public string password { get => Password; set => Password = value; }
        }

        public class Account
        {
            private string Username;
            private string Token;

            public Account(string username, string token)
            {
                this.Username = username;
                this.Token = token;
            }

            public string username { get => Username; set => Username = value; }
            public string token { get => Token; set => Token = value; }
        }

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        Boolean isWaiting = false;
        async void ClickLogin(object sender, EventArgs e)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                LoginAccount account = new LoginAccount(dUsername.Text, dPassword.Text);
                string url = "https://databasegip2.herokuapp.com/accounts/login";
                string jsonString = System.Text.Json.JsonSerializer.Serialize(account);
                //MessageBox.Show(jsonString);
                var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var httpClient = HttpClientFactory.Create();
                var httpResponseMessage = await httpClient.PostAsync(url, payload);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                    Account account1 = JsonConvert.DeserializeObject<Account>(responseBody);
                    await Navigation.PushAsync(new HomePage(account1));
                }
                else
                {
                    await DisplayAlert("Alert", "Er is een error!", "OK");
                }
                isWaiting = false;
            }
        }
        async void ClickCancel(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }

    }
}