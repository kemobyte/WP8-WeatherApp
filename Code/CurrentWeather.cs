using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WeatherApp.Code
{
    public class CurrentWeather : WeatherBase
    {
        public string SunsetTime { get; set; }
        public string SunriseTime { get; set; }
        public string Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public string LastUpdateDate { get; set; }
    }
}
