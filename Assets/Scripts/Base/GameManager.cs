using DemiurgEngine;
using DemiurgEngine.SpawnerLogic;
using DemiurgEngine.StatSystem;
using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] CharacterManager characterManager;
    CharactersTable characters;
    public ObjectCollection objectDatabase;
    public ChunkDatabase chunkDatabase;
    public SpawnerManager spawnerManager;
    public int chunkSize = 16;

    
    private Dictionary<string, GameObject> activeObjects = new Dictionary<string, GameObject>();

    public static CharacterManager ChunkManager { get => instance.characterManager; }
    public static CharactersTable Characters { get => instance.characters; }

    public List<CharacterFacade> group = new();

    private void Awake()
    {
        instance = this;

        GameObjectRuntimeInitializer.InitStatControllers();

        InitCharacters();
        InitSpawners();
    }

    //void InitializeChunks()
    //{
    //    foreach (var chunk in chunkDatabase.chunks)
    //    {
    //        foreach (var obj in chunk.objects)
    //        {

    //        }
    //    }
    //}

    void InitCharacters()
    {
        characterManager.InitializeCharacters();                            
        characters = new CharactersTable();
        characters.Init();

        foreach (var item in GameObject.FindGameObjectsWithTag("Character"))
        {
            InitCharacter(item.GetComponent<CharacterFacade>());
        }

    }
    void InitSpawners()
    {
        spawnerManager = new SpawnerManager();
        SpawnerInitialize.InitAll(spawnerManager, characters);

    }
    public void MoveObject(string objectId, Vector2Int newPosition)
    {
        if (activeObjects.TryGetValue(objectId, out var obj))
        {
            obj.transform.position = new Vector3(newPosition.x, newPosition.y, 0);
            // Обновить данные в базе данных
            var objectData = objectDatabase.objects.Find(o => o.id == objectId);
            if (objectData != null)
            {
                objectData.position = newPosition;
            }
        }
    }

    public void AddObject(ObjectCollection.ObjectData newObjectData)
    {
        
    }

    public void RemoveObject(string objectId)
    {
        
    }

    public void SaveDataTable()
    {

    }

    public void LoadDataTable()
    {

    }
    //List<CharacterFacade> _charactersToInit = new();
    public void OnCharacterInstantiated(CharacterFacade character)
    {
        if (character.Initialized == false)
        {
            InitCharacter(character);
        }
        //else
        //{
        //    _charactersToInit.Add(character);
        //}
    }
    void InitCharacter(CharacterFacade character)
    {
        if (character == null)
        {
            Debug.LogError("Trying to init object without CharacterFacade component");
            return;
        }



        CharacterRow row = characters.AddRow(character);

        character.InitializeCharacter(row);

        IInitializableUI[] ui = character.GetComponentsInChildren<IInitializableUI>();
        foreach (var item in ui)
        {
            item.Initialize();
        }
    }
}
