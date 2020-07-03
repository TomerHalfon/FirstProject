using Platformer.Classes.GameObjects;
using Platformer.Classes.GameObjects.Blocks;
using Platformer.Classes.GameObjects.CollidableObjects.MoveableObjects;
using Platformer.Classes.GameObjects.CollidableObjects.Projectiles;
using Platformer.Classes.GameObjects.FixedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Platformer.Classes.GameObjects.CollidableObjects
{
    //objects that will calculate the collision with the world and with other objects
    public abstract class CollidableObj : GameObject
    {

        public bool IsCollidingWithMap(List<Block> blocks ,Point newPosition)
         //checks collision with map
        {
            foreach (Block block in blocks)
            {
                bool isColliding = AreImagesColliding(Image, newPosition.Top, newPosition.Left, block.Image);
                if (isColliding) return isColliding;
            }
            return false;
        }

        public bool IsCollidingWithFixedObject(List<FixedObject> fixedObjects, Point newPosition, out FixedObject colidedObject)
        //checks collixion with fixed objects and outputs the spesific object it collided with gives null if there is no collision

        {
            foreach (FixedObject fixedObject in fixedObjects)
            {
                    bool isColliding = AreImagesColliding(Image, newPosition.Top, newPosition.Left, fixedObject.Image);
                    if (isColliding)
                    {
                        colidedObject = fixedObject;
                        return true; 
                    }
            }
            colidedObject = null;
            return false;
        }

        public bool IsCollidingWithProjectile(List<Projectile> Projectiles, Point newPosition, out Projectile CollisionShot)
            //checks collision with shots and ouputs the shot or null if no collision
        {
            foreach (Projectile projectile in Projectiles)
            {
                bool isColliding = AreImagesColliding(Image, newPosition.Top, newPosition.Left, projectile.Image);
                if (isColliding)
                {
                    CollisionShot = projectile;
                    return true;
                }
            }
            CollisionShot = null;
            return false;
        }
        public bool IsCollidingWithBoss(Boss boss, Point newPosition)
        {
            bool isColliding = AreImagesColliding(Image, newPosition.Top, newPosition.Left, boss.Image);

            return isColliding;
        }

        private bool AreImagesColliding(Image movingImage, double movingImageNewPositionTop, double movingImageNewPositionLeft, Image fixedImage)
            //the collision detection method

        {
            //Moving image var's
            double movingImageNewRight = movingImageNewPositionLeft + movingImage.ActualWidth;
            double movingImageNewBottom = movingImageNewPositionTop + movingImage.ActualHeight;

            //fixed image var's
            double fixedImageLeft = Canvas.GetLeft(fixedImage);
            double fixedImageTop = Canvas.GetTop(fixedImage);

            double fixedImageRight = fixedImageLeft + fixedImage.ActualWidth;
            double fixedImageBottom = fixedImageTop + fixedImage.ActualHeight;

            //checking if the moving image is about to enter the fixed image
            #region checks
            //check top right
            if (movingImageNewRight >= fixedImageLeft &&
                movingImageNewRight <= fixedImageRight &&
                movingImageNewPositionTop <= fixedImageBottom &&
                movingImageNewPositionTop >= fixedImageTop)
            {
                return true;
            }

            //check top left
            if (movingImageNewPositionLeft >= fixedImageLeft &&
                movingImageNewPositionLeft <= fixedImageRight &&
                movingImageNewPositionTop <= fixedImageBottom &&
                movingImageNewPositionTop >= fixedImageTop)
            {
                return true;
            }

            //check bottom right
            if (movingImageNewRight >= fixedImageLeft &&
                movingImageNewRight <= fixedImageRight &&
                movingImageNewBottom >= fixedImageTop &&
                movingImageNewBottom <= fixedImageBottom)
            {
                return true;
            }

            //check bottom left
            if (movingImageNewPositionLeft >= fixedImageLeft &&
                movingImageNewPositionLeft <= fixedImageRight &&
                movingImageNewBottom >= fixedImageTop &&
                movingImageNewBottom <= fixedImageBottom)
            {
                return true;
            }
            #endregion
            //checking if the fixed image is about to enter the moving image
            #region checks
            //check top right
            if (fixedImageRight >= movingImageNewPositionLeft &&
                fixedImageRight <= movingImageNewRight &&
                fixedImageTop >= movingImageNewPositionTop &&
                fixedImageTop <= movingImageNewBottom)
            {
                return true;
            }

            //check top left
            if (fixedImageLeft >= movingImageNewPositionLeft &&
                fixedImageLeft <= movingImageNewRight &&
                fixedImageTop >= movingImageNewPositionTop &&
                fixedImageTop <= movingImageNewBottom)
            {
                return true;
            }

            //check bottom right
            if (fixedImageRight >= movingImageNewPositionLeft &&
                fixedImageRight <= movingImageNewRight &&
                fixedImageBottom >= movingImageNewPositionTop &&
                fixedImageBottom <= movingImageNewBottom)
            {
                return true;
            }

            //check bottom left
            if (fixedImageLeft >= movingImageNewPositionLeft &&
                fixedImageLeft <= movingImageNewRight &&
                fixedImageBottom >= movingImageNewPositionTop &&
                fixedImageBottom <= movingImageNewBottom)
            {
                return true;
            }
            #endregion
            return false;
        }
    }
}
