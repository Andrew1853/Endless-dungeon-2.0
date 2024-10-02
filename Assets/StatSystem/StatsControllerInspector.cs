using UnityEditor;
using UnityEngine;

namespace Assets.StatSystem.Editor
{
    public class StatsControllerInspector : ScriptableObject
    {
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}