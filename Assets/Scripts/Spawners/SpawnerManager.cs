using DemiurgEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerManager
{
    List<Spawner> _spawners;
    int _spawnersCountAtStart = 0;
    float _spawnRateIncreaseValue;
    public void Init(Spawner[] spawners)
    {
        _spawners.AddRange(spawners);

        _spawnersCountAtStart = _spawners.Count;
        _spawnRateIncreaseValue = 1f / ((float)_spawnersCountAtStart);
    }

    public void OnSpawnerKilled(Spawner s)
    {
        _spawners.Remove(s);

    }
}
