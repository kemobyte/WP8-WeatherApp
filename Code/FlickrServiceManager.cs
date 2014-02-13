using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace WeatherApp.Code
{
    public class Photo
    {
        public string id { get; set; }
        public string owner { get; set; }
        public string secret { get; set; }
        public string server { get; set; }
        public int farm { get; set; }
        public string title { get; set; }
        public int ispublic { get; set; }
        public int isfriend { get; set; }
        public int isfamily { get; set; }
    }
    public class Photos
    {
        public int page { get; set; }
        public int pages { get; set; }
        public int perpage { get; set; }
        public string total { get; set; }
        public List<Photo> photo { get; set; }
    }
    public class FlickrPhotosSet
    {
        public Photos photos { get; set; }
        public string stat { get; set; }
    }
    public sealed class FlickrServiceManager
    {
        private const string FlickrAPIKey = "a890d02a6db02fcd59a6a4fb70e43318";
        private const string FlickrAPISecret = "0464286730e676b6";
        private const string FLickrAPIURLTemplate = "http://api.flickr.com/services/rest/?method=flickr.photos.search&format=json&nojsoncallback=1&api_key={0}&text={1}&per_page=10"; //&lat={2}&lon={3}&radius=1
        private const string PhotoURLTemplate = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_b.jpg";
        private static string[] FlickrAPILiscenseTypes = {"4","5","6","7"};
        public FlickrServiceManager()
        {

        }
        public static async Task<Uri> GetCityWeatherPhoto(City WeatherCity)
        {
            Uri PhotoURI = null;
            string Lisences = String.Join(",", FlickrAPILiscenseTypes);
            Lisences.Replace(",", "%2C");

            string TextSearch = WeatherCity.CityName + "%20" + WeatherCity.CityCurrentWeather.WeatherDescription.Replace(" ","%20");
            string SearchURL = string.Format(FLickrAPIURLTemplate, FlickrAPIKey, TextSearch);//, WeatherCity.Latitude, WeatherCity.Longitude);

            string JSONResponse = await WebServiceManager.CallWebService(SearchURL);

            FlickrPhotosSet FLickrData = JsonConvert.DeserializeObject<FlickrPhotosSet>(JSONResponse);
            List<Photo> FoundedPhotos = FLickrData.photos.photo;
            if (FoundedPhotos.Count > 0)
            {
                string PhotoURL = PhotoURL = string.Format(PhotoURLTemplate, FoundedPhotos[0].farm, FoundedPhotos[0].server, FoundedPhotos[0].id, FoundedPhotos[0].secret);
                PhotoURI = new Uri(PhotoURL);
            }
            return PhotoURI;
        }
    }
}
