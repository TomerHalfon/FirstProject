using Platformer.Classes;
using Platformer.Classes.GameObjects.Blocks;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using Platformer.Classes.GameObjects.FixedObjects;
using Platformer.Game;
using Platformer.MenuPages;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Platformer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        #region toturial
        //tutorial Consts
        const string _startingMsg = "Hello and welcome to the game!\n" +
            "Here you will learn the controls of the game\n" +
            " and the basic mechanics of it\n" +
            "press enter to continue...";
        const string _rightKeyMsg = "To move to the right use the right arrow key\n" +
            "move right to continue...";
        const string _leftKeyMsg = "To move to the left use the left arrow key\n" +
            "move left to continue...";
        const string _upKeyMsg = "In this game you have a jet pack, that means that you can fly!\n" +
            "But be carefull of flaying for too long\n" +
            "at the bottom of the screen ther's a blue bar, this is your fuel bar\n" +
            "Remember to monitor your fuel levels!\n" +
            "if you fail to do so you might lose the game...\n" +
            "press the up arrow and fly to the next platform to continue...";
        const string _fuelTankMsg = "This is a fuel tank it gives you some fuel!\n" +
            "it wont allways fill your tank completely but at least it's something\n" +
            "Walk towards it in order collect it\n" +
            "Collect the fuel tank to continue...";
        const string _enemyMsg = "That's an enemy and it's guarding a Helth Boost!\n" +
            "Well... it is a Stormtrooper after all so you will be just fine as long as you stay quick and smarter than him\n" +
            "kill the Stormtrooper to continue...";
        const string _healthBoostMsg = "That was great!\n" +
            "quickly get to the ship Baby Yoda awaits...";
        #endregion

        Map _map;

        //objects
        Canvas _game;
        Player _player;
        Point _newPosition;
        PlayerScoreKeeper _scoreKeeper;
        DispatcherTimer _timer;
        TextBlock _tutorialTextBlock;
        Button _retryBtn, _mainMenuBtn;
        ProgressBar _bossHealthBar;
        TextBlock _bossName;
        Point _tutorialTextBlockRightPosition, _tutorialTextBlockLeftPosition;
        Grid _pauseMenuGrid;
        SettingsProfile _settings;

        //collections
        List<string> _keys;

        //flags
        bool _jump;
        bool _shoot;
        bool _isPauseMenuOpen;
        bool _upKeyTutorialFlag, _rightKeyTutorialFlag, _leftKeTutorialFlag, _enterKeyTutorialFlag, _fuelTutorialFlag;
        //gravity
        double _forceOnPlayer;
        double _forceOnEnemies;
        double _forceOnBoss;

        public GamePage()
        {
            this.InitializeComponent();

            //initialize keys
            Window.Current.CoreWindow.KeyDown += User_KeyDown;
            Window.Current.CoreWindow.KeyUp += User_KeyUp;
            _keys = new List<string>();

            //set game flags as false
            _jump = false;
            _shoot = false;
            _isPauseMenuOpen = false;

            //tutorial flags
            _upKeyTutorialFlag = false;
            _leftKeTutorialFlag = false;
            _rightKeyTutorialFlag = false;
            _enterKeyTutorialFlag = false;
            _fuelTutorialFlag = false;

            //set canvas
            _game = Lvl0Canvas;

            //Init Settings
            _settings = Settings.SettingsProfile;

            //Init Game
            InitializeGame();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                if (_player.tutorial)
                {
                    InitializeTutorialMap();
                }
                else
                    InitializeLvl1();
            }
            else
            {
                _map = new Map((Map)e.Parameter, _game);

                foreach (var item in _map.GetBlocks())
                {
                    _game.Children.Add(item.Image);
                }
                foreach (var item in _map.GetFixedObjects())
                {
                    _game.Children.Add(item.Image);
                }
                foreach (var item in _map.GetEnemies())
                {
                    _game.Children.Add(item.Image);
                }
                _game.Background = _map.BackgroundImageBrush;
                _player.currentLvl = -2;
                _player.tutorial = false;
                _player.SetPositionInCanvas(_map.GetPlayerStartingPosition().Left, _map.GetPlayerStartingPosition().Top);
            }
            base.OnNavigatedTo(e);
        }

        #region Initilaizers
        private void InitializeGame()
        {
            InitializeGameClock();
            InitializeGravity();
            InitializePlayer();
            InitializeVolumeLevels();
        }

        private void InitializeVolumeLevels()
        {
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;
            jetPackEmptyFx.Volume = _settings.FxVolume;
            jetPackFx.Volume = _settings.FxVolume;
            emptyShotFx.Volume = _settings.FxVolume;
            shotFx.Volume = _settings.FxVolume;
            hitFx.Volume = _settings.FxVolume;
            playerHit.Volume = _settings.FxVolume;
            enemyDeath.Volume = _settings.FxVolume;
            pickUpFx.Volume = _settings.FxVolume;
            gameOverFx.Volume = _settings.FxVolume;
        }

        private void InitializeGravity()
        {
            _forceOnEnemies = 1;
            _forceOnPlayer = 1;
            _forceOnBoss = 1;
        }
        private void InitializeGameClock()
        {
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 15)
            };
            _timer.Tick += GameLoop;
            _timer.Start();
        }

        #region Initialize Player
        private void InitializePlayer()
        {
            // get the created Charecter
            _player = CharacterSelection._player;

            //add to canvas
            if (!_game.Children.Contains(_player.Image))
                _game.Children.Add(_player.Image);

            //initialize ScoreKeeper
            _scoreKeeper = new PlayerScoreKeeper(_player.Name, _player.GetScore());
            //initialize the visual representations
            InitializePlayerVisualTools();
        }
        #region Tools
        //initialize tools
        private void InitializePlayerVisualTools()
        {
            InitializeFuelBar();
            InitializeHealthBar();
            InitializeAmmo();
            InitializeScore();
        }
        private void InitializeFuelBar()
        {
            FuelBar.Value = _player.GetFuel();
        }
        private void InitializeHealthBar()
        {
            HealthBar.Value = _player.GetHealth();
        }
        private void InitializeAmmo()
        {
            ActualAmmoDisplayBox.Text = $"{_player.GetAmmo()}";
        }
        private void InitializeScore()
        {
            ScoreDisplayTextBlock.Text = $"{_player.GetScore()}";
        }
        #endregion
        #endregion
        #region Maps
        private void InitializeTutorialMap()
        {
            //Initialize Background Music
            backgroundMusicMediaElement.Source = new Uri("ms-appx:///Assets/Music/Lvl1Theme.mp3");
            //set volume
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;

            //initialize tutorial text block
            _tutorialTextBlock = new TextBlock
            {
                FontSize = 20
            };
            _game.Children.Add(_tutorialTextBlock);

            //set current lvl
            _player.currentLvl = 0;

            //define player starting location
            Point startlocation = new Point(0, 0);

            //initialize the map;
            _map = new Map(_game, "/Assets/Backgrounds/Lvl0_back.jpg", startlocation, 50, false);

            //update player starting position
            _player.UpdateStartingPosition(_map);

            //reset player game
            _player.ResetPlayerGame();
            InitializePlayerVisualTools();

            //adding "blocks" to map
            _map.CreateAndAddBlockToMap(new SandBlock(300, 50, new Point(_game.Height - 50, 0)));
            _map.CreateAndAddBlockToMap(new SandBlock(400, 50, new Point(300, 0)));
            _map.CreateAndAddBlockToMap(new SandBlock(400, 50, new Point(300, 600)));
            _map.CreateAndAddBlockToMap(new SandBlock(500, 50, new Point(550, _game.Width - 500)));

            //adding fixed objects to map
            _map.CreateAndAddFixedObjectToMap(new HealthBoost(), _game.Width - 70, 500);
            _map.CreateAndAddFixedObjectToMap(new FuelTank(), _game.Width / 2, 250);
            _map.CreateAndAddFixedObjectToMap(new Ship(), 100, 700);

            //adding enemies to map
            _map.CreateAndAddEnemyToMap(new StormTrooper(new Point(300, _game.Width - 250), 50, 50));
        }
        private void InitializeLvl1()
        {
            //Initialize Background Music
            backgroundMusicMediaElement.Source = new Uri("ms-appx:///Assets/Music/Lvl2Theme.mp3");
            //set volume
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;

            _player.currentLvl = 1;
            _player.tutorial = false;
            _player.ResetPlayerGame();
            _map = new Map(_game, "/Assets/Backgrounds/CityBack.jpg", new Point(_game.Height - 200, 100), 100, false);
            _player.UpdateStartingPosition(_map);
            InitializePlayerVisualTools();

            //add blocks
            //player starting block
            _map.CreateAndAddBlockToMap(new MetalBlock(200, 20, new Point(_game.Height - 40, 0)));

            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(_game.Height - 90, 500)));
            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(_game.Height - 90, 1500)));
            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(300, _game.Width - 100)));
            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(300, _game.Width - 700)));
            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(300, 500)));
            _map.CreateAndAddBlockToMap(new MetalBlock(150, 20, new Point(200, 250)));
            _map.CreateAndAddBlockToMap(new MetalBlock(100, 20, new Point(200, _game.Width - 500)));

            //ship platform
            _map.CreateAndAddBlockToMap(new MetalBlock(200, 20, new Point(200, 0)));

            //enemy platform
            _map.CreateAndAddBlockToMap(new MetalBlock(300, 20, new Point(_game.Height - 200, 700)));
            _map.CreateAndAddBlockToMap(new MetalBlock(300, 20, new Point(400, 700)));

            //enemies
            _map.CreateAndAddEnemyToMap(new StormTrooper(new Point(_game.Height - 352, 800), 100, 100));
            _map.CreateAndAddEnemyToMap(new StormTrooper(new Point(240, 800), 5, 100));

            //fixed objects
            _map.CreateAndAddFixedObjectToMap(new FuelTank(), 1520, _game.Height - 140);
            _map.CreateAndAddFixedObjectToMap(new FuelTank(), _game.Width - 450, 150);
            _map.CreateAndAddFixedObjectToMap(new HealthBoost(), _game.Width - 50, 250);
            _map.CreateAndAddFixedObjectToMap(new Ship(), 0, 0);
        }
        private void InitializeLvl2()
        {
            //Initialize Background Music
            backgroundMusicMediaElement.Source = new Uri("ms-appx:///Assets/Music/Lvl3Theme.mp3");
            //set volume
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;

            _player.currentLvl = 2;
            _player.ResetPlayerGame();
            _map = new Map(_game, "/Assets/Backgrounds/Lvl2Background.jpg", new Point(_game.Height - 250, _game.Width - 200), 200, false);
            _player.UpdateStartingPosition(_map);

            InitializePlayerVisualTools();

            //player starting block
            _map.CreateAndAddBlockToMap(new WoodBlock(200, 20, new Point(_game.Height - 90, _game.Width - 300)));

            //ship platform
            _map.CreateAndAddBlockToMap(new WoodBlock(400, 20, new Point(200, 0)));

            //blocks
            _map.CreateAndAddBlockToMap(new WoodBlock(600, 20, new Point(_game.Height - 40, _game.Width / 2 - 400)));
            _map.CreateAndAddBlockToMap(new WoodBlock(200, 20, new Point(800, 0)));
            _map.CreateAndAddBlockToMap(new WoodBlock(100, 20, new Point(600, 0)));
            _map.CreateAndAddBlockToMap(new WoodBlock(100, 20, new Point(500, 300)));
            _map.CreateAndAddBlockToMap(new WoodBlock(_game.Width - 800, 20, new Point(550, 800)));


            //fixed objects
            _map.CreateAndAddFixedObjectToMap(new Ship(), 20, 20);
            _map.CreateAndAddFixedObjectToMap(new HealthBoost(), 500, _game.Height - 100);
            _map.CreateAndAddFixedObjectToMap(new FuelTank(), 50, 550);
            _map.CreateAndAddFixedObjectToMap(new FuelTank(), _game.Width - 60, 500);

            //enemies
            _map.CreateAndAddEnemyToMap(new StormTrooper(new Point(_game.Height - 240, _game.Width / 2 + 5), 50, 50));
            _map.CreateAndAddEnemyToMap(new StormTrooper(new Point(_game.Height - 240, _game.Width / 2 - 200), 20, 100));
            _map.CreateAndAddEnemyToMap(new Droid(new Point(350, _game.Width - 500), 100, 200));
            _map.CreateAndAddEnemyToMap(new Droid(new Point(20, 300), 5, 50));

        }
        private void InitializeBossFight()
        {
            //Initialize Background Music
            backgroundMusicMediaElement.Source = new Uri("ms-appx:///Assets/Music/Lvl4Theme.mp3");
            //set volume
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;

            _player.currentLvl = 3;

            _player.ResetPlayerGame();
            InitializePlayerVisualTools();

            _map = new Map(_game, "/Assets/Backgrounds/BossFightBackground.jpg", new Point(_game.Height - 200, 1), 50, false);
            _player.UpdateStartingPosition(_map);

            //floor
            _map.CreateAndAddBlockToMap(new RustBlock(_game.Width, 100, new Point(_game.Height - 40, 0)));

            //boss
            _map.CreateAndAddBossToMap(new Boss(new Point(0, _game.Width - 500)));
            InitializeBossHealthVisual();
        }
        private void InitializeLevleCratorMap()
        {
            _player.currentLvl = -2;

            _player.ResetPlayerGame();
            InitializePlayerVisualTools();

            _map = new Map(LevelCreation.Map, _game);
            _player.UpdateStartingPosition(_map);
            _game.Background = _map.BackgroundImageBrush;
        }
        #endregion
        #endregion
        #region Gameloop
        private void GameLoop(object sender, object e)
        {
            //calculate gravity effects
            Gravity();

            //handle enemies in the map
            HandleEnemies();

            //handle player movement and interactions with the enviorment
            HandlePlayer();

            //take action if the lvl is compleate 
            CheckLvlCompletion();

            //activate the toturial text
            if (_player.tutorial)
                Tutorial();
        }
        private void Gravity()
        {
            //player gravity
            //toggle for jetpack effect
            if (!_jump)
            {
                // get the current position in canvas
                Point currentPosition = _player.GetPositionInCanvas();

                //make a new position to move to.
                _newPosition = new Point(currentPosition.Top + _forceOnPlayer, currentPosition.Left);

                //checks to see that the player is not standing on a block
                if (!_player.IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                {
                    //move the player down by the amount of force acting on him
                    _player.MoveDown(_forceOnPlayer);

                    //increase the force for next time
                    _forceOnPlayer++;
                }

                //player has hit the map so reset the force
                else _forceOnPlayer = 1;
            }

            //enemies gravity
            //do for each enemy not going to be very visible to the player, mostly there to allow a more dynamic enemy positionining
            foreach (Enemy enemy in _map.GetEnemies())
            {
                //get the current position in canvas
                Point currentPostion = enemy.GetPositionInCanvas();

                //make a new position to move to.
                _newPosition = new Point(currentPostion.Top + _forceOnEnemies, currentPostion.Left);

                //checks to see that the enemy is not standing on a block
                if (!enemy.IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                {
                    //move the enemy down by the amount of force acting on him
                    enemy.MoveDown(_forceOnEnemies);

                    //increase the force for next time
                    _forceOnEnemies++;
                }

                //enemy has hit the map so reset the force
                else _forceOnEnemies = 1;
            }

            //boss
            if (_map.GetBoss() != null)
            {
                //get the current position in canvas
                Point currentPostion = _map.GetBoss().GetPositionInCanvas();

                //make a new position to move to.
                _newPosition = new Point(currentPostion.Top + _forceOnBoss, currentPostion.Left);

                //checks to see that the enemy is not standing on a block
                if (!_map.GetBoss().IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                {
                    //move the enemy down by the amount of force acting on him
                    _map.GetBoss().MoveDown(_forceOnBoss);

                    //increase the force for next time
                    _forceOnBoss++;
                }
                else
                    _forceOnBoss = 1;
            }
        }
        private void CheckLvlCompletion()
        {
            if (_map.IsComplete)
            {
                switch (_player.currentLvl)
                {
                    //if finised tutorial will be 1
                    case 1:
                        _tutorialTextBlock.Visibility = Visibility.Collapsed;
                        InitializeLvl1();
                        _forceOnPlayer = 1;
                        break;

                    //if finised lvl 1 will be 2
                    case 2:
                        InitializeLvl2();
                        _forceOnPlayer = 1;
                        break;

                    //if finised lvl 2 will be 3
                    case 3:
                        InitializeBossFight();
                        _forceOnPlayer = 1;
                        break;

                    //if killed the boss will be lvl 4
                    case 4:
                        InitializeWiningSplashScreen();
                        break;

                    //if finised a user created map will go back to lvl creator
                    case -1:
                        Frame.Navigate(typeof(LevelCreation));
                        break;
                    default:
                        break;
                }
            }
        }
        #region Map
        private void Tutorial()
        {
            //position to the right of the player with a small fixed margin
            _tutorialTextBlockRightPosition = new Point(_player.GetPositionInCanvas().Top - 40, _player.GetPositionInCanvas().Left + 120);

            //position to the left of the player with a small fixed margin
            _tutorialTextBlockLeftPosition = new Point(_player.GetPositionInCanvas().Top - 40, _player.GetPositionInCanvas().Left - _tutorialTextBlock.ActualWidth - 20);

            //position right
            Canvas.SetLeft(_tutorialTextBlock, _tutorialTextBlockRightPosition.Left);
            Canvas.SetTop(_tutorialTextBlock, _tutorialTextBlockRightPosition.Top);

            //if goes out of screen
            if (_tutorialTextBlockRightPosition.Left + _tutorialTextBlock.ActualWidth > _game.ActualWidth &&
                _tutorialTextBlockLeftPosition.Left > 0)
            {
                Canvas.SetLeft(_tutorialTextBlock, _tutorialTextBlockLeftPosition.Left);
                Canvas.SetTop(_tutorialTextBlock, _tutorialTextBlockLeftPosition.Top);
            }

            if (_player.GetPositionInCanvas().Left >= 550 && _player.GetPositionInCanvas().Top <= 300)
            {
                _upKeyTutorialFlag = true;
            }

            //the logic
            _tutorialTextBlock.Text = _startingMsg;
            if (_enterKeyTutorialFlag)
            {
                _tutorialTextBlock.Text = _rightKeyMsg;
                if (_rightKeyTutorialFlag)
                {
                    _tutorialTextBlock.Text = _leftKeyMsg;
                    if (_leftKeTutorialFlag)
                    {
                        _tutorialTextBlock.Text = _upKeyMsg;
                        if (_upKeyTutorialFlag)
                        {
                            _tutorialTextBlock.Text = _fuelTankMsg;
                            if (_fuelTutorialFlag)
                            {
                                _tutorialTextBlock.Text = _enemyMsg;
                                if (_map.GetEnemies().Count == 0)
                                {
                                    _tutorialTextBlock.Text = _healthBoostMsg;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #endregion
        #region Enemies Controls
        private void HandleEnemies()
        {
            //patrol enemies
            foreach (Enemy enemy in _map.GetEnemies())
            {
                enemy.Patrol(_game, _map, _player);
            }

            //collision with shots fired from player
            List<Enemy> TempEnemies = new List<Enemy>(_map.GetEnemies());
            foreach (Enemy enemy in TempEnemies)
            {
                if (enemy.IsCollidingWithProjectile(_player.GetShots(), enemy.GetPositionInCanvas(), out Projectile projectile))
                {
                    //hit the enemy
                    enemy.Hit(enemy.shotDmg);

                    //delete the shot
                    projectile.Remove(_player.GetShots());
                    hitFx.Play();
                    //check if enemy died
                    if (!enemy.IsAlive)
                    {
                        //player killed enemy
                        _player.KillEnemy(_map);
                        enemyDeath.Play();
                        //map lost enemy
                        _map.RemoveEnemy(enemy);
                    }
                    //update visual representation of score
                    InitializeScore();
                }
            }

            //if there is a boss
            if (_map.GetBoss() != null)
            {
                //collision with player shots
                if (_map.GetBoss().IsCollidingWithProjectile(_player.GetShots(), _map.GetBoss().GetPositionInCanvas(), out Projectile projectile))
                {
                    //hit the boss
                    _map.GetBoss().Hit(Player.ShotDmg);

                    UpdateBossHealthVisual();

                    //delete the shot
                    projectile.Remove(_player.GetShots());

                    if (!_map.GetBoss().IsAlive)
                    {
                        _player.KillBoss();

                        _map.CompleteLvl();
                    }
                    //update visual representation of score
                    InitializeScore();
                }

                //logic
                if (_map.GetBoss() != null)
                {
                    _map.GetBoss().BossLogic(_game, _player, _map);
                }
                InitializeHealthBar();
            }
        }
        private void InitializeBossHealthVisual()
        {
            //create the health bar
            _bossHealthBar = new ProgressBar()
            {
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                Width = 600,
                Height = 50,
                Maximum = Boss.Max_Health,
                Minimum = 0,
                Value = _map.GetBoss().GetHealth(),
            };
            Canvas.SetLeft(_bossHealthBar, _game.Width / 2 - 300);
            Canvas.SetTop(_bossHealthBar, 60);
            _game.Children.Add(_bossHealthBar);

            _bossName = new TextBlock()
            {
                Text = "Baby Yoda",
                FontSize = 40,
                FontFamily = new FontFamily("Segon Print"),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            };
            Canvas.SetLeft(_bossName, _game.Width / 2 - 100);
            Canvas.SetTop(_bossName, 10);
            _game.Children.Add(_bossName);
        }
        private void UpdateBossHealthVisual()
        {
            _bossHealthBar.Value = _map.GetBoss().GetHealth();
        }
        #endregion
        #region Player Controls
        private void User_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            _keys.RemoveAll(key => key == args.VirtualKey.ToString());
        }
        private void User_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            _keys.Add(args.VirtualKey.ToString());
        }
        private void HandlePlayer()
        {
            #region Movement
            //var for player current top location
            double top = _player.GetPositionInCanvas().Top;
            double left = _player.GetPositionInCanvas().Left;

            //move right
            if (_keys.Contains("Right"))
            {
                _newPosition = new Point(top, _player.GetMoveRight());
                if (!_player.IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                {
                    _rightKeyTutorialFlag = true;
                    _player.MoveRight(_game);
                }
            }

            //move left
            if (_keys.Contains("Left"))
            {
                _newPosition = new Point(top, _player.GetMoveLeft());
                if (!_player.IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                {
                    _leftKeTutorialFlag = true;
                    _player.MoveLeft();
                }
            }

            //Up to activate jetPack
            if (_keys.Contains("Up"))
            {
                _newPosition = new Point(_player.GetMoveUp(), left);

                if (_player.GetFuel() > 0)
                {
                    _player.ConsumeFuel();
                    InitializeFuelBar();
                    if (!_player.IsCollidingWithMap(_map.GetBlocks(), _newPosition))
                    {
                        _jump = true;
                        _forceOnPlayer = 0;
                        _player.MoveUp();
                        jetPackFx.Play();
                    }
                }
                else if (_player.GetFuel() <= 0)
                {
                    jetPackEmptyFx.Play();
                    _jump = false;
                }
            }
            //released Up will resault in activating gravity
            if (!_keys.Contains("Up"))
            {
                _jump = false;
                jetPackFx.Stop();
            }
            #endregion
            #region Actions

            //Space to shoot
            if (_keys.Contains("Space"))
            {
                if (!_shoot)
                {
                    if (_player.GetAmmo() > 0)
                    {
                        _player.Shoot(_game, _map);
                        InitializeAmmo();
                        _shoot = true;
                        shotFx.Play();
                    }
                    else
                    {
                        emptyShotFx.Play();
                    }
                }
            }
            //only one shot per click
            if (!_keys.Contains("Space"))
            {
                _shoot = false;
                shotFx.Stop();
            }

            //flag for tutorial
            if (_keys.Contains("Enter"))
            {
                _enterKeyTutorialFlag = true;
            }

            if (_keys.Contains("Escape"))
            {
                if (_isPauseMenuOpen)
                {
                    ClosePauseMenu();
                }
                else
                    OpenPauseMenu();
            }
            #endregion
            #region Collisions with other objects
            //colision with enemy shots
            foreach (Enemy enemy in _map.GetEnemies())
            {
                if (_player.IsCollidingWithProjectile(enemy.GetShots(), _player.GetPositionInCanvas(), out Projectile projectile))
                {
                    //hit the player
                    _player.Hit(enemy.shotDmg);
                    InitializeHealthBar();
                    playerHit.Play();
                    //remove the shot from list
                    projectile.Remove(enemy.GetShots());
                }
            }

            //colision with fixed objects
            if (_player.IsCollidingWithFixedObject(_map.GetFixedObjects(), _player.GetPositionInCanvas(), out FixedObject fixedObject))
            //only null when false
            {
                pickUpFx.Play();

                //do action for the fixed object
                fixedObject.Action(_player, _map);

                if (fixedObject is HealthBoost)
                {
                    InitializeHealthBar();
                }
                if (fixedObject is FuelTank)
                {
                    InitializeFuelBar();
                    _fuelTutorialFlag = true;
                }
            }

            //collision with boss shots

            if (_map.GetBoss() != null)
            {
                if (_player.IsCollidingWithProjectile(_map.GetBoss().GetShots(), _player.GetPositionInCanvas(), out Projectile projectile))
                {
                    //hit the player
                    if (projectile is Bolder)
                    {
                        _player.Hit(Boss.BolderDamage);
                    }
                    if (projectile is Shot)
                    {
                        _player.Hit(Boss.ShotDmg);
                    }
                    InitializeHealthBar();

                    //remove the shot from list
                    projectile.Remove(_map.GetBoss().GetShots());
                }
            }
            #endregion
            //if fallse of the map end the game
            if (_player.GetPositionInCanvas().Top + _player.Image.ActualHeight >= _game.ActualHeight)
            {
                _player.Kill();
            }

            //check if player is dead
            if (!_player.IsAlive)
            {
                GameOver();
            }
        }

        #endregion
        #region Program Actions
        private void GameOver()
        {
            //clear the map
            _map.Clear();
            //hide the player
            _player.Image.Visibility = Visibility.Collapsed;
            //hide tutorial
            if (_player.tutorial)
            {
                _tutorialTextBlock.Visibility = Visibility.Collapsed;
            }

            gameOverFx.Play();
            //reset force
            _forceOnPlayer = 1;
            //stop timer
            _timer.Stop();

            //open game over screen
            #region GameOver Screen

            ImageBrush brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/GameOver.jpg")),
                Stretch = Stretch.Fill
            };
            _game.Background = brush;

            //stop music
            backgroundMusicMediaElement.Stop();

            //activate sound effect
            //TODO

            //retryBnt
            _retryBtn = new Button
            {
                FontSize = 40,
                Content = "Retry",
                Background = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)),
                Width = 200,
                Height = 200,
                CornerRadius = new CornerRadius(100)
            };
            Canvas.SetLeft(_retryBtn, _game.ActualWidth / 2 - 250);
            Canvas.SetTop(_retryBtn, _game.ActualHeight / 2 + 150);
            _game.Children.Add(_retryBtn);
            _retryBtn.Click += RetryBtn_Click;


            //Main Menu btn
            _mainMenuBtn = new Button
            {
                Width = 200,
                Height = 200,
                Content = (!_map.IsUserGenerated) ? "Return to\nmain menu" : "Create a new\nMap",
                Background = new SolidColorBrush(Color.FromArgb(200, 200, 0, 0)),
                FontSize = 30,
                CornerRadius = new CornerRadius(100)
            };
            Canvas.SetLeft(_mainMenuBtn, _game.ActualWidth / 2 + 50);
            Canvas.SetTop(_mainMenuBtn, _game.ActualHeight / 2 + 150);
            _game.Children.Add(_mainMenuBtn);
            if (!_map.IsUserGenerated)
            {
                _mainMenuBtn.Click += MainMenuBtn_Click;
            }
            else
                _mainMenuBtn.Click += LevelCreationBtn_Click;
            #endregion
        }

        #region Events
        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_map.IsUserGenerated)
            {
                UpdateScoreKeeper();
            }
            Frame.Navigate(typeof(MainPage));
        }
        private void LevelCreationBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LevelCreation));
        }

        private void RetryBtn_Click(object sender, RoutedEventArgs e)
        {
            //remove the buttons
            _game.Children.Remove(_retryBtn);
            _game.Children.Remove(_mainMenuBtn);

            //reset lvl
            switch (_player.currentLvl)
            {
                case 0:
                    InitializeTutorialMap();
                    _player.Image.Visibility = Visibility.Visible;
                    _tutorialTextBlock.Visibility = Visibility.Visible;
                    _timer.Start();
                    break;

                case 1:
                    InitializeLvl1();
                    _player.Image.Visibility = Visibility.Visible;
                    _timer.Start();
                    break;

                case 2:
                    InitializeLvl2();
                    _player.Image.Visibility = Visibility.Visible;
                    _timer.Start();
                    break;

                case 3:
                    InitializeBossFight();
                    _player.Image.Visibility = Visibility.Visible;
                    _timer.Start();
                    break;

                case -2:
                    InitializeLevleCratorMap();
                    _player.Image.Visibility = Visibility.Visible;
                    _timer.Start();
                    break;
                default:
                    break;
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            ClosePauseMenu();
        }
        private void VolSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            Settings.SettingsProfile.MasterVolume = e.NewValue;
            backgroundMusicMediaElement.Volume = Settings.SettingsProfile.MusicVolume;
            jetPackEmptyFx.Volume = Settings.SettingsProfile.FxVolume;
            jetPackFx.Volume = Settings.SettingsProfile.FxVolume;
        }
        #endregion

        #region Pause Menu
        private void OpenPauseMenu()
        {
            _isPauseMenuOpen = true;
            _timer.Stop();

            #region PauseMenu
            //create a brush
            ImageBrush backgroundbrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/Backgrounds/PauseMenuBackground.jpg"))
            };

            //create a new grid
            _pauseMenuGrid = new Grid
            {
                Background = backgroundbrush,
                Width = 800,
                Height = 800,
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
            };
            Canvas.SetLeft(_pauseMenuGrid, _game.ActualWidth / 2 - 400);
            Canvas.SetTop(_pauseMenuGrid, 40);
            _game.Children.Add(_pauseMenuGrid);

            //header
            RowDefinition row1 = new RowDefinition
            {
                Height = new GridLength(200)
            };
            _pauseMenuGrid.RowDefinitions.Add(row1);

            //contant
            RowDefinition row2 = new RowDefinition
            {
                Height = new GridLength(400)
            };
            _pauseMenuGrid.RowDefinitions.Add(row2);

            //navigation
            RowDefinition row3 = new RowDefinition
            {
                Height = new GridLength(200)
            };
            _pauseMenuGrid.RowDefinitions.Add(row3);

            //back button
            Button backBtn = new Button
            {
                Content = "Back to Game",
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 40,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                Background = null
            };
            backBtn.Click += BackBtn_Click;
            Grid.SetRow(backBtn, 0);
            _pauseMenuGrid.Children.Add(backBtn);

            //header text
            TextBlock textBlock = new TextBlock
            {
                Text = "Paused",
                FontFamily = new FontFamily("Segoe Print"),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                FontSize = 80,
                TextDecorations = Windows.UI.Text.TextDecorations.Underline,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(textBlock, 0);
            _pauseMenuGrid.Children.Add(textBlock);

            //navigation buttons grid
            Grid naviBtnGrid = new Grid();
            Grid.SetRow(naviBtnGrid, 2);
            _pauseMenuGrid.Children.Add(naviBtnGrid);

            ColumnDefinition col1 = new ColumnDefinition
            {
                Width = new GridLength(400)
            };
            naviBtnGrid.ColumnDefinitions.Add(col1);

            ColumnDefinition col2 = new ColumnDefinition
            {
                Width = new GridLength(400)
            };
            naviBtnGrid.ColumnDefinitions.Add(col2);

            //Home btn
            Button homeBtn = new Button
            {
                Content = "Home",
                FontFamily = new FontFamily("Segoe Print"),
                FontSize = 20,
                Width = 100,
                Height = 100,
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                CornerRadius = new CornerRadius(100),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            homeBtn.Click += MainMenuBtn_Click;
            Grid.SetColumn(homeBtn, 0);
            naviBtnGrid.Children.Add(homeBtn);

            //Settings Btn
            Button settingsBtn = new Button
            {
                Content = "Settings",
                FontFamily = new FontFamily("Segoe Print"),
                FontSize = 20,
                Width = 100,
                Height = 100,
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                CornerRadius = new CornerRadius(100),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            settingsBtn.Click += SettingsBtn_Click;
            Grid.SetColumn(settingsBtn, 1);
            naviBtnGrid.Children.Add(settingsBtn);

            //volume control
            StackPanel contantPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,

            };
            Grid.SetRow(contantPanel, 1);
            _pauseMenuGrid.Children.Add(contantPanel);

            TextBlock volbox = new TextBlock()
            {
                FontSize = 30,
                FontFamily = new FontFamily("Segoe Print"),
                Text = "Volume",
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                Margin = new Thickness(10, 0, 10, 0),
            };
            contantPanel.Children.Add(volbox);
            Slider volSlider = new Slider()
            {
                Width = 300,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                Minimum = 0,
                Maximum = 1,
                StepFrequency = 0.01,
                IsThumbToolTipEnabled = false,
                Value = Settings.SettingsProfile.MasterVolume,
            };
            volSlider.ValueChanged += VolSlider_ValueChanged;
            Grid.SetRow(volSlider, 1);
            contantPanel.Children.Add(volSlider);
            #endregion
        }
        private void ClosePauseMenu()
        {
            _isPauseMenuOpen = false;
            _timer.Start();
            _game.Children.Remove(_pauseMenuGrid);
        }

        #endregion
        private void UpdateScoreKeeper()
        {
            if (_player.GetScore() > 0)
            {
                _scoreKeeper.PlayerName = _player.Name;
                _scoreKeeper.PlayerScore = _player.GetScore();
                if (ScoreBoard.PlayerScoreKeepers != null)
                {
                    ScoreBoard.PlayerScoreKeepers.Add(_scoreKeeper);
                }
                else
                {
                    ScoreBoard.PlayerScoreKeepers = new List<PlayerScoreKeeper>
                    {
                        _scoreKeeper
                    };
                }

            }

        }
        private void InitializeWiningSplashScreen()
        {
            //clear map
            _map.Clear();
            _player.Image.Visibility = Visibility.Collapsed;
            //stop timer
            _timer.Stop();
            //change background
            ImageBrush brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/EndBcakground.png")),
                Stretch = Stretch.Fill
            };
            _game.Background = brush;

            //remove boss health lvl visual
            _game.Children.Remove(_bossHealthBar);
            _game.Children.Remove(_bossName);

            TextBlock name = new TextBlock()
            {
                Text = _player.Name,
                FontSize = 40,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
            };
            Canvas.SetLeft(name, _game.Width / 2 - 60);
            Canvas.SetTop(name, 250);
            _game.Children.Add(name);

            //home button
            Button homeBtn = new Button
            {
                Content = "Home",
                FontFamily = new FontFamily("Segoe Print"),
                FontSize = 20,
                Width = 100,
                Height = 100,
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31)),
                CornerRadius = new CornerRadius(100),

            };
            homeBtn.Click += MainMenuBtn_Click;
            Canvas.SetLeft(homeBtn, _game.Width / 2 - 50);
            Canvas.SetTop(homeBtn, _game.Height - 300);
            _game.Children.Add(homeBtn);

        }

        #region SoundEffects

        #endregion
        #endregion
    }
}
