using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Classes
{
    class cFood : DrawableGameComponent // så att funktionerna Draw() och Update() kallas automatiskt.
    {
        SpriteBatch spriteBatch; // rita massa spriter samtidigt
        Texture2D pixel; 

        public bool active { get; set; } = false; // deklaration av en variabel: falskt
        public int posX { get; set; } // för att ha tillgång till data och information i privata fält 
        public int posY { get; set; } // (get) används för att återvända en Property värde (set) för att bestämma ett nytt värde

        int size;

        // här är funktionerna på Food elementen
        public cFood (Game game, SpriteBatch spriteBatch, GraphicsDevice graphics, int size) : base (game)
        {
            this.spriteBatch = spriteBatch;
            this.size = size;
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White }); // färgen på food elementen
        }

        // här ritas Food elementen
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
