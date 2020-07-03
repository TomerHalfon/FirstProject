using Platformer.Classes;
using Platformer.Classes.GameObjects;
using Platformer.Classes.GameObjects.Blocks;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using Platformer.Classes.GameObjects.FixedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Platformer.Game
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelCreation : Page
    {
        public static Map Map;

        Image _selectedImage ;
        GameObject _selectefGameObject;

        TextBox _widthInput, _heightInput, _maxRightInput, _maxLeftInput;
        StackPanel _sp;
        Classes.Point _playerMarkerPosition;
        bool _isMousePressed, _playerPositionMarkerFlag;
        
        #region background image path
        const string _cityPath = "/Assets/Backgrounds/CityBack.jpg";
        const string _desertPath = "/Assets/Backgrounds/Lvl0_back.jpg";
        const string _villagePath = "/Assets/Backgrounds/Lvl2Background.jpg";
        const string _deathStarPath = "/Assets/Backgrounds/BossFightBackground.jpg";
        const string _controlRoomPath = "/Assets/Backgrounds/SettingsBackground.jpg"; 
        #endregion

        public LevelCreation()
        {
            this.InitializeComponent();
            InitializeComboBoxes();
            _selectedImage = new Image();
            _selectefGameObject = new GameObject();
            _isMousePressed = false;
            _playerPositionMarkerFlag = false;
            _playerMarkerPosition = new Classes.Point();
            Map = new Map(canvas, "/Assets/Backgrounds/White.jpg", new Classes.Point(), 0, true);
        }

        #region Initializers
        private void InitializeComboBoxes()
        {
            InitializeBackgroundComboBox();
            InitializeEnemiesComboBox();
            InitializeFixedObjectsComboBox();
            InitializeBlocksComboBox();
        }

        private void InitializeBlocksComboBox()
        {
            blockComboBox.Items.Add("Wood");
            blockComboBox.Items.Add("Metal");
            blockComboBox.Items.Add("Rust");
            blockComboBox.Items.Add("Sand");
            blockComboBox.SelectedIndex = 0;
        }

        private void InitializeFixedObjectsComboBox()
        {
            fixedObjectsComboBox.Items.Add("Fuel Tank");
            fixedObjectsComboBox.Items.Add("Health Boost");
            fixedObjectsComboBox.Items.Add("Ship");
            fixedObjectsComboBox.SelectedIndex = 0;
        }

        private void InitializeEnemiesComboBox()
        {
            enemiesComboBox.Items.Add("StormTrooper");
            enemiesComboBox.Items.Add("Droid");
            enemiesComboBox.SelectedIndex = 0;
        }

        private void InitializeBackgroundComboBox()
        {
            backgroundComboBox.Items.Add("City");
            backgroundComboBox.Items.Add("Desert");
            backgroundComboBox.Items.Add("Village");
            backgroundComboBox.Items.Add("Death Star");
            backgroundComboBox.Items.Add("Control Room");
            backgroundComboBox.SelectedIndex = 0;
        } 
        #endregion

        #region Events
        private void BackgroundChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            string backPath = "";
            switch (backgroundComboBox.SelectedItem)
            {
                case "City":
                    backPath = _cityPath;
                    break;
                case "Desert":
                    backPath = _desertPath;
                    break;
                case "Village":
                    backPath = _villagePath;
                    break;
                case "Death Star":
                    backPath = _deathStarPath;
                    break;
                case "Control Room":
                    backPath = _controlRoomPath;
                    break;
            }
            Map.UpdateBackground(backPath);

        }

        private void EnemiesAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Enemy enemy = new StormTrooper(new Classes.Point(), 100, 100);

            switch (enemiesComboBox.SelectedItem)
            {
                case "StormTrooper":
                    enemy = new StormTrooper(new Classes.Point(), 100, 100);
                    break;
                case "Droid":
                    enemy = new Droid(new Classes.Point(), 100, 100);
                    break;
                default:
                    break;
            }
            enemy.Image.PointerPressed += SelectedImage_onMuseClick;
            Map.CreateAndAddEnemyToMap(enemy);
           // DataUpdater(enemy);
        }

        private void FixedObjectsAddBtn_Click(object sender, RoutedEventArgs e)
        {
            FixedObject fixedObject = new FuelTank();

            switch (fixedObjectsComboBox.SelectedItem)
            {
                
                case "Fuel Tank":
                    fixedObject = new FuelTank();
                    break;

                case "Health Boost":
                    fixedObject = new HealthBoost();
                    break;

                case "Ship":
                    fixedObject = new Ship();
                    break;
            }

            fixedObject.Image.PointerPressed += SelectedImage_onMuseClick;
            Map.CreateAndAddFixedObjectToMap(fixedObject, 0, 0);
        }

        private void BlockAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Block block = new WoodBlock(100, 100, new Classes.Point()); 
            switch (blockComboBox.SelectedItem)
            {
                case "Wood":
                    block = new WoodBlock(100, 10, new Classes.Point());
                    break;
                case "Metal":
                    block = new MetalBlock(100, 10, new Classes.Point());
                    break;
                case "Rust":
                    block = new RustBlock(100, 10, new Classes.Point());
                    break;
                case "Sand":
                    block = new SandBlock(100, 10, new Classes.Point());
                    break;
                default:
                    break;
            }
            block.Image.PointerPressed += SelectedImage_onMuseClick;
            _selectedImage = block.Image;
            Map.CreateAndAddBlockToMap(block);
           // DataUpdater(block);
            
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(_sp);
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (_selectefGameObject)
            {
                //if selected block
                case Block block:
                    if (double.TryParse(_widthInput.Text, out double width))
                    {
                        _selectedImage.Width = width;
                    }
                    if (double.TryParse(_heightInput.Text, out double height))
                    {
                        _selectedImage.Height = height;
                    }
                    break;
                    //if selected enemy
                case Enemy enemy:
                    if (double.TryParse(_maxRightInput.Text, out double maxRight)&&
                        double.TryParse(_maxLeftInput.Text, out double maxLeft))
                    {
                        enemy.SetMaxMovements(maxLeft, maxRight);
                    }
                    break;
                default:
                    break;
            }
            canvas.Children.Remove(_sp);
        }

        private void Canvas_OnMouseClick(object sender, PointerRoutedEventArgs e)
        {
            _isMousePressed = true;

        }

        private void Canvas_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (_selectefGameObject != null)
            {
                DataUpdater(_selectefGameObject);
            }
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Map != null)
            {
                Map = new Map(canvas, "/Assets/Backgrounds/White.jpg", new Classes.Point(), 0,true);
                canvas.Children.Clear();
            }
        }

        private void PlayerPositionBtn_Click(object sender, RoutedEventArgs e)
        {
            Image playerPositionMarker = new Image();
            if (_playerPositionMarkerFlag == false)
            {
                //create a marker
                 playerPositionMarker = new Image()
                {
                    Width = 100,
                    Height = 150,
                    Source = new BitmapImage(new Uri($"ms-appx:///Assets/Characters/Playable/Mando.png")),
                };
                playerPositionMarker.PointerPressed += PlayerPositionMarker_PointerPressed;
                Canvas.SetLeft(playerPositionBtn, Map.GetPlayerStartingPosition().Left);
                Canvas.SetTop(playerPositionBtn, Map.GetPlayerStartingPosition().Top);
                canvas.Children.Add(playerPositionMarker);
                _playerPositionMarkerFlag = true;
            }
            else
            {
                Map.SetPlayerStartingPosition(_playerMarkerPosition);
                _playerPositionMarkerFlag = false;
            }

        }

        private void PlayerPositionMarker_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _selectedImage = (Image)e.OriginalSource;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            Frame.Navigate(typeof(GamePage),Map);
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SelectedImage_onMuseClick(object sender, PointerRoutedEventArgs e)
        {
            _selectedImage = (Image)e.OriginalSource;
           _selectefGameObject =  ConvertImageToGameObject(_selectedImage);
            _playerPositionMarkerFlag = false;
        }
        private void Canvas_OnMouseUnClick(object sender, PointerRoutedEventArgs e)
        {
            _isMousePressed = false;
        }

        private void Canvas_OnMouseMove(object sender, PointerRoutedEventArgs e)
        {
            if (_isMousePressed)
            {
                PointerPoint point = e.GetCurrentPoint((Canvas)sender);

                double left = point.Position.X;
                double top = point.Position.Y;

                if (_playerPositionMarkerFlag == false)
                    _selectefGameObject.SetPositionInCanvas(left - _selectefGameObject.Image.Width / 2, top - _selectefGameObject.Image.Height / 2);
                else
                {
                    Canvas.SetLeft(_selectedImage, left);
                    Canvas.SetTop(_selectedImage, top);
                    _playerMarkerPosition.Left = left;
                    _playerMarkerPosition.Top = top;
                }
            }
        }
        private void Input_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
        #endregion

        //update data window
        private void DataUpdater(GameObject gameObject)
        {
            _sp = new StackPanel()
            {
                Orientation = Orientation.Vertical,
            };

            Button updateBtn = new Button()
            {
                Content = "Update"
            };
            updateBtn.Click += UpdateBtn_Click;

            Button closeBtn = new Button()
            {
                Content = "Cencel"
            };
            closeBtn.Click += CloseBtn_Click;
            Canvas.SetLeft(_sp, gameObject.GetPositionInCanvas().Left);
            Canvas.SetTop(_sp, gameObject.GetPositionInCanvas().Top);
            switch (gameObject)
            {
                case Block block:
                    TextBlock widthBlock = new TextBlock()
                    {
                        Text = "Enter the width of the block",
                        FontSize = 20,
                    };
                    _widthInput = new TextBox()
                    {
                        Width = 100,
                        Height = 30,
                    };
                    _widthInput.BeforeTextChanging += Input_BeforeTextChanging;
                    TextBlock heightBlock = new TextBlock()
                    {
                        Text = "Enter the height of the block",
                        FontSize = 20,
                    };
                    _heightInput = new TextBox()
                    {
                        Width = 100,
                        Height = 30,
                    };
                    _heightInput.BeforeTextChanging += Input_BeforeTextChanging;
                    _sp.Children.Add(widthBlock);
                    _sp.Children.Add(_widthInput);
                    _sp.Children.Add(heightBlock);
                    _sp.Children.Add(_heightInput);
                    _sp.Children.Add(updateBtn);
                    _sp.Children.Add(closeBtn);
                    break;
                case Enemy enemy:
                    TextBlock maxRightBlock = new TextBlock()
                    {
                        Text = "Enter the distance the enemy can move right",
                        FontSize = 20,
                    };
                    _maxRightInput = new TextBox()
                    {
                        Width = 100,
                        Height = 30,
                    };
                    _maxRightInput.BeforeTextChanging += Input_BeforeTextChanging;
                    TextBlock maxLeftBlock = new TextBlock()
                    {
                        Text = "Enter the distance the enemy can move Left",
                        FontSize = 20,
                    };
                    _maxLeftInput = new TextBox()
                    {
                        Width = 100,
                        Height = 30,
                    };
                    _maxLeftInput.BeforeTextChanging += Input_BeforeTextChanging;
                    _sp.Children.Add(maxRightBlock);
                    _sp.Children.Add(_maxRightInput);
                    _sp.Children.Add(maxLeftBlock);
                    _sp.Children.Add(_maxLeftInput);
                    _sp.Children.Add(updateBtn);
                    _sp.Children.Add(closeBtn);

                    break;
            }
            canvas.Children.Add(_sp);
            _selectedImage = gameObject.Image;
        }

        //conver the selected image to a game Object
        private GameObject ConvertImageToGameObject(Image image)
        {
            foreach (Block block in Map.GetBlocks())
            {
                if (image == block.Image)
                {
                    return block;
                }
            }
            foreach (Enemy enemy in Map.GetEnemies())
            {
                if (image == enemy.Image)
                {
                    return enemy;
                }
            }
            foreach (FixedObject fixedObject in Map.GetFixedObjects())
            {
                if (image == fixedObject.Image)
                {
                    return fixedObject;
                }
            }
            return null;
        }
    }
}

