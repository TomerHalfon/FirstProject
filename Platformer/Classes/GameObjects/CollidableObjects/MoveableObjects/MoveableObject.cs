using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
   public abstract class MoveableObject : CollidableObj
    {
        public const int MAX_HEALTH = 100;
        public const double MOVEMENT_SPEED = 10;
        public const double _flightSpeed = 10;

        protected List<Projectile> Projectiles;
        protected int _health;
        protected bool _isAlive;
        protected bool _isLookingRight;
        protected string _leftImagePath, _rightImagePath; //will be initialize in the dirived class
        public bool IsAlive { get { return _isAlive; } }
        public List<Projectile> GetShots() { return Projectiles; }

        public MoveableObject()
        {
            _isLookingRight = true;
            _health = MAX_HEALTH;
            _isAlive = true;
            Projectiles = new List<Projectile>();
        }

        #region Horizontal Movement Control
        //right side movement
        public virtual void MoveRight(Canvas gameArea) 
        {
            _isLookingRight = true;

            if (GetPositionInCanvas().Left + Image.ActualWidth <= gameArea.ActualWidth)// check if still in canvas
            {
                SetPositionInCanvas(GetPositionInCanvas().Left + MOVEMENT_SPEED, GetPositionInCanvas().Top);
            }
        }
        public virtual double GetMoveRight()
        {
            return GetPositionInCanvas().Left + MOVEMENT_SPEED;
        }

        //left side movement
        public virtual void MoveLeft()
        {
            _isLookingRight = false;

            Point newPosition = new Point(GetPositionInCanvas().Top, GetPositionInCanvas().Left - MOVEMENT_SPEED);

            if (newPosition.Left >= 0) // check if still in canvas
            {
                SetPositionInCanvas(newPosition);
            }
        }
        public virtual double GetMoveLeft()
        {
            return GetPositionInCanvas().Left - MOVEMENT_SPEED;
        } 
        #endregion
        #region Vertical Movement Control
        //down movement
        public virtual void MoveDown(double force)
        {           
                SetPositionInCanvas(GetPositionInCanvas().Left, GetPositionInCanvas().Top + force);
        }

        //up movement
        public virtual void MoveUp()
        {
            if (GetPositionInCanvas().Top > 0)
            {
                SetPositionInCanvas(GetPositionInCanvas().Left, GetPositionInCanvas().Top - _flightSpeed);
            }
        }
        public virtual double GetMoveUp()
        {
            return GetPositionInCanvas().Top - _flightSpeed;
        }
        #endregion

        #region Dmg
        public virtual void Kill()
        {
            _isAlive = false;
            Image.Visibility = Visibility.Collapsed;
        }
        public virtual void Hit(int dmg)
        {
            _health -= dmg;
            //check if should be dead
            if (_health <= 0)
            {
                Kill();
            }
        } 
        #endregion
        #region health
        public int GetHealth()
        {
            return _health;
        }
        public void UpdateHealth(int amount)
        {
            if (_health + amount <= MAX_HEALTH)
                _health += amount;
            else
            {
                _health = MAX_HEALTH;
            }

            if (_health <= 0)
            {
                Kill();
            }
        }
        protected void ResetIsAlive() { _isAlive = true; }
        #endregion

    }
}
