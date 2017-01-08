using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    public abstract class Enemy
    {
        public abstract string Name { get; set; }
        public abstract EnemyType EnemyType { get; }
        public abstract Texture2D Texture { get; set; }
        public abstract Position Position { get; set; }
        public abstract int Health { get; set; }
        public abstract int MaxHealth { get; set; }
        public abstract int Attack { get; set; }
        public abstract int XPReward { get; set; }
        public abstract EnemyKI EnemeKi { get; }
    }

    public enum EnemyType
    {
        Dwarf,
        Elf
    }
}
