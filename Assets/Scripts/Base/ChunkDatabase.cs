using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkDatabase", menuName = "Database/ChunkDatabase")]
public class ChunkDatabase : ScriptableObject
{
    public List<ChunkData> chunks;

    [System.Serializable]
    public class ChunkData
    {
        public string id;
        public List<ObjectCollection.ObjectData> objects; // Список объектов в чанке
    }
}
