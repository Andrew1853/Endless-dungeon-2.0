using InventorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class CharacterDropData
{
    public List<DropEntry> entries;
}
[Serializable]
public class DropEntry
{
    public Item item;
    public int minQuantity;
    public int maxQuantity;

    /// <summary>
    /// from 0 to 1
    /// </summary>
    public int dropChance;
}