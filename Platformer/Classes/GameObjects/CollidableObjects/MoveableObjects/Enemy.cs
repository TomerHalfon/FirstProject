using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
   public abstract class Enemy : MoveableObject
    {
        const int _shootingInterval = 20;
        
        public int shotDmg;
        
        Point _startingPosition;
        Point _lastPosition;
        int _preferdPatrolingDirection;
        double _maxRightPositionInCanvas;
        double _maxLeftPositionInCanvas;
        double _shoot;
        double _movementSpeed;
        double _shootingHorizontalOffset;
        double _shootingVerticalOffset;
        double _visionRange;
        bool _isMovingRight;

        public Enemy(string rightImagePath, string leftImagePath, Point startingPosition, double maxRightMovement, double maxLeftMovement,double movementSpeed,double enemyWidth,double enemyHeight, double shootingHorizontalOffset,double shootingVerticalOffset,double visionRange, int shotDmg)
        {
            //set data members from parameters
            _startingPosition = startingPosition;
            _movementSpeed = movementSpeed;
            _maxLeftPositionInCanvas = _startingPosition.Left - maxLeftMovement;
            _maxRightPositionInCanvas = _startingPosition.Left + maxRightMovement;
            _lastPosition = _startingPosition; //initialize with starting position
            _rightImagePath = rightImagePath;
            _leftImagePath = leftImagePath;
            _shootingHorizontalOffset = shootingHorizontalOffset;
            _shootingVerticalOffset = shootingVerticalOffset;
            _visionRange = visionRange;
            this.shotDmg = shotDmg;
            _shoot = 0;

            //Randomize preferd patroling direction (0 is right,1 is left)
            Random random = new Random();
            _preferdPatrolingDirection = random.Next(2);

            //public from base class
            if (_preferdPatrolingDirection ==0)
            {
                _isMovingRight = true;
                _isLookingRight = true;

            }
            else
            {
                _isMovingRight = true;
                _isLookingRight = false;

            }

            //define and set the image (initialized in the base ctor)
            SetImageSource(rightImagePath);
            Image.Width = enemyWidth;
            Image.Height = enemyHeight;
        }
        
        #region Starting Positon
        public Point GetStartingPosition() { return _startingPosition; } 
        #endregion

        #region "AI" not really, but the logic of the enemy's decision making
        public void Patrol(Canvas canvas, Map map, Player player)
        {
            //update last position left value 
            _lastPosition.Left = GetPositionInCanvas().Left;

            // only patrol when player is not visible
            if (!IsPlayerVisible(player)) 
            {
                //move right if random is 0 and if enemy isn't stuck or when left is stuck
                if (_isMovingRight&&!IsCollidingWithMap(map.GetBlocks(), new Point(GetPositionInCanvas().Top, GetMoveRight())) &&
                    _lastPosition.Left + Image.ActualWidth < canvas.ActualWidth)
                {
                    //move
                    MoveRight(canvas);
                }
                //move left if random is 1 and if the enemy isn't stuck or when right is stuck
               else if (!IsCollidingWithMap(map.GetBlocks(), new Point(GetPositionInCanvas().Top, GetMoveLeft())) &&
                    _lastPosition.Left > 0)
                {
                    //move
                    MoveLeft();

                }
                //reset shoot counter
                _shoot = 0;
            }

            //if the player is visible to the enemy, and shoot is divideable with the interval const
            if (IsPlayerVisible(player) && _shoot % _shootingInterval == 0) 
            {
                //shoot
                Shoot(canvas, map);
            }

            //update shoot counter
            _shoot++;
        }

        private bool IsPlayerVisible(Player player)
        {
            //make a local var with the location of the player
            Point playerLocation = player.GetPositionInCanvas();
            Point enemyLocation = GetPositionInCanvas();

            //look for player in the enemy vision range and diraction of view
            //right
            if (_isLookingRight)
            {
                if (enemyLocation.Left + _visionRange >=playerLocation.Left && 
                    enemyLocation.Left <= playerLocation.Left &&
                    ((enemyLocation.Top>=playerLocation.Top&&
                    enemyLocation.Top <= playerLocation.Top+player.Image.ActualHeight)||
                    (enemyLocation.Top >=playerLocation.Top &&
                    enemyLocation.Top <= playerLocation.Top+player.Image.ActualHeight)))
                {
                    return true;      
                }
            }
            //left
            else
            {
                if (enemyLocation.Left -  _visionRange <= playerLocation.Left &&
                    enemyLocation.Left >= playerLocation.Left &&   
                    ((enemyLocation.Top >= playerLocation.Top &&
                    enemyLocation.Top <= playerLocation.Top + player.Image.ActualHeight) ||
                    (enemyLocation.Top >= playerLocation.Top &&
                    enemyLocation.Top <= playerLocation.Top + player.Image.ActualHeight)))
                {
                    return true;
                }
            }
            //if cant see anything return false
            return false;
        }
        #endregion

        #region Movement Controls
        public override void MoveLeft()
        {
            //set image for direction
            if (_isLookingRight)
            {
                SetImageSource(_leftImagePath);
                _isLookingRight = false;
            }
            //move image
            AddToLeftPosition(-_movementSpeed);

            if (_lastPosition.Left <= _maxLeftPositionInCanvas)
            {
                _isMovingRight = true;
            }
        }
        public override void MoveRight(Canvas gameArea)
        {
            //set image for direction
            if (!_isLookingRight)
            {
                SetImageSource(_rightImagePath);
                _isLookingRight = true;
            }
            //move image
            AddToLeftPosition(_movementSpeed);
            if (_lastPosition.Left >= _maxRightPositionInCanvas)
            {
                _isMovingRight = false;
            }
        }
        public void SetMaxMovements(double maxLeft, double maxRight)
        {
            _maxLeftPositionInCanvas = _startingPosition.Left - maxLeft;
            _maxRightPositionInCanvas = _startingPosition.Left + maxRight;
        }
        private void Shoot(Canvas canvas, Map map)
        {
            Point RightShootingPosition = new Point(GetPositionInCanvas().Top + _shootingVerticalOffset,
                                    GetPositionInCanvas().Left + _shootingHorizontalOffset);
            Point lefttShootingPosition = new Point(GetPositionInCanvas().Top + _shootingVerticalOffset,
                                               GetPositionInCanvas().Left - _shootingHorizontalOffset / 2);

            if (_isLookingRight)
            {
                Shot shot = new Shot(RightShootingPosition, canvas, map, true, this);
                Projectiles.Add(shot);
            }
            else
            {
                Shot shot = new Shot(lefttShootingPosition, canvas, map, false, this);
                Projectiles.Add(shot);
            }
        }
        #endregion
    }
}
