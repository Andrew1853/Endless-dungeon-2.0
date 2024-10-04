using DemiurgEngine.StatSystem;
using UnityEngine;

public class VariableConditionInterpretator
{
    public object Execute(string id)
    {
        string[] parts = id.Split(' ');
        if (parts[0] == "Player")
        {
            StatsController playerStats = GameObject.FindWithTag("Player").GetComponentInChildren<StatsController>();
            if (parts.Length >= 2)
            {
                Stat stat = playerStats.GetStat(parts[1]);
                if (parts.Length >= 3 && parts[2] == "current")
                {
                    return stat.CurrentValue;
                }
                else
                {
                    return stat.BaseValue;

                }
            }
        }
        return null;
    }
}