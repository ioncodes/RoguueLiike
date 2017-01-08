using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Enemies
{
    class Dwarf : Enemy
    {
        public override string Name { get; set; }
        public override EnemyType EnemyType { get; } = EnemyType.Dwarf;
        public override Texture2D Texture { get; set; }
        public override Position Position { get; set; }
        public override int Health { get; set; } = 20;
        public override int MaxHealth { get; set; } = 20;
        public override int Attack { get; set; } = 5;
        public override int XPReward { get; set; } = 50;
        public override EnemyKI EnemeKi { get; } = new EnemyKI();
    }
}
