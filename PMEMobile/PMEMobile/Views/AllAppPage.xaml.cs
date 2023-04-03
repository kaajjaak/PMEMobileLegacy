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
    public partial class AllAppPage : ContentPage
    {
        LoginPage.Account account;
        HomePage.ApplicationJson appJson;
        public AllAppPage(LoginPage.Account pAccount, HomePage.ApplicationJson pAppJson)
        {
            InitializeComponent();
            account = pAccount;
            appJson = pAppJson;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    LoadAndroid();
                    break;
            }
        }

        private void LoadAndroid()
        {
            BindingContext = new InstalledAppViewModel();
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            AppProcess process = new AppProcess(appJson.application.Name, e.Item.ToString(), account.token);
            string url = $"https://databasegip2.herokuapp.com/application/{appJson.application.Id}/process/createProcess";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(process);
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Alert", "Toegevoegd!", "OK");
            }
            else
            {
                await DisplayAlert("Alert","Error", "OK");
            }
        }
        public class AppProcess
        {
            public string applicationName { get; set; }
            public string processName { get; set; }
            public string jwt { get; set; }

            public AppProcess(string appName, string pProcess, string token)
            {
                this.applicationName = appName;
                this.processName = pProcess;
                this.jwt = token;
            }
        }
    }
}