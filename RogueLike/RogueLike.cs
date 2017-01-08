using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueLike.Enemies;
using RogueLike.Levels;
using RogueLikeMapBuilder;

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
        private const int WIDTH = 150;
        private const int HEIGHT = 150;
        private const int FOV = 20;

        private const int ENEMY_AMOUNT = 50;
        private const int ENEMY_RADIUS = 3;
        private int enemyCounter = 0;
        Random r = new Random();

        //Texture2D[,] _map = new Texture2D[WIDTH,HEIGHT];
        MapTile[,] _map = new MapTile[WIDTH,HEIGHT];

        private Player _player = new Player();
        readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();
        List<Enemy> _enemyTypes = new List<Enemy>();

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
            _graphics.PreferredBackBufferWidth = FOV*TILE_WIDTH;
            _graphics.PreferredBackBufferHeight = FOV*TILE_HEIGHT;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _enemyTypes = GetEnemies(); // Load the Enemy classes

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _textures.Add("floor", Content.Load<Texture2D>("floor/vines0"));
            _textures.Add("wall", Content.Load<Texture2D>("wall/vines0"));

            _player.Texture = Content.Load<Texture2D>("player/base/human_m");

            csMapbuilder mpbuild = new csMapbuilder(WIDTH, HEIGHT)
            {
                MaxRooms = 20
            };
            int[,] randomMap = new int[WIDTH,HEIGHT];
            if (mpbuild.Build_ConnectedStartRooms() == true)
            {
                randomMap = mpbuild.map;
            }

            bool playerPosSet = false;

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (randomMap[i, j] == 0)
                    {
                        _map[i, j] = new MapTile()
                        {
                            Texture = _textures.FirstOrDefault(t => t.Key == "floor").Value
                        };
                        if (!playerPosSet && i > 50 && j > 50)
                        {
                            _player.Position = new Position(i*TILE_WIDTH, j*TILE_HEIGHT);
                            playerPosSet = true;
                            continue;
                        }
                        if (r.Next(0,3) == 1 && _map[i,j].EntityTexture == null && enemyCounter < ENEMY_AMOUNT && !IsEnemyNearby(i,j))
                        {
                            int enemyRandSelector = r.Next(0, 10);
                            var kvp = new KeyValuePair<string, Enemy>();
                            if (enemyRandSelector >= 0 && enemyRandSelector < 7)
                            {
                                //elf
                                kvp = new KeyValuePair<string, Enemy>(
                                "elf" + Guid.NewGuid().ToString().Substring(0, 10),
                                new Dwarf()
                                {
                                    Texture = Content.Load<Texture2D>("enemy/elf"),
                                    Position = new Position(i * TILE_WIDTH, j * TILE_HEIGHT)
                                });
                            }
                            else if(enemyRandSelector >= 7)
                            {
                                //dwarf
                                kvp = new KeyValuePair<string, Enemy>(
                                "dwarf" + Guid.NewGuid().ToString().Substring(0, 10),
                                new Dwarf()
                                {
                                    Texture = Content.Load<Texture2D>("enemy/dwarf"),
                                    Position = new Position(i * TILE_WIDTH, j * TILE_HEIGHT)
                                });
                            }
                            _map[i, j].EntityTexture = kvp.Value.Texture;
                            _map[i, j].EntityName = kvp.Key;
                            _enemies.Add(kvp.Key, kvp.Value);
                            enemyCounter++;
                        }
                    }
                    else
                    {
                        _map[i, j] = new MapTile()
                        {
                            Texture = _textures.FirstOrDefault(t => t.Key == "wall").Value
                        };
                    }
                }
            }

            _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture = _player.Texture;
            //foreach (var enemy in _enemies)
            //{
            //    _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityTexture =
            //        enemy.Value.Texture;
            //    _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityName =
            //        enemy.Key;
            //}


            // Load Healthbars
            DamageDescriber.AlmostDead = Content.Load<Texture2D>("health/almost");
            DamageDescriber.SeverelyDamaged = Content.Load<Texture2D>("health/severely");
            DamageDescriber.HeavilyDamaged = Content.Load<Texture2D>("health/heavily");
            DamageDescriber.ModeratelyDamaged = Content.Load<Texture2D>("health/moderately");
            DamageDescriber.LightlyDamaged = Content.Load<Texture2D>("health/lightly");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _textures.Clear();
            _enemies.Clear();
            _map = new MapTile[WIDTH,HEIGHT];
            _player = new Player();
            enemyCounter = 0;
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
            {
                if (VerifyMovement(-1, 0))
                {
                    _player.Position.X -= TILE_WIDTH;
                    _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X / TILE_WIDTH)+1, _player.Position.Y / TILE_HEIGHT].EntityTexture = null;
                }
                else if (IsEnemyNext(-1, 0))
                {
                    if (Attack(-1, 0))
                    {
                        _player.Position.X -= TILE_WIDTH;
                        _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                        _map[(_player.Position.X / TILE_WIDTH) + 1, _player.Position.Y / TILE_HEIGHT].EntityTexture = null;
                    }
                }
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (VerifyMovement(1, 0))
                {
                    _player.Position.X += TILE_WIDTH;
                    _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X / TILE_WIDTH)-1, _player.Position.Y / TILE_HEIGHT].EntityTexture = null;
                }
                else if (IsEnemyNext(1, 0))
                {
                    if (Attack(1, 0))
                    {
                        _player.Position.X += TILE_WIDTH;
                        _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                        _map[(_player.Position.X / TILE_WIDTH) - 1, _player.Position.Y / TILE_HEIGHT].EntityTexture = null;
                    }
                }
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (VerifyMovement(0, 1))
                {
                    _player.Position.Y += TILE_HEIGHT;
                    _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X / TILE_WIDTH), (_player.Position.Y / TILE_HEIGHT) - 1].EntityTexture = null;
                }
                else if (IsEnemyNext(0, 1))
                {
                    if (Attack(0, 1))
                    {
                        _player.Position.Y += TILE_HEIGHT;
                        _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                        _map[(_player.Position.X / TILE_WIDTH), (_player.Position.Y / TILE_HEIGHT) - 1].EntityTexture = null;
                    }
                }
                MoveEnemies();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (VerifyMovement(0, -1))
                {
                    _player.Position.Y -= TILE_HEIGHT;
                    _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X/TILE_WIDTH), (_player.Position.Y/TILE_HEIGHT) + 1].EntityTexture = null;
                }
                else if (IsEnemyNext(0, -1))
                {
                    if (Attack(0, -1))
                    {
                        _player.Position.Y -= TILE_HEIGHT;
                        _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].EntityTexture = _player.Texture;
                        _map[(_player.Position.X / TILE_WIDTH), (_player.Position.Y / TILE_HEIGHT) + 1].EntityTexture = null;
                    }
                }
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

            for (int i = -10; i < FOV/2; i++)
            {
                var x = (_player.Position.X/TILE_WIDTH) + i;
                if (x >= WIDTH)
                    continue;
                if(x < 0)
                    continue;
                for (int j = -10; j < FOV/2; j++)
                {
                    var y = (_player.Position.Y/TILE_HEIGHT) + j;
                    if(y < 0)
                        continue;
                    if (y >= HEIGHT)
                        continue;
                    _spriteBatch.Draw(_map[x,y].Texture, new Vector2((i+10)*TILE_WIDTH, (j+10)*TILE_HEIGHT));
                    if (_map[x, y].EntityTexture != null)
                    {
                        var pos = new Vector2((i + 10)*TILE_WIDTH, (j + 10)*TILE_HEIGHT);
                        _spriteBatch.Draw(_map[x, y].EntityTexture, pos);
                        if (_map[x, y].EntityTexture == _player.Texture)
                        {
                            DrawHealthBar(_player.Health, _player.MaxHealth, pos);
                        }
                        else
                        {
                            Enemy enemy = GetEnemy(x, y);
                            DrawHealthBar(enemy.Health, enemy.MaxHealth, pos);
                        }
                    }
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void MoveEnemies()
        {
            foreach (var enemy in _enemies)
            {
                int[] mov = enemy.Value.EnemeKi.Decide(_map, enemy.Value, enemy.Key, _player);
                _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityTexture = null;
                enemy.Value.Position.X += mov[0];
                enemy.Value.Position.Y += mov[1];
                //enemy.Value.Position.X += 0; // debug
                //enemy.Value.Position.Y += 0; // debug
                _map[enemy.Value.Position.X / TILE_WIDTH, enemy.Value.Position.Y / TILE_HEIGHT].EntityTexture = enemy.Value.Texture;
                _map[enemy.Value.Position.X / TILE_WIDTH, enemy.Value.Position.Y / TILE_HEIGHT].EntityName =
                    enemy.Key;
            }
        }

        bool VerifyMovement(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);

            bool isValid = _map[virtPos[0], virtPos[1]].Texture.Name != "wall/vines0";
            return isValid && _enemies.All(enemy => !IsCovering(enemy.Value.Position, new Position(virtPos[0]*TILE_WIDTH, virtPos[1]*TILE_HEIGHT)));
        }

        bool Attack(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);
            if (
                !_enemies.Any(
                    enemy =>
                            IsCovering(enemy.Value.Position, new Position(virtPos[0]*TILE_WIDTH, virtPos[1]*TILE_HEIGHT))))
                return false;
            Console.WriteLine("Attack");

            var target = GetEnemy(virtPos);

            _player.Health -= target.Value.Attack;
            target.Value.Health -= _player.Attack;

            if (IsPlayerDead())
            {
                Die();
                return false;
            }
            if (target.Value.Health > 0) return false;
            KillEnemey(target.Key);
            return true;
        }

        bool IsCovering(Position p1, Position p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y)
                return true;
            return false;
        }

        bool IsEnemyNext(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);
            return _map[virtPos[0], virtPos[1]].EntityTexture != null && _map[virtPos[0], virtPos[1]].EntityTexture.Name.StartsWith("enemy/");
        }

        int[] GetVirtualPostition(int x, int y)
        {
            int virtPosX = 0;
            int virtPosY = 0;
            if (x != 0)
            {
                virtPosX = (_player.Position.X + (x * TILE_WIDTH)) / TILE_WIDTH;
                virtPosY = _player.Position.Y / TILE_HEIGHT;
            }
            else if (y != 0)
            {
                virtPosX = _player.Position.X / TILE_WIDTH;
                virtPosY = (_player.Position.Y + (y * TILE_HEIGHT)) / TILE_HEIGHT;
            }

            return new[] {virtPosX, virtPosY};
        }

        Enemy GetEnemy(int x, int y)
        {
            x *= TILE_WIDTH;
            y *= TILE_HEIGHT;

            return (from enemy in _enemies where IsCovering(enemy.Value.Position, new Position(x, y)) select enemy.Value).FirstOrDefault();
        }

        KeyValuePair<string, Enemy> GetEnemy(int[] virtPos)
        {
            virtPos[0] *= TILE_WIDTH;
            virtPos[1] *= TILE_HEIGHT;
            return (from enemy in _enemies where IsCovering(enemy.Value.Position, new Position(virtPos[0], virtPos[1])) select enemy).FirstOrDefault();
        }

        bool IsPlayerDead()
        {
            return _player.Health <= 0;
        }

        void Die()
        {
            UnloadContent();
            LoadContent();
        }

        void KillEnemey(string name)
        {
            Drop(_enemies[name].XPReward); // enemy list is fast empty
            _enemies.Remove(name);
        }

        void Drop(int xp)
        {
            // give xp
            _player.XP += xp;
            // drop item
            _player.Inventory.Items.Add(new LargeSword());
            UpdatePlayerStats(new LargeSword());
        }

        void UpdatePlayerStats(InventoryItem ii = null)
        {
            if (ii != null)
            {
                // Update Player Stats with Item Stats
                switch (ii.ItemType)
                {
                    case ItemType.Weapon:
                        _player.Attack += ii.Damage;
                        break;
                    case ItemType.HealthSlot:
                        _player.MaxHealth += ii.Health;
                        break;
                    case ItemType.Armor:
                        _player.Shield += ii.Shield;
                        break;
                }
            }
            _player.Inventory.Amount = _player.Inventory.Items.Count;
            if (_player.XP >= _player.Level.XP)
            {
                int xp = _player.Level.XP - _player.XP;
                LevelUp();
                _player.XP += xp;
            }
        }

        void LevelUp()
        {
            if(_player.Level.Level == 1) return; // max level here
            int lvl = _player.Level.Level;
            int origDamage = _player.Level.Damage;
            int origShield = _player.Level.Shield;
            _player.Level = (LevelInfo)Activator.CreateInstance(Type.GetType("RogueLike.Levels.Level" + (lvl + 1)));
            _player.Attack -= origDamage;
            _player.Attack += _player.Level.Damage;
            _player.MaxHealth += _player.Level.Health;
            _player.Health = _player.MaxHealth;
            _player.Shield -= origShield;
            _player.Shield += _player.Level.Shield;
        }

        void DrawHealthBar(int health, int maxHealth, Vector2 pos)
        {
            if (health <= maxHealth / 5)
                _spriteBatch.Draw(DamageDescriber.AlmostDead, pos);
            else if (health <= maxHealth / 4)
                _spriteBatch.Draw(DamageDescriber.SeverelyDamaged, pos);
            else if (health <= maxHealth / 3)
                _spriteBatch.Draw(DamageDescriber.HeavilyDamaged, pos);
            else if (health <= maxHealth / 2)
                _spriteBatch.Draw(DamageDescriber.ModeratelyDamaged, pos);
            else if (health < maxHealth)
                _spriteBatch.Draw(DamageDescriber.LightlyDamaged, pos);
        }

        bool IsEnemyNearby(int x, int y)
        {
            // x
            if (x + 1 < WIDTH && _map[(x + 1), y] != null && _map[(x + 1), y].EntityTexture != null && _map[(x + 1), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && _map[(x + 2), y] != null && _map[(x + 2), y].EntityTexture != null && _map[(x + 2), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && _map[(x + 3), y] != null && _map[(x + 3), y].EntityTexture != null && _map[(x + 3), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 1 >= 0 && _map[x - 1, y] != null && _map[x - 1, y].EntityTexture != null && _map[x - 1, y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && _map[x - 2, y] != null && _map[x - 2, y].EntityTexture != null && _map[x - 2, y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && _map[x - 3, y] != null && _map[x - 3, y].EntityTexture != null && _map[x - 3, y].EntityTexture.Name.Contains("enemy/"))
                return true;

            // y
            if (y + 1 < HEIGHT && _map[x, y + 1] != null && _map[x, y + 1].EntityTexture != null && _map[x, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y + 2 < HEIGHT && _map[x, y + 2] != null && _map[x, y + 2].EntityTexture != null && _map[x, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y + 3 < HEIGHT && _map[x, y + 3] != null && _map[x, y + 3].EntityTexture != null && _map[x, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 1 >= 0 && _map[x, y - 1] != null && _map[x, y - 1].EntityTexture != null && _map[x, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 2 >= 0 && _map[x, y - 2] != null && _map[x, y - 2].EntityTexture != null && _map[x, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 3 >= 0 && _map[x, y - 3] != null && _map[x, y - 3].EntityTexture != null && _map[x, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            // diag
            if (x + 1 < WIDTH && y + 1 < HEIGHT && _map[x + 1, y + 1] != null && _map[x + 1, y + 1].EntityTexture != null && _map[x + 1, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && y + 2 < HEIGHT && _map[x + 2, y + 2] != null && _map[x + 2, y + 2].EntityTexture != null && _map[x + 2, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && y + 3 < HEIGHT && _map[x + 3, y + 3] != null && _map[x + 3, y + 3].EntityTexture != null && _map[x + 3, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 1 >= 0 && y - 1 >= 0 && _map[x - 1, y - 1] != null && _map[x - 1, y - 1].EntityTexture != null && _map[x - 1, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && y - 2 >= 0 && _map[x - 2, y - 2] != null && _map[x - 2, y - 2].EntityTexture != null && _map[x - 2, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && y - 3 >= 0 && _map[x - 3, y - 3] != null && _map[x - 3, y - 3].EntityTexture != null && _map[x - 3, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            // diag
            if (x - 1 >= 0 && y + 1 < HEIGHT && _map[x - 1, y + 1] != null && _map[x - 1, y + 1].EntityTexture != null && _map[x - 1, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && y + 2 < HEIGHT && _map[x - 2, y + 2] != null && _map[x - 2, y + 2].EntityTexture != null && _map[x - 2, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && y + 3 < HEIGHT && _map[x - 3, y + 3] != null && _map[x - 3, y + 3].EntityTexture != null && _map[x - 3, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 1 < WIDTH && y - 1 >= 0 && _map[x + 1, y - 1] != null && _map[x + 1, y - 1].EntityTexture != null && _map[x + 1, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && y - 2 >= 0 && _map[x + 2, y - 2] != null && _map[x + 2, y - 2].EntityTexture != null && _map[x + 2, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && y - 3 >= 0 && _map[x + 3, y - 3] != null && _map[x + 3, y - 3].EntityTexture != null && _map[x + 3, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            return false;
        }

        List<Enemy> GetEnemies()
        {
            List<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => string.Equals(t.Namespace, "RogueLike.Enemies", StringComparison.Ordinal)).ToArray().ToList();
            return (from type in types where type.Name != "Enemy" select (Enemy) Activator.CreateInstance(type)).ToList();
        }
    }
}
