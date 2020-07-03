using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes
{
    public class SettingsProfile
    {
        public double MasterVolume
        { 
            get
            { 
                return _masterVolume;
            } 
            set 
            { 
                _masterVolume = value;
                MusicVolume = value;
                FxVolume = value ;
            } 
        }
        public double MusicVolume { get; set; }
        public double FxVolume { get; set; }

        private double _masterVolume;

        public SettingsProfile(double masterVolume =0.2 , double musicVolume = 0.2, double fxVolume = 0.2)
        {
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            FxVolume = fxVolume;
        }
    }
}
