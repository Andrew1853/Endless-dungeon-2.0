using DemiurgEngine.StatSystem;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class StatLinkFiller
{
    public static void InitializeStats(GameObject gameObject, StatsController statsController)
    {
        var components = gameObject.GetComponents<MonoBehaviour>();

        foreach (var component in components)
        {
            foreach (var field in StatFields(component))
            {

                var stat = field.GetValue(component) as Stat;
                if (stat == null)
                {
                    Debug.LogError("Field with Stat Attribute is not assigned, assign it in inspector!");
                    return;
                }
                var statName = stat.name.ToLower();

                Stat statEntry = statsController.GetStat(statName);
                    
                field.SetValue(component, statEntry);
            }
        }
    }
    static FieldInfo[] StatFields(MonoBehaviour component)
    {
        var allFields = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        List<FieldInfo> statFields = new(allFields.Length);
        foreach (var field in allFields)
        {
            AutoAssignStatAttribute statAttribute = field.GetCustomAttribute<AutoAssignStatAttribute>();
            if (statAttribute != null)
            {
                statFields.Add(field);
            }
        }
        return statFields.ToArray();
    }

}