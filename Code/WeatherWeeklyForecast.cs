using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Code
{
    public class WeatherWeeklyForecast : WeatherBase
    {
        public string MinTemperature { get; set; }
        public string MaxTemperature { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public string WeekDay { get; set; } 

    }
}
