using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SnakeGame.Classes
{
    class cSnake : DrawableGameComponent
    {
        const int updateInterval = 33;

        int size = 0;
        int milliSecondsSinceLastUpdate = 0;
        int oldPosX = 0;
        int oldPosY = 0;

        public int score { get; set; } = 0;
        public bool run { get; set; } = false;
        public int posX { get; set; } = 0;
        public int posY { get; set; } = 0;
        public int dirX { get; set; } = 1;
        public int dirY { get; set; } = 0;

        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        Texture2D pixel;
        List<Rectangle> tailList;

        public cSnake(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int size) : base(game)
        {
            this.size = size;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            posX = graphics.Viewport.Width / 2;
            posY = graphics.Viewport.Height / 2;

            tailList = new List<Rectangle>();
            tailList.Add(new Rectangle(posX, posY, size, size));
        }

        public void ResetSnake()
        {
            tailList.Clear();
            score = 0;
            posX = graphics.Viewport.Width / 2;
            posY = graphics.Viewport.Height / 2;

            tailList.Add(new Rectangle(posX, posY, size, size));
            run = true;
        }

        public override void Update(GameTime gameTime)
        {
            milliSecondsSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if (milliSecondsSinceLastUpdate >= updateInterval && run)
            {
                milliSecondsSinceLastUpdate = 0;
                oldPosX = posX;
                oldPosY = posY;

                posX = posX + dirX * size;
                posY = posY + dirY * size;

                if(posY == -size || posY == graphics.Viewport.Height || posX == -size || posX == graphics.Viewport.Width)
                {
                    run = false;
                    posX = oldPosX;
                    posY = oldPosY;
                    return;
                }

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
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (run)
            {
                foreach (var item in tailList)
                {
                    spriteBatch.Draw(pixel, new Rectangle(item.X - 1, item.Y - 1, size + 2, size + 2), Color.Gray);
                    spriteBatch.Draw(pixel, item, Color.Red);
                }
            }
            else
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
