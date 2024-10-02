using DemiurgEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DemiurgEngine 
{
    public class GridController : MonoBehaviour
    {
        ObjectsGrid grid; 

        List<TrackedObject> toTrack = new();
        public struct TrackedObject
        {
            public Transform obj;
            public Vector2Int currentCell;
        }
        private void Update()
        {
            if (grid != null)
            {
                

            }
        }
        public void AddObjectToTrack(Transform obj)
        {
            toTrack.Add(new TrackedObject() { obj = obj, currentCell = IntPos(obj.position) });
        }
        public void RemoveTrackedObject(Transform obj)
        {
            toTrack.Find(o => o.obj == obj);
        }
        public void UpdateObjectPosition(Transform obj)
        {
            //TODO
        }
        Vector2Int IntPos(Vector3 pos) => Vector2Int.FloorToInt(pos);
    }
}