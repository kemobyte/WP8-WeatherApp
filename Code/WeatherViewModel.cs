using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WeatherApp.Code
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private DateTime _lastUpdateDate;
        private readonly ObservableCollection<City> _cityCollection;
        public ObservableCollection<City> CityCollection
        {
            get
            {
                return _cityCollection;
            }
        }
        private static WeatherViewModel instance;
        private WeatherViewModel()
        {
            _cityCollection = new ObservableCollection<City>();
            _cityCollection.CollectionChanged += CityCollectionChanged;
        }
        public static WeatherViewModel GetInstanse()
        {
            if (instance == null)
            {
                instance = new WeatherViewModel();
            }
            return instance;
        }
        public void BindCityCollection(ObservableCollection<City> CopyFromCollection)
        {
            _cityCollection.Clear();
            foreach (City c in CopyFromCollection)
            {
               _cityCollection.Add(c);
            }
        }
        public bool RemoveCity(City CityToDelete)
        {
            return this.CityCollection.Remove(CityToDelete);
        }
        public bool IsCityExists(City FindCity){
            foreach (City c in CityCollection)
            {
                if (c.CityName == FindCity.CityName)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsCityExists(City FindCity, bool IsCurrentCityFlag)
        {
            bool IsExist = false;
            foreach (City c in CityCollection)
            {
                if (c.CityName == FindCity.CityName)
                {
                    if (c.IsCurrentCity != IsCurrentCityFlag)
                    {
                        c.IsCurrentCity = IsCurrentCityFlag;
                    }
                    IsExist = true;
                }
                else
                {
                    c.IsCurrentCity = !IsCurrentCityFlag;
                } 
            }
            return IsExist;
        }
        public City GetCurrentCity()
        {
            foreach (City c in CityCollection)
            {
                if (c.IsCurrentCity)
                {
                    return c;
                }
            }
            return null;
        }
        private void CityCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("CityCollection");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
