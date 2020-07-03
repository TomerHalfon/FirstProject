using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Platformer.Classes.GameObjects.Blocks
{
   public abstract class Block : GameObject
    {
        public Block(string imgSource,double width, double height ,Point position):base()
        {
            //set the source
            SetImageSource(imgSource);
            //set the position
            SetPositionInCanvas(position);
            //strech
            Image.Stretch = Stretch.Fill;
            //define size
            Image.Width = width;
            Image.Height = height;
        }
    }
}
