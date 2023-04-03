using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PMEMobile.Views.LoginPage;

namespace PMEMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        Boolean isWaiting = false;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (pass1.Text != pass2.Text)
            {
                await DisplayAlert("Alert", "Passwords need to be the same!", "OK");
                return;
            }
            if (!isWaiting)
            {
                LoginAccount account = new LoginAccount(username.Text, pass1.Text);
                string url = "https://databasegip2.herokuapp.com/accounts";
                string jsonString = System.Text.Json.JsonSerializer.Serialize(account);
                //MessageBox.Show(jsonString);
                var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var httpClient = HttpClientFactory.Create();
                var httpResponseMessage = await httpClient.PostAsync(url, payload);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
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
    }
}