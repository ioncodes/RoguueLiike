using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike
{
    class LargeSword : InventoryItem
    {
        public override string Name { get; } = "Large Sword";
        public override ItemType ItemType { get; } = ItemType.Weapon;
        public override int Damage { get; } = 6;
        public override int Health { get; } = 0;
    }
}
