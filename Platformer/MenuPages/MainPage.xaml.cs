using Platformer.Classes;
using Platformer.Game;
using Platformer.MenuPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Platformer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            if (Settings.SettingsProfile == null)
            {
                Settings.SettingsProfile = new SettingsProfile();
            }
            UpdateMediaElemts();
        }
        private void UpdateMediaElemts()
        {
            BackgroundMusic.Volume = Settings.SettingsProfile.MusicVolume;
        }
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CharacterSelection));
        }

        private void ScoreBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScoreBoard));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
