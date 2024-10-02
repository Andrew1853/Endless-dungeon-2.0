using DemiurgEngine.SpawnerLogic;
using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spawner : MonoBehaviour
{
    CharactersTable _table;
    [SerializeField] CharacterFacade _creaturePrefab;
    [SerializeField] int _maxCreatures;
    [SerializeField] float _delay;
    int _aliveCount;
    bool _spawnIsActive = false;
    SpawnerManager _sm;
    StatsControllerCharacter _sc;
    public StatsControllerCharacter StatsController { get => _sc; set => _sc = value; }

    bool _initialized = false;
    private void Start()
    {
        if (_initialized == false)
        {
            SpawnerInitialize.InitSpawner(this);

        }
    }
    public void Init(SpawnerManager sm, CharactersTable table)
    {
        _table = table;
        _sc = GetComponent<StatsControllerCharacter>();

        _sm = sm;

        StartSpawn();

        _initialized = true;
    }

    public void OnCreatureDeath(GameObject creature)
    {
        _aliveCount--;
    }
    protected virtual bool AbleToSpawn()
    {
        return _aliveCount <= _maxCreatures;
    }
    List<CharacterFacade> _spawnedCreatures = new(99);
    void Spawn()
    {
        CharacterFacade newChar = Instantiate(_creaturePrefab, transform.position, Quaternion.identity);

        newChar.BindToSpawner(this);

        _aliveCount++;
    }
    public void StartSpawn()
    {
        StartCoroutine(SpawnRoutine());
    }
    public void StopSpawn()
    {
        StopAllCoroutines();
    }
    IEnumerator SpawnRoutine()
    {
        _spawnIsActive = true;
        while (_spawnIsActive)
        {
            if (AbleToSpawn())
            {
                Spawn();
            }
            yield return new WaitForSeconds(_delay);
        }
    }
}
