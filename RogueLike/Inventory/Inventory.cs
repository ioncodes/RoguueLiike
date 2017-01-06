using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike
{
    public class Inventory
    {
        public List<InventoryItem> Items { get; } = new List<InventoryItem>();
        public int Amount { get; set; } = 0;
    }
}
