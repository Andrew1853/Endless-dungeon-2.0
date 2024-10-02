using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "Database/ObjectDatabase")]
public class ObjectCollection : ScriptableObject
{
    public List<ObjectData> objects;

    [System.Serializable]
    public class ObjectData
    {
        public string id;
        public Vector2Int position; // Позиция объекта в координатах тайлов
        public GameObject prefab; // Префаб объекта
    }
}
