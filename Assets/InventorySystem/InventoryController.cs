using DemiurgEngine.StatSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    [RequireComponent(typeof(ItemsCollection))]
    public class InventoryController : MonoBehaviour
    {
        //[SerializeField] KeyCode _openKey;

        [SerializeField] InventoryView _view;
        [SerializeField] GameObject itemPopUpWindow;
        [SerializeField] GameObject ButtonPopUpWindow;
        [SerializeField] GameObject PopUpRoot;


        ItemsCollection _collection;

        [AutoAssignStat]
        Stat _money;

        public ItemsCollection Collection => _collection;

        public int Money() => (int)_money.BaseValue;
        public void AddMoney(int money) {
            _money.AddBaseValue((float)money, true);
        }
        public void WithdrawMoney(int money) 
        {
            _money.DecreaseBaseValue(money, true);
        }
        public int CountItems(Item item) => _collection.Count;
        private void Start()
        {
            _collection = GetComponent<ItemsCollection>();
        }
        void Update()
        {
            
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (!IsPointerOverUIObject(itemPopUpWindow, ButtonPopUpWindow))
            //    {
            //        OnGlobalClick();
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (!IsPointerOverUIObject(itemPopUpWindow, ButtonPopUpWindow))
            //    {
            //        OnGlobalClick();
            //    }
            //}
        }
        public void ThrowItemToWorld(int itemIndex)
        {
            ItemEntry e = _collection[itemIndex];
            ItemUtility.CreateAndAttach(transform.position, e);
        }
        public void ThrowItemToWorld(int itemIndex, int quantity)
        {
            ItemEntry old = _collection[itemIndex];
            ItemEntry newItem = old.Clone();
            _collection.Reduce(itemIndex, quantity);
            newItem.quantity = quantity;

            ItemUtility.CreateAndAttach(transform.position, newItem);
        }
        public void AddFromWorld(ItemController itemObject)
        {
            _collection.Add(itemObject.inventoryEntry.item, itemObject.inventoryEntry.quantity);
            Destroy(itemObject.gameObject);
        }
        private bool IsPointerOverUIObject(GameObject target1, GameObject target2)
        {
            // Проверяем, находится ли указатель мыши над защищенным объектом
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };

            // Список результатов Raycast'а
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            foreach (var result in results)
            {
                if (result.gameObject == target1 || result.gameObject == target2)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnGlobalClick()
        {
            //GameManager.Instance.PopUpRoot.SetActive(false);
        }
        public void AddItem(Item i)
        {
            _collection.Add(i, 1);
        }
        public void AddItem(Item i, int quantity)
        {
            _collection.Add(i, quantity);
        }
        public void Remove(Item i)
        {
            _collection.Remove(i);
        }
        public ItemEntry Get(Item item)
        {
            return _collection.GetItemAt(0);
        }
    }
    public class Money : Stat
    {

    }
}