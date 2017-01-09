using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    public abstract class InventoryItem
    {
        public abstract string Name { get; }
        public abstract ItemType ItemType { get; }
        public abstract int Damage { get; }
        public abstract int Health { get; }
        public abstract int Shield { get; }
        public abstract Texture2D Texture { get; set; }
        public abstract int Value { get; set; }
        public abstract int Amount { get; set; }
        public abstract string Key { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Helmet,
        Mail,
        Boots,
        HealthSlot,
        Shield,
        Money
    }
}
