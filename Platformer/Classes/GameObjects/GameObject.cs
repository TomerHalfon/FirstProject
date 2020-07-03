using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Platformer.Classes.GameObjects
{
    public class GameObject
    //an image and a position the minimum needed for an object to apper
    {
        //the image of the object itself
        public Image Image { get; set; } 
        
        //the object position in root canvas
        private Point _positionInCanvas;
        
        public GameObject()
        {
            Image = new Image();
            _positionInCanvas = new Point();
        }

        
        //get the position in the canvas (used to simplfy working with canvas)
        public Point GetPositionInCanvas() 
        {
            _positionInCanvas.Top = Canvas.GetTop(Image);
            _positionInCanvas.Left = Canvas.GetLeft(Image);

            return _positionInCanvas;
        }

        //set the position in the canvas
        public void SetPositionInCanvas(double left, double top) 
        {
            _positionInCanvas.Left = left;
            _positionInCanvas.Top = top;
            Canvas.SetTop(Image, top);
            Canvas.SetLeft(Image, left);
        }
        public void SetPositionInCanvas(Point newPosition)
        {
            _positionInCanvas = newPosition;
            Canvas.SetTop(Image, newPosition.Top);
            Canvas.SetLeft(Image, newPosition.Left);
        }

        // adds to the Left value of the object in the canvas
        protected void AddToLeftPosition(double left)
        {
            _positionInCanvas.Left += left;
            Canvas.SetLeft(Image, _positionInCanvas.Left);
        }
        
        //set the image source
        protected void SetImageSource(string source)
        {
            Image.Source = new BitmapImage(new Uri($"ms-appx://{source}"));
        }
        
    }
}
