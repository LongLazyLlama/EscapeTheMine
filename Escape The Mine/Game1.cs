using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Escape_The_Mine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameScreens startScreen;
        GameScreens gameOverScreen;

        GameScreens lightControlsButton;
        GameScreens darkControlsButton;

        GameScreens lightHighScoresButton;
        GameScreens darkHighScoresButton;

        GameScreens lightMenuButton;
        GameScreens darkMenuButton;

        GameScreens lightRetryButton;
        GameScreens darkRetryButton;

        GameScreens lightCrossButton;
        GameScreens darkCrossButton;

        GameScreens controlsScreen;
        GameScreens highScoreScreen;

        SoundEffect buttonPressed1;

        SoundEffect backgroundmusic1;

        Pickaxe pickaxe;
        Player player1;
        Spider spider;

        private SpriteFont _arial;

        private MouseState _currentMouseState, _previousMouseState;
        private KeyboardState _currentKeyboardState, _previousKeyboardState;

        private int scoreCount;
        private int record;

        private int spawnTime = 1;
        private int spawnCounter = 0;

        private int framecounter;

        private Random random = new Random();

        private List<Texture2D> _gameScreensList = new List<Texture2D>();
        private List<Texture2D> _textureList = new List<Texture2D>();
        private List<Environment> _environmentElements = new List<Environment>();
        private List<GameObject> _obstacles = new List<GameObject>();
        private List<Wall> _destructibleObstacles = new List<Wall>();

        private string gameState = "StartScreen";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            SetStandardGameSettings();
        }

        private void SetStandardGameSettings()
        {
            IsMouseVisible = true;
            
            graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = GameSettings.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameSettings.ScreenHeight;
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            UseStreamReader();

            base.Initialize();
        }

        private void UseStreamReader()
        {
            StreamReader sr = new StreamReader(@"HighScores.txt");

            record = int.Parse(sr.ReadLine());


            sr.Close();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Start, Settings and Endscreen + buttons.
            LoadScreens();
            LoadSpriteFont();

            //Environment Textures.
            LoadGameEnvironmentParts();

            //Assets.
            LoadGameAssets();

            //Soundeffects and music.
            LoadGameSounds();

            // TODO: use this.Content to load your game content here
            GenerateScreens();
            GenerateButtons();

            GenerateEnvironment();
            GenerateGameObjects();
        }

        private void LoadGameSounds()
        {
            buttonPressed1 = Content.Load<SoundEffect>("196979__peepholecircus__sci-fi-button-beep");

            backgroundmusic1 = Content.Load<SoundEffect>("196979__peepholecircus__sci-fi-button-beep");
            SoundEffectInstance backgroundInstance = backgroundmusic1.CreateInstance();

            backgroundInstance.IsLooped = true;
        }

        private void LoadSpriteFont()
        {
            _arial = (Content.Load<SpriteFont>("Arial"));

        }

        private void GenerateScreens()
        {
            startScreen = new GameScreens(_gameScreensList[0], new Vector2(0, 0));
            gameOverScreen = new GameScreens(_gameScreensList[10], new Vector2(0, 0));
            controlsScreen = new GameScreens(_gameScreensList[6], new Vector2(0, 0));
            highScoreScreen = new GameScreens(_gameScreensList[9], new Vector2(0, 0));
        }
        private void GenerateButtons()
        {
            lightControlsButton = new GameScreens(_gameScreensList[3],
                            new Vector2(10, (GameSettings.ScreenHeight - _gameScreensList[3].Height) - 30));

            darkControlsButton = new GameScreens(_gameScreensList[2],
                new Vector2(10, (GameSettings.ScreenHeight - _gameScreensList[2].Height) - 30));

            lightHighScoresButton = new GameScreens(_gameScreensList[4],
                new Vector2(((GameSettings.ScreenWidth - _gameScreensList[4].Width) - 30),
                (GameSettings.ScreenHeight - _gameScreensList[4].Height) - 30));

            darkHighScoresButton = new GameScreens(_gameScreensList[5],
                new Vector2(((GameSettings.ScreenWidth - _gameScreensList[5].Width) - 30),
                (GameSettings.ScreenHeight - _gameScreensList[5].Height) - 30));

            lightCrossButton = new GameScreens(_gameScreensList[8], new Vector2(30, 30));

            darkCrossButton = new GameScreens(_gameScreensList[7], new Vector2(30, 30));

            lightMenuButton = new GameScreens(_gameScreensList[14], new Vector2(GameSettings.ScreenWidth / 2
                + 50, GameSettings.ScreenHeight / 2 + 100));

            darkMenuButton = new GameScreens(_gameScreensList[13], new Vector2(GameSettings.ScreenWidth / 2
                + 50, GameSettings.ScreenHeight / 2 + 100));

            lightRetryButton = new GameScreens(_gameScreensList[12], new Vector2(GameSettings.ScreenWidth / 2
                - _gameScreensList[12].Width - 30, GameSettings.ScreenHeight / 2 + 100));

            darkRetryButton = new GameScreens(_gameScreensList[11], new Vector2(GameSettings.ScreenWidth / 2
                - _gameScreensList[11].Width - 30, GameSettings.ScreenHeight / 2 + 100));
        }

        private void LoadScreens()
        {
            //StartScreen and Buttons.
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_StartScreen"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_PressStart"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Controls_Button_dark"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Controls_Button_LightBlue"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_HighScoresButton_LightBlue"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_HighScoresButton_dark"));

            //ControlScreen.
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Controls_Screen"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_CrossButton_dark"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_CrossButton_LightBlue"));

            //HighScrore Screen.
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_HighScore_Screen"));

            //GameOverScreen and Buttons.
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_GameOver_Screen"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Retry_Button_dark"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Retry_Button_LightBlue"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Menu_Button_dark"));
            _gameScreensList.Add(Content.Load<Texture2D>("ETM_Menu_Button_LightBlue"));
        }
        private void LoadGameEnvironmentParts()
        {
            _textureList.Add(Content.Load<Texture2D>("ETM_Background"));
            _textureList.Add(Content.Load<Texture2D>("ETM_Second_Midground"));
            _textureList.Add(Content.Load<Texture2D>("ETM_First_Midground"));
            _textureList.Add(Content.Load<Texture2D>("ETM_Ground"));
            _textureList.Add(Content.Load<Texture2D>("ETM_ForeGround"));
        }
        private void LoadGameAssets()
        {
            _textureList.Add(Content.Load<Texture2D>("ETM_Spider_Obstacle"));
            _textureList.Add(Content.Load<Texture2D>("ETM_Wall_Obstacle"));
            _textureList.Add(Content.Load<Texture2D>("ETM_SpriteSheet"));
            _textureList.Add(Content.Load<Texture2D>("ETM_Numbers"));
            _textureList.Add(Content.Load<Texture2D>("ETM_Score+HP"));
        }
        private void GenerateGameObjects()
        {
            //Creates new units that stay in the game.
            player1 = new Player(_textureList[7], new Vector2(200, 308), new Vector2(0, 0), 4, new Vector2(0, 55),
                new Vector2(4, 4), false, 0, SpriteEffects.None);

            spider = new Spider(_textureList[5], new Vector2(-280, 308), new Vector2(0, 0), 4, new Vector2(50, 55),
                new Vector2(4, 1), false, 0, SpriteEffects.None);

            pickaxe = new Pickaxe(_textureList[7], new Vector2(200, 308), new Vector2(0, 2), 4, new Vector2(0, 0),
                new Vector2(4, 4), false, 0, SpriteEffects.None);
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        private void GenerateEnvironment()
        {
            Environment _backgroundElement;
            Environment _secondMidgroundElement;
            Environment _firstMidgroundElement;
            Environment _groundElement;
            Environment _foregroundElement;

            _backgroundElement = new Environment(_textureList[0], -5);
            _secondMidgroundElement = new Environment(_textureList[1], -3);
            _firstMidgroundElement = new Environment(_textureList[2], 0);
            _groundElement = new Environment(_textureList[3], 0);
            _foregroundElement = new Environment(_textureList[4], 2);

            _environmentElements.Add(_backgroundElement);
            _environmentElements.Add(_secondMidgroundElement);
            _environmentElements.Add(_firstMidgroundElement);
            _environmentElements.Add(_groundElement);
            _environmentElements.Add(_foregroundElement);
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here

            GetCurrentMouseState();
            GetCurrentKeyboardState();
            ChangeGameState();

            if (gameState == "StartScreen")
            {
                IsControlsClicked();
                IsHighScoresClicked();
            }

            if (gameState == "Controls")
            {
                IsCrossClicked();
            }

            if (gameState == "HighScores")
            {
                IsCrossClicked();
            }

            if (gameState == "InGame")
            {
                IsMouseVisible = false;

                IncreaseGameSpeed(gameTime);
                SpawnObstacle();

                b_a_t(keyboardState);

                UpdateEnvironment(gameTime);
                UpdateGameObjects(gameTime);
                UpdatePickaxe();
                UpdateCollision();
                UpdateScore(gameTime);
                UpdatePlayerHPStatus();

                if (player1.PlayerHP > 0)
                {
                    //When you die you'll be able to see what object killed you.
                    RemoveInactives();
                }
            }

            if (gameState == "GameOver")
            {
                IsMouseVisible = true;
                
                CalculateHighScore();
                UseStreamWriter();

                IsMenuButtonClicked();
                IsRetryButtonClicked();
            }

            SetPreviousKeyboardState();
            SetPreviousMouseState();

            base.Update(gameTime);
        }

        private void IsRetryButtonClicked()
        {
            if (IsButtonInRange(darkRetryButton.Position, 13) && IsLMBClicked())
            {
                backgroundmusic1.Play();

                ResetGameObjects();
                ResetEnvironment();
                ResetGaneSpeed();

                gameState = "InGame";
            }
        }
        private void IsMenuButtonClicked()
        {
            if (IsButtonInRange(darkMenuButton.Position, 11) && IsLMBClicked())
            {
                buttonPressed1.Play();
                player1 = new Player(_textureList[7], new Vector2(200, 308), new Vector2(0, 0), 4, new Vector2(0, 55), new Vector2(4, 4), false, 0, SpriteEffects.None);

                gameState = "StartScreen";
            }
        }

        private void b_a_t(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.B) && keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.T))
            {
                SpriteEffects flipSprite = SpriteEffects.FlipHorizontally;

                player1 = new Player(_textureList[7], new Vector2(200, 305), new Vector2(0, 1), 4, new Vector2(-10, -5), new Vector2(4, 4), false, 0, flipSprite);
                player1.Accelaration = -30;
            }
        }

        private void IsHighScoresClicked()
        {
            if (IsButtonInRange(darkHighScoresButton.Position, 5) && IsLMBClicked())
            {
                buttonPressed1.Play();
                gameState = "HighScores";
            }
        }
        private void IsCrossClicked()
        {
            if (IsButtonInRange(darkCrossButton.Position, 7) && IsLMBClicked())
            {
                buttonPressed1.Play();
                gameState = "StartScreen";
            }
        }
        private void IsControlsClicked()
        {
            if (IsButtonInRange(darkControlsButton.Position, 2) && IsLMBClicked())
            {
                buttonPressed1.Play();
                gameState = "Controls";
            }
        }
        private bool IsButtonInRange(Vector2 position, int textureNumber)
        {
            if ((Mouse.GetState().X > (int)position.X) && (Mouse.GetState().Y > (int)position.Y))
            {
                if ((Mouse.GetState().X < (int)(position.X + _gameScreensList[textureNumber].Width) && 
                    (Mouse.GetState().Y < (int)(position.Y + _gameScreensList[textureNumber].Height))))
                {
                    return true;
                }
            }
            return false;
        }

        private void IncreaseGameSpeed(GameTime gameTime)
        {
            framecounter++;
            if (framecounter >= 300)
            {
                GameSettings.GameSpeed += 1;
                framecounter = 0;
            }
        }
        private void UpdatePlayerHPStatus()
        {
            if (player1.PlayerHP == 0)
            {
                backgroundmusic1.Play(1.0f, 0.1f, 0);
                gameState = "GameOver";
            }
        }

        private void UseStreamWriter()
        {
            using (StreamWriter sw = new StreamWriter(@"HighScores.txt"))
            {
                sw.WriteLine($"{record}");

                sw.Close();
            }
        }

        private void CalculateHighScore()
        {
            if(record < scoreCount)
            {
                record = scoreCount;
            }
        }
        private void UpdateScore(GameTime gameTime)
        {
            scoreCount += (int)(gameTime.ElapsedGameTime.TotalMilliseconds/5);
        }

        private void SetPreviousKeyboardState()
        {
            _previousKeyboardState = _currentKeyboardState;
        }
        private void GetCurrentKeyboardState()
        {
            _currentKeyboardState = Keyboard.GetState();
        }

        private void ChangeGameState()
        {
            if (gameState == "StartScreen" && IsEnterPressed() == true || gameState == "GameOver" && IsEnterPressed() == true)
            {
                buttonPressed1.Play(1.0f, 0.8f, 0);

                ResetGameObjects();
                ResetEnvironment();
                ResetGaneSpeed();

                backgroundmusic1.Play();

                gameState = "InGame";
            }

            if (gameState == "Controls" && IsBackSpacePressed() == true)
            {
                gameState = "StartScreen";
            }

            if (gameState == "HighScores" && IsBackSpacePressed() == true)
            {
                gameState = "StartScreen";
            }

            if (gameState == "GameOver" && IsBackSpacePressed() == true)
            {
                gameState = "StartScreen";
            }

            if (gameState == "InGame" && IsBackSpacePressed() == true)
            {
                gameState = "GameOver";
            }

        }

        private void ResetGaneSpeed()
        {
            GameSettings.GameSpeed = 9;
        }
        private void ResetEnvironment()
        {
            for (int i = 0; i < _environmentElements.Count; i++)
            {
                _environmentElements[i].DestinationRectangle1.X = 0;
            }
        }
        private void ResetGameObjects()
        {
            _obstacles.Clear();
            _destructibleObstacles.Clear();

            player1.IsJumping = false;
            player1.Accelaration = -30;
            player1.Destination = new Vector2(200, 308);

            pickaxe.ThrowPickaxe = false;

            scoreCount = 0;
            player1.PlayerHP = 3;
        }

        private bool IsBackSpacePressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Back) && _previousKeyboardState.IsKeyUp(Keys.Back))
                return true;
            return false;
        }
        private bool IsEnterPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                return true;
            return false;
        }

        private void SetPreviousMouseState()
        {
            _previousMouseState = _currentMouseState;
        }
        private void GetCurrentMouseState()
        {
            _currentMouseState = Mouse.GetState();
        }
        private bool IsLMBClicked()
        {
            if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        private void SpawnObstacle()
        {
            spawnCounter++;
            if (spawnTime == spawnCounter)
            {
                int temp = random.Next(1, 4);

                //Spawns a new bat.
                if (temp == 1)
                {
                    Bat bat = (new Bat(_textureList[7], new Vector2(1800, random.Next(80, 300)), new Vector2(0, 1), 4,
                        new Vector2(5, 0), new Vector2(4, 4), true, 5, SpriteEffects.None));
                    _obstacles.Add(bat);

                    spawnTime = random.Next(100, 180);
                }
                //Spawns a new wall.
                if (temp == 2)
                {
                    Wall wall = (new Wall(_textureList[6], new Vector2(1800, 88), new Vector2(0, 0),
                        4, new Vector2(5, 110), new Vector2(4, 1), true, 0, SpriteEffects.None));
                    _destructibleObstacles.Add(wall);

                    spawnTime = random.Next(40, 110);
                }
                //Spawns a new minecart.
                if (temp == 3)
                {
                    MineCart mineCart = (new MineCart(_textureList[7], new Vector2(1800, 308), new Vector2(0, 3),
                        0, new Vector2(35, 55), new Vector2(4, 4), true, 0, SpriteEffects.None));
                    _obstacles.Add(mineCart);

                    spawnTime = random.Next(100, 180);
                }

                //Changes the spawntime of obstacles.
                //y = 800 standard.
                spawnCounter = 0;
            }
        }
        private void RemoveInactives()
        {
            //Removes inactive gameobjects.
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_obstacles[i].IsActive == false)
                {
                     _obstacles.Remove(_obstacles[i]);
                }
            }

            for (int i = 0; i < _destructibleObstacles.Count; i++)
            {
                if (_destructibleObstacles[i].IsActive == false)
                {
                    _destructibleObstacles.Remove(_destructibleObstacles[i]);
                }
            }
        }

        private void UpdatePickaxe()
        {
            if (IsLMBClicked() && pickaxe.ThrowPickaxe == false)
            {
                pickaxe.Destination.Y = player1.Destination.Y;
                pickaxe.ThrowPickaxe = true;
            } 
            else if (pickaxe.ThrowPickaxe == false || pickaxe.Destination.X > (GameSettings.ScreenWidth + GameSettings.TextureWidth))
            {
                pickaxe.Destination = player1.Destination;
                pickaxe.AssetLocationNumber = new Vector2(0, 2);
            }
        }
        private void UpdateCollision()
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_obstacles[i].IsColldingWith(player1))
                {
                    _obstacles[i].IsActive = false;
                    player1.ReduceHP();
                }

                if (_obstacles[i] .IsColldingWith(pickaxe))
                {
                    pickaxe.ThrowPickaxe = false;
                    pickaxe.AssetLocationNumber = new Vector2(0, 2);
                    pickaxe.Destination = player1.Destination;
                }
            }

            for (int i = 0; i < _destructibleObstacles.Count; i++)
            {
                if (_destructibleObstacles[i].IsColldingWith(player1))
                {
                    _destructibleObstacles[i].IsActive = false;
                    player1.ReduceHP();
                }

                if (_destructibleObstacles[i].IsColldingWith(pickaxe))
                {
                    _destructibleObstacles[i].IsActive = false;
                    pickaxe.ThrowPickaxe = false;
                }
            }
        }
        private void UpdateEnvironment(GameTime gameTime)
        {
            foreach (Environment env in _environmentElements)
            {
                env.Update(gameTime);
            }
        }
        private void UpdateGameObjects(GameTime gameTime)
        {
            player1.Update(gameTime);
            spider.Update(gameTime);
            pickaxe.Update(gameTime);

            for (int i = 0; i < _obstacles.Count; i++)
            {
                _obstacles[i].Update(gameTime);
            }
            for (int i = 0; i < _destructibleObstacles.Count; i++)
            {
                _destructibleObstacles[i].Update(gameTime);
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            if (gameState == "StartScreen")
            {
                DrawStartScreen();
            }

            if (gameState == "Controls")
            {
                DrawControlScreen();
            }

            if (gameState == "HighScores")
            {
                DrawHighScoresScreen();
            }

            if (gameState == "InGame")
            {
                DrawEnvironments(spriteBatch);
                DrawObstacles();
                DrawScore();
                DrawPlayerLives();
            }

            if (gameState == "GameOver")
            {
                DrawEnvironments(spriteBatch);
                DrawObstacles();
                DrawGameOverScreen();
                DrawEndScores();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawHighScoresScreen()
        {
            highScoreScreen.Draw(spriteBatch);
            if (IsButtonInRange(darkCrossButton.Position, 7))
            {
                lightCrossButton.Draw(spriteBatch);
            }
            else
                darkCrossButton.Draw(spriteBatch);

            spriteBatch.DrawString(_arial, $"{record}", new Vector2(GameSettings.ScreenWidth / 2
                - _arial.MeasureString($"{record}").X / 2, GameSettings.ScreenHeight / 2 + 5), Color.CornflowerBlue);
        }
        private void DrawControlScreen()
        {
            controlsScreen.Draw(spriteBatch);

            if (IsButtonInRange(darkCrossButton.Position, 7))
            {
                lightCrossButton.Draw(spriteBatch);
            }
            else
                darkCrossButton.Draw(spriteBatch);
        }
        private void DrawPlayerLives()
        {
            spriteBatch.DrawString(_arial, "Lives " + player1.PlayerHP, new Vector2(GameSettings.ScreenWidth - GameSettings.ScreenWidth / 5,
                                   GameSettings.ScreenHeight / 25), Color.CornflowerBlue);
        }
        private void DrawEndScores()
        {
            spriteBatch.DrawString(_arial, "Score " + scoreCount, new Vector2(GameSettings.ScreenWidth / 2
                                - _arial.MeasureString("Score" + scoreCount).X / 2, GameSettings.ScreenHeight / 2 - 64), Color.CornflowerBlue);
            spriteBatch.DrawString(_arial, "Record " + record, new Vector2(GameSettings.ScreenWidth / 2
                - _arial.MeasureString("Record" + record).X / 2, GameSettings.ScreenHeight / 2 + 5), Color.CornflowerBlue);
        }
        private void DrawScore()
        {
            spriteBatch.DrawString(_arial, "Score " + scoreCount, new Vector2(GameSettings.ScreenWidth / 15,
                                GameSettings.ScreenHeight / 25), Color.CornflowerBlue);
        }
        private void DrawStartScreen()
        {
            startScreen.Draw(spriteBatch);

            if (IsButtonInRange(darkControlsButton.Position, 2))
            {
                lightControlsButton.Draw(spriteBatch);
            }
            else
                darkControlsButton.Draw(spriteBatch);

            if (IsButtonInRange(darkHighScoresButton.Position, 4))
            {
                lightHighScoresButton.Draw(spriteBatch);
            }
            else
                darkHighScoresButton.Draw(spriteBatch);
        }
        private void DrawGameOverScreen()
        {
            gameOverScreen.Draw(spriteBatch);

            if (IsButtonInRange(darkMenuButton.Position, 11))
            {
                lightMenuButton.Draw(spriteBatch);
            }
            else
                darkMenuButton.Draw(spriteBatch);

            if (IsButtonInRange(lightRetryButton.Position, 13))
            {
                lightRetryButton.Draw(spriteBatch);
            }
            else
                darkRetryButton.Draw(spriteBatch);
        }
        private void DrawObstacles()
        {
            player1.Draw(spriteBatch);
            spider.Draw(spriteBatch);
            pickaxe.Draw(spriteBatch);

            foreach (GameObject gameObject in _obstacles)
            {
                gameObject.Draw(spriteBatch);
            }

            foreach (GameObject gameObject in _destructibleObstacles)
            {
                gameObject.Draw(spriteBatch);
            }
        }
        private void DrawEnvironments(SpriteBatch spriteBatch)
        {
            foreach (Environment env in _environmentElements)
            {
                env.DrawEnvironment(spriteBatch);
            }
        }
    }
}