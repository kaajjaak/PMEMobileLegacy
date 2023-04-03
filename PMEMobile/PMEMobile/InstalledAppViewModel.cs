using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace PMEMobile
{
    public class InstalledAppViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<InApp> installedApps;
        public ObservableCollection<InApp> InstalledApps
        {
            get { return installedApps; }
            set
            {

                installedApps = value;
            }
        }

        public InstalledAppViewModel()
        {
            List<InApp> listOfInstalledApps = DependencyService.Get<IAndroidService>().GetIntalledApps();
            InstalledApps = new ObservableCollection<InApp>(listOfInstalledApps);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
