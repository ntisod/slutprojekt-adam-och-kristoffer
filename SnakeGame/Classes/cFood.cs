using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Classes
{
    class cFood : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D pixel;

        public bool active { get; set; } = false;
        public int posX { get; set; }
        public int posY { get; set; }

        int size;

        public cFood (Game game, SpriteBatch spriteBatch, GraphicsDevice graphics, int size) : base (game)
        {
            this.spriteBatch = spriteBatch;
            this.size = size;
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (active)
            {
                spriteBatch.Draw(pixel, new Rectangle(posX, posY, size, size), Color.GreenYellow);
            }
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
