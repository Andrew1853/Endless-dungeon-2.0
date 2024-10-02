using DemiurgEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{

    public class ItemSlotEvent
    {
        //HACK
        public bool shiftPressed = false;
        public bool isClick = false;
        public bool isMouseDown = false;
        public ItemSlot clickedSlot;
        public ItemEntry entry;
    }
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] Image _image;
        public Image ItemImage => _image;
        [SerializeField] GameObject _highlightImage;
        [SerializeField] GameObject _selectImage;

        public Item item { get; set; }

        public bool Highlighted => _highlightImage.activeSelf;
        public bool Selected => _selectImage.activeSelf;
        
        public void Set(Sprite sprite)
        {
            ItemImage.sprite = sprite;
        }
        private void OnMouseDown()
        {
            ItemSlotEvent slotClickedEvent = new ItemSlotEvent();
            slotClickedEvent.shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            slotClickedEvent.clickedSlot = this;
            slotClickedEvent.isMouseDown = true;
            slotClickedEvent.isClick = true;
            EventBus.Publish<ItemSlotEvent>(slotClickedEvent);
        }
        public void Highlight(bool v)
        {
            _highlightImage?.SetActive(v);
        }
        public void DisplaySelectFrame(bool v)
        {
            _selectImage?.SetActive(v);
        }
    }

}
