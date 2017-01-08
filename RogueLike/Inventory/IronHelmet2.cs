using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class IronHelmet2 : InventoryItem
    {
        public override string Name { get; } = "Iron Helmet #2";
        public override ItemType ItemType { get; } = ItemType.Armor;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 2;
        public override Texture2D Texture { get; set; }
    }
}
