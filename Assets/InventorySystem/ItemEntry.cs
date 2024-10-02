using System;
namespace InventorySystem
{
    [Serializable]
    public class ItemEntry
    {
        public Item item;
        public int quantity;

        public bool InWorld => worldObject != null;
        public ItemController worldObject;

        public ItemEntry Clone()
        {
            return new ItemEntry() { item = item, quantity = quantity, worldObject = null };
        }
    }


}