using UnityEditor;
using UnityEngine;
using System.Reflection;


[CustomEditor(typeof(ResourceItem_Seed))]
public class SeedEditor : Editor
{
    //public ResourceItem_Seed seed;

    public override void OnInspectorGUI()
    {       
        ResourceItem_Seed seedTarget = serializedObject.targetObject as ResourceItem_Seed;
        ResourceItem_Seed seed = EditorGUILayout.ObjectField("", seedTarget, typeof(ResourceItem_Seed), true) as ResourceItem_Seed;

        var resourceType = typeof(ResourceItem_Seed).GetField("resourceType", BindingFlags.Instance | BindingFlags.NonPublic);
        resourceType.SetValue(seed, ResourceType.Seed);

        var obj_id = typeof(Item).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);

        string[] strs = target.name.Split("_");
        int id = stringToInt(strs[0]);
        obj_id.SetValue(seed, id);

        var obj_name = typeof(Item).GetField("itemName", BindingFlags.Instance | BindingFlags.NonPublic);
        obj_name.SetValue(seed, strs[1]);

        base.OnInspectorGUI();
    }

    private int stringToInt(string str)
    {
        int outValue = 0;
        for (int i = 0; i < str.Length; i++)
        {
            outValue = outValue * 10 + (str[i] - '0');
        }
        return outValue;
    }
}

