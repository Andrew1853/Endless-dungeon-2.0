using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InventorySystem
{


    public partial class ItemsCollection : MonoBehaviour, IEnumerable<ItemEntry>
    {
        public event Action onChange;

        public int maxCount = 10;
        [SerializeField] List<ItemEntry> _itemEntries = new();
        public int Count => _itemEntries.Count;
        public bool RemoveWhenQuantityZero { get; set; } = true;
        public void Add(Item item, int quantity)
        {
            _itemEntries.Add(new ItemEntry() { item = item, quantity = quantity });

            onChange?.Invoke();
        }
        public void Remove(Item item)
        {
            ItemEntry entryToRemove = _itemEntries.Find(e => e.item == item);
            _itemEntries.Remove(entryToRemove);

            onChange?.Invoke();
        }
        public void Remove(int itemIndex)
        {
            _itemEntries.RemoveAt(itemIndex);

            onChange?.Invoke();
        }
        public void Remove(ItemEntry itemEntry)
        {
            Remove(_itemEntries.IndexOf(itemEntry));
        }
        public void Reduce(int itemIndex, int reduceAmount)
        {
            ItemEntry e = _itemEntries[itemIndex];
            int newQuantity = reduceAmount >= e.quantity ? 0 : e.quantity - reduceAmount;

            _itemEntries[itemIndex].quantity = newQuantity;
            onChange?.Invoke();
            if (RemoveWhenQuantityZero && newQuantity == 0)
            {
                Remove(e);
            }
        }
        public ItemEntry GetItemAt(int num)
        {
            if (num < _itemEntries.Count)
            {
                return null;
            }
            return _itemEntries[num];
        }
        public ItemEntry Find(Item i)
        {
            return _itemEntries.Find(e => e.item == i);
        }
        public int FindIndex(Item i)
        {
            return _itemEntries.IndexOf(_itemEntries.Find(e => e.item == i));
        }
        public int FindIndex(ItemEntry e)
        {
            return _itemEntries.IndexOf(e);
        }
        public IEnumerator<ItemEntry> GetEnumerator()
        {
            return new ItemCollectionEnumerator(_itemEntries);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public ItemEntry this[int index]
        {
            get => _itemEntries[index];
            set
            {
                if (index < _itemEntries.Count)
                {
                    _itemEntries[index] = value;
                }
            }
        }
    }
    public class ItemCollectionEnumerator : IEnumerator<ItemEntry>
    {
        private List<ItemEntry> _items;
        private int _position = -1;

        public ItemCollectionEnumerator(List<ItemEntry> items)
        {
            _items = items;
        }

        public ItemEntry Current
        {
            get
            {
                if (_position < 0 || _position >= _items.Count)
                    throw new InvalidOperationException();
                return _items[_position];
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _position++;
            return _position < _items.Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        public void Dispose()
        {
            // Если есть необходимость освободить ресурсы
        }
    }
}
