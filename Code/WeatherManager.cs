using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.IO;
using Windows.Storage;
using System.Windows.Resources;

namespace WeatherApp.Code
{
    class WeatherManager
    {
        static public async Task<CurrentWeather> GetCityCurrentWeather(City CityObj)
        {

            string WeatherDataXML = await WebServiceManager.CallWeatherWebService(CityObj.Latitude, CityObj.Longitude, CityObj.CityName, "Current");

            XDocument CityWeatherXMLDoc = XDocument.Parse(WeatherDataXML);

            CurrentWeather CityWeather = (from XWeather in CityWeatherXMLDoc.Descendants("current")
                                          select new CurrentWeather
                                          {
                                              SunriseTime = XWeather.Element("city").Element("sun").Attribute("rise").Value,
                                              SunsetTime = XWeather.Element("city").Element("sun").Attribute("set").Value,
                                              Temperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("value").Value)).ToString() + "°",
                                              Humidity = XWeather.Element("humidity").Attribute("value").Value + XWeather.Element("humidity").Attribute("unit").Value,
                                              Pressure = XWeather.Element("pressure").Attribute("value").Value + " " + XWeather.Element("pressure").Attribute("unit").Value,
                                              WindSpeed = (double.Parse(XWeather.Element("wind").Element("speed").Attribute("value").Value) * 3.6).ToString("0.00") + " KpH",
                                              WindName = XWeather.Element("wind").Element("speed").Attribute("name").Value,
                                              WindDirection = XWeather.Element("wind").Element("direction").Attribute("value").Value,
                                              WindDirectionCode = XWeather.Element("wind").Element("direction").Attribute("code").Value,
                                              CloudCoverage = XWeather.Element("clouds").Attribute("value").Value,
                                              CloudCoverageDescription = XWeather.Element("clouds").Attribute("name").Value,
                                              WeatherDescription = XWeather.Element("weather").Attribute("value").Value.ToUpper(),
                                              WeatherIcon = "/Assets/WeatherImages/" + XWeather.Element("weather").Attribute("icon").Value + ".png",
                                              LastUpdateDate = XWeather.Element("lastupdate").Attribute("value").Value
                                          }).FirstOrDefault<CurrentWeather>();

