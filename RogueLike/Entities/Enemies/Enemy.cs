using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Levels;

namespace RogueLike
{
    public abstract class Enemy : IEntityInfo
    {
        public abstract string Name { get; set; }
        public abstract EnemyType EnemyType { get; }
        public abstract Texture2D Texture { get; set; }
        public abstract string TextureName { get; set; }
        public abstract Position Position { get; set; }
        public abstract int XPReward { get; set; }
        public abstract EnemyKI EnemyKi { get; }
        public abstract LevelInfo Level { get; set; }
        public abstract int Health { get; set; }
        public abstract int MaxHealth { get; set; }
        public abstract int Attack { get; set; }
        public abstract int Shield { get; set; }
        public abstract Inventory.Inventory Inventory { get; set; }
        public abstract Equipment Equipment { get; set; }
    }

    public enum EnemyType
    {
        Dwarf,
        Elf,
        Water,
        Wizard,
        Human,
        Goblin,
        Creature,
        Special
    }
}
