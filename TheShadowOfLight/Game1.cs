using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Xml;
using TheShadowOfLight.Sprites;
using static System.Net.Mime.MediaTypeNames;

namespace TheShadowOfLight
{
    public class Game1 : Game
    {
        enum GameState
        {
            Intro,
            Menu,
            Options,
            Gameplay,
            Death,
            Win
        }

        GameState state;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont font;

        Song menuMusic;
        Song runningMusic;
        Song gameplayMusic;
        Song winMusic;
        Song deathMusic;

        SoundEffect apSound;
        SoundEffect coinSound;
        SoundEffect doorSound;
        SoundEffect monsterStepsSound;
        SoundEffect roarSound;
        SoundEffect winSound;
        SoundEffect amamSound;

        Rectangle newGameButtonRect, settingsButtonRect, exitButtonRect, backToMenuButtonRect, lightButtonRect, standartButtonRect, shadowButtonRect;
        Rectangle musicControllerRect, soundControllerRect;
        bool newGameButtonHovered, settingsButtonHovered, exitButtonHovered, backToMenuButtonHovered, musicControllerHovered, soundControllerHovered, lightButtonHovered, standartButtonHovered, shadowButtonHovered;
        bool gameStarted = false;

        float musicVolume = 1f;
        float soundVolume = 1f;

        int difficult = 1;
        int currentIntroFrame = 1;
        int currentRoom;
        KeyboardState prevKeyboardState;

        bool playerNearExit = false;
        bool playerNearFirstDoor = false;
        bool playerNearSecondDoor = false;
        bool playerNearRightDoor = false;
        bool playerNearLeftDoor = false;

        bool playerNearCoin = false;
        bool playerNearVendingAp = false;
        bool playerNearMiska = false;
        bool playerNearKey = false;

        bool playerHasCoin = false;
        bool playerHasSnickers = false;
        bool playerHasKey = false;
        bool playerDroppedSnickers = false;
        bool playerDroppedSnickersAndLeft = false;

        bool monsterLeft = false;

        Texture2D musicControllerTextureWhite, musicControllerTextureBlack;

        Player playerSprite;
        Flashlight flashlight;
        Flashlight flashlightHard;
        Sprite room1;
        Sprite room2;
        Sprite room3;
        Sprite room32;
        Sprite room33;
        Sprite main;
        Sprite options;
        Sprite death;
        Sprite e;
        Sprite key;
        Sprite coin;
        Sprite snickers;

        Sprite intro1;
        Sprite intro2;
        Sprite intro3;
        Sprite intro4;

        Sprite win2;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            state = GameState.Menu;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font/myfont");

            musicControllerTextureWhite = Content.Load<Texture2D>("Sprites/whitePol");
            musicControllerTextureBlack = Content.Load<Texture2D>("Sprites/blackPol");

            musicControllerRect = new Rectangle((GraphicsDevice.Viewport.Width - musicControllerTextureWhite.Width) / 2, 450, musicControllerTextureWhite.Width, musicControllerTextureWhite.Height);
            soundControllerRect = new Rectangle((GraphicsDevice.Viewport.Width - musicControllerTextureWhite.Width) / 2, 600, musicControllerTextureWhite.Width, musicControllerTextureWhite.Height);

            Texture2D playerTexture = Content.Load<Texture2D>("Sprites/player");
            playerSprite = new Player(playerTexture, new Vector2(1000, 0));

            Texture2D flashlightTexture = Content.Load<Texture2D>("Sprites/flashlight");
            flashlight = new Flashlight(flashlightTexture, new Vector2(0, 0));
            Texture2D flashlightHardTexture = Content.Load<Texture2D>("Sprites/flashlightHard");
            flashlightHard = new Flashlight(flashlightHardTexture, new Vector2(0, 0));

            Texture2D room1Texture = Content.Load<Texture2D>("Backgrounds/room1");
            room1 = new Sprite(room1Texture, new Vector2(0, 0));

            Texture2D room2Texture = Content.Load<Texture2D>("Backgrounds/room2");
            room2 = new Sprite(room2Texture, new Vector2(0, 0));

