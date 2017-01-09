using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    class BootsMiddleBrown3 : InventoryItem
    {
        public override string Name { get; } = "Boots Middle Brown #3";
        public override ItemType ItemType { get; } = ItemType.Boots;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 1;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 10;
        public override int Amount { get; set; } = 0;
        public override string Key { get; set; } = "boot_middle_brown3";
    }
}
