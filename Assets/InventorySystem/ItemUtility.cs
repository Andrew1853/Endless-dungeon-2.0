using InventorySystem;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ItemUtility
{
    static ItemsCollection _worldItemsCollection;
    public static void Init(ItemsCollection worldItems)
    {
        _worldItemsCollection = worldItems;
    }
    public static void CreateAndAttach(Vector3 pos, ItemEntry entry)
    {
        ItemController ic = CreateWorldItem(pos, entry);

        entry.worldObject = ic;
    }
    public static ItemController CreateWorldItem(Vector3 pos, ItemEntry entry)
    {
        ItemController instance = Object.Instantiate(entry.item.Prefab, pos, Quaternion.identity);
        instance.inventoryEntry = entry;
        return instance;
    }
}