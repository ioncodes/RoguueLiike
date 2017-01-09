﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class KnightShield : InventoryItem
    {
        public override string Name { get; } = "Knight Shield";
        public override ItemType ItemType { get; } = ItemType.Shield;
        public override int Damage { get; } = 0;
        public override int Health { get; } = 0;
        public override int Shield { get; } = 10;
        public override Texture2D Texture { get; set; }
        public override int Value { get; } = 50;
    }
}
