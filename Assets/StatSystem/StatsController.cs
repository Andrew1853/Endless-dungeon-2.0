using Assets.StatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    public class StatsController : MonoBehaviour
    {
        [SerializeField] bool save = true;
        [SerializeField] List<Stat> _stats;
        [SerializeField] List<StatOverride> _overridesOnStart;

        public bool Save { get => save; set => save = value; }

        public int StatsCount => _stats.Count;

        bool _initialized = false;
        void Start()
        {
            //GameObjectRuntimeInitializer.InitStatController(this);
            StatsManager.instance.OnStatsControllerStarted(this);
        }
        private void OnDestroy()
        {
            //GameObjectRuntimeInitializer.RemoveStatsController(this);
            StatsManager.instance.OnStatsControllerDestroyed(this);
        }

        public void Init()
        {
            if (_initialized)
            {
                return;
            }
            List<Stat> statsOrigins = new(_stats.Count);
            for (int i = 0; i < _stats.Count; i++)
            {
                statsOrigins.Add(_stats[i]);
            }
            for (int i = 0; i < _stats.Count; i++)
            {
                _stats[i] = Instantiate(statsOrigins[i]);
                _stats[i].name = _stats[i].name.TrimEnd("(Clone)");
                _stats[i].Init(this, statsOrigins[i]);
            }

            for (int i = 0; i < _overridesOnStart.Count; i++)
            {
                Stat s = _stats.Find(s => _overridesOnStart[i].stat == s.AssetRef);
                if (s == null)
                {
                    Debug.Log(_overridesOnStart[i].stat.name + " not found in StatController's stats");
                    continue;
                }
                s.Override(_overridesOnStart[i].baseValue);
            }

            StatLinkFiller.InitializeStats(gameObject, this);

            _initialized = true;
        }

        public Stat GetStat(Stat stat)
        {
            return GetStat(stat.name);
        }
        public virtual Stat GetStat(string name)
        {
            StringBuilder sb = new StringBuilder(name);
            sb[0] = char.ToUpper(name[0]);
            name = sb.ToString();
            return _stats.Find(s => s.name == name);
        }
        public Stat GetStat(int index)
        {
            return _stats[index];
        }
        public void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", name);
            List<object> statsList = new List<object>();
            for (int i = 0; i < _stats.Count; i++)
            {
                Stat stat = _stats[i];
                if (stat != null)
                {
                    Dictionary<string, object> statData = new Dictionary<string, object>();
                    stat.GetObjectData(statData);
                    statsList.Add(statData);
                }

            }
            data.Add("StatsList", statsList);
        }

        public void SetObjectData(Dictionary<string, object> data)
        {
            if (data.ContainsKey("StatsList"))
            {
                List<object> statList = data["Stats"] as List<object>;
                for (int i = 0; i < statList.Count; i++)
                {
                    Dictionary<string, object> statData = statList[i] as Dictionary<string, object>;
                    if (statData != null)
                    {
                        Stat stat = GetStat((string)statData["Name"]);

                        if (stat != null)
                        {
                            stat.SetObjectData(statData);
                        }
                    }
                }
            }
        }
    }
}