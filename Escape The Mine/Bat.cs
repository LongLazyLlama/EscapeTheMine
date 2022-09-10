using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_The_Mine
{
    public class Bat : GameObject
    {
        public Bat(Texture2D texture, Vector2 destination, Vector2 assetLocation,
            int assetAnimationCount, Vector2 hitBoxOffset, Vector2 spriteRowsAndColumns, bool moving, int movement, SpriteEffects spriteEffect) :
            base(texture, destination, assetLocation, assetAnimationCount, hitBoxOffset, spriteRowsAndColumns, moving, movement, spriteEffect)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //Makes the animation faster.
            SpeedFrameCounter = 4;

            base.Update(gameTime);
        }
    }
}
