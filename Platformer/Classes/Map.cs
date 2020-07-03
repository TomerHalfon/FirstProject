using Platformer.Classes.GameObjects;
using Platformer.Classes.GameObjects.Blocks;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using Platformer.Classes.GameObjects.FixedObjects;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes
{
    public class Map
    {
        public bool IsComplete { get { return _isCompleate; } set { CompleteLvl(); } }
        public bool IsUserGenerated { get; set; }
        public ImageBrush BackgroundImageBrush { get; set; }

        private List<Block> _blocks;
        private List<FixedObject> _fixedObjects;
        private List<Enemy> _enemies;

        private Boss _boss;
        private Point _playerStartingLocation;
        private Canvas _gameArea;

        private int _enemiesKillScore;
        private bool _isCompleate;

        public Map(Canvas gameArea,string backgroudImageSource , Point playerStartingLocation,int enmiesKillScore,bool isUserGenerated)
        {
            _playerStartingLocation = playerStartingLocation;
            _gameArea = gameArea;
            IsUserGenerated = isUserGenerated;
            BackgroundImageBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri($"ms-appx://{backgroudImageSource}")),
                Stretch = Stretch.Fill
            };
            gameArea.Background = BackgroundImageBrush;

            _blocks = new List<Block>();
            _fixedObjects = new List<FixedObject>();
            _enemies = new List<Enemy>();

            _enemiesKillScore = enmiesKillScore;
            _isCompleate = false;
        }

        //clone a map
        public Map(Map existingMap, Canvas canvas)
        {
            _playerStartingLocation = existingMap._playerStartingLocation;
            _gameArea = canvas;
            IsUserGenerated = existingMap.IsUserGenerated;
            BackgroundImageBrush = existingMap.BackgroundImageBrush;
            _gameArea.Background = BackgroundImageBrush;

            _blocks = existingMap._blocks;
            _fixedObjects = existingMap._fixedObjects;
            _enemies = existingMap._enemies;

            _enemiesKillScore = existingMap._enemiesKillScore;
            _isCompleate = false;
        }

        //adding platforms
        public void CreateAndAddBlockToMap(Block block)
        {
            _gameArea.Children.Add(block.Image);
            _blocks.Add(block);
        }

        //adding fixed objects
        public void CreateAndAddFixedObjectToMap(FixedObject fixedObject, double left, double top)
        {
            Canvas.SetLeft(fixedObject.Image, left);
            Canvas.SetTop(fixedObject.Image, top);
            _gameArea.Children.Add(fixedObject.Image);
            _fixedObjects.Add(fixedObject);
        }

        //adding enemies
        public void CreateAndAddEnemyToMap(Enemy enemy)
        {
            _gameArea.Children.Add(enemy.Image);
            Canvas.SetLeft(enemy.Image, enemy.GetStartingPosition().Left);
            Canvas.SetTop(enemy.Image, enemy.GetStartingPosition().Top);
            _enemies.Add(enemy);
        }

        //adding boss
        public void CreateAndAddBossToMap(Boss boss)
        {
            Canvas.SetLeft(boss.Image, boss.GetStartingPosition().Left);
            Canvas.SetTop(boss.Image, boss.GetStartingPosition().Top);
            _gameArea.Children.Add(boss.Image);
            _boss = boss;
        }


        public List<Block> GetBlocks() { return _blocks; }
        public List<FixedObject> GetFixedObjects() { return _fixedObjects; }
        public List<Enemy> GetEnemies(){ return _enemies; }
        public Boss GetBoss(){ return _boss; }
        public Point GetPlayerStartingPosition() { return _playerStartingLocation; }

        public int GetEnemisKillScore()
        {
            return _enemiesKillScore;
        }
        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }
        public void SetPlayerStartingPosition(Point startingPosition) { _playerStartingLocation = startingPosition; }
        public void CompleteLvl()
        {
            //cleat the canvas
            Clear();

            //update status
            _isCompleate = true;
        }
        public void UpdateBackground(string source)
        {
            BackgroundImageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri($"ms-appx://{source}")),
            };

            _gameArea.Background = BackgroundImageBrush;
        }
        public void Clear()
        {
            //deal with blocks
            foreach (Block block in _blocks)
            {
                _gameArea.Children.Remove(block.Image);
            }
            //deal with enemies
            foreach (Enemy enemy in _enemies)
            {
                _gameArea.Children.Remove(enemy.Image);
            }

            //deal with fixed objects
            foreach (FixedObject fixedObject in _fixedObjects)
            {
                _gameArea.Children.Remove(fixedObject.Image);
            }

            if (_boss != null)
            {
                _gameArea.Children.Remove(_boss.Image);
                _boss = null;
            }
        }
    }
}
