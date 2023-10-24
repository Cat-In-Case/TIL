using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ItemDB))]
public class ItemDB_Editor : Editor
{
    public string path;
    [SerializeField] public ItemDB items;
    [SerializeField] public List<Item> item2 = new List<Item>();

    SerializedProperty serializedProperty;
    public override void OnInspectorGUI()
    {
        ItemDB itemDB = serializedObject.targetObject as ItemDB;

        EditorGUILayout.ObjectField(itemDB, typeof(ItemDB));

        //base.OnInspectorGUI();
        //path = EditorGUILayout.TextField("Path", path);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("path"));
        var path_field = typeof(ItemDB).GetField("path", BindingFlags.Instance | BindingFlags.NonPublic);
        path = path_field.GetValue(itemDB) as string;


        GUILayout.Space(10f);
        if (GUILayout.Button("Generate Database"))
        {
            //번호에 따른으로 바꿔야됨

            string[] assetName = AssetDatabase.FindAssets("_", new[] { path });
            item2.Clear();

            Array.Resize(ref assetName, assetName.Length - 1);

            string[] split = path.Split("/");
            string lastPath = split[split.Length-1];


            foreach(string SO_Name in assetName)
            {
                var SO_Path = AssetDatabase.GUIDToAssetPath(SO_Name);

                string[] splits = SO_Path.Split(lastPath);
                string number = splits[splits.Length - 1];
                Debug.Log(number);

                var Item = AssetDatabase.LoadAssetAtPath<Item>(SO_Path);

                item2.Add(Item);
            }
        }


        for (int i = 0; i < item2.Count; i++)
        {
            //var test = serializedProperty.GetArrayElementAtIndex(i);
            var test = item2[i];
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("ItemNum" + i, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField(test.ID.ToString(), GUILayout.MinWidth(30f));
            EditorGUILayout.ObjectField(test, typeof(Item), GUILayout.MinWidth(200f));
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Apply Database"))
        {
            Debug.Log(item2.Count);
            var list_field = typeof(ItemDB).GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);
            List<Item> _items = list_field.GetValue(itemDB) as List<Item>;

            _items.Clear();


            list_field.SetValue(itemDB, item2);
            serializedObject.Update();
        }

        serializedProperty = serializedObject.FindProperty("items");
        EditorGUILayout.PropertyField(serializedProperty);
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
