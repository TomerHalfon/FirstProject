using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes
{
   public class PlayerScoreKeeper
    {
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
        public PlayerScoreKeeper(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }
    }
}
