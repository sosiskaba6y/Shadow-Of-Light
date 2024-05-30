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
    internal class Flashlight : Sprite
    {
        public Flashlight(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void Update(GameWindow window)
        {
            CalculatePosition(window);
        }

        private void CalculatePosition(GameWindow window)
        {
            var mouseCoord = new Vector2(Mouse.GetState(window).X, Mouse.GetState(window).Y); 
            position.X = mouseCoord.X - texture.Width / 2;
            position.Y = mouseCoord.Y - texture.Height / 2;
        }
    }


}

