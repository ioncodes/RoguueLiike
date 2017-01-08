using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Levels;

namespace RogueLike
{
    public class Player
    {
        public Texture2D Texture { get; set; }
        public Position Position { get; set; } = new Position(64,64);
        public int Health { get; set; } = 20;
        public int MaxHealth { get; set; } = 20;
        public int Attack { get; set; } = 10;
        public int Shield { get; set; } = 0;
        public int XP { get; set; } = 0;
        public LevelInfo Level = new Level0();
        public Inventory Inventory { get; } = new Inventory();
        public Equipment Equipment { get; } = new Equipment();
    }
}
