using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Enemies
{
    class Elf : Enemy
    {
        public override string Name { get; set; }
        public override EnemyType EnemyType { get; } = EnemyType.Elf;
        public override Texture2D Texture { get; set; }
        public override Position Position { get; set; }
        public override int Health { get; set; } = 10;
        public override int MaxHealth { get; set; } = 10;
        public override int Attack { get; set; } = 3;
        public override int XPReward { get; set; } = 20;
        public override EnemyKI EnemeKi { get; } = new EnemyKI();
    }
}
