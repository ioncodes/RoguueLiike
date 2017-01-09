using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    class GreatSword : InventoryItem
    {
        public override string Name { get; } = "Great Sword";
        public override ItemType ItemType { get; } = ItemType.Weapon;
        public override int Damage { get; } = 6;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 0;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 20;
        public override int Amount { get; set; }
    }
}
