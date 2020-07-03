using Platformer.Classes;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Platformer.MenuPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class Settings : Page
    {
        public static SettingsProfile SettingsProfile;

        public Settings()
        {
            this.InitializeComponent();
            InitializeSliders();
            if (SettingsProfile == null)
            {
                SettingsProfile = new SettingsProfile();
            }
        }
        private void InitializeSliders()
        {
            MasterVolumeSlider.Value = SettingsProfile.MasterVolume;
            musicVolumeSlider.Value = SettingsProfile.MusicVolume;
            FxVolumeSlider.Value = SettingsProfile.FxVolume;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void MasterVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SettingsProfile.MasterVolume = MasterVolumeSlider.Value;
            InitializeSliders();
        }

        private void musicVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SettingsProfile.MusicVolume = musicVolumeSlider.Value;
        }

        private void FxVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SettingsProfile.FxVolume = FxVolumeSlider.Value;
        }
    }
}
