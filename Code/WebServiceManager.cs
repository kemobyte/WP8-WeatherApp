using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WeatherApp.Code
{
    class WebServiceManager
    {
        const string WebServiceURL = "http://api.openweathermap.org/data/2.5/";
        const string AppID = "2b83ed9c621a18521d58dc6c0cc3f4c1";

        static private string BuildWSURL(double Latitude, double Longitude, string CityName, string RequestType)
        {
            string URL = WebServiceURL;
            string StaticParameters = "&mode=xml&units=metric";
            string Action = string.Empty;

            switch (RequestType)
            {
                case "Current":
                    Action = "weather?";
                    break;
                case "Weekly":
                    Action = "forecast/daily?&cnt=7&";
                    break;
                case "Daily":
                    Action = "forecast?";
                    break;
                case "Find":
                    Action = "find?";
                    break;
            }

            if (String.IsNullOrEmpty(CityName))
            {
                URL += Action + "lat=" + Latitude.ToString("0.00") + "&lon=" + Longitude.ToString("0.00");
            }
            else
            {
                URL += Action + "q=" + CityName;
            }

            URL += StaticParameters;
            return URL;
        }

        static public async Task<string> CallWeatherWebService(double Latitude, double Longitude, string CityName, string RequestType)
        {
            string XMLData = string.Empty;
            string WeatherRequestURL = BuildWSURL(Latitude, Longitude, CityName, RequestType);

            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(WeatherRequestURL);
                XMLData = await response.Content.ReadAsStringAsync();
                return XMLData;
            }
            catch (HttpRequestException)
            {
                return XMLData;
            }
        }

        static public async Task<string> CallWebService(string URLWithQueryString)
        {
            string ResponseStr = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(URLWithQueryString);
                ResponseStr = await response.Content.ReadAsStringAsync();
                return ResponseStr;
            }
            catch (HttpRequestException)
            {
                return ResponseStr;
            }

        }
        static public async Task<string> CallWeatherWebService(string CityName, string RequestType)
        {
            string XMLData = string.Empty;
            string WeatherRequestURL = BuildWSURL(0,0, CityName, RequestType);

            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(WeatherRequestURL);
                XMLData = await response.Content.ReadAsStringAsync();
                return XMLData;
            }
            catch (HttpRequestException)
            {
                return XMLData;
            }
        }
    }
}
