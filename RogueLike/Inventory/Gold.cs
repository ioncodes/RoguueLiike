using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    public class Gold : InventoryItem
    {
        public override string Name { get; } = "Gold";
        public override ItemType ItemType { get; } = ItemType.Money;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 0;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 0;
        public override int Amount { get; set; } = 0;
        public override string Key { get; set; }
    }
}
