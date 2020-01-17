using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SnakeGame.Classes;
using System;
using System.Text;

namespace SnakeGame
{

    public class SnakeGame : Game
    {
        //Deklaration eller innehåll av själva spelet
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Snake snake;
        Food food;
        Random rnd;
        Song song;
        KeyboardState current, previous;

        MouseState mouseState, previousMouseState;
        KeyboardState ks;
        Color col;

        const byte MENU = 0, GAMEOVER = 1, PlayGame = 2;
        int CurrentScreen = MENU;

        //Variabler för MENU skärmen
        public Texture2D playgameText;
        Button playGameButton;
        public Texture2D bgimage;
        public Texture2D GameOver;

        //Storleken av spel skärmen och Snake elementen
        const int gameHeight = 50;
        const int gameWidth = 100;
        const int snakeSize = 10;
        bool gamePaused = false;

        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            GameOver = null;
        }
  
        //Här passar man in skärmen med Snake storleken i spelet
        protected override void Initialize()
        {
            col = Color.White;

            graphics.PreferredBackBufferHeight = gameHeight * snakeSize;
            graphics.PreferredBackBufferHeight = gameWidth * snakeSize;
            base.Initialize();
        }

        //Här laddar man upp det man vill ha till spelet
        protected override void LoadContent()
        {
            rnd = new Random(); //Skapar funktionalitet för att generera nya slumpmässiga nummer
            spriteBatch = new SpriteBatch(GraphicsDevice);
            snake = new Snake(this, GraphicsDevice, spriteBatch, snakeSize); //Här man laddar upp Snake elementen
            food = new Food(this, spriteBatch, GraphicsDevice, snakeSize); //Här man laddar upp Food elementen
            font = Content.Load<SpriteFont>("font");
            song = Content.Load<Song>("Music/Nice"); //Här man laddar upp musik till spelet

            //Saker vi vill ladda på MENU skärmen
            playgameText = Content.Load<Texture2D>("Bilder/options");
            bgimage = Content.Load<Texture2D>("Bilder/main menu");

            playGameButton = new Button(new Rectangle(300, 50, playgameText.Width, playgameText.Height), true);
            playGameButton.load(Content, "Bilder/options");

            this.Components.Add(snake);
            this.Components.Add(food);
            IsMouseVisible = true;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Positioneringen av Food elementen
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
                snake.score++; //Varje gång man äter upp en Food element ska talet öka med 1
                food.active = false;
                snake.AddTail();
            }
        }

        //Tangentbords Kontroll i spelet
        protected override void Update(GameTime gameTime)
        {
            //Kontrollera STATE's mus 

            mouseState = Mouse.GetState();
            ks = Keyboard.GetState();

            switch (CurrentScreen)
            {
                case MENU:
                    //Ändringar på MENU skärmen är här

                    //GÅR TILL PlayGame SKÄRM

                    if (playGameButton.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        CurrentScreen = PlayGame;
                    }

                    break;
            }

            previous = current;
            current = Keyboard.GetState();

            if (current.IsKeyUp(Keys.P) && previous.IsKeyDown(Keys.P)) gamePaused = !gamePaused;

            if (gamePaused) return;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) //Ifall man vill avsluta spelet så ska man trycka (esc) knappen
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

            //När man vill spela om så ska man trycka (Space) knappen på tangentbordet
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !snake.run && CurrentScreen == PlayGame)
            {
                snake.run = true;
                MediaPlayer.Play(song); //Här startas musiken när spelet börjar
                MediaPlayer.IsRepeating = true; //Här spelas musiken om och om i en loop
                snake.ResetSnake();

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                MediaPlayer.Pause();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                MediaPlayer.Resume();
            }

            previousMouseState = mouseState;
            CheckSnakeFood();         
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51, 51, 51));

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + snake.score.ToString(), new Vector2(snakeSize), Color.Gray);
            if (!snake.run)
                spriteBatch.DrawString(font, "Press P to Pause and unPause Game", new Vector2(255, 195), Color.Red);
            if (!snake.run )
                spriteBatch.DrawString(font, "Press Spacebar to Begin Game", new Vector2(255, 225), Color.Red);
            if (!snake.run)
                spriteBatch.DrawString(font, "Press Escape to Quit Game", new Vector2(255, 255), Color.Red);
            

            //Hur texten ska vara 

            switch (CurrentScreen)
            {
                case MENU:
                    //Det som vi vill att det ska hända på MENU skärmen går här

                    spriteBatch.Draw(bgimage, new Rectangle(0, 0, bgimage.Width, bgimage.Height), Color.White);
                    spriteBatch.Draw(playgameText, new Rectangle(300, 50, playgameText.Width, playgameText.Height), Color.White);            
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
