using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class Gold : InventoryItem
    {
        public override string Name { get; } = "Gold";
        public override ItemType ItemType { get; } = ItemType.Money;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 0;
        public override Texture2D Texture { get; set; }
        public override int Value { get; } = 0;
        public int Amount { get; set; } = 0;
    }
}
