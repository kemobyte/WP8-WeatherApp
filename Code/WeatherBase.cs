using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Code
{
    public abstract class WeatherBase
    {
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string WindSpeed { get; set; }
        public string WindName { get; set; }
        public string WindDirection { get; set; }
        public string WindDirectionCode { get; set; }
        public string CloudCoverage { get; set; }
        public string CloudCoverageDescription { get; set; }

    }
}
