using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace WeatherApp.Code
{
    public class Country
    {
        public string CountryName { get; set; }
        public string CountryIso2Code { get; set; }
        public string CountryIso3Code { get; set; }
        public string CountryFlag
        {
            get
            {
                return "/Assets/CountryFlags/" + CountryIso3Code.ToUpper() + ".png";
            }
        }

    }
    
    /// <summary>
    /// Singleton Class Implementation
    /// </summary>
    public sealed class CountryCollection
    {
        private static CountryCollection instance;
        public List<Country> CountryList { get; set; }
        private CountryCollection()
        {
           
        }

        private async Task<CountryCollection> LoadAsyncCountriesFromXML()
        {
            if (CountryList == null)
            {
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string FileName = "Data\\CountryList.xml";
                StorageFile file = await InstallationFolder.GetFileAsync(FileName);
                Stream XmlFileStream = await file.OpenStreamForReadAsync();
                XDocument CountriesXMLData = XDocument.Load(XmlFileStream);
                XmlFileStream.Dispose();

                CountryList = (from xcountry in CountriesXMLData.Descendants("country")
                               select new Country
                               {
                                   CountryIso2Code = xcountry.Element("countryCode").Value,
                                   CountryIso3Code = xcountry.Element("isoAlpha3").Value,
                                   CountryName = xcountry.Element("countryName").Value
                               }).ToList<Country>();

            }
            return this;
        }

        public static Task<CountryCollection> CreateAsync()
        {
            if (instance == null)
            {
                instance = new CountryCollection();
            }
            return instance.LoadAsyncCountriesFromXML();
        }
    }
}
