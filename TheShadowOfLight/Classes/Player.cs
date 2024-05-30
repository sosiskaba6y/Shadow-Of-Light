using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShadowOfLight.Sprites
{
    internal class Player : Sprite
    {
        int currentTime = 0; // сколько времени прошло
        int period = 200; // частота обновления в миллисекундах

        int frameWidth = 32;
        int frameHeight = 64;

        public int speed = 5;

        Point currentFrame = new Point(0, 0);
        Point walkingSpriteSize = new Point(5, 2);
        Point stayingSpriteSize = new Point(4, 1);
        public bool direction;
        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public void Update(GameTime gameTime)
        {
            Move();
            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime > period)
            {
                currentTime -= period;

                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    if (direction == true)
                    {
                        
                        currentFrame.Y = 1;
                    }
                    else
                        currentFrame.Y = 2;
                    ++currentFrame.X;
                    if (currentFrame.X >= walkingSpriteSize.X)
                    {
                        currentFrame.X = 0;
                    }
                }
                else
                {
                    currentFrame.Y = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= stayingSpriteSize.X)
                    {
                        currentFrame.X = 0;
                    }
                }


            }
        }

        public void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction = true;
                position.X -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction = false;
                position.X += speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position,
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                10, SpriteEffects.None, 0);
        }
    }
}
