using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    public class Enemy
    {
        public Texture2D Texture { get; set; }
        public Position Position { get; set; }
        public int Health { get; set; } = 20;
        public int Attack { get; set; } = 5;
        public int XPReward { get; set; }
        public EnemyKI EnemeKi { get; } = new EnemyKI();
    }
}
