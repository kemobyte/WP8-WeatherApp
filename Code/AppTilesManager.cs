using CustomLiveTileExample;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace WeatherApp.Code
{
    public sealed class AppTilesManager
    {
        public AppTilesManager()
        {

        }

        static private string GetWeatherTileTemplateAsImageFile(City SelectedCity)
        {
            TileControl TileTemplate = new TileControl();
            TileTemplate.DataContext = SelectedCity;

            TileTemplate.Measure(new Size(173, 173));
            TileTemplate.Arrange(new Rect(0, 0, 173, 173));
            var bmp = new WriteableBitmap(173, 173);
            bmp.Render(TileTemplate, null);
            bmp.Invalidate();

            var isf = IsolatedStorageFile.GetUserStoreForApplication();
            var filename = "/Shared/ShellContent/tile.jpg";

            if (!isf.DirectoryExists("/Shared/ShellContent"))
            {
                isf.CreateDirectory("/Shared/ShellContent");
            }

            using (var stream = isf.OpenFile(filename, FileMode.OpenOrCreate))
            {
                bmp.SaveJpeg(stream, 173, 173, 0, 100);
            }

            return filename;
        }
        static private bool IsCityTilePinned(string CityName)
        {
            bool found = false;
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("City=" + CityName));
            if (tile != null)
            {
                found = true;
            }
            return found;
        }
        static public void PinCityWeatherTile(City SelectedCity)
        {
            if (SelectedCity.IsCurrentCity)
            {
                MessageBox.Show(SelectedCity.CityName + " is Aleardy Pinned");
                return;
            }
            FlipTileData PrimaryTileData = new FlipTileData();
            PrimaryTileData.Count = 0;
            PrimaryTileData.Title = "WeatherApp";
            PrimaryTileData.BackgroundImage = new Uri("isostore:" + GetWeatherTileTemplateAsImageFile(SelectedCity), UriKind.Absolute);
            if (!IsCityTilePinned(SelectedCity.CityName))
            {
                ShellTile.Create(new Uri("/MainPage.xaml?City=" + SelectedCity.CityName, UriKind.Relative), PrimaryTileData, false);
            }
            else
            {
                MessageBox.Show(SelectedCity.CityName + " is Aleardy Pinned");
            }
        }
        static public void UpdateAppPrimaryTile()
        {
            WeatherViewModel WVM = WeatherViewModel.GetInstanse();
            City CurrentCity = WVM.GetCurrentCity();

            FlipTileData PrimaryTileData = new FlipTileData();
            PrimaryTileData.Count = 0;
            PrimaryTileData.BackgroundImage = new Uri("isostore:" + GetWeatherTileTemplateAsImageFile(CurrentCity), UriKind.Absolute);

            IEnumerator<ShellTile> WeatherAppTiles = ShellTile.ActiveTiles.GetEnumerator();
            WeatherAppTiles.MoveNext();
            if (WeatherAppTiles.Current != null)
            {
                ShellTile primaryTile = WeatherAppTiles.Current;
                primaryTile.Update(PrimaryTileData);
            }
            else
            {
                ShellTile.Create(new Uri("/MainPage.xaml", UriKind.Relative), PrimaryTileData, false);
            }

        }
        static public void DeleteCityTile(string CityName)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("City=" + CityName));
            if (tile != null)
            {
                tile.Delete();
            }
        }

    }
}
