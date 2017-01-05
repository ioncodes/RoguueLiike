using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLike
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RogueLike : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D[,] map = new Texture2D[3,3];
        private Player player = new Player();

        private const int WIDTH = 16;
        private const int HEIGHT = 16;
        int framesPassed = 0;

        public RogueLike()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load map
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    map[i,j] = Content.Load<Texture2D>("f" + i + j);
                }
            }

            /* Fix the order to display easy */
            var fix2 = map[2, 2];
            var fix1 = map[1, 2];
            var fix0 = map[0, 2];
            map[0, 2] = map[0, 0];
            map[1, 2] = map[1, 0];
            map[2, 2] = map[2, 0];
            map[0, 0] = fix0;
            map[1, 0] = fix1;
            map[2, 0] = fix2;

            // Load Player
            player.Texture = Content.Load<Texture2D>("p1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (framesPassed > 5)
            {
                framesPassed = 0;
            }
            framesPassed++;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Left))
                player.Position.X -= WIDTH;
            if (framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Right))
                player.Position.X += WIDTH;
            if (framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Down))
                player.Position.Y += HEIGHT;
            if (framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Up))
                player.Position.Y -= HEIGHT;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            int x = 0;
            spriteBatch.Begin();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    spriteBatch.Draw(map[i, j], new Vector2(x*WIDTH, j*HEIGHT));
                }
                x++;
            }
            spriteBatch.Draw(player.Texture, new Vector2(player.Position.X, player.Position.Y));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
