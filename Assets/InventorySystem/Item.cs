using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] ItemController _prefab;
        [SerializeField] string _itemName;
        [SerializeField] Sprite _sprite;

        public int price;
        public float damage;
        public string ItemName => _itemName;
        public Sprite Sprite => _sprite;
        public ItemController Prefab => _prefab;

        public ItemController CreateInstanceInWorld()
        {
            return Instantiate(_prefab);
        }
        
    }
}
