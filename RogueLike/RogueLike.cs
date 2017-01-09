using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueLike.Enemies;
using RogueLike.Inventory;
using RogueLike.Levels;
using RogueLikeMapBuilder;
using Color = Microsoft.Xna.Framework.Color;

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
        private const int FOV = 20; // 20 is recommended
        private const int PLAYER_FOV = 1; // 1 or 2 is best
        private const int EQUIPMENT_WIDTH = TILE_WIDTH*4;
        private const int STATUS_HEIGHT = TILE_HEIGHT;

        private const int ENEMY_AMOUNT = 150; // around 150 is a good number
        private const int ENEMY_RADIUS = 3;
        private int enemyCounter = 0;
        private bool drawOverlay = false;
        Random r = new Random();

        MapTile[,] _map = new MapTile[WIDTH, HEIGHT];

        private Player _player = new Player();
        readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _thumbWeapons = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _thumbShields = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _thumbHelmets = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _thumbMails = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _thumbBoots = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _weapons = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _shields = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _boots = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _mails = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, Texture2D> _helmets = new Dictionary<string, Texture2D>();
        readonly Dictionary<int, Gold> _gold = new Dictionary<int, Gold>();
        readonly Dictionary<string, InventoryItem> _items = new Dictionary<string, InventoryItem>();
        readonly Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();
        private List<Tuple<string, Color, int>> _messageQueue = new List<Tuple<string, Color, int>>();
        List<Enemy> _enemyTypes = new List<Enemy>();
        List<InventoryItem> _itemTypes = new List<InventoryItem>();
        private SpriteFont _font;

        readonly InternalSettings _internalSettings = new InternalSettings();

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
            _graphics.PreferredBackBufferWidth = FOV*TILE_WIDTH + EQUIPMENT_WIDTH;
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
            _itemTypes = GetItems(); // Load the InventoryItem classes

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            /* Load Map textures */
            _textures.Add("floor", Content.Load<Texture2D>("floor/vines0"));
            _textures.Add("wall", Content.Load<Texture2D>("wall/vines0"));
            _textures.Add("unseen", Content.Load<Texture2D>("floor/unseen"));
            _textures.Add("blood_red", Content.Load<Texture2D>("enemy/additions/blood_red"));
            _textures.Add("blood_green", Content.Load<Texture2D>("enemy/additions/blood_green"));
            _textures.Add("slot", Content.Load<Texture2D>("player/equipment/slot"));

            /* Load internal settings */
            _internalSettings.Cursor.Texture = Content.Load<Texture2D>("user/cursor");

            /* Load thumbnails for weapons */
            _thumbWeapons.Add("greatsword", Content.Load<Texture2D>("weapons/thumbnails/greatsword1"));

            /* Load thumbnails for shields */
            _thumbShields.Add("shield_knight_gray", Content.Load<Texture2D>("shields/thumbnails/large_shield1"));

            /* Load thumbnails for mails */
            _thumbMails.Add("leather_armour", Content.Load<Texture2D>("armour/thumbnails/mail/leather_armour1"));
            _thumbMails.Add("leather_armour2", Content.Load<Texture2D>("armour/thumbnails/mail/leather_armour2"));
            _thumbMails.Add("leather_armour3", Content.Load<Texture2D>("armour/thumbnails/mail/leather_armour3"));

            /* Load thumbnails for boots */
            _thumbBoots.Add("boot_middle_brown3", Content.Load<Texture2D>("armour/thumbnails/boots/boots1_brown"));

            /* Load thumbnails for helmets */
            _thumbHelmets.Add("iron_helmet1", Content.Load<Texture2D>("armour/thumbnails/helmet/helmet3"));
            _thumbHelmets.Add("iron_helmet2", Content.Load<Texture2D>("armour/thumbnails/helmet/helmet3"));
            _thumbHelmets.Add("iron_helmet3", Content.Load<Texture2D>("armour/thumbnails/helmet/helmet3"));

            /* Load weapons */
            _weapons.Add("greatsword", Content.Load<Texture2D>("weapons/skins/great_sword"));

            /* Load shields */
            _shields.Add("shield_knight_gray", Content.Load<Texture2D>("shields/skins/shield_knight_gray"));

            /* Load helmets */
            _helmets.Add("iron_helmet1", Content.Load<Texture2D>("armour/skins/helmet/iron1"));
            _helmets.Add("iron_helmet2", Content.Load<Texture2D>("armour/skins/helmet/iron2"));
            _helmets.Add("iron_helmet3", Content.Load<Texture2D>("armour/skins/helmet/iron3"));

            /* Load boots */
            _boots.Add("boot_middle_brown3", Content.Load<Texture2D>("armour/skins/boots/middle_brown3"));

            /* Load mails */
            _mails.Add("leather_armour", Content.Load<Texture2D>("armour/skins/mail/leather_armour"));
            _mails.Add("leather_armour2", Content.Load<Texture2D>("armour/skins/mail/leather_armour2"));
            _mails.Add("leather_armour3", Content.Load<Texture2D>("armour/skins/mail/leather_armour3"));

            /* Load items */
            _items.Add("greatsword", new GreatSword());
            _items.Add("shield_knight_gray", new KnightShield());
            _items.Add("leather_armour", new LeatherArmour1());
            _items.Add("leather_armour2", new LeatherArmour2());
            _items.Add("leather_armour3", new LeatherArmour3());
            _items.Add("iron_helmet1", new IronHelmet1());
            _items.Add("iron_helmet2", new IronHelmet2());
            _items.Add("iron_helmet3", new IronHelmet3());
            _items.Add("boot_middle_brown3", new BootsMiddleBrown3());
            _items.Add("gold", new Gold());

            /* Load gold */
            _gold.Add(1, new Gold() { Texture = Content.Load<Texture2D>("money/01")});
            _gold.Add(2, new Gold() { Texture = Content.Load<Texture2D>("money/02")});
            _gold.Add(3, new Gold() { Texture = Content.Load<Texture2D>("money/03")});
            _gold.Add(4, new Gold() { Texture = Content.Load<Texture2D>("money/04")});
            _gold.Add(5, new Gold() { Texture = Content.Load<Texture2D>("money/05")});
            _gold.Add(6, new Gold() { Texture = Content.Load<Texture2D>("money/06")});
            _gold.Add(7, new Gold() { Texture = Content.Load<Texture2D>("money/07")});
            _gold.Add(8, new Gold() { Texture = Content.Load<Texture2D>("money/08")});
            _gold.Add(9, new Gold() { Texture = Content.Load<Texture2D>("money/09")});
            _gold.Add(10, new Gold() { Texture = Content.Load<Texture2D>("money/10")});
            _gold.Add(16, new Gold() { Texture = Content.Load<Texture2D>("money/16")});
            _gold.Add(19, new Gold() { Texture = Content.Load<Texture2D>("money/19")});
            _gold.Add(23, new Gold() { Texture = Content.Load<Texture2D>("money/23")});
            _gold.Add(25, new Gold() { Texture = Content.Load<Texture2D>("money/25")});

            _player.Texture = Content.Load<Texture2D>("player/base/human_m");

            // Load font
            _font = Content.Load<SpriteFont>("font/SDS");

            csMapbuilder mpbuild = new csMapbuilder(WIDTH, HEIGHT)
            {
                MaxRooms = 20,
                Corridor_Max = 5,
                Corridor_Min = 2,
                RoomDistance = 5,
                Room_Max = new Size(7,7),
                Room_Min = new Size(4,4),
                Corridor_MaxTurns = 5
            };
            int[,] randomMap = new int[WIDTH, HEIGHT];
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
                        if (r.Next(0, 8) == 1 && _map[i, j].EntityTexture == null && enemyCounter < ENEMY_AMOUNT &&
                            !IsEnemyNearby(i, j)) // around 10 is a good number
                        {
                            int enemyRandSelector = r.Next(0, _enemyTypes.Count);
                            var type = _enemyTypes[enemyRandSelector].GetType();
                            var enemy = (Enemy)Activator.CreateInstance(type);
                            enemy.Position = new Position(i*TILE_WIDTH, j*TILE_HEIGHT);
                            enemy.Texture = Content.Load<Texture2D>(enemy.TextureName);
                            enemy.Inventory.Items.AddRange(GenerateRandomInventory());
                            var kvp = new KeyValuePair<string, Enemy>(enemy.Name + Guid.NewGuid(), enemy);
                            _map[i, j].EntityTexture = kvp.Value.Texture;
                            _map[i, j].EntityName = kvp.Key;
                            _enemies.Add(kvp.Key, kvp.Value);
                            enemyCounter++;
                        }
                        if (r.Next(0, 15) == 6)
                        {
                            _map[i, j].Gold = new Gold();
                            var gold = _gold.ElementAt(r.Next(_gold.Count));
                            _map[i, j].AdditionalTextures.Add(gold.Value.Texture);
                            var ggold = new Gold();
                            _map[i, j].Gold = ggold;
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
            CalculateUnseen();

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
            // TODO: Don't unload unnecessary things
            _textures.Clear();
            _enemies.Clear();
            _map = new MapTile[WIDTH, HEIGHT];
            _player = new Player();
            _weapons.Clear();
            _shields.Clear();
            _boots.Clear();
            _helmets.Clear();
            _mails.Clear();
            _thumbBoots.Clear();
            _thumbHelmets.Clear();
            _thumbMails.Clear();
            _thumbShields.Clear();
            _thumbWeapons.Clear();
            _items.Clear();
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
                _framesPassed = 0;
            _framesPassed++;

            var temp = (from message in _messageQueue where message.Item3 < 180 select new Tuple<string, Color, int>(message.Item1, message.Item2, message.Item3 + 1)).ToList();
            _messageQueue = temp;

            MouseState mouseState = Mouse.GetState();
            _internalSettings.Cursor.Position = new Vector2(mouseState.X, mouseState.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                drawOverlay = drawOverlay == false;
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (VerifyMovement(-1, 0))
                {
                    _player.Position.X -= TILE_WIDTH;
                    _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X/TILE_WIDTH) + 1, _player.Position.Y/TILE_HEIGHT].EntityTexture = null;
                }
                else if (IsEnemyNext(-1, 0))
                {
                    if (Attack(-1, 0))
                    {
                        _player.Position.X -= TILE_WIDTH;
                        _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture =
                            _player.Texture;
                        _map[(_player.Position.X/TILE_WIDTH) + 1, _player.Position.Y/TILE_HEIGHT].EntityTexture = null;
                    }
                }
                MoveEnemies();
                CalculateUnseen();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (VerifyMovement(1, 0))
                {
                    _player.Position.X += TILE_WIDTH;
                    _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X/TILE_WIDTH) - 1, _player.Position.Y/TILE_HEIGHT].EntityTexture = null;
                }
                else if (IsEnemyNext(1, 0))
                {
                    if (Attack(1, 0))
                    {
                        _player.Position.X += TILE_WIDTH;
                        _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture =
                            _player.Texture;
                        _map[(_player.Position.X/TILE_WIDTH) - 1, _player.Position.Y/TILE_HEIGHT].EntityTexture = null;
                    }
                }
                MoveEnemies();
                CalculateUnseen();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (VerifyMovement(0, 1))
                {
                    _player.Position.Y += TILE_HEIGHT;
                    _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X/TILE_WIDTH), (_player.Position.Y/TILE_HEIGHT) - 1].EntityTexture = null;
                }
                else if (IsEnemyNext(0, 1))
                {
                    if (Attack(0, 1))
                    {
                        _player.Position.Y += TILE_HEIGHT;
                        _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture =
                            _player.Texture;
                        _map[(_player.Position.X/TILE_WIDTH), (_player.Position.Y/TILE_HEIGHT) - 1].EntityTexture = null;
                    }
                }
                MoveEnemies();
                CalculateUnseen();
            }
            if (_framesPassed > 5 && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (VerifyMovement(0, -1))
                {
                    _player.Position.Y -= TILE_HEIGHT;
                    _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture = _player.Texture;
                    _map[(_player.Position.X/TILE_WIDTH), (_player.Position.Y/TILE_HEIGHT) + 1].EntityTexture = null;
                }
                else if (IsEnemyNext(0, -1))
                {
                    if (Attack(0, -1))
                    {
                        _player.Position.Y -= TILE_HEIGHT;
                        _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].EntityTexture =
                            _player.Texture;
                        _map[(_player.Position.X/TILE_WIDTH), (_player.Position.Y/TILE_HEIGHT) + 1].EntityTexture = null;
                    }
                }
                MoveEnemies();
                CalculateUnseen();
            }

            CheckForPickup();
            _player.Health = 1000;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            for (int i = FOV/2*-1; i < FOV/2; i++)
            {
                var x = (_player.Position.X/TILE_WIDTH) + i;
                if (x >= WIDTH)
                    continue;
                if (x < 0)
                    continue;
                for (int j = FOV / 2 * -1; j < FOV/2; j++)
                {
                    var y = (_player.Position.Y/TILE_HEIGHT) + j;
                    if (y < 0)
                        continue;
                    if (y >= HEIGHT)
                        continue;
                    if (!_map[x, y].IsUnseen)
                    {
                        _spriteBatch.Draw(_map[x, y].Texture, new Vector2((i + FOV / 2) *TILE_WIDTH, (j + FOV / 2) *TILE_HEIGHT));
                        var pos = new Vector2((i + FOV / 2) *TILE_WIDTH, (j + FOV / 2) *TILE_HEIGHT);

                        if (_map[x, y].AdditionalTextures.Count > 0)
                        {
                            // blood, etc...
                            bool drawMoney = false;
                            foreach (var texture in _map[x, y].AdditionalTextures)
                            {
                                if (!texture.Name.Contains("money"))
                                    _spriteBatch.Draw(texture, pos);
                                else
                                    drawMoney = true;
                            }
                            if (drawMoney)
                            {
                                foreach (var texture in _map[x, y].AdditionalTextures)
                                {
                                    if (!texture.Name.Contains("money")) continue;
                                    _spriteBatch.Draw(texture, pos);
                                    break;
                                }
                            }
                        }

                        if (_map[x, y].EntityTexture != null)
                        {
                            _spriteBatch.Draw(_map[x, y].EntityTexture, pos);
                            if (_map[x, y].EntityTexture == _player.Texture)
                            {
                                foreach (var item in _player.Equipment.Skins.EquipmentSet)
                                {
                                    _spriteBatch.Draw(item, pos);
                                }
                                DrawHealthBar(_player.Health, _player.MaxHealth, pos);
                            }
                            else
                            {
                                Enemy enemy = GetEnemy(x, y);
                                DrawHealthBar(enemy.Health, enemy.MaxHealth, pos);
                            }
                        }
                    }
                    else
                    {
                        _spriteBatch.Draw(_textures.FirstOrDefault(t => t.Key == "unseen").Value,
                            new Vector2((i + FOV/2)*TILE_WIDTH, (j + FOV/2)*TILE_HEIGHT));
                    }
                }
            }

            _spriteBatch.Draw(_internalSettings.Cursor.Texture, _internalSettings.Cursor.Position, Color.White);

            DrawEquipment();

            DrawStatus();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void MoveEnemies()
        {
            foreach (var enemy in _enemies)
            {
                int[] mov = enemy.Value.EnemyKi.Decide(_map, enemy.Value, enemy.Key, _player);
                _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityTexture = null;
                enemy.Value.Position.X += mov[0];
                enemy.Value.Position.Y += mov[1];
                _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityTexture =
                    enemy.Value.Texture;
                _map[enemy.Value.Position.X/TILE_WIDTH, enemy.Value.Position.Y/TILE_HEIGHT].EntityName =
                    enemy.Key;
            }
        }

        bool VerifyMovement(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);

            bool isValid = _map[virtPos[0], virtPos[1]].Texture.Name != "wall/vines0";
            Enemy enemy = GetEnemy(virtPos[0], virtPos[1]);
            return isValid && enemy == null;
        }

        bool Attack(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);
            Enemy enemy = GetEnemy(virtPos[0], virtPos[1]);
            if (enemy == null)
                return false;
            Console.WriteLine("Attack");
            _messageQueue.Add(new Tuple<string, Color, int>("You attacked " + enemy.Name, Color.Red, 0));
            _player.Health -= enemy.Attack;
            enemy.Health -= _player.Attack;

            if (IsPlayerDead())
            {
                Die();
                return false;
            }
            if (enemy.Health > 0) return false;
            KillEnemy(enemy);
            return true;
        }

        bool IsEnemyNext(int x, int y)
        {
            int[] virtPos = GetVirtualPostition(x, y);
            return _map[virtPos[0], virtPos[1]].EntityTexture != null &&
                   _map[virtPos[0], virtPos[1]].EntityTexture.Name.StartsWith("enemy/");
        }

        int[] GetVirtualPostition(int x, int y)
        {
            int virtPosX = 0;
            int virtPosY = 0;
            if (x != 0)
            {
                virtPosX = (_player.Position.X + (x*TILE_WIDTH))/TILE_WIDTH;
                virtPosY = _player.Position.Y/TILE_HEIGHT;
            }
            else if (y != 0)
            {
                virtPosX = _player.Position.X/TILE_WIDTH;
                virtPosY = (_player.Position.Y + (y*TILE_HEIGHT))/TILE_HEIGHT;
            }

            return new[] {virtPosX, virtPosY};
        }

        Enemy GetEnemy(int x, int y)
        {
            return _map[x, y].EntityName == null ? null : _enemies[_map[x, y].EntityName];
        }

        string GetEnemyName(int x, int y)
        {
            return _map[x, y].EntityName;
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

        void KillEnemy(string name)
        {
            Drop(_enemies[name].XPReward, _enemies[name].Inventory.Items);
            _map[_enemies[name].Position.X/TILE_WIDTH, _enemies[name].Position.Y/TILE_HEIGHT].AdditionalTextures.Add(
                _textures["blood_red"]);
            _enemies.Remove(name);
        }

        void KillEnemy(Enemy enemy)
        {
            _messageQueue.Add(new Tuple<string, Color, int>("You killed " + enemy.Name, Color.Green, 0));
            Drop(enemy.XPReward, enemy.Inventory.Items); 
            _map[enemy.Position.X / TILE_WIDTH, enemy.Position.Y / TILE_HEIGHT].AdditionalTextures.Add(
                _textures["blood_red"]);
            _enemies.Remove(GetEnemyName(enemy.Position.X / TILE_WIDTH, enemy.Position.Y / TILE_HEIGHT));
            _map[enemy.Position.X / TILE_WIDTH, enemy.Position.Y / TILE_HEIGHT].EntityTexture = null;
            _map[enemy.Position.X / TILE_WIDTH, enemy.Position.Y / TILE_HEIGHT].EntityName = null;
        }

        void Drop(int xp, List<InventoryItem> items)
        {
            // give xp
            _player.XP += xp;
            _messageQueue.Add(new Tuple<string, Color, int>("You received " + xp + " XP", Color.Blue, 0));
            // drop item
            foreach (var item1 in items)
            {
                KeyValuePair<string, InventoryItem> ii = new KeyValuePair<string, InventoryItem>();
                foreach (var i in _items)
                {
                    if (i.Value.Name == item1.Name)
                    {
                        ii = i;
                        ii.Value.Value = i.Value.Value;
                        break;
                    }
                }
                _messageQueue.Add(new Tuple<string, Color, int>("You received " + ii.Value.Name, Color.Blue, 0));
                _player.Inventory.Items.Add(ii.Value);
                switch (ii.Value.ItemType)
                {
                    case ItemType.Weapon:
                        if (_player.Equipment.Thumbnails.Weapon == null)
                        {
                            _player.Equipment.Thumbnails.Weapon = _thumbWeapons[ii.Key];
                            _player.Equipment.Skins.Weapon = _weapons[ii.Key];
                        }
                        break;
                    case ItemType.Shield:
                        if (_player.Equipment.Thumbnails.Shield == null)
                        {
                            _player.Equipment.Thumbnails.Shield = _thumbShields[ii.Key];
                            _player.Equipment.Skins.Shield = _shields[ii.Key];
                        }
                        break;
                    case ItemType.Boots:
                        if (_player.Equipment.Thumbnails.Boots == null)
                        {
                            _player.Equipment.Thumbnails.Boots = _thumbBoots[ii.Key];
                            _player.Equipment.Skins.Boots = _boots[ii.Key];
                        }
                        break;
                    case ItemType.Helmet:
                        if (_player.Equipment.Thumbnails.Helmet == null)
                        {
                            _player.Equipment.Thumbnails.Helmet = _thumbHelmets[ii.Key];
                            _player.Equipment.Skins.Helmet = _helmets[ii.Key];
                        }
                        break;
                    case ItemType.Mail:
                        if (_player.Equipment.Thumbnails.Mail == null)
                        {
                            _player.Equipment.Thumbnails.Mail = _thumbMails[ii.Key];
                            _player.Equipment.Skins.Mail = _mails[ii.Key];
                        }
                        break;
                    case ItemType.Money:
                        if (_player.Inventory.Items.Count > 0)
                        {
                            foreach (var item in _player.Inventory.Items)
                            {
                                if (item.Name == "Gold")
                                {
                                    item.Value += ii.Value.Value;
                                }
                            }
                        }
                        break;
                }
                int index = _player.Inventory.Items.FindIndex(item => item.Name == ii.Value.Name);
                if (index < 0)
                {
                    if (ii.Value.Name != "Gold")
                    {
                        _player.Inventory.Items.Add(ii.Value);
                    }
                }
                else
                {
                    _player.Inventory.Items[index].Amount++;
                }
                UpdatePlayerStats(ii.Value);
            }
            _player.Equipment.Thumbnails.Update();
            _player.Equipment.Skins.Update();
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
                    case ItemType.Shield: case ItemType.Helmet:
                    case ItemType.Boots: case ItemType.Mail:
                        _player.Shield += ii.Shield;
                        break;
                }
            }
            if (_player.XP >= _player.Level.XP)
            {
                int xp = _player.Level.XP - _player.XP;
                LevelUp();
                _player.XP += xp;
            }
        }

        void LevelUp()
        {
            if (_player.Level.Level == 1) return; // max level here
            _messageQueue.Add(new Tuple<string, Color, int>("You reached level " + _player.Level.Level+1, Color.Blue, 0));
            int lvl = _player.Level.Level;
            int origDamage = _player.Level.Damage;
            int origShield = _player.Level.Shield;
            _player.Level = (LevelInfo) Activator.CreateInstance(Type.GetType("RogueLike.Levels.Level" + (lvl + 1)));
            _player.Attack -= origDamage;
            _player.Attack += _player.Level.Damage;
            _player.MaxHealth += _player.Level.Health;
            _player.Health = _player.MaxHealth;
            _player.Shield -= origShield;
            _player.Shield += _player.Level.Shield;
        }

        void DrawHealthBar(int health, int maxHealth, Vector2 pos)
        {
            if (health <= maxHealth/5)
                _spriteBatch.Draw(DamageDescriber.AlmostDead, pos);
            else if (health <= maxHealth/4)
                _spriteBatch.Draw(DamageDescriber.SeverelyDamaged, pos);
            else if (health <= maxHealth/3)
                _spriteBatch.Draw(DamageDescriber.HeavilyDamaged, pos);
            else if (health <= maxHealth/2)
                _spriteBatch.Draw(DamageDescriber.ModeratelyDamaged, pos);
            else if (health < maxHealth)
                _spriteBatch.Draw(DamageDescriber.LightlyDamaged, pos);
        }

        bool IsEnemyNearby(int x, int y)
        {
            // x
            if (x + 1 < WIDTH && _map[(x + 1), y] != null && _map[(x + 1), y].EntityTexture != null &&
                _map[(x + 1), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && _map[(x + 2), y] != null && _map[(x + 2), y].EntityTexture != null &&
                _map[(x + 2), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && _map[(x + 3), y] != null && _map[(x + 3), y].EntityTexture != null &&
                _map[(x + 3), y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 1 >= 0 && _map[x - 1, y] != null && _map[x - 1, y].EntityTexture != null &&
                _map[x - 1, y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && _map[x - 2, y] != null && _map[x - 2, y].EntityTexture != null &&
                _map[x - 2, y].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && _map[x - 3, y] != null && _map[x - 3, y].EntityTexture != null &&
                _map[x - 3, y].EntityTexture.Name.Contains("enemy/"))
                return true;

            // y
            if (y + 1 < HEIGHT && _map[x, y + 1] != null && _map[x, y + 1].EntityTexture != null &&
                _map[x, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y + 2 < HEIGHT && _map[x, y + 2] != null && _map[x, y + 2].EntityTexture != null &&
                _map[x, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y + 3 < HEIGHT && _map[x, y + 3] != null && _map[x, y + 3].EntityTexture != null &&
                _map[x, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 1 >= 0 && _map[x, y - 1] != null && _map[x, y - 1].EntityTexture != null &&
                _map[x, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 2 >= 0 && _map[x, y - 2] != null && _map[x, y - 2].EntityTexture != null &&
                _map[x, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (y - 3 >= 0 && _map[x, y - 3] != null && _map[x, y - 3].EntityTexture != null &&
                _map[x, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            // diag
            if (x + 1 < WIDTH && y + 1 < HEIGHT && _map[x + 1, y + 1] != null &&
                _map[x + 1, y + 1].EntityTexture != null && _map[x + 1, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && y + 2 < HEIGHT && _map[x + 2, y + 2] != null &&
                _map[x + 2, y + 2].EntityTexture != null && _map[x + 2, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && y + 3 < HEIGHT && _map[x + 3, y + 3] != null &&
                _map[x + 3, y + 3].EntityTexture != null && _map[x + 3, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 1 >= 0 && y - 1 >= 0 && _map[x - 1, y - 1] != null && _map[x - 1, y - 1].EntityTexture != null &&
                _map[x - 1, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && y - 2 >= 0 && _map[x - 2, y - 2] != null && _map[x - 2, y - 2].EntityTexture != null &&
                _map[x - 2, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && y - 3 >= 0 && _map[x - 3, y - 3] != null && _map[x - 3, y - 3].EntityTexture != null &&
                _map[x - 3, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            // diag
            if (x - 1 >= 0 && y + 1 < HEIGHT && _map[x - 1, y + 1] != null && _map[x - 1, y + 1].EntityTexture != null &&
                _map[x - 1, y + 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 2 >= 0 && y + 2 < HEIGHT && _map[x - 2, y + 2] != null && _map[x - 2, y + 2].EntityTexture != null &&
                _map[x - 2, y + 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x - 3 >= 0 && y + 3 < HEIGHT && _map[x - 3, y + 3] != null && _map[x - 3, y + 3].EntityTexture != null &&
                _map[x - 3, y + 3].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 1 < WIDTH && y - 1 >= 0 && _map[x + 1, y - 1] != null && _map[x + 1, y - 1].EntityTexture != null &&
                _map[x + 1, y - 1].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 2 < WIDTH && y - 2 >= 0 && _map[x + 2, y - 2] != null && _map[x + 2, y - 2].EntityTexture != null &&
                _map[x + 2, y - 2].EntityTexture.Name.Contains("enemy/"))
                return true;
            if (x + 3 < WIDTH && y - 3 >= 0 && _map[x + 3, y - 3] != null && _map[x + 3, y - 3].EntityTexture != null &&
                _map[x + 3, y - 3].EntityTexture.Name.Contains("enemy/"))
                return true;

            return false;
        }

        List<Enemy> GetEnemies()
        {
            List<Type> types =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "RogueLike.Enemies", StringComparison.Ordinal))
                    .ToArray()
                    .ToList();
            return
                (from type in types where type.Name != "Enemy" select (Enemy) Activator.CreateInstance(type)).ToList();
        }

        List<InventoryItem> GetItems()
        {
            List<Type> types =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "RogueLike.Inventory", StringComparison.Ordinal))
                    .ToArray()
                    .ToList();
            return
                (from type in types where type.Name != "Inventory" && type.Name != "InventoryItem" && type.Name != "ItemType" select (InventoryItem)Activator.CreateInstance(type)).ToList();
        }

        void CalculateUnseen()
        {
            for (int i = PLAYER_FOV*-1; i <= PLAYER_FOV; i++)
            {
                for (int j = PLAYER_FOV*-1; j <= PLAYER_FOV; j++)
                {
                    _map[(_player.Position.X/TILE_WIDTH) + i, (_player.Position.Y/TILE_HEIGHT) + j].IsUnseen = false;
                }
            }
        }

        void DrawEquipment()
        {
            // helmet, mail, boots, weapon, shield
            int xborder = (FOV*TILE_WIDTH) - EQUIPMENT_WIDTH + (TILE_WIDTH*5);
            int yborder = TILE_HEIGHT;

            for (int i = 0; i < 5; i++)
            {
                if (i == 3)
                {
                    _spriteBatch.Draw(_textures["slot"], new Vector2(xborder - (TILE_WIDTH/2) - TILE_WIDTH/4, yborder));

                    // draw weapon
                    if(_player.Equipment.Thumbnails.Weapon != null)
                        _spriteBatch.Draw(_player.Equipment.Thumbnails.Weapon, new Vector2(xborder - (TILE_WIDTH / 2) - TILE_WIDTH / 4, yborder));
                }
                else if (i == 4)
                {
                    yborder -= 2*TILE_HEIGHT;
                    _spriteBatch.Draw(_textures["slot"], new Vector2(xborder + (TILE_WIDTH/2) + TILE_WIDTH/4, yborder));

                    // draw shield
                    if (_player.Equipment.Thumbnails.Shield != null)
                        _spriteBatch.Draw(_player.Equipment.Thumbnails.Shield, new Vector2(xborder + (TILE_WIDTH / 2) + TILE_WIDTH / 4, yborder));
                }
                else
                {
                    _spriteBatch.Draw(_textures["slot"], new Vector2(xborder, yborder));
                }

                if (i == 0)
                {
                    // draw helmet
                    if (_player.Equipment.Thumbnails.Helmet != null)
                        _spriteBatch.Draw(_player.Equipment.Thumbnails.Helmet, new Vector2(xborder, yborder));
                }

                if (i == 1)
                {
                    // draw mail
                    if (_player.Equipment.Thumbnails.Mail != null)
                        _spriteBatch.Draw(_player.Equipment.Thumbnails.Mail, new Vector2(xborder, yborder));
                }

                if (i == 2)
                {
                    // draw boots
                    if (_player.Equipment.Thumbnails.Boots != null)
                        _spriteBatch.Draw(_player.Equipment.Thumbnails.Boots, new Vector2(xborder, yborder));
                }

                yborder += TILE_HEIGHT * 2;
            }
        }

        void DrawPlayerBars()
        {
            
        }

        void DrawStatus()
        {
            int x = TILE_WIDTH/2;
            int y = TILE_HEIGHT/2;

            foreach (var message in _messageQueue)
            {
                _spriteBatch.DrawString(_font, message.Item1, new Vector2(x, y), message.Item2, 0f, Vector2.Zero, new Vector2(0.75f,0.75f), SpriteEffects.None, 0f);
                y += TILE_HEIGHT/2;
            }
        }

        List<InventoryItem> GenerateRandomInventory()
        {
            var items = new List<InventoryItem>();
            int amountToGenerate = r.Next(0, 6);
            for (int i = 0; i < amountToGenerate; i++)
            {
                var type = _itemTypes[r.Next(_itemTypes.Count)].GetType();
                var item = (InventoryItem)Activator.CreateInstance(type);
                items.Add(item);
            }
            return items;
        }

        void CheckForPickup()
        {
            Texture2D t = null;
            foreach (var texture in _map[_player.Position.X / TILE_WIDTH, _player.Position.Y / TILE_HEIGHT].AdditionalTextures)
            {
                if (texture.Name.StartsWith("money/"))
                {
                    var gold = _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].Gold;
                    foreach (var item in _player.Inventory.Items)
                    {
                        if (item.Name == "Gold")
                        {
                            item.Value += gold.Value;
                            break;
                        }
                    }
                    t = texture;
                }
            }
            //int index = _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].AdditionalTextures.IndexOf(t);
            if (t == null) return;
            _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].AdditionalTextures.Remove(t);
            _map[_player.Position.X/TILE_WIDTH, _player.Position.Y/TILE_HEIGHT].Gold = null;
        }
    }
}