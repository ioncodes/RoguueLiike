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

        private const int TILE_WIDTH = 32;
        private const int TILE_HEIGHT = 32;
        private const int WIDTH = 20;
        private const int HEIGHT = 20;

        Texture2D[,] _map = new Texture2D[WIDTH,HEIGHT];

        private Player _player = new Player();
        readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();


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
            _graphics.PreferredBackBufferWidth = WIDTH*TILE_WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT*TILE_HEIGHT;
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

            _enemies.Add("dwarf", new Enemy()
            {
                Texture = Content.Load<Texture2D>("enemy/dwarf"),
                Position = new Position(32, 32)
            });

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (i == 0 || j == 0 || j == HEIGHT-1 || i == WIDTH-1)
                    {
                        _map[j, i] = _textures.FirstOrDefault(t => t.Key == "wall").Value;
                    }
                    else
                    {
                        _map[j, i] = _textures.FirstOrDefault(t => t.Key == "floor").Value;
                    }
                }
            }
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
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Left) && VerifyMovement(-1, 0))
            {
                _player.Position.X -= TILE_WIDTH;
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Right) && VerifyMovement(1, 0))
            {
                _player.Position.X += TILE_WIDTH;
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Down) && VerifyMovement(0, 1))
            {
                _player.Position.Y += TILE_HEIGHT;
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Up) && VerifyMovement(0, -1))
            {
                _player.Position.Y -= TILE_HEIGHT;
                MoveEnemies();
            }

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

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (i == 0 || j == 0 || j == HEIGHT-1 || i == WIDTH-1)
                    {
                        _spriteBatch.Draw(_textures.FirstOrDefault(t => t.Key == "wall").Value, new Vector2(i * TILE_WIDTH, j * TILE_HEIGHT));
                    }
                    else
                    {
                        _spriteBatch.Draw(_textures.FirstOrDefault(t => t.Key == "floor").Value, new Vector2(i * TILE_WIDTH, j * TILE_HEIGHT));
                    }
                }
            }

            _spriteBatch.Draw(_player.Texture, new Vector2(_player.Position.X, _player.Position.Y));

            foreach (var enemy in _enemies)
            {
                _spriteBatch.Draw(enemy.Value.Texture, new Vector2(enemy.Value.Position.X, enemy.Value.Position.Y));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void MoveEnemies()
        {
            foreach (var enemy in _enemies)
            {
                int[] mov = enemy.Value.EnemeKi.Decide();
                enemy.Value.Position.X += mov[0];
                enemy.Value.Position.X += mov[1];
            }
        }

        bool VerifyMovement(int x, int y)
        {
            if (x != 0)
            {
                int virtPosX = _player.Position.X + (x*WIDTH) / WIDTH;
                int virtPosY = _player.Position.Y / HEIGHT;

                
            }

            return true;
        }
    }
}
