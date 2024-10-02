using InventorySystem;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] ItemsCollection _shopItemCollection;

    [SerializeField] InventoryController _playerInventory;
    [SerializeField] float _forceWhenBuy = 5;
    ItemsCollection _collection;

    public ItemsCollection ShopItemCollection => _shopItemCollection;

    private void Awake()
    {
        _collection = GetComponent<ItemsCollection>();
    }
    private void Start()
    {
    }
    public ItemEntry BuyItem(int num, ItemSlot slot = null)
    {
        _playerInventory.WithdrawMoney(_collection[num].item.price);

        ItemEntry itemEntry = _collection[num];
        Item item = itemEntry.item;

        _collection.Remove(num);
        //_playerInventory.AddItem(itemEntry.item);

        if (slot != null)
        {
            SpawnItemWorldObjectAndPushIt(item, slot.transform);

        }
        return itemEntry;
    }
    public bool TryToBuy(int num, ItemSlot slot = null)
    {
        if (num == -1 || num >= _collection.Count)
        {
            return false;
        }
        if (_playerInventory.Money() < _collection[num].item.price)
        {
            return false;
        }
        BuyItem(num, slot);
        return true;
    }
    void SpawnItemWorldObjectAndPushIt(Item item, Transform initialTransform)
    {
        ItemEntry itemEntry = new ItemEntry() { item = item, quantity = 1 };
        ItemUtility.CreateAndAttach(Vector3.zero, itemEntry);
        Transform itemTransform = itemEntry.worldObject.transform;
        itemTransform.position = initialTransform.position;
        itemTransform.rotation = initialTransform.rotation;
        Rigidbody rb = itemTransform.GetComponent<Rigidbody>();
        Vector3 cameraForward = Camera.main.transform.forward;
        rb.AddForce(cameraForward * _forceWhenBuy, ForceMode.Impulse);
    }

}
