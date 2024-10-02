using DemiurgEngine.StatSystem;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Reqires StatsController on GameObject to work
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class AutoAssignStatAttribute : PropertyAttribute
{
}