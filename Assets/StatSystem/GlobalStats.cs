using System;
using System.Collections.Generic;

namespace DemiurgEngine.StatSystem
{
    public static class GlobalStats
    {
        static Dictionary<string, Stat> _globals = new();
        static Dictionary<Type, Stat> _entries = new();
        static List<EntryOnInit> _invokeOnInit = new();
        static bool _initialized = false;

        class EntryOnInit
        {
            public Type statType;
            public Action<float, float> handler;
            public Action<Stat> onInitializedHandler;
        }
        public static void Init(IEnumerable<Stat> allStats, IEnumerable<Stat> globalStats)
        {
            //custom stats
            foreach (var stat in allStats)
            {
                Type type = stat.GetType();
                if (type == typeof(Stat))
                {
                    continue;
                }
                if (_entries.ContainsKey(type) == false)
                {
                    _entries.Add(type, stat);
                }
            }

            //globals
            foreach (var item in globalStats)
            {
                _globals.TryAdd(item.name, item);
            }

            //callbacks
            Stat s = null;
            foreach (var item in _invokeOnInit)
            {
                if (_entries.ContainsKey(item.statType) == false)
                {
                    s = FindStat(item.statType);

                    if (s != null)
                    {
                        _entries.Add(item.statType, s);

                        ProcessCallbacks(s, item.handler, item.onInitializedHandler);
                    }
                }
                else
                {
                    s = _entries[item.statType];

                    ProcessCallbacks(s, item.handler, item.onInitializedHandler);
                }
            }
            _initialized = true;
        }
        public static void AddCustomStat(Stat stat)
        {
            Type type = stat.GetType();
            if (_entries.ContainsKey(type) == false)
            {
                _entries.Add(type, stat);
            }
        }
        public static Stat GetGlobalStat(string name)
        {
            Stat s;
            _globals.TryGetValue(name, out s);
            return s;
        }
        public static void SubscribeToCustomStat<StatT>(Action<float, float> handler, Action<Stat> onStatInitilized = null)
        {
            if (_initialized == false)
            {
                _invokeOnInit.Add(new EntryOnInit() { statType = typeof(StatT), handler = handler, onInitializedHandler = onStatInitilized });
                return;
            }

            Stat stat = _entries[typeof(StatT)];

            ProcessCallbacks(stat, handler, onStatInitilized);
        }
        public static void Unsubscribe<StatT>(Action<float, float> handler)
        {
            if (_initialized == false)
            {
                return;
            }

            Stat s = _entries[typeof(StatT)];
            s.onChange -= handler;
        }
        static Stat FindStat(Type type)
        {
            Stat stat;

            if (_entries.TryGetValue(type, out stat))
            {
                return stat;
            }
            return null;
        }
        static void ProcessCallbacks(Stat stat, Action<float, float> handler, Action<Stat> getStatOnInitHandler)
        {
            stat.onChange += handler;
            getStatOnInitHandler?.Invoke(stat);
        }
    }
}