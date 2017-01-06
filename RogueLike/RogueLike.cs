using System;
using System.Collections.Generic;
using System.Linq;
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
        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        private const int WIDTH = 32;
        private const int HEIGHT = 32;
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
            graphics.PreferredBackBufferWidth = 20*WIDTH;
            graphics.PreferredBackBufferHeight = 20*HEIGHT;
            graphics.ApplyChanges();
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
            
            textures.Add("floor", Content.Load<Texture2D>("floor/vines0"));
            textures.Add("wall", Content.Load<Texture2D>("wall/vines0"));

            player.Texture = Content.Load<Texture2D>("player/base/human_m");


            /* Fix the order to display easy */
            //var fix2 = map[2, 2];
            //var fix1 = map[1, 2];
            //var fix0 = map[0, 2];
            //map[0, 2] = map[0, 0];
            //map[1, 2] = map[1, 0];
            //map[2, 2] = map[2, 0];
            //map[0, 0] = fix0;
            //map[1, 0] = fix1;
            //map[2, 0] = fix2;
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
            spriteBatch.Begin();

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || j == 0 || j == 19 || i == 19)
                    {
                        spriteBatch.Draw(textures.FirstOrDefault(t => t.Key == "wall").Value, new Vector2(i*32, j*32));
                    }
                    else
                    {
                        spriteBatch.Draw(textures.FirstOrDefault(t => t.Key == "floor").Value, new Vector2(i * 32, j * 32));
                    }
                }
            }

            spriteBatch.Draw(player.Texture, new Vector2(player.Position.X, player.Position.Y));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
