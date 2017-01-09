using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Levels;

namespace RogueLike.Enemies
{
    class Elf : Enemy
    {
        public override string Name { get; set; } = "Elf";
        public override EnemyType EnemyType { get; } = EnemyType.Elf;
        public override Texture2D Texture { get; set; }
        public override string TextureName { get; set; } = "enemy/elf";
        public override Position Position { get; set; }
        public override int Health { get; set; } = 10;
        public override int MaxHealth { get; set; } = 10;
        public override int Attack { get; set; } = 3;
        public override int Shield { get; set; } = 0;
        public override Inventory Inventory { get; set; } = new Inventory();
        public override Equipment Equipment { get; set; } = new Equipment();
        public override int XPReward { get; set; } = 20;
        public override EnemyKI EnemyKi { get; } = new EnemyKI();
        public override LevelInfo Level { get; set; } = new Level0();
    }
}
