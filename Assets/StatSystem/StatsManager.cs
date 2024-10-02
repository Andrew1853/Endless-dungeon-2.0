using Assets.InventorySystem;
using DemiurgEngine.StatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StatSystem
{
    public class StatsManager : MonoBehaviour
    {
        public static StatsManager instance;
        [SerializeField] StatsDatabase _database;
        List<StatsController> _statsControllers = new();
        bool _isActive = false;
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            StatsManager founded = GameObject.FindObjectOfType<StatsManager>();
            if (founded != null && founded != this)
            {
                Destroy(gameObject);
                return;
            }

            StatsController[] foundedSC = GameObject.FindObjectsByType<StatsController>(FindObjectsSortMode.None);
            foreach (var item in foundedSC)
            {
                if (_statsControllers.Contains(item) == false)
                {
                    _statsControllers.Add(item);
                }
            }

            List<Stat> allStats = new();
            foreach (var sc in _statsControllers)
            {
                for (int i = 0; i < sc.StatsCount; i++)
                {
                    allStats.Add(sc.GetStat(i));
                }
            }
            foreach (var item in _statsControllers)
            {
                item.Init();
            }
            
            
            GlobalStats.Init(allStats, _database.globalStats);
            _database.SaveGlobalStatsDefaults();

            DontDestroyOnLoad(gameObject);
            _isActive = true;
        }
        private void OnDestroy()
        {
            if (_isActive)
            {
            _database.LoadGlobalStatDefaults();
            }
        }
        public void OnStatsControllerStarted(StatsController sc)
        {
            if (_statsControllers.Contains(sc) == false)
            {
            _statsControllers.Add(sc);
                sc.Init();
            }
            for (int i = 0; i < sc.StatsCount; i++)
            {
                Stat s = sc.GetStat(i);
                if (s.GetType() != typeof(Stat))
                {
                    GlobalStats.AddCustomStat(s);
                }
            }
        }
        public void OnStatsControllerDestroyed(StatsController sc)
        {
            _statsControllers.Remove(sc);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}