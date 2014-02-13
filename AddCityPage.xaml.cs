using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using WeatherApp.Code;

namespace WeatherApp
{
    public partial class AddCityPage : PhoneApplicationPage
    {
        public AddCityPage()
        {
            InitializeComponent();
        }
        private async void CityNameSearchTxtBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // if the enter key is pressed
            if (e.Key == Key.Enter)
            {
                this.Focus();
                List<City> FoundedCities = await WeatherManager.FindCityByName(CityNameSearchTxtBox.Text);
                SearchResultLB.ItemsSource = FoundedCities;
            }

        }
        private void CityNameSearchTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CityNameSearchTxtBox.Text = string.Empty;
        }
        private async void NewCitySaveBtn_Click(object sender, EventArgs e)
        {
            if (SearchResultLB.SelectedIndex >= 0)
            {
                City NewCity = SearchResultLB.SelectedItem as City;
                WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                if (!WVM.IsCityExists(NewCity))
                {
                    WeatherProgressIndicator.ShowProgressIndicator();
                    WeatherProgressIndicator.UpdateProgressIndicator("Getting " + NewCity.CityName + " Weather Info");
                    NewCity.CityCurrentWeather = await WeatherManager.GetCityCurrentWeather(NewCity);
                    NewCity.CityWeatherWeeklyForecast = await WeatherManager.GetCityWeatherWeeklyForecast(NewCity);
                    NewCity.CityWeatherDailyForecast = await WeatherManager.GetCityWeatherHourlyForecast(NewCity);
                    NewCity.FLickrImageURL = await FlickrServiceManager.GetCityWeatherPhoto(NewCity);
                    WVM.CityCollection.Add(NewCity);

                    WeatherProgressIndicator.UpdateProgressIndicator("Saving " + NewCity.CityName);

                    await WeatherAppStorage.SaveWeatherData();

                    WeatherProgressIndicator.UpdateProgressIndicator("Added " + NewCity.CityName);
                    WeatherProgressIndicator.HideProgressIndicator();
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("City Already Exists!");
                }
            }
        }
        private void NewCityDiscardBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        private void AddCityPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }
    }
}