            return CityWeather;
        }
        static public async Task<List<WeatherWeeklyForecast>> GetCityWeatherWeeklyForecast(City CityObj)
        {
            string format = "yyyy-MM-dd";
            CultureInfo provider = CultureInfo.InvariantCulture;

            string WeatherDataXML = await WebServiceManager.CallWeatherWebService(CityObj.Latitude, CityObj.Longitude, CityObj.CityName, "Weekly");

            XDocument CityWeatherXMLDoc = XDocument.Parse(WeatherDataXML);

            List<WeatherWeeklyForecast> WeeklyForecast = (from XWeather in CityWeatherXMLDoc.Descendants("time")
                                                          select new WeatherWeeklyForecast
                                                          {
                                                              WeekDay = DateTime.ParseExact(XWeather.Attribute("day").Value, format, provider).ToString("dddd"),
                                                              MinTemperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("min").Value)).ToString() + "°",
                                                              MaxTemperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("max").Value)).ToString() + "°",
                                                              Humidity = XWeather.Element("humidity").Attribute("value").Value + XWeather.Element("humidity").Attribute("unit").Value,
                                                              Pressure = XWeather.Element("pressure").Attribute("value").Value + XWeather.Element("pressure").Attribute("unit").Value,
                                                              WindSpeed = XWeather.Element("windSpeed").Attribute("mps").Value,
                                                              WindName = XWeather.Element("windSpeed").Attribute("name").Value,
                                                              WindDirection = XWeather.Element("windDirection").Attribute("deg").Value,
                                                              WindDirectionCode = XWeather.Element("windDirection").Attribute("code").Value,
                                                              CloudCoverage = XWeather.Element("clouds").Attribute("all").Value,
                                                              CloudCoverageDescription = XWeather.Element("clouds").Attribute("value").Value,
                                                              WeatherDescription = XWeather.Element("symbol").Attribute("name").Value,
                                                              WeatherIcon = "/Assets/WeatherImages/" + XWeather.Element("symbol").Attribute("var").Value + ".png",
                                                          }).ToList<WeatherWeeklyForecast>();

            return WeeklyForecast;
        }
        static public async Task<List<WeatherDailyForecast>> GetCityWeatherHourlyForecast(City CityObj)
        {
            string format = "yyyy-MM-ddTHH:mm:ss";
            CultureInfo provider = CultureInfo.InvariantCulture;

            int CurrentHour = int.Parse(DateTime.Now.ToString("HH"));
            string WeatherDataXML = await WebServiceManager.CallWeatherWebService(CityObj.Latitude, CityObj.Longitude, CityObj.CityName, "Daily");

            XDocument CityWeatherXMLDoc = XDocument.Parse(WeatherDataXML);

            List<WeatherDailyForecast> DailyForecast = (from XWeather in CityWeatherXMLDoc.Descendants("time")
                                                        select new WeatherDailyForecast
                                                        {
                                                            FromHour = DateTime.ParseExact(XWeather.Attribute("from").Value, format, provider).ToString("hh:mm tt"),
                                                            ToHour = XWeather.Attribute("to").Value,
                                                            FromDate = DateTime.ParseExact(XWeather.Attribute("from").Value, format, provider),
                                                            ToDate = DateTime.ParseExact(XWeather.Attribute("to").Value, format, provider),
                                                            Temperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("value").Value)).ToString() + "°",
                                                            MinTemperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("min").Value)).ToString() + "°",
                                                            MaxTemperature = Convert.ToInt32(double.Parse(XWeather.Element("temperature").Attribute("max").Value)).ToString() + "°",
                                                            Humidity = XWeather.Element("humidity").Attribute("value").Value + XWeather.Element("humidity").Attribute("unit").Value,
                                                            Pressure = XWeather.Element("pressure").Attribute("value").Value + XWeather.Element("pressure").Attribute("unit").Value,
                                                            WindSpeed = XWeather.Element("windSpeed").Attribute("mps").Value,
                                                            WindName = XWeather.Element("windSpeed").Attribute("name").Value,
                                                            WindDirection = XWeather.Element("windDirection").Attribute("deg").Value,
                                                            WindDirectionCode = XWeather.Element("windDirection").Attribute("code").Value,
                                                            CloudCoverage = XWeather.Element("clouds").Attribute("all").Value,
                                                            CloudCoverageDescription = XWeather.Element("clouds").Attribute("value").Value,
                                                            WeatherDescription = XWeather.Element("symbol").Attribute("name").Value,
                                                            WeatherIcon = "/Assets/WeatherImages/" + XWeather.Element("symbol").Attribute("var").Value + ".png",
                                                        }).Where<WeatherDailyForecast>(o => (o.FromDate.Date == DateTime.Today.Date)).Where<WeatherDailyForecast>(x => (CurrentHour <= x.ToDate.Hour)).ToList<WeatherDailyForecast>();

            return DailyForecast;
        }
        static public async Task<List<City>> FindCityByName(string CityNameQuery)
        {

            string WeatherDataXML = await WebServiceManager.CallWeatherWebService(CityNameQuery, "Find");

            XDocument CityWeatherXMLDoc = XDocument.Parse(WeatherDataXML);
            List<City> FoundedCities = (from XWeather in CityWeatherXMLDoc.Descendants("item")
                                      select new City
                                      {
                                          CityName = XWeather.Element("city").Attribute("name").Value,
                                          CountryCode = XWeather.Element("city").Element("country").Value,
                                          Latitude = double.Parse(XWeather.Element("city").Element("coord").Attribute("lat").Value),
                                          Longitude = double.Parse(XWeather.Element("city").Element("coord").Attribute("lon").Value)
                                      }).ToList<City>();

            foreach (City c in FoundedCities)
            {
                c.CountryName = await GetCountryName(c.CountryCode);
                c.CountryCode = await GetCountryIso3Code(c.CountryCode);
            }
            return FoundedCities;

        }
        static async public Task<bool> RefreshWeatherData()
        {
            List<City> TempCities = new List<City>();

            WeatherViewModel WVM = WeatherViewModel.GetInstanse();
            foreach(City c in WVM.CityCollection)
            {
                c.CityCurrentWeather = await GetCityCurrentWeather(c);
                c.CityWeatherDailyForecast = await GetCityWeatherHourlyForecast(c);
                c.FLickrImageURL = await FlickrServiceManager.GetCityWeatherPhoto(c);
                TempCities.Add(c);
            }
            WVM.CityCollection.Clear();
            foreach (City c in TempCities)
            {
                WVM.CityCollection.Add(c);
            }
            return true;
        }

        #region Helper Functions
        static async private Task<string> GetCountryName(string CountryIsoCode)
        {
            CountryCollection Countries = await CountryCollection.CreateAsync();
            string CountryName = Countries.CountryList.Where(i => i.CountryIso2Code == CountryIsoCode).Select(i => i.CountryName).FirstOrDefault<string>();
            return CountryName;
        }
        static async private Task<string> GetCountryIso3Code(string CountryIsoCode)
        {
            CountryCollection Countries = await CountryCollection.CreateAsync();
            string CountryCode = Countries.CountryList.Where(i => i.CountryIso2Code == CountryIsoCode).Select(i => i.CountryIso3Code).FirstOrDefault<string>();
            return CountryCode;
        }
        #endregion

    }
}
