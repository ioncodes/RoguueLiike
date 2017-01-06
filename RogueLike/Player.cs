using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    public class Player
    {
        public Texture2D Texture { get; set; }
        public Position Position { get; set; } = new Position(64,64);
        public int Health { get; set; } = 1;
        public int Attack { get; set; } = 15;
        public int Shield { get; set; } = 0;
        public Inventory Inventory { get; } = new Inventory();
    }
}
