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
    public partial class SelectedAppPage : ContentPage
    {
        HomePage.ApplicationJson applicatie;
        LoginPage.Account account;
        public SelectedAppPage(LoginPage.Account pAccount, HomePage.ApplicationJson pApplicationJson)
        {
            InitializeComponent();
            applicatie = pApplicationJson;
            account = pAccount;
            loadProcessesFromServer();
        }

        async void listView_ItemTapped(object sender, EventArgs e) { 
            
        }
        async void AddApplication(object sender, EventArgs e)
        {

        }
        async void Refresh(object sender, EventArgs e)
        {

        }
        private async void loadProcessesFromServer()
        {
            string url = $"https://databasegip2.herokuapp.com/application/{applicatie.application.Id}/processList";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new HomePage.Token(account.token));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                //string responsedata = responseBody.Replace(@"\", "");
                List<ApplicationManager.ProcessJson> applicaties = JsonConvert.DeserializeObject<List<ApplicationManager.ProcessJson>>(responseBody);
                HomePage.processes.RemoveAll(x => applicaties.Contains(x));
                HomePage.processes.AddRange(applicaties); ;
                List<string> appNaam = new List<string>();
                foreach (ApplicationManager.ProcessJson invProcess in applicaties)
                {
                    appNaam.Add(invProcess.process.Name);
                }
                listView.ItemsSource = appNaam;
            }
            else
            {
                await DisplayAlert("Alert", "Er is een error!", "OK");
            }
        }
    }
}