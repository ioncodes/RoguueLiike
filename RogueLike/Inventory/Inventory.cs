using System.Collections.Generic;

namespace RogueLike.Inventory
{
    public class Inventory
    {
        public List<InventoryItem> Items { get; } = new List<InventoryItem>();

        public Inventory()
        {
            Items.Add(new Gold());
            Items[0].Value = 10;
        }
    }
}
