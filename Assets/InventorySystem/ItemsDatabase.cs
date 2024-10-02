using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemsDatabase : ScriptableObject
    {
        public static ItemsDatabase instance;
        public List<Item> items;
        public void Init()
        {
            instance = this;
        }
        public static Item Get(string name) => instance.items.Find(i => i.name == name);
    }
}