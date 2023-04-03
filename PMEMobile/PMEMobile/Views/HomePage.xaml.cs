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
    public partial class HomePage : ContentPage
    {
        LoginPage.Account account;
        public HomePage(LoginPage.Account pAccount)
        {
            InitializeComponent();
            account = pAccount;
            LoadApplications();
            startTimer();
            startChecker();
            startKiller();
             
        }
        public static List<ApplicationJson> applicaties = new List<ApplicationJson>();
        List<string> applicatienamen;
        public async void LoadApplications()
        {
            Token token = new Token(account.token);
            string url = "https://databasegip2.herokuapp.com/application/applicationList";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(token);
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                //string responsedata = responseBody.Replace(@"\", "");


                applicaties = JsonConvert.DeserializeObject<List<ApplicationJson>>(responseBody);

                applicatienamen = new List<string>();
                foreach (ApplicationJson application in applicaties)
                {
                    applicatienamen.Add(application.application.Name);
                }
                listView.ItemsSource = applicatienamen;

            }
            else
            {
            }
        }


        public class Token
        {
            public string token { get; set; }

            public Token(string token)
            {
                this.token = token;
            }
        }
        public class ApplicationGroup
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            public override string ToString()
            {
                return $"{Id} - {Name}";
            }
        }
        public class ApplicationJson
        {
            [JsonProperty("application")]
            public ApplicationGroup application { get; set; }
        }

        async void AddApplication(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ApplicationCreator(account));
        }
        private void Refresh(object sender, EventArgs e)
        {
            LoadApplications();
        }
        public static List<ApplicationManager.ProcessJson> processes = new List<ApplicationManager.ProcessJson>();
        private void check()
        {
            List<Tuple<string, int>> processNames = new List<Tuple<string, int>>();
            foreach (ApplicationManager.ProcessJson process in processes)
            {
                processNames.Add(new Tuple<string,int>(process.process.Name, process.id));
            }
            iGetRunningActivity.getAppRunning(processNames, account.token);
        }

        async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ApplicationManager(account, applicaties[e.ItemIndex]));
        }

        void startTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                check();

                return true; // True = Repeat again, False = Stop the timer
            });
        }
        List<string> processesToKill = new List<string>();
        async void startChecker()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(async () =>
                {
                    string url = $"https://databasegip2.herokuapp.com/accounts/checklimits";
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(new HomePage.Token(account.token));
                    //MessageBox.Show(jsonString);
                    var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var httpClient = HttpClientFactory.Create();
                    var httpResponseMessage = await httpClient.PostAsync(url, payload);

                    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                        processesToKill = JsonConvert.DeserializeObject<List<string>>(responseBody);
                        kill();
                    }
                    else
                    {

                    }
                });

                return true; // True = Repeat again, False = Stop the timer
            });
        }

        void startKiller()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                kill();

                return true; // True = Repeat again, False = Stop the timer
            });
        }

        void kill()
        {
            foreach (string process in processesToKill)
            {
                Killer.killer(process);
            }
        }
    }
}