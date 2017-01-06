﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike
{
    public abstract class InventoryItem
    {
        public abstract string Name { get; }
        public abstract ItemType ItemType { get; }
        public abstract int Damage { get; }
        public abstract int Health { get; }
        public abstract int Shield { get; }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        HealthSlot
    }
}