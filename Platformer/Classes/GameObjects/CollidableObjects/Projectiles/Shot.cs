using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Platformer.MenuPages;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;

namespace Platformer.Classes.GameObjects.CollidableObjects.Projectiles
{
    public class Shot : Projectile
    {
        const string _imageLocation = "/Assets/FixedObjects/laser.png";
        const double _width = 50;
        const double _shotMovementSpeed = 20;
        const double _height = 15;
        
        public Shot(Point startPosition,Canvas canvas, Map map,bool isMovingRight,MoveableObject shotBy) 
            :base(canvas,map,shotBy, startPosition, _imageLocation, _width, _height,_shotMovementSpeed,isMovingRight)
        {
        }
    }
}
