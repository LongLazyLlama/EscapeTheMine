using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Escape_The_Mine
{
    class Pickaxe : Player
    {
        public bool ThrowPickaxe;

        public Pickaxe(Texture2D texture, Vector2 destination, Vector2 assetLocation,
            int assetAnimationCount, Vector2 hitBoxOffset, Vector2 spriteRowsAndColumns, bool moving,
            int movement, SpriteEffects spriteEffect) :
            base(texture, destination, assetLocation, assetAnimationCount, hitBoxOffset, spriteRowsAndColumns, moving, movement, spriteEffect)
        {
            MovementOffset = 3;
        }

        public override void Update(GameTime gameTime)
        {
            GetCenterPosition();

            //Activates the pickaxe animation when thrown.
            if (ThrowPickaxe)
            {
                SpeedFrameCounter = 4;
                Destination.X += GameSettings.GameSpeed + MovementOffset;
                UpdateSpriteAnimation();
            }

            //Returns the pickaxe to the players hand when out of screen.
            if (IsOutOfScreen())
            {
                ThrowPickaxe = false;
            }
        }
    }
}
