using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    [CreateAssetMenu]
    public class StatsDatabase : ScriptableObject
    {
        public List<Stat> stats;
        public List<Stat> globalStats;

        public void SaveGlobalStatsDefaults()
        {
            _globalStatsDefaultValues.Clear();
            foreach (Stat stat in stats)
            {
                Stat defaultStatInstance = Instantiate(stat);
                defaultStatInstance.SetBaseValue(stat.BaseValue, false);
                defaultStatInstance.SetCurrentValue(stat.BaseValue, false);

                _globalStatsDefaultValues.Add(defaultStatInstance);
            }
        }
        public void LoadGlobalStatDefaults()
        {
            stats.Clear();
            stats.AddRange(_globalStatsDefaultValues);
        }
        List<Stat> _globalStatsDefaultValues;
    }
}