using DemiurgEngine;
using InventorySystem;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] ShopController _shopController;

    [SerializeField] Transform _container;

    [SerializeField] ItemSlot _slotPrefab;
    int _selected;


    // Use this for initialization
    void Start()
    {
        _shopController.ShopItemCollection.onChange += OnCollectionChange;

        EventBus.Subscribe<ItemSlotEvent>(OnItemSlotEvent);

        for (int i = 0; i < _shopController.ShopItemCollection.Count; i++)
        {
            ItemSlot slot = Instantiate(_slotPrefab, _container);
            slot.item = _shopController.ShopItemCollection[i].item;
            slot.ItemImage.sprite = _shopController.ShopItemCollection[i].item.Sprite;
        }
    }
    void OnCollectionChange()
    {
        //for (int i = 0; i < _container.childCount; i++)
        //{
        //    _container.GetChild(i).GetComponent<ItemSlot>();
        //}   
    }
    void OnItemSlotEvent(ItemSlotEvent data)
    {
        if (data.isClick)
        {
            ItemSlot slot = data.clickedSlot;
            int clicked = SlotNum(slot);

            if (_shopController.TryToBuy(_shopController.ShopItemCollection.FindIndex(slot.item), data.clickedSlot))
            {
                Destroy(slot.gameObject);
            }
            _selected = clicked;
        }
    }
    int SlotNum(ItemSlot slot)
    {
        for (int i = 0; i < _container.childCount; i++)
        {
            ItemSlot slotToCompare = _container.GetChild(i).GetComponent<ItemSlot>();
            if (slotToCompare == slot)
            {
                return i;
            }
        }
        return -1;
    }
    ItemSlot ToSlot(int num)
    {
        return _container.GetChild(num).GetComponent<ItemSlot>();
    }
    int ToCollectionNum(int slotNum)
    {
        return slotNum;
    }
    void OnSelectedSlotActivated()
    {
        if (_shopController.TryToBuy(ToCollectionNum(_selected), ToSlot(_selected)))
        {
            GameObject slotToDestroy = ToSlot(_selected).gameObject;

            Destroy(slotToDestroy);
        }
    }
}
