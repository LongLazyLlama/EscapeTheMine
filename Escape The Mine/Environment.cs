using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeoSketch;

namespace Escape_The_Mine
{
    public class Environment
    {
        private Texture2D _texture;

        private int _layerMovementOffset;
        public int LayerMovementOffset
        {
            get { return _layerMovementOffset; }
            set { _layerMovementOffset = value; }
        }

        private int _position;
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private int _size;
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Rectangle DestinationRectangle1;
        public Rectangle DestinationRectangle2;

        public Environment(Texture2D texture, int movementOffset)
        {
            _texture = texture;
            LayerMovementOffset = movementOffset;

            //Places target destination.
            DestinationRectangle1 = new Rectangle(0, 0, _texture.Width * GameSettings.MapSizeFactor, _texture.Height * GameSettings.MapSizeFactor);
            DestinationRectangle2 = DestinationRectangle1;
        }

        public void Update(GameTime gameTime)
        {
            //Updates the Movement of the Layers.
            UpdateBackgroundMovement();

        }
        private void UpdateBackgroundMovement()
        {
            //MovementSpeed.
            DestinationRectangle1.X -= (GameSettings.GameSpeed + LayerMovementOffset);

            //Places the second Rectangle after the first one.
            DestinationRectangle2.X = DestinationRectangle1.X + (_texture.Width * GameSettings.MapSizeFactor);

            //Checks if layer is out of the screen.
            if (DestinationRectangle1.X + (_texture.Width * GameSettings.MapSizeFactor) < 0)
            {
                DestinationRectangle1.X = 0;
            }
        }

        public void DrawEnvironment(SpriteBatch spriteBatch)
        {
            ////Draws the Layers Individually.
            //spriteBatch.DrawCircle(DestinationRectangle1.X, DestinationRectangle1.Y, 10, Color.Red, Color.Yellow, 3);
            spriteBatch.Draw(_texture, DestinationRectangle1, Color.White);
            spriteBatch.Draw(_texture, DestinationRectangle2, Color.White);
        }
    }
}
