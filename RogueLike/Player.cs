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
        public Position Position { get; set; } = new Position();
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
