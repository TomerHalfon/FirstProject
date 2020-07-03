using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
    class Droid : Enemy
    {
        public static int ShotDmg = 30;


        const string _pathR = "/Assets/Characters/droid.png";
        const string _pathL = "/Assets/Characters/droid.png";
        const int _shotDmg = 40;
        const double _movementSpeed = 1;
        const double _shootingHorizontalOffset = 80;
        const double _shootingVerticalOffset = 25;
        const double _visionRange = 500;
        const double _enemyWidth = 100;
        const double _enemyHeight = 150;


        public Droid(Point startingPosition, double maxRightMovement, double maxLeftMovement)
            : base(_pathR, _pathL, startingPosition, maxLeftMovement, maxRightMovement, _movementSpeed, _enemyWidth, _enemyHeight, _shootingHorizontalOffset, _shootingVerticalOffset, _visionRange,_shotDmg)
        {

        }
    }
}
