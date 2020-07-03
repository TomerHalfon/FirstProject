using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects
{
    public class Player : MoveableObject
    {
        public static SettingsProfile Settings;

        public const int MAX_FUEL_AMOUNT = 100;
        public const int ShotDmg = 40;
        public const int MAX_FUEL = 100;

        const int _startAmmo = 30;
        const int _startFuel = 100;
        const int _fuelConsumption = 1;
        const double _playerWidth = 100;
        const double _playerHeight = 150;
        const double _shootingPositionHotizontalOffset = 80;
        const double _shootingPositionVerticalOffset = 25;
        const int _deathScorePenalty = 50;

        public int currentLvl;
        public bool isJumping;
        public bool tutorial;
        public bool _hasUnlockedBabyYoda;

        private bool _isBabyYoda;
        private int _fuel;
        private int _ammo;
        private int _score;
        private readonly string _name;
        public string Name { get { return _name; } }

        public Player(string name, string rightImagePath,string leftImagePath, bool tutorial, bool isBabyYoda,SettingsProfile settings)
        {
            //set the data members from parameters
            _name = name;
            _rightImagePath = rightImagePath;
            _leftImagePath = leftImagePath;
            this.tutorial = tutorial;
            _isBabyYoda = isBabyYoda;
            
            //set data members from constants
            _score = 0;
            _ammo = _startAmmo;
            _fuel = _startFuel;
            isJumping = false;
            Settings = settings;
            _hasUnlockedBabyYoda = false;

            //define and set the image (initialized in the base ctor)
            SetImageSource(_rightImagePath);
            Image.Width = _playerWidth;
            Image.Height = _playerHeight;
        }

        //used in starting new level with out creating a new player
        public void ResetPlayerGame()
        {
            ResetIsAlive();
            ResetAmmo();
            ResetFuel();
            ResetHealth();
        }

        #region Movement

        //Left
        public override void MoveLeft()
        {
            if (!_isBabyYoda)
            {
                SetImageSource(_leftImagePath);
            }
            base.MoveLeft();
        }

        //Right
        public override void MoveRight(Canvas gameArea)
        {
            if (!_isLookingRight && !_isBabyYoda)
            {
                SetImageSource(_rightImagePath);
            }
            base.MoveRight(gameArea);
        }
        #endregion
        #region Fuel
        //get current fuel amount
        public int GetFuel()
        {
            return _fuel;
        }

        //Update fuel amounts with logic
        public void UpdateFuel(int amount)
        {
            if (_fuel + amount <= MAX_FUEL_AMOUNT)
            {
                _fuel += amount;
            }
            else
            {
                _fuel = MAX_FUEL_AMOUNT;
            }
        } 
        public void ConsumeFuel()
        {
            UpdateFuel(-_fuelConsumption);
        }

        //reset fuel amount (for starting a new lvl)
        private void ResetFuel()
        {
            _fuel = _startFuel;
        }
        #endregion
        #region Ammo

        //Reset ammo amount (for starting new lvl)
        private void ResetAmmo() { _ammo = _startAmmo; }

        //Get the current ammo amount
        public int GetAmmo()
        {
            return _ammo;
        }
        #endregion
        #region Health
        public override void Kill()
        {
            ResetScore();
            base.Kill();
        }
        //reset helth (for starting a new lvl)
        private void ResetHealth() { _health = MAX_HEALTH; }
        #endregion
        #region Starting Position
        //Move player to starting position based on map
        public void UpdateStartingPosition(Map map)
        {
            SetPositionInCanvas(map.GetPlayerStartingPosition());
        } 
        #endregion
        #region Shooting
        
        //Shooting action
        public void Shoot(Canvas canvas, Map map)
        {
            Point RightShootingPosition = new Point(GetPositionInCanvas().Top + _shootingPositionVerticalOffset,
                                                GetPositionInCanvas().Left + _shootingPositionHotizontalOffset);
            Point lefttShootingPosition = new Point(GetPositionInCanvas().Top + _shootingPositionVerticalOffset,
                                               GetPositionInCanvas().Left - _shootingPositionHotizontalOffset / 2);
            if (_isLookingRight) //right shot
            {
                Shot shot = new Shot(RightShootingPosition, canvas, map, true, this);
                Projectiles.Add(shot);
            }
            if (!_isLookingRight) //left shot
            {
                Shot shot = new Shot(lefttShootingPosition, canvas, map, false, this); ;
                Projectiles.Add(shot);
            }
            if (map.GetBoss() == null)
            {
                _ammo--;
            }
        }
      
        //Handle what to do when killing an enemy (player POV)
        public void KillEnemy(Map map)
        {
            AddToScore(map.GetEnemisKillScore());
        }
        public void KillBoss()
        {
            AddToScore(Boss.BossKillScore);
            Unlock();
            currentLvl++;
        }
        #endregion
        #region Score

        //Add to score
        private void AddToScore(int amount)
        {
            _score += amount;
        }

        //returns the value of player's score
        public int GetScore()
        {
            return _score;
        }

        private void ResetScore()
        {
            _score -= _deathScorePenalty;
            if (_score <0)
            {
                _score = 0;
            }
        }

        //unlock babby yoda
        private void Unlock()
        {
            _hasUnlockedBabyYoda = true;
        }
        #endregion
    }
}
