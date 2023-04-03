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
    public partial class ApplicationManager : ContentPage
    {
        LoginPage.Account account;
        HomePage.ApplicationJson applicationJson;
        public ApplicationManager(LoginPage.Account pAccount, HomePage.ApplicationJson pJson)
        {
            InitializeComponent();
            account = pAccount;
            applicationJson = pJson;
            
        }
        async void SelectApp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllAppPage(account, applicationJson));
        }

        async void ShowApp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectedAppPage(account, applicationJson));
        }

        async void LimitApp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LimitPage(applicationJson, account));
        }
        async void ClearUsage(object sender, EventArgs e)
        {
            string url = $"https://databasegip2.herokuapp.com/application/{applicationJson.application.Id}/usage/deleteAllUsage";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new HomePage.Token(account.token));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Success", "Gelukt!", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Er is een error!", "OK");
            }
        }
        public class ProcessGroup
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }



        }
        public class ProcessJson
        {
            [JsonProperty("process")]
            public ProcessGroup process { get; set; }

            [JsonProperty("id")]
            public int id { get; set; }
        }
    }
}