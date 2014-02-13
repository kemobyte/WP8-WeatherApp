using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Code
{
    public static class WeatherProgressIndicator
    {
        static WeatherProgressIndicator()
        {

        }
        public static void ShowProgressIndicator()
        {
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }
        public static void HideProgressIndicator()
        {
            SystemTray.ProgressIndicator.IsIndeterminate = false;
            SystemTray.ProgressIndicator.IsVisible = false;
        }
        public static void UpdateProgressIndicator(string Message)
        {
            SystemTray.ProgressIndicator.Text = Message;
        }

    }
}
