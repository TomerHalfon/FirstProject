using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Platformer.Classes.GameObjects.CollidableObjects.Projectiles
{
   public abstract class Projectile : CollidableObj
    {
        string _imageLocation;
        double _width;
        double _movementSpeed;
        double _height;

        DispatcherTimer _timer;
        MoveableObject _shooter;
        readonly Canvas _game;
        readonly Map _map;
        Point _startingPosition;
        bool _isMovingRight;

        public Projectile(Canvas canvas, Map map,MoveableObject shotBy, Point startingPosition, string imageLocation,double width, double height, double speed, bool isMovingRight)
        {
            //fill data memebers
            _imageLocation = imageLocation;
            _width = width;
            _height = height;
            _movementSpeed = speed;
            _game = canvas;
            _map = map;
            _isMovingRight = isMovingRight;
            _startingPosition = startingPosition;
            _shooter = shotBy;
            //define the projectile image
            InitializeProjectiletImage();

            //position in canvas
            SetPositionInCanvas(_startingPosition.Left, _startingPosition.Top);
            Canvas.SetLeft(Image, GetPositionInCanvas().Left);
            Canvas.SetTop(Image, GetPositionInCanvas().Top);

            //initialize Timer
            InitializeTimer();

        }
        private void InitializeProjectiletImage()
        {
            SetImageSource(_imageLocation);
            Image.Width = _width;
            Image.Height = _height;
            Image.Stretch = Stretch.Fill;
            _game.Children.Add(Image);
        }
        #region Timer Controls
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 10)
            };
            if (_isMovingRight)
            {
                _timer.Tick += AnimationRight;
            }
            if (!_isMovingRight)
            {
                _timer.Tick += AnimationLeft;
            }
            _timer.Start();
        }
        #endregion
        #region Animations
        //Left
        private void AnimationLeft(object sender, object e)
        {
            if (Canvas.GetLeft(Image) + Image.ActualWidth > 0 && !this.IsCollidingWithMap(_map.GetBlocks(),
                new Point(Canvas.GetTop(Image), Canvas.GetLeft(Image))))
            {
                Canvas.SetLeft(Image, Canvas.GetLeft(Image) - _movementSpeed);
            }
            else //out of bounds
            {
                Remove(_shooter.GetShots());
            }
        }

        //right
        private void AnimationRight(object sender, object e)
        {
            if (Canvas.GetLeft(Image) < _game.ActualWidth &&
                !this.IsCollidingWithMap(_map.GetBlocks(), new Point(Canvas.GetTop(Image), Canvas.GetLeft(Image))))
            {
                Canvas.SetLeft(Image, Canvas.GetLeft(Image) + _movementSpeed);
            }
            else //out of bounds
            {
                Remove(_shooter.GetShots());
            }
        }
        #endregion
        public void Remove(List<Projectile> projectiles)
        {
            Image.Visibility = Visibility.Collapsed;
            _game.Children.Remove(Image);
            _timer.Stop();
            projectiles.Remove(this);
        }
    }
}
