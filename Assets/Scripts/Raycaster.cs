using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Raycaster : MonoBehaviour
    {
        public KeyCode raycastKey = KeyCode.Space; 
        [SerializeField] Transform rayOrigin; 
        Vector2 RayOriginPos { get => new Vector2(rayOrigin.position.x, rayOrigin.position.y); }
        public float rayDistance = 10f; 

        public bool raycastToCursor = true;
        public bool rcDir =false;
        public bool rcPos =false;

        [SerializeField] Vector2 _dir = Vector2.right;
        void Update()
        {
            if (Input.GetKeyDown(raycastKey))
            {
                if (raycastToCursor)
                {
                    CastRayToCursor();
                }
                if (rcDir)
                {
                    CastRayInDirection(_dir);
                }
                if (rcPos)
                {
                    CastRayToPosition(MouseToWorld());
                }
            }
        }

        void CastRayToCursor()
        {
            Vector2 mousePosition = MouseToWorld();
            Vector2 direction = mousePosition - RayOriginPos;
            RaycastHit2D hit = Physics2D.Raycast(RayOriginPos, direction, rayDistance);

            Debug.DrawRay(rayOrigin.position, direction * rayDistance, Color.red, 1f);
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
        }

        void CastRayInDirection(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, direction, rayDistance);

            Debug.DrawRay(rayOrigin.position, direction * rayDistance, Color.green, 1f);
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
        }

        void CastRayToPosition(Vector2 position)
        {
            Vector2 direction = position - (Vector2)rayOrigin.position;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, direction, rayDistance);

            Debug.DrawRay(rayOrigin.position, direction * rayDistance, Color.blue, 1f);
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
        }
        Vector2 MouseToWorld()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePosition.x, mousePosition.y);
        }
    }

}
