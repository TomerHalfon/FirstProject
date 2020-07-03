using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Platformer.Classes.GameObjects.CollidableObjects.Projectiles
{
    public class Bolder : Projectile
    {
        const string _imageLocation = "/Assets/FixedObjects/Bolder.gif";
        const double _width = 200;
        const double _shotMovementSpeed = 15;
        const double _height = 200;

        public Bolder(Point startPosition, Canvas canvas, Map map, bool isMovingRight, MoveableObject shotBy)
            : base(canvas, map, shotBy, startPosition, _imageLocation, _width, _height, _shotMovementSpeed, isMovingRight)
        {
            
        }
    }
}
