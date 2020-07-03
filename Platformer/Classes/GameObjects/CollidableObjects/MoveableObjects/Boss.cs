using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using Platformer.Classes.GameObjects.FixedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
    public class Boss : MoveableObject
    {
        public static int ShotDmg = 5;
        public static int BolderDamage = 20;

        public const int BossKillScore = 1000;
        public const int Max_Health =  1000;

        const string _pathR = "/Assets/Characters/BossRight.png";
        const string _pathL = "/Assets/Characters/BossLeft.png";
        const double _width = 480;
        const double _height = 550;
        const double _grabingRange = 100;
        const int _shoveDmg = 2;
        const int _shotInterval = 60;
        double _movementSpeed = 1;
        int _shotCounter;
        bool _walkingDiraction;
        Point _startingPosistion;
        Random _rand;
        public Boss(Point startingPosition)
        {
            _shotCounter = 0;
            _isLookingRight = false;
            _walkingDiraction = false;
            SetImageSource(_pathL);
            Image.Width = _width;
            Image.Height = _height;
            Image.Stretch = Stretch.Fill;
            _health = Max_Health;
            _startingPosistion = startingPosition;
        }
        public Point GetStartingPosition() { return _startingPosistion; }

        public override void Hit(int dmg)
        {
            base.Hit(dmg);
            _movementSpeed += 0.1;
        }
        //public override void Kill()
        //{
        //    base.Kill();
        //    Image.Visibility = Visibility.Visible;
        //    Image.Width = 100;
        //    Image.Width = 150;
        //    SetImageSource("/Assets/Characters/Playable/Baby_Yoda.png");

        //}

        public void BossLogic(Canvas canvas, Player player,Map map)
        {
            if (!IsPlayerInShovingRange(player))
            {
                if (++_shotCounter%_shotInterval ==0)
                {
                   ReturnFire(canvas, map);

                    if (!Projectiles.OfType<Bolder>().Any())
                    {
                        Point fuelPoint = GetPositionInCanvas();
                        if (_walkingDiraction)
                        {
                            fuelPoint.Left = canvas.Width -50;
                            fuelPoint.Top += 500;
                        }
                        else
                        {
                            fuelPoint.Left = 0;
                            fuelPoint.Top += 500;
                        }

                        //create a fuel tank only one at a time
                        if (!map.GetFixedObjects().OfType<FuelTank>().Any())
                        {
                            map.CreateAndAddFixedObjectToMap(new FuelTank(), fuelPoint.Left, fuelPoint.Top);
                        }

                        //roll bolder
                        RollBolder(canvas, map);
                        
                    }
                }
                if (!_walkingDiraction)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight(canvas);
                }
            }
            else
            {
                ShoveAndDmgPlayer(canvas, player);
            }
        }
        public override void MoveLeft()
        {
            if (_isLookingRight)
            {
                _isLookingRight = false;
                SetImageSource(_pathL);
            }

            Point newPosition = new Point(GetPositionInCanvas().Top, GetPositionInCanvas().Left - _movementSpeed);

            if (newPosition.Left >= 0) // check if still in canvas
            {
                SetPositionInCanvas(newPosition);
            }
            else
            {
                _walkingDiraction = true;
                _movementSpeed = 1;
            }
        }
        public override void MoveRight(Canvas gameArea)
        {
            if (!_isLookingRight)
            {
                _isLookingRight = true;
                SetImageSource(_pathR);
            }

            if (GetPositionInCanvas().Left + Image.ActualWidth <= gameArea.ActualWidth)// check if still in canvas
            {
                SetPositionInCanvas(GetPositionInCanvas().Left + _movementSpeed, GetPositionInCanvas().Top);
            }
            else
            {
                _movementSpeed = 1;
                _walkingDiraction = false;

            }
        }

        private bool IsPlayerInShovingRange(Player player)
        {
            if (_isLookingRight)
            {
                if (player.GetPositionInCanvas().Left < GetPositionInCanvas().Left + Image.Width + _grabingRange &&
                    player.GetPositionInCanvas().Left > GetPositionInCanvas().Left &&
                    player.GetPositionInCanvas().Top > GetPositionInCanvas().Top)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (player.GetPositionInCanvas().Left > GetPositionInCanvas().Left - _grabingRange &&
                    player.GetPositionInCanvas().Left < GetPositionInCanvas().Left &&
                    player.GetPositionInCanvas().Top > GetPositionInCanvas().Top)
                {
                    return true;
                }
                return false;
            }

        }
        private void ShoveAndDmgPlayer(Canvas canvas, Player player)
        {
            _rand = new Random();
            Point playerPosition = player.GetPositionInCanvas();
            Point moveTo;
            if (!_isLookingRight)
            {
                moveTo = new Point(playerPosition.Top + _rand.Next(-200, 0), playerPosition.Left + _rand.Next(-100, 0));

                if (moveTo.Left < 0)
                {
                    moveTo.Left = 0;
                }
            }
            else
            {
                moveTo = new Point(playerPosition.Top + _rand.Next(-200, 0), playerPosition.Left + _rand.Next(0, 100));
                if (moveTo.Left > canvas.ActualWidth)
                {
                    moveTo.Left = canvas.ActualWidth;
                }
            }
            player.SetPositionInCanvas(moveTo);
            player.Hit(_shoveDmg);
        }
        private void ReturnFire(Canvas canvas, Map map)
        {
            Point shootingPosition = GetPositionInCanvas();

                Shot shot1 = new Shot(new Point(shootingPosition.Top + 100, shootingPosition.Left), canvas, map, _isLookingRight, this);
                Shot shot2 = new Shot(new Point(shootingPosition.Top + 120, shootingPosition.Left), canvas, map, _isLookingRight, this);
                Shot shot3 = new Shot(new Point(shootingPosition.Top + 140, shootingPosition.Left), canvas, map, _isLookingRight, this);
                Shot shot4 = new Shot(new Point(shootingPosition.Top + 160, shootingPosition.Left), canvas, map, _isLookingRight, this);
                Shot shot5 = new Shot(new Point(shootingPosition.Top + 180, shootingPosition.Left), canvas, map, _isLookingRight, this);
                Projectiles.Add(shot1);
                Projectiles.Add(shot2);
                Projectiles.Add(shot3);
                Projectiles.Add(shot4);
                Projectiles.Add(shot5);
        }
        private void RollBolder(Canvas canvas, Map map)
        {
            Point rollingPosition = GetPositionInCanvas();
            rollingPosition.Left += 200;
            rollingPosition.Top += 350;

            Bolder bolder = new Bolder(rollingPosition, canvas, map, _isLookingRight, this);
            Projectiles.Add(bolder);
        }
    }
}
        
