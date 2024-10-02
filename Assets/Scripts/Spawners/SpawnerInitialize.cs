using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemiurgEngine.SpawnerLogic
{
    public static class SpawnerInitialize
    {
        static SpawnerManager _sm;
        static CharactersTable _table;

        static bool _initialized = false;
        public static void InitAll(SpawnerManager sm, CharactersTable table)
        {
            Spawner[] s = GameObject.FindObjectsOfType<Spawner>();

            foreach (var item in s)
            {
                item.Init(sm,table);
            }

            _sm = sm;
            _table = table;
            _initialized = true;
        }
        

        public static void InitSpawner(Spawner spawnerInstance)
        {
            if (_initialized == false)
            {
                return;
            }
            spawnerInstance.Init(_sm, _table);
        } 
    }
}