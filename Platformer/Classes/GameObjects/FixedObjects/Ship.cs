using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects.FixedObjects
{
    class Ship : FixedObject
    {
        const double _width = 200;
        const double _height = 200;
        const string _imageLocation = "/Assets/FixedObjects/ship.png";

        public Ship()
        {
            Image = new Image
            {
                Source = new BitmapImage(new Uri($"ms-appx://{_imageLocation}")),
                Width = _width,
                Height = _height
            };
        }

        //if entering the ship and the map is not complete, than complete
        public override void Action(Player player, Map map)
        {
            if (!map.IsComplete)
            {
                map.CompleteLvl();
                player.currentLvl++;
            }
            
        }
    }
}