            Texture2D room3Texture = Content.Load<Texture2D>("Backgrounds/room3-1");
            Texture2D room32Texture = Content.Load<Texture2D>("Backgrounds/room3-2");
            Texture2D room33Texture = Content.Load<Texture2D>("Backgrounds/room3-3");
            room3 = new Sprite(room3Texture, new Vector2(0, 0));
            room32 = new Sprite(room32Texture, new Vector2(0, 0));
            room33 = new Sprite(room33Texture, new Vector2(0, 0));
            
            Texture2D mainTexture = Content.Load<Texture2D>("Backgrounds/main_background");
            main = new Sprite(mainTexture, new Vector2(0, 0));

            Texture2D deathTexture = Content.Load<Texture2D>("Backgrounds/end_background");
            death = new Sprite(deathTexture, new Vector2(0, 0));

            Texture2D optionsTexture = Content.Load<Texture2D>("Backgrounds/options_background");
            options = new Sprite(optionsTexture, new Vector2(0, 0));

            Texture2D eTexture = Content.Load<Texture2D>("Sprites/E");
            e = new Sprite(eTexture, new Vector2(0, 0));

            Texture2D coinTexture = Content.Load<Texture2D>("Sprites/coin");
            coin = new Sprite(coinTexture, new Vector2(900, 565));

            Texture2D keyTexture = Content.Load<Texture2D>("Sprites/key");
            key = new Sprite(keyTexture, new Vector2(1800, 50));

            Texture2D snickersTexture = Content.Load<Texture2D>("Sprites/snickers");
            snickers = new Sprite(snickersTexture, new Vector2(1700, 50));

            Texture2D intro1Texture = Content.Load<Texture2D>("Backgrounds/1");
            intro1 = new Sprite(intro1Texture, new Vector2(0, 0));

            Texture2D intro2Texture = Content.Load<Texture2D>("Backgrounds/2");
            intro2 = new Sprite(intro2Texture, new Vector2(0, 0));

            Texture2D intro3Texture = Content.Load<Texture2D>("Backgrounds/3");
            intro3 = new Sprite(intro3Texture, new Vector2(0, 0));

            Texture2D intro4Texture = Content.Load<Texture2D>("Backgrounds/4");
            intro4 = new Sprite(intro4Texture, new Vector2(0, 0));

            Texture2D fin2Texture = Content.Load<Texture2D>("Backgrounds/fin2");
            win2 = new Sprite(fin2Texture, new Vector2(0, 0));

            menuMusic = Content.Load<Song>("Audio/main");

            MediaPlayer.Play(menuMusic);
            MediaPlayer.IsRepeating = true;

            runningMusic = Content.Load<Song>("Audio/run");
            gameplayMusic = Content.Load<Song>("Audio/gameplay");
            winMusic = Content.Load<Song>("Audio/win");
            deathMusic = Content.Load<Song>("Audio/deathMusic");

            apSound = Content.Load<SoundEffect>("Audio/ApSound");
            coinSound = Content.Load<SoundEffect>("Audio/coinSound");
            doorSound = Content.Load<SoundEffect>("Audio/doorSound");
            monsterStepsSound = Content.Load<SoundEffect>("Audio/monsterSteps");
            roarSound = Content.Load<SoundEffect>("Audio/RoarSound");
            winSound = Content.Load<SoundEffect>("Audio/winSound");
            amamSound = Content.Load<SoundEffect>("Audio/amam");

            newGameButtonRect = new Rectangle(145, 500, 130, 50);
            settingsButtonRect = new Rectangle(145, 600, 115, 50);
            exitButtonRect = new Rectangle(145, 700, 60, 50);
            backToMenuButtonRect = new Rectangle(200, 880, 200, 50);
            lightButtonRect = new Rectangle(250, 770, 200, 50);
            standartButtonRect = new Rectangle(750, 770, 200, 50);
            shadowButtonRect = new Rectangle(1250, 770, 200, 50);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.Menu:
                    UpdateMenu(gameTime);
                    break;
                case GameState.Options:
                    UpdateOptions(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.Death:
                    UpdateDeath(gameTime);
                    break;
                case GameState.Win:
                    UpdateWin(gameTime);
                    break;
                case GameState.Intro:
                    UpdateIntro(gameTime);
                    break;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                state = GameState.Options;
            }
                // TODO: Add your update logic here
                base.Update(gameTime);

        }

