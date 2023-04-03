using System;
using System.Collections.Generic;
using System.Text;

namespace PMEMobile
{
    public class InApp
    {
        public string AppName { get; set; }
        public string PackageName { get; set; }

        public InApp(string appName, string packageName)
        {
            AppName = appName;
            PackageName = packageName;
        }
        public override string ToString()
        {
            return PackageName;
        }
    }
}
