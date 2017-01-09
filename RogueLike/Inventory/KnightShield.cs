using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    class KnightShield : InventoryItem
    {
        public override string Name { get; } = "Knight Shield";
        public override ItemType ItemType { get; } = ItemType.Shield;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 10;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 50;
        public override int Amount { get; set; }
    }
}
