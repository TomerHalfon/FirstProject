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
    class HealthBoost: FixedObject
    {
        const int _healthAmount = 50;

        const double _width = 50;
        const double _height = 50;
        const Stretch _stretch = Stretch.Fill;
        const string _imageLocation = "/Assets/FixedObjects/potion.png";
        public HealthBoost()
        {
            SetImageSource(_imageLocation);

            Image.Width = _width;
            Image.Height = _height;
            Image.Stretch = _stretch;
        }

        public override void Action(Player player, Map map)
        {
            if (player.GetHealth() < MoveableObject.MAX_HEALTH)
            {
                player.UpdateHealth(_healthAmount);
                map.GetFixedObjects().Remove(this);
                Image.Visibility = Visibility.Collapsed;
            }
        }
    }
}
