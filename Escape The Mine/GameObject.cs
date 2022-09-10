using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSketch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_The_Mine
{
    public class GameObject
    {
        private Texture2D _texture;

        private SpriteEffects _effect;

        public bool IsActive = true;
        public bool IsMoving = true;

        private int _frameCounter;
        public int FrameCounter
        {
            get { return _frameCounter; }
            set { _frameCounter = value; }
        }

        private int _speedFrameCounter;
        public int SpeedFrameCounter
        {
            get { return _speedFrameCounter; }
            set { _speedFrameCounter = value; }
        }

        private int _movement;
        public int MovementOffset
        {
            get { return _movement; }
            set { _movement = value; }
        }

        private int _spriteWidth;
        private int _spriteHeight;
        private Vector2 _spriteRowsAndColumns;

        private int _assetAnimationCount;  //Number of sprites an object or character -1 (for looping the animation).
        private Vector2 _assetLocationNumber; //Location of the needed sprite's place on the spritesheet (in rows and columns).
        public Vector2 AssetLocationNumber
        {
            get { return _assetLocationNumber; }
            set { _assetLocationNumber = value; }
        }

        private Vector2 _spriteOrigin;

        private Vector2 _hitBoxOffset;
        public Vector2 Destination;

        //private Vector2 HitBoxDestination;

        private Vector2 centerDestination;
        private Vector2 centerSize;

        public GameObject(Texture2D texture, Vector2 destination,
            Vector2 assetLocation, int assetAnimationCount, Vector2 hitBoxOffset,
            Vector2 spriteRowsAndColumns, bool moving, int movement, SpriteEffects spriteEffect)
        {
            _texture = texture;
            Destination = destination;
            AssetLocationNumber = assetLocation;
            _assetAnimationCount = assetAnimationCount;
            _hitBoxOffset = hitBoxOffset;
            _spriteRowsAndColumns = spriteRowsAndColumns;
            MovementOffset = movement;
            _effect = spriteEffect;
            IsMoving = moving;

            //Holds the original location of the first sprite to be able to replay the animation from there.
            _spriteOrigin = AssetLocationNumber;

            GetSourceRectangle();
        }

        private Rectangle HitBoxRectangle
        {
            get
            {
                int leftTopXFromCenter = (int)centerDestination.X - (int)centerSize.X / 2;
                int leftTopYFromCenter = (int)centerDestination.Y - (int)centerSize.Y / 2;

                return new Rectangle(
                    leftTopXFromCenter - (int)_hitBoxOffset.X,
                    leftTopYFromCenter,
                    (int)centerSize.X + ((int)_hitBoxOffset.X * 2),
                    (int)centerSize.Y + (int)_hitBoxOffset.Y);
            }
        }

        private Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle((int)Destination.X, (int)Destination.Y, _spriteWidth, _spriteHeight);
            }
        }

        public void GetCenterPosition()
        {
            centerDestination = new Vector2(Destination.X + (_spriteWidth / 2), Destination.Y + (_spriteHeight / 2));
            centerSize = new Vector2(_spriteWidth / 2, _spriteHeight / 2);
        }

        private Rectangle GetSourceRectangle()
        {
            int x = (int)AssetLocationNumber.X * GetSpriteWidth();
            int y = (int)AssetLocationNumber.Y * GetSpriteHeight();
            return new Rectangle(x, y, GetSpriteWidth(), GetSpriteHeight());
        }

        public virtual void Update(GameTime gameTime)
        {
            GetCenterPosition();

            UpdateSpriteAnimation();

            if (IsMoving)
            {
                UpdateMovement();
            }

            IsOutOfScreen();
        }
        private void UpdateMovement()
        {
            Destination.X -= MovementOffset + GameSettings.GameSpeed;
        }
        public void UpdateSpriteAnimation()
        {
            if (_assetAnimationCount > 0)
            {
                _frameCounter++;
                if (_frameCounter >= _speedFrameCounter)
                {
                    _frameCounter = 0;
                    //AssetLocationNumber.X += 1;
                    AssetLocationNumber = new Vector2(AssetLocationNumber.X + 1, AssetLocationNumber.Y);
                    if (AssetLocationNumber.X >= _assetAnimationCount)
                        AssetLocationNumber = _spriteOrigin;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, DestinationRectangle, GetSourceRectangle(), Color.White, 0.0f, new Vector2(0,0), _effect, 0.0f);

            ////Show spriteRectangle.
            //spriteBatch.DrawRectangle(DestinationRectangle.X,
            //DestinationRectangle.Y, DestinationRectangle.Width,
            //    DestinationRectangle.Height,
            //    Color.Transparent,
            //    Color.Yellow,
            //    4);

            ////Show Hitbox.
            //spriteBatch.DrawRectangle(HitBoxRectangle.X,
            //HitBoxRectangle.Y, HitBoxRectangle.Width,
            //    HitBoxRectangle.Height,
            //    Color.Transparent,
            //    Color.Red,
            //    4);
        }

        private int GetSpriteWidth()
        {
            return _spriteWidth = _texture.Width / (int)_spriteRowsAndColumns.X;
        }
        private int GetSpriteHeight()
        {
            return _spriteHeight = _texture.Height  / (int)_spriteRowsAndColumns.Y;
        }

        public bool IsColldingWith(GameObject anotherGameObject)
        {
            //Checks collision by hitbox.
            if (HitBoxRectangle.Intersects(anotherGameObject.HitBoxRectangle)
                && IsActive == true && anotherGameObject.IsActive == true)
            {
                return true;
            }
            return false;
        }
        public bool IsOutOfScreen()
        {
            if (Destination.X + _spriteWidth < 0 || Destination.X > (GameSettings.ScreenWidth + _spriteWidth))
            {
                return true;
            }
            return false;
        }
    }
}
