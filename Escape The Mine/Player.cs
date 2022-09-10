using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeoSketch;
using Microsoft.Xna.Framework.Input;

namespace Escape_The_Mine
{
    public class Player : GameObject
    {
        private int InAirDuration = 30;

        private int _accelaration;
        public int Accelaration
        {
            get { return _accelaration; }
            set { _accelaration = value; }
        }

        private int _playerHP = 42069;
        public int PlayerHP
        {
            get { return _playerHP; }
            set { _playerHP = value; }
        }

        public bool IsJumping = false;

        private KeyboardState _currentKeyboardState;

        public Player(Texture2D texture, Vector2 destination, Vector2 assetLocation,
            int assetAnimationCount, Vector2 hitBoxOffset, Vector2 spriteRowsAndColumns, bool moving,
            int movement, SpriteEffects spriteEffect) :
            base(texture, destination, assetLocation, assetAnimationCount, hitBoxOffset, spriteRowsAndColumns, moving, movement, spriteEffect)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            //checks if Player Jumps.
            IsPlayerJumping();

            //Changes the animation speed.
            SpeedFrameCounter = 6;

            base.Update(gameTime);
        }

        private void IsPlayerJumping()
        {
            if (IsSpacePressed())
            {
                IsJumping = true;
            }
            UpdateGravitation();
        }
        private void UpdateGravitation()
        {
            if (IsJumping)
            {
                if (_accelaration <= InAirDuration && IsJumping)
                {
                    //adds the decreasing gravity.
                    Destination.Y += _accelaration / 1.5f;
                    _accelaration++;

                    if (_accelaration == InAirDuration)
                    {
                        //Reverses gravity.
                        IsJumping = false;
                        _accelaration = -30;
                        Destination.Y = 308;
                    }
                }
            }
        }
        private bool IsSpacePressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Space))
                return true;
            return false;
        }

        public void ReduceHP()
        {
            _playerHP--;
        }
    }
}
