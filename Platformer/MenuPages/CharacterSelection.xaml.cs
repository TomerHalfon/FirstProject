using Platformer.Classes;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using Platformer.Game;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Platformer.MenuPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CharacterSelection : Page
    {
        public static Player _player;
        public static double VolumeLvl;

        const string _mandoSource = "/Assets/Characters/Mando_Selection.png";
        const string _mandoRightSource = "/Assets/Characters/Playable/Mando.png";
        const string _mandoLeftSource = "/Assets/Characters/Playable/Mando_Left.png";

        const string _babyYodaSource = "/Assets/Characters/Baby_Yoda_Selection.png";
        const string _babyYodaRightSource = "/Assets/Characters/Playable/Baby_Yoda.png";
        const string _babyYodaLeftSource = "/Assets/Characters/Playable/Baby_Yoda.png";
        
        SettingsProfile _settings;
        List<Image> _skins;
        int _selectionIndex;
        public CharacterSelection()
        {
            this.InitializeComponent();
            InitCharacterList();
            RefreshDisplay();
            _settings = Settings.SettingsProfile;
            backgroundMusicMediaElement.Volume = _settings.MusicVolume;
        }
        private void InitCharacterList()
        {
            _selectionIndex = 0;
            Image img = new Image();

            //init the list
            _skins = new List<Image>();

            //add img
            img.Source = new BitmapImage(new Uri($"ms-appx://{_mandoSource}"));  //0
            _skins.Add(img);

            //only after boss
            if (_player != null)
            {
                if (_player._hasUnlockedBabyYoda)
                {
                    img = new Image
                    {
                        Source = new BitmapImage(new Uri($"ms-appx://{_babyYodaSource}"))  //1
                    };
                    _skins.Add(img);
                }
            }
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            CreatePlayer();
            //go to game
            Frame.Navigate(typeof(GamePage));
        }
        private void NextCButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null && !_player._hasUnlockedBabyYoda || _player == null)
            {
                Flyout flyout = new Flyout();
                TextBlock textBlock = new TextBlock
                {
                    Text = "Finish the game to unlock new characters"
                };
                flyout.Content = textBlock;
                flyout.ShowAt(NextCButton);
            }
            //loop
            if (_selectionIndex >= _skins.Count - 1)
            {
                //reached the last image and than clicked next
                _selectionIndex = 0;
            }
            else _selectionIndex++;
            RefreshDisplay();
        }

        private void PrevCButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null && !_player._hasUnlockedBabyYoda ||_player== null)
            {
                Flyout flyout = new Flyout();
                TextBlock textBlock = new TextBlock
                {
                    Text = "Finish the game to unlock new characters"
                };
                flyout.Content = textBlock;
                flyout.ShowAt(PrevCButton);
            }

            //loop
            if (_selectionIndex <= 0)
            {
                _selectionIndex = _skins.Count - 1;
            }
            else _selectionIndex--;
            RefreshDisplay();
        }
        private void CreatePlayer()
        {
            //select the image shown at the screen
            string rightImg = "";
            string leftImg = "";
            bool isBabyYoda = false;
            switch (_selectionIndex)
            {
                case 0:
                    //mando was selected
                    rightImg = _mandoRightSource;
                    leftImg = _mandoLeftSource;
                    break;
                case 1:
                    //baby yoda was selected
                    rightImg = _babyYodaRightSource;
                    leftImg = _babyYodaLeftSource;
                    isBabyYoda = true;
                    break;
            }

            //create the player
            if (Settings.SettingsProfile != null)
            {

            }
            _player = new Player(playerNameBox.Text, rightImg, leftImg, TutorialCheckBox.IsChecked.Value, isBabyYoda, Settings.SettingsProfile); //update the player

        }
        private void RefreshDisplay()
        {
            CharSelectionImage.Source = _skins[_selectionIndex].Source;
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void CreateLvlBtn_Click(object sender, RoutedEventArgs e)
        {
            CreatePlayer();
            Frame.Navigate(typeof(LevelCreation));
        }
    }
}
