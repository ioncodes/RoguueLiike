using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Inventory
{
    class LeatherArmour3 : InventoryItem
    {
        public override string Name { get; } = "Leather Armour #3";
        public override ItemType ItemType { get; } = ItemType.Mail;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 1;
        public override Texture2D Texture { get; set; }
        public override int Value { get; set; } = 5;
        public override int Amount { get; set; } = 0;
    }
}
