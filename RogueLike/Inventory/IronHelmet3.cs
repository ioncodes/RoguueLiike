using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class IronHelmet3 : InventoryItem
    {
        public override string Name { get; } = "Iron Helmet #3";
        public override ItemType ItemType { get; } = ItemType.Helmet;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 2;
        public override Texture2D Texture { get; set; }
        public override int Value { get; } = 10;
    }
}
