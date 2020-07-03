using Platformer.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Platformer.MenuPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScoreBoard : Page
    {
        public static List<PlayerScoreKeeper> PlayerScoreKeepers;
        public static double VolumeLvl;

        List<TextBlock> _positions, _names, _scores;
        SettingsProfile _settings;
        StackPanel _positionPanel, _namePanel, _scorePanel;
        public ScoreBoard()
        {
            this.InitializeComponent();
            
            InitializeLists();
            InitializePage();
            UpdateView();
        }

        //add to the score board
        public void AddToPlayerScoreKeepers(PlayerScoreKeeper scoreKeeper)
        {
            PlayerScoreKeepers.Add(scoreKeeper);
        }

        private void MBtn_Click(object sender, RoutedEventArgs e)
        {
            if (menuSV.IsPaneOpen)
            {
                menuSV.IsPaneOpen = false;
            }
            else
            {
                menuSV.IsPaneOpen = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _settings.MasterVolume = VolumeSlider.Value;
            backgrounMusicMediaElement.Volume = _settings.MasterVolume;
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void MuteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (backgrounMusicMediaElement.IsMuted)
            {
                backgrounMusicMediaElement.IsMuted = false;
                VolumeSlider.Value = VolumeLvl;
            }
            else
            {
                VolumeLvl = _settings.MasterVolume;
                backgrounMusicMediaElement.IsMuted = true;
                VolumeSlider.Value = 0;
            }
        }

        private void InitializeLists()
        {
            if (PlayerScoreKeepers == null)
            {
                PlayerScoreKeepers = new List<PlayerScoreKeeper>();
            }
            _names = new List<TextBlock>();
            _positions = new List<TextBlock>();
            _scores = new List<TextBlock>();
        }
        private void UpdateView()
        {
            //sort list
            PlayerScoreKeepers.Sort(delegate (PlayerScoreKeeper scoreKeeper1, PlayerScoreKeeper scoreKeeper2)
            {
                return scoreKeeper2.PlayerScore.CompareTo(scoreKeeper1.PlayerScore);
            });

            //add a new line
            foreach (PlayerScoreKeeper scoreKeeper in PlayerScoreKeepers)
            {
                AddNewLineToView(scoreKeeper);
            }

        }
        private void InitializePage()
        {
            #region initialize and position the view objects
            #region Panels
            //create a panel to place the position
            _positionPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //grid positioning
            scoreGrid.Children.Add(_positionPanel);
            Grid.SetColumn(_positionPanel, 0);
            Grid.SetRow(_positionPanel, 1);

            //create a panel to place the name
            _namePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //grid positioning
            scoreGrid.Children.Add(_namePanel);
            Grid.SetColumn(_namePanel, 1);
            Grid.SetRow(_namePanel, 1);

            //create a panel to place the score
            _scorePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //grid positioning
            scoreGrid.Children.Add(_scorePanel);
            Grid.SetColumn(_scorePanel, 2);
            Grid.SetRow(_scorePanel, 1);
            #endregion
            #region Volume
            _settings = Settings.SettingsProfile;
            VolumeSlider.Value = _settings.MasterVolume;
            backgrounMusicMediaElement.Volume = _settings.MasterVolume;
            #endregion
            #endregion
        }
        private void AddNewLineToView(PlayerScoreKeeper scoreKeeper)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, 255, 232, 31));

            #region Text Blocks
            //create poition text block
            TextBlock position = new TextBlock
            {
                Foreground = color,
                FontSize = 50
            };

            _positionPanel.Children.Add(position);

                position.Text = $"{PlayerScoreKeepers.IndexOf(scoreKeeper) + 1}";
                _positions.Add(position);

            //create name text block
            TextBlock name = new TextBlock
            {
                Foreground = color,
                FontSize = 50
            };

            _namePanel.Children.Add(name);

                name.Text = scoreKeeper.PlayerName;
                _names.Add(name);

            //create score text block
            TextBlock score = new TextBlock
            {
                Foreground = color,
                FontSize = 50
            };

            _scorePanel.Children.Add(score);

                score.Text = $"{scoreKeeper.PlayerScore}";
                _scores.Add(score);
                #endregion
        }
    }
}
