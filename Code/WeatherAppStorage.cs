using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WeatherApp.Code
{
    [XmlRoot(ElementName = "WeatherCities")]
    public class CityCollection
    {
        [XmlArray("Cities")]
        [XmlArrayItem("City")]
        public ObservableCollection<City> WeatherCities { set; get; }
    }
    public sealed class WeatherAppStorage
    {
        public WeatherAppStorage()
        {

        }
        private static async Task<StorageFile> CreateFile()
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile SettingFile = await storageFolder.CreateFileAsync("WeatherApp.xml", CreationCollisionOption.ReplaceExisting);
            return SettingFile;
        }
        public static async Task<bool> SaveWeatherData()
        {
            try
            {
                WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                StorageFile WeatherDataFile = await CreateFile();
                XmlSerializer serializer = new XmlSerializer(typeof(CityCollection));
                CityCollection WeatherData = new CityCollection { WeatherCities = WVM.CityCollection };
                using (Stream stream = await WeatherDataFile.OpenStreamForWriteAsync())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        serializer.Serialize(writer, WeatherData);
                        await writer.FlushAsync();
                    }
                    await stream.FlushAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

            /*try
            {
                WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                StorageFile SettingFile = await CreateFile();
                using (Stream stream = await SettingFile.OpenStreamForWriteAsync())
                {
                    MemoryStream sessionData = new MemoryStream();
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<City>));
                    serializer.WriteObject(sessionData, WVM.CityCollection);
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(stream);
                    await stream.FlushAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }*/
        }
        public static async Task<bool> LoadWeatherData()
        {
            try
            {
                StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile WeatherFile = await storageFolder.GetFileAsync("WeatherApp.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(CityCollection));

                Stream XmlFileStream = await WeatherFile.OpenStreamForReadAsync();
                XDocument CityCollectionXMLDoc = XDocument.Load(XmlFileStream);
                XmlFileStream.Dispose(); 
                                
                CityCollection WeatherSavedCities = serializer.Deserialize(CityCollectionXMLDoc.CreateReader()) as CityCollection;
                WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                WVM.BindCityCollection(WeatherSavedCities.WeatherCities);
                return true;
                /*using (IRandomAccessStream stream = await WeatherFile.OpenAsync(FileAccessMode.Read))
                {
                    stream.Seek(0);
                    CityCollection WeatherSavedCities = serializer.Deserialize(stream.AsStream()) as CityCollection;
                    WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                    WVM.BindCityCollection(WeatherSavedCities.WeatherCities);
                    await stream.FlushAsync();
                }*/
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            

            /*try
            {
                StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile WeatherFile = await storageFolder.GetFileAsync("WeatherApp.xml");
                using (IInputStream stream = await WeatherFile.OpenSequentialReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<City>));
                    ObservableCollection<City> SavedWeatherCityCollection = serializer.ReadObject(stream.AsStreamForRead()) as ObservableCollection<City>;
                    WeatherViewModel WVM = WeatherViewModel.GetInstanse();
                    WVM.BindCityCollection(SavedWeatherCityCollection);
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }*/
        }
    }
}
