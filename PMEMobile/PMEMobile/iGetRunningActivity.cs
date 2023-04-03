using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PMEMobile
{
    public class iGetRunningActivity
    {
        public static void getAppRunning(List<Tuple<string, int>> names, string jwt)
        {
            DependencyService.Get<IAndroidService>().RunChecker(names, jwt);
        }
    }
}
