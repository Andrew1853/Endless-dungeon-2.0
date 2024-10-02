using InventorySystem;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.InventorySystem
{
    public class ItemsManager : MonoBehaviour
    {
        [SerializeField] ItemsCollection _worldItems;

        public ItemsCollection WorldItems => _worldItems;

        // Use this for initialization
        void Start()
        {
            ItemsManager founded = GameObject.FindObjectOfType<ItemsManager>();
            if (founded != null && founded != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            ItemUtility.Init(_worldItems);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}