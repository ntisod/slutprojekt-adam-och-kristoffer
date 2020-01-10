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
        // deklaration eller innehåll av själva spelet
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        cSnake snake;
        cFood food;
        Random rnd;
        Song song;

        // storleken av spel skärmen och Snake elementen
        const int gameHeight = 50;
        const int gameWidth = 100;
        const int snakeSize = 10;
               
        public snakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
  
        // här passar man in skärmen med Snake storleken i spelet
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = gameHeight * snakeSize;
            graphics.PreferredBackBufferHeight = gameWidth * snakeSize;
            base.Initialize();
        }

        // här laddar man upp det man vill ha till spelet
        protected override void LoadContent()
        {
            rnd = new Random(); // skapar funktionalitet för att generera nya slumpmässiga nummer
            spriteBatch = new SpriteBatch(GraphicsDevice);
            snake = new cSnake(this, GraphicsDevice, spriteBatch, snakeSize); // här man laddar upp Snake elementen
            food = new cFood(this, spriteBatch, GraphicsDevice, snakeSize); // här man laddar upp Food elementen
            font = Content.Load<SpriteFont>("font");
            song = Content.Load<Song>("Music/Nice"); // här man laddar upp musik till spelet


            MediaPlayer.Play(song); // här startas musiken när spelet börjar
            MediaPlayer.IsRepeating = true; // här spelas musiken om och om i en loop

            this.Components.Add(snake);
            this.Components.Add(food);

            IsMouseVisible = true;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // positioneringen av Food elementen
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
                snake.score++; // varje gång man äter upp en Food element ska talet öka med 1
                food.active = false;
                snake.AddTail();
            }
        }

        // Tangentbords Kontroll i spelet
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // ifall man vill avsluta spelet så ska man trycka (esc) knappen
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

            // när man vill spela om så ska man trycka (Space) knappen på tangentbordet
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
            // hur (Score) texten ska vara 

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
