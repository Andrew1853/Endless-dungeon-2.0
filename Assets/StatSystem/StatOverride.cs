using System;

namespace DemiurgEngine.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        public Stat stat;
        public float baseValue;
        public float currentValue;
    }
}
