using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WeatherApp.Code
{
    public class City
    {
        public City()
        {

        }
        public string CityName { set; get; }
        public string CountryName { set; get; }
        public string CountryCode { set; get; }
        [XmlIgnore()]
        public string CountryFlag
        {
            get
            {
                if (!string.IsNullOrEmpty(CountryCode))
                {
                    return "/Assets/CountryFlags/" + CountryCode.ToUpper() + ".png";

                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public double Latitude { set; get; }
        public double Longitude { set; get; }
        [XmlIgnore()]
        public Uri FLickrImageURL { set; get; }
        public bool IsCurrentCity { set; get; }
        [XmlIgnore()]
        public string CurrentCityFlag
        {
            get
            {
                string ImagePath = string.Empty;
                if (IsCurrentCity)
                {
                    ImagePath = "/Assets/geolocate48.png";
                }
                return ImagePath;
            }
        }
        public CurrentWeather CityCurrentWeather { set; get; }
        public List<WeatherDailyForecast> CityWeatherDailyForecast { set; get; }
        public List<WeatherWeeklyForecast> CityWeatherWeeklyForecast { set; get; }
    }
}
