using DemiurgEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem {
    public class ItemsListView : MonoBehaviour
    {

        [SerializeField] Transform _container;

        public ItemsCollection Collection { get; set; }

        private void Start()
        {
            EventBus.Subscribe<ItemSlotEvent>(OnItenSlotClicked);
        }
        void OnItenSlotClicked(ItemSlotEvent e)
        {
            UIDragHandler.instance.StartDragging(e.clickedSlot.GetComponent<RectTransform>());
        }
        public void SetItemCollection(ItemsCollection c)
        {
            Collection = c;
        }
        public void Display(bool v)
        {
            for (int i = 0; i < _container.childCount; i++)
            {
                if (i >= Collection.Count)
                {
                    break;
                }
                _container.GetChild(i).GetComponent<ItemSlot>().Set(Collection.GetItemAt(i).item.Sprite);
            }
        }
    }
}
