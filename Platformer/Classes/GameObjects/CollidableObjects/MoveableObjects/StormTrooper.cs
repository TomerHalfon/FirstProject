using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
    class StormTrooper : Enemy
    {
        const int _shotDmg = 30;  
        const string _pathR = "/Assets/Characters/stormTrooperRight.png";
        const string _pathL = "/Assets/Characters/stormTrooper.png";
        const double _movementSpeed = 1.5;
        const double _shootingHorizontalOffset = 80;
        const double _shootingVerticalOffset = 25;
        const double _visionRange = 300;
        const double _enemyWidth = 100;
        const double _enemyHeight = 150;


        public StormTrooper(Point startingPosition, double maxRightMovement, double maxLeftMovement) 
            :base(_pathR,_pathL,startingPosition,maxLeftMovement,maxRightMovement,_movementSpeed,_enemyWidth,_enemyHeight, _shootingHorizontalOffset, _shootingVerticalOffset,_visionRange, _shotDmg)
        {

        }
    }
}
