using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_The_Mine
{
    class GameScreens
    {
        private Texture2D _texture;

        public Vector2 Position;

        public GameScreens(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height), null, Color.White);
        }
    }
}