        private void UpdateMenu(GameTime deltaTime)
        {
            IsMouseVisible = true;
            MouseState mouseState = Mouse.GetState();
            newGameButtonHovered = newGameButtonRect.Contains(mouseState.Position);
            settingsButtonHovered = settingsButtonRect.Contains(mouseState.Position);
            exitButtonHovered = exitButtonRect.Contains(mouseState.Position);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (newGameButtonHovered)
                {
                    IsMouseVisible = false;
                    StartNewGame();
                }
                else if (settingsButtonHovered)
                {
                    state = GameState.Options;
                }
                else if (exitButtonHovered)
                {
                    Exit();
                }
            }
        }

        private void UpdateOptions(GameTime deltaTime)
        {
            MediaPlayer.Pause();
            IsMouseVisible = true;
            MouseState mouseState = Mouse.GetState();
            musicControllerHovered = musicControllerRect.Contains(mouseState.Position);
            soundControllerHovered = soundControllerRect.Contains(mouseState.Position);
            backToMenuButtonHovered = backToMenuButtonRect.Contains(mouseState.Position);

            lightButtonHovered = lightButtonRect.Contains(mouseState.Position);
            standartButtonHovered = standartButtonRect.Contains(mouseState.Position);
            shadowButtonHovered = shadowButtonRect.Contains(mouseState.Position);
            if (backToMenuButtonHovered && mouseState.LeftButton == ButtonState.Pressed)
            {
                gameStarted = false;
                state = GameState.Menu;
                MediaPlayer.Play(menuMusic);
                MediaPlayer.Resume();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E) && gameStarted && state != GameState.Intro)
            {
                state = GameState.Gameplay;
                MediaPlayer.Resume();
            }
            if (musicControllerHovered && mouseState.LeftButton == ButtonState.Pressed)
            {
                int mouseX = mouseState.Position.X;
                mouseX = MathHelper.Clamp(mouseX, musicControllerRect.X, musicControllerRect.X + musicControllerRect.Width);
                musicVolume = (float)(mouseX - musicControllerRect.X) / musicControllerRect.Width;
                MediaPlayer.Volume = musicVolume;
            }
            if (soundControllerHovered && mouseState.LeftButton == ButtonState.Pressed)
            {
                int mouseX = mouseState.Position.X;
                mouseX = MathHelper.Clamp(mouseX, soundControllerRect.X, soundControllerRect.X + soundControllerRect.Width);
                soundVolume = (float)(mouseX - soundControllerRect.X) / soundControllerRect.Width;
                SoundEffect.MasterVolume = soundVolume;
            }
            if (lightButtonHovered && mouseState.LeftButton == ButtonState.Pressed)
                difficult = 0;
            if (standartButtonHovered && mouseState.LeftButton == ButtonState.Pressed)
                difficult = 1;
            if (shadowButtonHovered && mouseState.LeftButton == ButtonState.Pressed)
                difficult = 2;
        }

        private void UpdateIntro(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (prevKeyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
            {
                currentIntroFrame++;
                
            }
            prevKeyboardState = keyboardState;
            if (currentIntroFrame > 4)
            {
                MediaPlayer.Play(gameplayMusic);
                state = GameState.Gameplay;
                gameStarted = true;
            }
        }

        private void UpdateGameplay(GameTime deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            playerSprite.Update(deltaTime);
            flashlight.Update(Window);
            flashlightHard.Update(Window);
            switch (currentRoom)
            {
                case 1:
                    if (playerSprite.position.X >= 1300 && playerSprite.position.X <= 1600 && !playerDroppedSnickersAndLeft)
                    {
                        e.position = new Vector2(1550, 150);
                        playerNearFirstDoor = true;
                        if (prevKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E))
                        {
                            doorSound.Play();
                            roarSound.Play();
                            currentRoom = 3;
                            playerSprite.position.X = 1400;
                        }
                    }
                    else
                        playerNearFirstDoor = false;
                    if (playerSprite.position.X >= 150 && playerSprite.position.X <= 450 && playerHasKey)
                    {
                        e.position = new Vector2(400, 150);
                        playerNearExit = true;
                        if (prevKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E))
                        {
                            winSound.Play();
                            MediaPlayer.Play(winMusic);
                            state = GameState.Win;
                        }
                    }
                    else
                        playerNearExit = false;
                    if (playerSprite.position.X <= 10 && playerSprite.direction == true)
                        playerSprite.speed = 0;
                    else
                        playerSprite.speed = 5;
                    if (playerSprite.position.X >= 1920)
                    {
                        currentRoom = 2;
                        playerSprite.position.X = 0;
                    }
                    prevKeyboardState = keyboardState;
                    break;
                case 2:
                    if (playerSprite.position.X >= 750 && playerSprite.position.X <= 1050)
                    {
                        e.position = new Vector2(1000, 150);
                        playerNearSecondDoor = true;
                        if (prevKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E))
                        {
                            doorSound.Play();
                            roarSound.Play();
                            currentRoom = 3;
                            playerSprite.position.X = 350;
                        }
                    }
                    else
                        playerNearSecondDoor = false;
                    if (playerSprite.position.X >= 1200 && playerSprite.position.X <= 1500 && playerHasCoin)
                    {
                        e.position = new Vector2(1450, 150);
                        playerNearVendingAp = true;
                        if (keyboardState.IsKeyDown(Keys.E))
                        {
                            apSound.Play();
                            playerHasCoin = false;
                            playerHasSnickers = true;
                            snickers.position = new Vector2(1700, 50);
                        }
                    }
                    else
                        playerNearVendingAp = false;
                    if (playerSprite.position.X >= 1650 && playerSprite.direction == false)
                        playerSprite.speed = 0;
                    else
                        playerSprite.speed = 5;
                    if (playerSprite.position.X < -200)
                    {
                        currentRoom = 1;
                        playerSprite.position.X = 1650;
                    }
                    prevKeyboardState = keyboardState;
                    break;
                case 3:

                    if ((!playerDroppedSnickersAndLeft && playerSprite.position.X <= 250) || (playerDroppedSnickersAndLeft && playerSprite.position.X >= 1000))
                    {
                        MediaPlayer.Play(deathMusic);
                        state = GameState.Death;
                        amamSound.Play();


                    }
                    if (playerSprite.position.X >= 1250 && playerSprite.position.X <= 1550)
                    {
                        e.position = new Vector2(1500, 150);
                        playerNearRightDoor = true;
                        if (prevKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E))
                        {
                            doorSound.Play();
                            if (playerDroppedSnickers)
                            {
                                playerDroppedSnickersAndLeft = true;
                                if (!monsterLeft)
                                {
                                    monsterStepsSound.Play();
                                    monsterLeft = true;
                                }
                            }
                            currentRoom = 1;
                            playerSprite.position.X = 1400;
                        }
                    }
                    else
                        playerNearRightDoor = false;
                    if (playerSprite.position.X >= 250 && playerSprite.position.X <= 550)
                    {
                        e.position = new Vector2(470, 150);
                        playerNearLeftDoor = true;
                        if (prevKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E))
                        {
                            doorSound.Play();
                            if (playerDroppedSnickers)
                            {
                                playerDroppedSnickersAndLeft = true;
                                if (!monsterLeft)
                                {
                                    monsterStepsSound.Play();
                                    monsterLeft = true;
                                }
                            }
                            currentRoom = 2;
                            playerSprite.position.X = 900;
                        }
                    }
                    else
                        playerNearLeftDoor = false;
                    if (playerSprite.position.X >= 1650 && playerSprite.direction == false)
                        playerSprite.speed = 0;
                    else if (playerSprite.position.X <= 10 && playerSprite.direction == true)
                        playerSprite.speed = 0;
                    else
                        playerSprite.speed = 5;
                    prevKeyboardState = keyboardState;

                    if (playerSprite.position.X >= 700 && playerSprite.position.X <= 1000 && !playerHasCoin && !playerHasSnickers && !playerHasKey && !playerDroppedSnickers)
                    {
                        e.position = new Vector2(900, 150);
                        playerNearCoin = true;
                        if (keyboardState.IsKeyDown(Keys.E))
                        {
                            coinSound.Play();
                            playerHasCoin = true;
                            coin.position = new Vector2(1800, 50);
                        }
                    }
                    else
                        playerNearCoin = false;

                    if (playerSprite.position.X > 1550 && playerSprite.position.X < 1650 && !playerHasCoin && playerHasSnickers && !playerHasKey)
                    {
                        e.position = new Vector2(1650, 400);
                        playerNearMiska = true;
                        if (keyboardState.IsKeyDown(Keys.E))
                        {
                            playerDroppedSnickers = true;
                            snickers.position = new Vector2(2000, 2000);
                            coin.position = new Vector2(2000, 2000);
                            playerHasSnickers = false;
                        }
                    }
                    else
                        playerNearMiska = false;

                    if (playerSprite.position.X > 10 && playerSprite.position.X < 250 && !playerHasKey)
                    {
                        e.position = new Vector2(200, 400);
                        playerNearKey = true;
                        if (keyboardState.IsKeyDown(Keys.E))
                        {
                            playerHasKey = true;
                            MediaPlayer.Play(runningMusic);
                        }
                    }
                    else
                        playerNearKey = false;
                    break;
            }

        }

        private void UpdateWin(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                MediaPlayer.Play(menuMusic);
                state = GameState.Menu;
            }
        }

        private void UpdateDeath(GameTime deltaTime)
        {
            gameStarted = false;
            IsMouseVisible = true;
            MouseState mouseState = Mouse.GetState();
            backToMenuButtonHovered = backToMenuButtonRect.Contains(mouseState.Position);
            if (backToMenuButtonHovered && mouseState.LeftButton == ButtonState.Pressed)
            {
                MediaPlayer.Play(menuMusic);
                state = GameState.Menu;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            switch (state)
            {
                case GameState.Menu:
                    DrawMenu(gameTime);
                    break;
                case GameState.Options:
                    DrawOptions(gameTime);
                    break;
                case GameState.Gameplay:
                    DrawGameplay(gameTime);
                    break;
                case GameState.Death:
                    DrawDeath(gameTime);
                    break;
                case GameState.Win:
                    DrawWin(gameTime);
                    break;
                case GameState.Intro:
                    DrawIntro(gameTime);
                    break;
            }
            _spriteBatch.End();
        }

        private void DrawIntro(GameTime gameTime)
        {
            intro1.Draw(_spriteBatch);
            if (currentIntroFrame == 2)
                intro2.Draw(_spriteBatch);
            if (currentIntroFrame == 3)
                intro3.Draw(_spriteBatch);
            if (currentIntroFrame == 4)
                intro4.Draw(_spriteBatch);
        }

        private void DrawMenu(GameTime deltaTime)
        {
            main.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, "New Game", new Vector2(newGameButtonRect.X, newGameButtonRect.Y), newGameButtonHovered ? Color.Black : Color.White);
            _spriteBatch.DrawString(font, "Settings", new Vector2(settingsButtonRect.X, settingsButtonRect.Y), settingsButtonHovered ? Color.Black : Color.White);
            _spriteBatch.DrawString(font, "Exit", new Vector2(exitButtonRect.X, exitButtonRect.Y), exitButtonHovered ? Color.Black : Color.White);

        }

        private void DrawOptions(GameTime deltaTime)
        {
            options.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, "Back to menu", new Vector2(backToMenuButtonRect.X, backToMenuButtonRect.Y), backToMenuButtonHovered ? Color.Black : Color.White);
            if (gameStarted && state != GameState.Intro)
                _spriteBatch.DrawString(font, "Press E to continue", new Vector2(backToMenuButtonRect.X + 1250, backToMenuButtonRect.Y), Color.White);
            _spriteBatch.DrawString(font, "Music", new Vector2(backToMenuButtonRect.X + 50, 400), Color.White);
            _spriteBatch.Draw(musicControllerTextureBlack, musicControllerRect, Color.White);
            int width = (int)(musicVolume * musicControllerTextureWhite.Width);
            Rectangle volumeIndicatorRect = new Rectangle(musicControllerRect.X, musicControllerRect.Y, width, musicControllerRect.Height);
            _spriteBatch.Draw(musicControllerTextureWhite, volumeIndicatorRect, new Rectangle(0, 0, width, musicControllerTextureBlack.Height), Color.White);

            _spriteBatch.DrawString(font, "Sounds", new Vector2(backToMenuButtonRect.X + 50, 550), Color.White);
            _spriteBatch.Draw(musicControllerTextureBlack, soundControllerRect, Color.White);
            int width2 = (int)(soundVolume * musicControllerTextureWhite.Width);
            Rectangle volumeIndicatorRect2 = new Rectangle(soundControllerRect.X, soundControllerRect.Y, width2, soundControllerRect.Height);
            _spriteBatch.Draw(musicControllerTextureWhite, volumeIndicatorRect2, new Rectangle(0, 0, width2, musicControllerTextureBlack.Height), Color.White);

            _spriteBatch.DrawString(font, "Difficulty", new Vector2(backToMenuButtonRect.X + 50, 700), Color.White);
            _spriteBatch.DrawString(font, "Light", new Vector2(lightButtonRect.X, lightButtonRect.Y), difficult == 0 ? Color.Black : Color.White);
            _spriteBatch.DrawString(font, "Standart", new Vector2(standartButtonRect.X, standartButtonRect.Y), difficult == 1 ? Color.Black : Color.White);
            _spriteBatch.DrawString(font, "Shadow", new Vector2(shadowButtonRect.X, shadowButtonRect.Y), difficult == 2 ? Color.Black : Color.White);
        }

        private void DrawGameplay(GameTime deltaTime)
        {
            IsMouseVisible = false;
            switch (currentRoom)
            {
                case 1:
                    room1.Draw(_spriteBatch);
                    playerSprite.Draw(_spriteBatch);
                    if (playerNearFirstDoor || playerNearExit)
                        e.Draw(_spriteBatch);

                    break;
                case 2:
                    room2.Draw(_spriteBatch);
                    playerSprite.Draw(_spriteBatch);
                    if (playerNearSecondDoor || playerNearVendingAp)
                        e.Draw(_spriteBatch);
                    break;
                case 3:
                    if (!playerDroppedSnickers && !playerDroppedSnickersAndLeft)
                        room3.Draw(_spriteBatch);
                    else if (playerDroppedSnickersAndLeft)
                        room33.Draw(_spriteBatch);
                    else
                        room32.Draw(_spriteBatch);
                    if (!playerHasCoin && !playerHasKey && !playerHasSnickers && !playerDroppedSnickersAndLeft)
                        coin.Draw(_spriteBatch);

                    playerSprite.Draw(_spriteBatch);
                    if (playerNearRightDoor || playerNearLeftDoor || playerNearCoin || playerNearKey || playerNearMiska)
                        e.Draw(_spriteBatch);
                    break;
            }
            if (difficult == 1)
                flashlight.Draw(_spriteBatch);
            else if (difficult == 2)
                flashlightHard.Draw(_spriteBatch);
            if (playerHasCoin && !playerHasKey && !playerHasSnickers)
                coin.Draw(_spriteBatch);
            if (playerHasSnickers && !playerHasCoin && !playerHasKey)
                snickers.Draw(_spriteBatch);
            if (playerHasKey)
                key.Draw(_spriteBatch);
        }

        private void DrawWin(GameTime gameTime)
        {
            win2.Draw(_spriteBatch);
        }

        private void DrawDeath(GameTime deltaTime)
        {
            death.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, "Back to menu", new Vector2(backToMenuButtonRect.X, backToMenuButtonRect.Y), backToMenuButtonHovered ? Color.Black : Color.White);
        }

        void StartNewGame()
        {
            currentIntroFrame = 1;
            playerSprite.position = new Vector2(100, 300);
            currentRoom = 1;
            playerHasCoin = false;
            playerHasSnickers = false;
            playerHasKey = false;
            monsterLeft = false;
            playerDroppedSnickers = false;
            playerDroppedSnickersAndLeft = false;
            coin.position = new Vector2(900, 565);
            gameStarted = false;
            state = GameState.Intro;
        }
    }
}
