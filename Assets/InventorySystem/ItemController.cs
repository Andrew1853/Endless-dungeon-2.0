using System.Collections;
using UnityEngine;
namespace InventorySystem
{
    public class ItemController : MonoBehaviour
    {
        public ItemEntry inventoryEntry;

        [SerializeField] private float attractionDelay = 1f;
        [SerializeField] private float itemAttractSpeed = 5f;
        [SerializeField] private float attractionRadius = 5f;

        public Item Item => inventoryEntry.item;

        private void Start()
        {
            StartCoroutineAttractionItem();
        }
        public void StartCoroutineAttractionItem()
        {
            StartCoroutine(AttractingItem(gameObject));
        }
        private IEnumerator AttractingItem(GameObject item)
        {
            yield return new WaitForSeconds(attractionDelay); // Wait for the delay before starting attraction

            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            
            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            while (item != null && Vector3.Distance(item.transform.position, player.position) > 2f && player != null)
            {
                float distance = Vector3.Distance(item.transform.position, player.position);
                if (distance <= attractionRadius)
                {
                    Vector3 direction = (player.position - item.transform.position).normalized;
                    itemRb.velocity = direction * itemAttractSpeed; // Use velocity for movement
                }
                //else
                //{
                //    itemRb.velocity = Vector3.zero; // Stop moving if player is out of range
                //}
                yield return null;
            }

            //if (item != null && player != null)
            //{
            //    player.GetComponent<InventoryController>().AddItem(inventoryEntry.item);
            //    Destroy(item);
            //}
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.GetComponentInChildren<InventoryController>().AddFromWorld(this);
            }
        }
    }
}
