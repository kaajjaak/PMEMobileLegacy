using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMEMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LimitPage : ContentPage
    {
        HomePage.ApplicationJson app;
        LoginPage.Account account;
        public LimitPage(HomePage.ApplicationJson applicationJson, LoginPage.Account pToken)
        {
            InitializeComponent();
            app = applicationJson;
            account = pToken;
            loadLimits();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            string url = $"https://databasegip2.herokuapp.com/application/{app.application.Id}/limits/createlimit";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new Limit(account.token, berekenTijdInS()));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Succes!", "Succes!", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Er is een error!", "OK");
            }
        }

        public class Limit
        {
            public string token { get; set; }
            public int limit { get; set; }

            public Limit(string token, int limit)
            {
                this.token = token;
                this.limit = limit;
            }
        }

        async void loadLimits()
        {
            string url = $"https://databasegip2.herokuapp.com/application/{app.application.Id}/checkLimit";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new Limit(account.token, berekenTijdInS()));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);
            var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            string limit = JsonConvert.DeserializeObject<string>(responseBody);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                int limitTime = Convert.ToInt32(limit);
                TimeSpan time = TimeSpan.FromSeconds(limitTime);
                txtSeconds.Text = time.Seconds.ToString();
                txtMinutes.Text = time.Minutes.ToString();
                txtHours.Text = time.Hours.ToString();
                txtDays.Text = time.Days.ToString();
            }
            else
            {
                await DisplayAlert("Alert", "Er is een error!", "OK");
            }
        }

        private int berekenTijdInS()
        {
            int totaalD = String.IsNullOrEmpty(txtDays.Text) ? 0 : Convert.ToInt32(txtDays.Text);
            int totaalH = String.IsNullOrEmpty(txtHours.Text) ? totaalD * 24 : totaalD * 24 + Convert.ToInt32(txtHours.Text);
            int totaalM = String.IsNullOrEmpty(txtMinutes.Text) ? totaalH * 60 : totaalH * 60 + Convert.ToInt32(txtMinutes.Text);
            int totaalS = String.IsNullOrEmpty(txtSeconds.Text) ? totaalM * 60 : totaalM * 60 + Convert.ToInt32(txtSeconds.Text);
            return totaalS;
        }
    }
}