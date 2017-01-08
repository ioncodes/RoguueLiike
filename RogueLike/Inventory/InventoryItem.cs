using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    public abstract class InventoryItem
    {
        public abstract string Name { get; }
        public abstract ItemType ItemType { get; }
        public abstract int Damage { get; }
        public abstract int Health { get; }
        public abstract int Shield { get; }
        public abstract Texture2D Texture { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Helmet,
        Mail,
        Boots,
        HealthSlot,
        Shield
    }
}
