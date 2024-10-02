using System.Collections.Generic;
using UnityEngine;

namespace DemiurgEngine
{
    public class CharacterManager : MonoBehaviour
    {
        public LayerMask characterLayer;

        public ObjectsGrid grid;

        [SerializeField] Vector2Int minCoord;
        [SerializeField] Vector2Int maxCoord;

        [SerializeField] Vector2 _offsetFromCellCenter = new Vector2(.5f, .5f);
        void Awake()
        {
            for (int x = minCoord.x; x < maxCoord.x; x++)
            {
                for (int y = minCoord.y; y < maxCoord.y; y++)
                {
                    grid.cells.Add(new Vector2Int(x, y), new Cell());
                }
            }
            CollectObjects();
        }
        public Vector2Int GetRandomPos() => new Vector2Int(Random.Range(minCoord.x, maxCoord.x), Random.Range(minCoord.y, maxCoord.y));
        void CollectObjects()
        {
            var objects = FindObjectsOfType<GameObject>();
            foreach (var obj in objects)
            {
                if (((1 << obj.layer) & characterLayer) != 0)
                {
                    var position = new Vector2Int(Mathf.RoundToInt(obj.transform.position.x), Mathf.RoundToInt(obj.transform.position.y));
                    if (position.x >= maxCoord.x || position.x < minCoord.x || position.y >= maxCoord.y || position.y < minCoord.y)
                    {
                        continue;
                    }
                    grid.cells[position].Add(obj);
                }
            }
        }

        public void InitializeCharacters()
        {
            //foreach (var cell in grid.cells)
            //{
            //    foreach (var obj in cell.Value.objects)
            //    {
            //        obj.transform.position = new Vector3Int(Mathf.RoundToInt(obj.transform.position.x), Mathf.RoundToInt(obj.transform.position.y)) + (Vector3)_offsetFromCellCenter;
            //    }
            //}
        }
        public void GetObject()
        {

        }
        public void SetPosition()
        {

        }
        public void MoveObject(Vector2Int from, Vector2Int to, GameObject obj)
        {
            Debug.Log(obj.name + "moves from " + from + " to " + to);
            Cell fromCell;
            if (grid.cells.TryGetValue(from, out fromCell))
            {
                fromCell.Remove(obj);
            }
            if (grid.cells.ContainsKey(to))
            {
                grid.cells[to].Add(obj);
            }
            else
            {
                grid.cells.Add(to, new Cell());
                grid.cells[to].Add(obj);
            }
        }
        //public void CreateObject(ObjectDatabase.ObjectData template, Vector2Int position)
        //{
        //    var objInstance = Instantiate(template.prefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        //    var instanceData = new ObjectInstanceData
        //    {
        //        id = template.id,
        //        instance = objInstance,
        //        position = position
        //    };
        //    activeObjects.Add(instanceData);
        //}

        //public void RemoveObject(string id)
        //{
        //    var objData = activeObjects.Find(o => o.id == id);
        //    if (objData != null)
        //    {
        //        Destroy(objData.instance);
        //        activeObjects.Remove(objData);
        //    }
        //}

        //public void RemoveObject(GameObject obj)
        //{
        //    var objData = activeObjects.Find(o => o.instance == obj);
        //    if (objData != null)
        //    {
        //        Destroy(objData.instance);
        //        activeObjects.Remove(objData);
        //    }
        //}

        //public void RemoveObject(Vector2Int position)
        //{
        //    var objData = activeObjects.FindLast(o => o.position == position);
        //    if (objData != null)
        //    {
        //        Destroy(objData.instance);
        //        activeObjects.Remove(objData);
        //    }
        //}
    }
    public class Cell
    {
        public List<GameObject> objects = new(10);
        public void Add(GameObject o)
        {
            objects.Add(o);
        }
        public void Remove(GameObject o)
        {
            objects.Remove(o);
        }
    }
}