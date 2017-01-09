using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    class IronHelmet1 : InventoryItem
    {
        public override string Name { get; } = "Iron Helmet #1";
        public override ItemType ItemType { get; } = ItemType.Helmet;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 2;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 10;
        public override int Amount { get; set; } = 0;
    }
}
