using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    public class StatsControllersBeforeGameStartInitializer
    {
        StatsControllerCharacter[] _temp;

        public void InitControllers()
        {
            StatsControllerCharacter[] sc = GameObject.FindObjectsOfType<StatsControllerCharacter>();
            foreach (var item in sc)
            {
                item.Init();
            }

            _temp = sc;
        }
        public List<StatsControllerCharacter> CreateAndFillControllersCollection()
        {
            List<StatsControllerCharacter> collection = new();
            if (_temp == null)
            {
                _temp = GameObject.FindObjectsOfType<StatsControllerCharacter>();
            }
            return collection;
        }
    }
}