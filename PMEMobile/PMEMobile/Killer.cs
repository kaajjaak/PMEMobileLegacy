using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PMEMobile
{
    public class Killer
    {
        public static void killer(string packageName)
        {
            DependencyService.Get<IAndroidService>().kill(packageName);
        }
    }
}
