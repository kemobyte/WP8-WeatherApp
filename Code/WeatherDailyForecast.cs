using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Code
{
    public class WeatherDailyForecast : WeatherBase
    {
        public string Temperature { get; set; }
        public string MinTemperature { get; set; }
        public string MaxTemperature { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public string FromHour { get; set; }
        public string ToHour { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
