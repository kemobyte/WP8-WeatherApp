using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Net.Http;
using System.IO;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;
using System.IO.IsolatedStorage;

namespace WeatherApp.Code
{
    public sealed class LocationManager
    {
        private static City CurrentCity { set; get; }
        public LocationManager()
        {

        }
        static private async Task<Geocoordinate> GetCurrentGeoPosition()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
                return geoposition.Coordinate;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
        public static async void GetCurrentDeviceGeoLocation()
        {
            Geocoordinate GeoCorResult = await GetCurrentGeoPosition();
            CurrentCity = new City();
            CurrentCity.Latitude = GeoCorResult.Latitude;
            CurrentCity.Longitude = GeoCorResult.Longitude;

            ReverseGeocodeQuery RGeoQ = new ReverseGeocodeQuery();
            RGeoQ.GeoCoordinate = new GeoCoordinate(GeoCorResult.Latitude, GeoCorResult.Longitude);
            RGeoQ.QueryCompleted += reverseGeocode_QueryCompleted;
            RGeoQ.QueryAsync();
        }
        private static async void reverseGeocode_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Result.Count > 0)
            {
                MapAddress geoAddress = e.Result[0].Information.Address;
                CurrentCity.CityName = geoAddress.City;        
                CurrentCity.CountryName = geoAddress.Country;
                CurrentCity.CountryCode = geoAddress.CountryCode;
                WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                if (!WVM.IsCityExists(CurrentCity,true))
                {
                    CurrentCity.IsCurrentCity = true;
                    CurrentCity.CityCurrentWeather = await WeatherManager.GetCityCurrentWeather(CurrentCity);
                    CurrentCity.FLickrImageURL = await FlickrServiceManager.GetCityWeatherPhoto(CurrentCity);
                    CurrentCity.CityWeatherWeeklyForecast = await WeatherManager.GetCityWeatherWeeklyForecast(CurrentCity);
                    CurrentCity.CityWeatherDailyForecast = await WeatherManager.GetCityWeatherHourlyForecast(CurrentCity);
                    WVM.CityCollection.Add(CurrentCity);
                    AppTilesManager.UpdateAppPrimaryTile();
                }
            }
        }
    }
 
}
