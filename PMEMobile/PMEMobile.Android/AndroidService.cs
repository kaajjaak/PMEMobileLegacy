using Android.App;
using Android.App.Usage;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using PMEMobile.Droid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.App.ActivityManager;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidService))]
namespace PMEMobile.Droid
{
    public class AndroidService : IAndroidService
    {
        public List<InApp> GetIntalledApps()
        {
            List<InApp> inApps = new List<InApp>();
            IList<ApplicationInfo> apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);
            for (int i = 0; i < apps.Count; i++)
            {
                inApps.Add(new InApp(apps[i].LoadLabel(Android.App.Application.Context.PackageManager), apps[i].PackageName));
            }
            return inApps;
        }


        IDictionary<int, bool> usage = new Dictionary<int, bool>();
        string previous = null;
        public void RunChecker(List<Tuple<string, int>> names, string jwt)
        {
            List<string> namen = new List<string>();
            List<int> ids = new List<int>();
            for (int i = 0; i < names.Count; i++)
            {
                namen.Add(names[i].Item1);
                ids.Add(names[i].Item2);
            }
            Context context = Android.App.Application.Context;
            Intent intent = new Intent(Android.Provider.Settings.ActionUsageAccessSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            UsageStatsManager usageStatsManager = (UsageStatsManager)context.GetSystemService("usagestats");
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            UsageEvents usageEvents = usageStatsManager.QueryEvents(milliseconds - 50000, milliseconds);
            UsageEvents.Event usageEvent = new UsageEvents.Event();
                while(usageEvents.HasNextEvent)
            {
                usageEvents.GetNextEvent(usageEvent);
                if(usageEvent.PackageName == "com.companyname.pmemobile")
                {
                    continue;
                }
                if(namen.Contains(usageEvent.PackageName) || (previous != null && namen.Contains(previous)))
                {
                        if (previous == null || previous != usageEvent.PackageName)
                        {
                            for (int i = 0; i < namen.Count; i++)
                            {
                                if (namen[i] == usageEvent.PackageName && (!usage.ContainsKey(ids[i]) || usage[ids[i]] != true))
                                {
                                    usage[ids[i]] = true;
                                    startUsage(ids[i], jwt);
                                }
                                else if (usage.ContainsKey(ids[i]) && usage[ids[i]] == true && previous == namen[i])
                            {
                                usage[ids[i]] = false;
                                endUsage(ids[i], jwt);
                            }
                            }
                        }
                }
                    previous = usageEvent.PackageName;
                
            }
        }

        private async void startUsage(int app_id, string token)
        {
            string url = $"https://databasegip2.herokuapp.com/application/{app_id}/usage/startUsage";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new Token(token));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
            }
            else
            {
            }
        }

        private async void endUsage(int app_id, string token)
        {
            string url = $"https://databasegip2.herokuapp.com/application/{app_id}/usage/endUsage";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(new Token(token));
            //MessageBox.Show(jsonString);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpClient = HttpClientFactory.Create();
            var httpResponseMessage = await httpClient.PostAsync(url, payload);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
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

        public async void kill(string packageName)
        {
            /*Context context = Android.App.Application.Context;
            ActivityManager activityManager = (ActivityManager)context.GetSystemService(Context.ActivityService);
            activityManager.KillBackgroundProcesses(packageName);*/
            //activityManager.KillBackgroundProcesses(packageName);
            Context context = Android.App.Application.Context;
            Intent intent = new Intent(Android.Provider.Settings.ActionUsageAccessSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            UsageStatsManager usageStatsManager = (UsageStatsManager)context.GetSystemService("usagestats");
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            UsageEvents usageEvents = usageStatsManager.QueryEvents(milliseconds - 2000, milliseconds);
            UsageEvents.Event usageEvent = new UsageEvents.Event();
            while (usageEvents.HasNextEvent)
            {
                usageEvents.GetNextEvent(usageEvent);
                if (usageEvent.PackageName == packageName)
                {
                    string url = "https://play.google.com/store/apps/details?id=com.sisystems.Sisystems";
                    await Browser.OpenAsync(url, BrowserLaunchMode.External);
                }

            }

        }
    }
}