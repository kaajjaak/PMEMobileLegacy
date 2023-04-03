using System;
using System.Collections.Generic;
using System.Text;

namespace PMEMobile
{
    public interface IAndroidService
    {
        List<InApp> GetIntalledApps();
        void RunChecker(List<Tuple<string, int>> names, string jwt);
        void kill(string packageName);
    }
}
