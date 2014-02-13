using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeatherApp.Resources;
using WeatherApp.Code;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;

namespace WeatherApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        private void AddCityIconBtn_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddCityPage.xaml", UriKind.Relative));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = WeatherViewModel.GetInstanse();
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
        }
        private void ShareWeatherBtn_Click(object sender, EventArgs e)
        {
            WeatherViewModel WVM = WeatherViewModel.GetInstanse();
            City CurrentCity = WVM.GetCurrentCity();
            if (CurrentCity != null)
            {
                ShareStatusTask ShareWeatherStatus = new ShareStatusTask()
                {
                    Status = "It's " + CurrentCity.CityCurrentWeather.Temperature + "(" + CurrentCity.CityCurrentWeather.WeatherDescription + ") at " + CurrentCity.CityName + "," + CurrentCity.CountryName  
                };
                ShareWeatherStatus.Show();
            }
        }
        private void PinAsTileMenuBtn_Click(object sender, EventArgs e)
        {
            City SelectedCity = CitiesPivot.SelectedItem as City;
            if (SelectedCity != null)
            {
                AppTilesManager.PinCityWeatherTile(SelectedCity);
            }
        }
        private async void RemoveCityMenuBtn_Click(object sender, EventArgs e)
        {
            MessageBoxResult res =  MessageBox.Show("This City will be deleted.","Delete?",MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                City SelectedCity = CitiesPivot.SelectedItem as City;
                if (SelectedCity != null)
                {
                    WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                    WeatherProgressIndicator.ShowProgressIndicator();
                    WeatherProgressIndicator.UpdateProgressIndicator("Deleting " + SelectedCity.CityName);
                    if (WVM.RemoveCity(SelectedCity))
                    {
                        AppTilesManager.DeleteCityTile(SelectedCity.CityName);
                        await WeatherAppStorage.SaveWeatherData();
                        WeatherProgressIndicator.UpdateProgressIndicator("Deleted " + SelectedCity.CityName);
                    }
                    WeatherProgressIndicator.HideProgressIndicator();
                }                
            }

        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }

    }
}