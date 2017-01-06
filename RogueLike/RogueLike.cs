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
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        //Texture2D[,] _map = new Texture2D[3,3];

        private Player _player = new Player();
        readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        private const int WIDTH = 32;
        private const int HEIGHT = 32;

        int _framesPassed = 0;

        public RogueLike()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            _graphics.PreferredBackBufferWidth = 20*WIDTH;
            _graphics.PreferredBackBufferHeight = 20*HEIGHT;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _textures.Add("floor", Content.Load<Texture2D>("floor/vines0"));
            _textures.Add("wall", Content.Load<Texture2D>("wall/vines0"));

            _player.Texture = Content.Load<Texture2D>("player/base/human_m");
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
            if (_framesPassed > 5)
            {
                _framesPassed = 0;
            }
            _framesPassed++;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Left))
                _player.Position.X -= WIDTH;
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Right))
                _player.Position.X += WIDTH;
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Down))
                _player.Position.Y += HEIGHT;
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Up))
                _player.Position.Y -= HEIGHT;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || j == 0 || j == 19 || i == 19)
                    {
                        _spriteBatch.Draw(_textures.FirstOrDefault(t => t.Key == "wall").Value, new Vector2(i * WIDTH, j * HEIGHT));
                    }
                    else
                    {
                        _spriteBatch.Draw(_textures.FirstOrDefault(t => t.Key == "floor").Value, new Vector2(i * WIDTH, j * HEIGHT));
                    }
                }
            }

            _spriteBatch.Draw(_player.Texture, new Vector2(_player.Position.X, _player.Position.Y));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
