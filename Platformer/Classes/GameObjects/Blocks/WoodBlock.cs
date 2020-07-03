using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Classes.GameObjects.Blocks
{
   public class WoodBlock:Block
    {
        //path to source
        const string _path = "/Assets/Tertrain/wood.jpg";
        public WoodBlock(double width,double height,Point position):base(_path,width, height,position)
        {
        }
    }
}
