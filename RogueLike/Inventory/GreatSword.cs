using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class GreatSword : InventoryItem
    {
        public override string Name { get; } = "Great Sword";
        public override ItemType ItemType { get; } = ItemType.Weapon;
        public override int Damage { get; } = 6;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 0;
        public override Texture2D Texture { get; set; }
        public override int Value { get; } = 20;
    }
}
