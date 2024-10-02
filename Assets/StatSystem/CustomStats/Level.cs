using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    [System.Serializable]
    public class Level : Stat
    {
        [SerializeField] float _expBaseValueMultiplierOnLevelUp = 100;
        [SerializeField]
        protected Stat _expStatName;

        Stat exp;

        public override void Init(StatsController handler, Stat origin)
        {
            base.Init(handler,origin);
            exp = handler.GetStat(_expStatName.name);
            exp.onChange += (current, max) =>
            {
                if (current == max)
                {
                    exp.SetBaseValue(BaseValue * _expBaseValueMultiplierOnLevelUp, true);
                    exp.SetCurrentValue(0, true);
                    AddBaseValue(1, true);
                }
            };
        }
    }
}