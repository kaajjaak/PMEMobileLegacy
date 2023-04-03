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
    public partial class ApplicationCreator : ContentPage
    {
        LoginPage.Account account;
        public ApplicationCreator(LoginPage.Account pAccount)
        {
            InitializeComponent();
            account = pAccount;
        }

        public class ApplicationName
        {
            public ApplicationName(string application, string jwt)
            {
                this.applicationName = application;
                this.jwt = jwt;
            }
            public string applicationName { get; }
            public string jwt { get; }
        }
        public class ID
        {
            public ID(string id)
            {
                this.id = id;
            }
            public string id { get; }
        }

        async void CreateApplication(object sender, EventArgs e)
        {
            ApplicationName appName = new ApplicationName(AppName.Text, account.token);
            string url = "https://databasegip2.herokuapp.com/application/createApplication";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(appName);
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                ID ID1 = JsonConvert.DeserializeObject<ID>(responseBody);
                await Navigation.PushAsync(new HomePage(account));
            }
            else
            {
                await DisplayAlert("Alert", "Er is een error!", "OK");
            }
        }


    }
}