using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects.FixedObjects
{
    public abstract class FixedObject : GameObject
    {
        //force all of the fixed objects to have an action 
        public abstract void Action(Player player, Map map);
    }
}
