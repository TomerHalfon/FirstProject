using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects.FixedObjects
{
   public class FuelTank : FixedObject
    {
        const int _fuelAmount = 50;
        const double _width = 50;
        const double _height = 50;
        const Stretch _stretch = Stretch.Fill;
        const string _imageLocation = "/Assets/FixedObjects/fuel.png";
        public FuelTank()
        {
            SetImageSource(_imageLocation);
            Image.Width = _width;
            Image.Height = _height;
            Image.Stretch = _stretch;
        }
        public override void Action(Player player, Map map)
        {
            if (player.GetFuel() < Player.MAX_FUEL) //avoid the tank if fuel is full
            {
                player.UpdateFuel(_fuelAmount);
                map.GetFixedObjects().Remove(this);
                Image.Visibility = Visibility.Collapsed;
            }
        }
    }
}
