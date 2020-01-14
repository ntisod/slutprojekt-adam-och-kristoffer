using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SnakeGame.Classes
{
    public class cSnake : DrawableGameComponent
    {
        const int updateInterval = 33; // den bestämmer hastigheten hos Snake elementen

        // detta är standardinställningsfunktionen
        int size = 0; 
        int milliSecondsSinceLastUpdate = 0;
        int oldPosX = 0;
        int oldPosY = 0;

        // detta säger till Snake elementen hur den ska reagera
        public int score { get; set; } = 0;
        public bool run { get; set; } = false;
        public int posX { get; set; } = 0;
        public int posY { get; set; } = 0;
        public int dirX { get; set; } = 1;
        public int dirY { get; set; } = 0;

        GraphicsDevice graphics; // skapar resurser, hanterar variabler på systemnivå och skapar skuggor
        SpriteBatch spriteBatch;
        Texture2D pixel;
        List<Rectangle> tailList; // den gör antal rektanglar och drar rektanglarna 
        // med olika bredder och höjder men längs samma baslinje


        public cSnake(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int size) 
            : base(game)
        {
            this.size = size;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White }); // är färgen på Snake elementen när man förlorar

            // positionering av Snake elementen så att den börjar i mitten av skärmen
            posX = graphics.Viewport.Width / 2; 
            posY = graphics.Viewport.Height / 2;

            tailList = new List<Rectangle>();
            tailList.Add(new Rectangle(posX, posY, size, size)); // gör att man kan lägga till, ta bort och få en mängd av elementen i samlingen.
        }

        //Spelet resetas när man dör
        public void ResetSnake()
        {
            tailList.Clear();
            score = 0; //Din score börjar på noll när du dör
            posX = graphics.Viewport.Width / 2;
            posY = graphics.Viewport.Height / 2;

            tailList.Add(new Rectangle(posX, posY, size, size));
            run = true;
        }

        public override void Update(GameTime gameTime)
        {
            milliSecondsSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds; // återvänder tiden som gått sedan den senaste uppdateringen i millisekunder

            // tid updatering
            // om milliSecondsSinceLastUpdate är större eller lika med updateInterval && run ska:
            if (milliSecondsSinceLastUpdate >= updateInterval && run)
            {
                milliSecondsSinceLastUpdate = 0; // milliSecondsSinceLastUpdate vara 0
                oldPosX = posX; // gammla PosX lika med posX
                oldPosY = posY; // gammla PosY lika med posY

                posX = posX + dirX * size;
                posY = posY + dirY * size;

                if(posY == -size || posY == graphics.Viewport.Height || posX == -size || posX == graphics.Viewport.Width)
                {
                    run = false;
                    posX = oldPosX;
                    posY = oldPosY;
                    return;
                }

                // detta gör att Snake elementet ökar i storleken när den äter Food elementen 
                if (tailList.Count > 1)
                {
                    for (int i = tailList.Count - 1; i > 0; i--)
                    {
                        if (posX == tailList[i].X && posY == tailList[i].Y)
                        {
                            run = false;
                            posX = oldPosX;
                            posY = oldPosY;
                            return;
                        }
                        tailList[i] = new Rectangle(tailList[i - 1].X, tailList[i - 1].Y, size, size);

                    }
                }
            }
            tailList[0] = new Rectangle(posX, posY, size, size);
            base.Draw(gameTime);
        }

        public void AddTail()
        {
            tailList.Add(new Rectangle(posX, posY, size, size));
            // lägger till ett element eller alla element i en annan lista i slutet av en lista
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (run) // är funktionerna på spelet när den körs
            {
                foreach (var item in tailList)
                {
                    spriteBatch.Draw(pixel, new Rectangle(item.X - 1, item.Y - 1, size + 2, size + 2), Color.Gray);
                    spriteBatch.Draw(pixel, item, Color.Red);
                }
            }
            else // är funktionerna på spelet när man dör
            {
                foreach (var item in tailList)
                {
                    spriteBatch.Draw(pixel, new Rectangle(item.X - 1, item.Y - 1, size + 2, size + 2), Color.Gray);
                    spriteBatch.Draw(pixel, item, Color.White);
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
