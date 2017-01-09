using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueLike.Levels;

namespace RogueLike
{
    interface IEntityInfo
    {
        LevelInfo Level { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        int Attack { get; set; }
        int Shield { get; set; }
        Inventory Inventory { get; set; }
        Equipment Equipment { get; set; }
    }
}
