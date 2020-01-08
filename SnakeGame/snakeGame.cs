using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Classes;
using System;

namespace SnakeGame
{

    public class snakeGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        cSnake snake;
        cFood food;
        Random rnd;
        Song song;

        const int gameHeight = 50;
        const int gameWidth = 100;
        const int snakeSize = 10;
               
        public snakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

   
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = gameHeight * snakeSize;
            graphics.PreferredBackBufferHeight = gameWidth * snakeSize;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            rnd = new Random();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            snake = new cSnake(this, GraphicsDevice, spriteBatch, snakeSize);
            food = new cFood(this, spriteBatch, GraphicsDevice, snakeSize);
            font = Content.Load<SpriteFont>("font");
            song = Content.Load<Song>("Music/Nice");


            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            this.Components.Add(snake);
            this.Components.Add(food);

            IsMouseVisible = true;

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SetFoodLocation()
        {
            food.posX = rnd.Next(0, GraphicsDevice.Viewport.Width / snakeSize) * snakeSize;
            food.posY = rnd.Next(0, GraphicsDevice.Viewport.Height / snakeSize) * snakeSize;
            food.active = true;
        }

        public void CheckSnakeFood()
        {
            if (snake.posX == food.posX && snake.posY == food.posY)
            {
                snake.score++;
                food.active = false;
                snake.AddTail();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!food.active)
            {
                SetFoodLocation();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                snake.dirX = 0;
                snake.dirY = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                snake.dirX = 0;
                snake.dirY = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                snake.dirX = 1;
                snake.dirY = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                snake.dirX = -1;
                snake.dirY = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !snake.run)
            {
                snake.run = true;
                snake.ResetSnake();
            }

            CheckSnakeFood();         
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51, 51, 51));

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + snake.score.ToString(), new Vector2(snakeSize), Color.Gray);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
