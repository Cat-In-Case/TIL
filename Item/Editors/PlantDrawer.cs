using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditorInternal;

[CustomEditor(typeof(Plant))]
public class PlantDrawer : Editor
{
    public class  temp
    {
        //헤더는 라벨로
        [SerializeField] private int plantNumber = 0;
        [SerializeField] private string plantName = "";
        [SerializeField] private List<GrowthData> growthProcedure = new List<GrowthData>();
        [SerializeField] private Vector2Int plantSize = Vector2Int.one;

        [SerializeField] private bool harvestMultipleTimes = false; //여러번 수확
        [SerializeField] private List<DropItem> harvestDrop = new List<DropItem>();

        [SerializeField] private bool isTree = false;   //나무
        [SerializeField] private int harvestTerm = 3;
        [SerializeField] private Vector2Int treeSizeTransition = new Vector2Int(3, 3);
        [SerializeField] private List<DropItem> treeChopDrop = new List<DropItem>();
    }

    private bool initialized = false;

    private SerializedProperty plantNumber;
    private SerializedProperty plantName;
    private SerializedProperty plantSize;
    private SerializedProperty growthProcedure;
    private ReorderableList growthProcedure_list;

    private SerializedProperty harvestMultipleTimes;
    private SerializedProperty harvestDrop;
    private ReorderableList harvestDrop_list;

    private SerializedProperty isTree;
    private SerializedProperty harvestTerm;
    private SerializedProperty treeSizeTransition;
    private SerializedProperty treeChopDrop;
    private ReorderableList treeChopDrop_list;

    private GUIStyle labelStyle;

    private GUIContent nullLabel = new GUIContent("");
    private GUILayoutOption[] option = { GUILayout.Width(40), GUILayout.Height(17) };

    private bool nameFoldout = false;

    private void OnEnable()
    {
        growthProcedure = serializedObject.FindProperty("growthProcedure");
        CreateList(ref growthProcedure_list, serializedObject, growthProcedure, 80);

        harvestDrop = serializedObject.FindProperty("harvestDrop");
        CreateList(ref harvestDrop_list, serializedObject, harvestDrop, EditorGUIUtility.singleLineHeight);

        isTree = serializedObject.FindProperty("isTree");
        if (isTree.boolValue == true)
        {
            treeChopDrop = serializedObject.FindProperty("treeChopDrop");
            CreateList(ref treeChopDrop_list, serializedObject, treeChopDrop, EditorGUIUtility.singleLineHeight);
        }
    }

    private void OnDisable()
    {
        growthProcedure_list = null;
    }
    private void CreateList(ref ReorderableList list, SerializedObject serializedObject, SerializedProperty prop, float size)
    {
        list = new ReorderableList(serializedObject, prop, true, false, true, true);
        list.onChangedCallback = (ReorderableList list) => { serializedObject.ApplyModifiedProperties(); };
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //var element = prop.GetArrayElementAtIndex(index);

                var element = prop.GetArrayElementAtIndex(index);

                if (element.isExpanded)
                {
                    rect.height += size;
                }
                rect.x += 8;

                rect.width = GUILayoutUtility.GetLastRect().width - 35;

                EditorGUIUtility.labelWidth = 70.0f;
                
                EditorGUI.PropertyField(rect, element, new GUIContent("Drop Item"),true);
            };
        list.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = prop.GetArrayElementAtIndex(index);
            //return EditorGUIUtility.singleLineHeight;
            return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
        };
    }


    public override void OnInspectorGUI()
    {
        if (!initialized)
            Initialize();
        serializedObject.Update();

        Plant plantSO = serializedObject.targetObject as Plant;
        Plant plant = EditorGUILayout.ObjectField("", plantSO, typeof(Plant), true) as Plant;

        FieldInfo info;


        string[] strs = target.name.Split("_");
        GUIStyle objectNameStyle = EditorStyles.foldoutHeader;
        objectNameStyle.alignment = TextAnchor.MiddleLeft;
        objectNameStyle.normal = new GUIStyleState() { textColor = Color.gray };
        objectNameStyle.onNormal = new GUIStyleState() { textColor = Color.white };
        nameFoldout = EditorGUILayout.Foldout(nameFoldout,"Number Name", objectNameStyle);


        info = typeof(Plant).GetField("plantNumber", BindingFlags.Instance | BindingFlags.NonPublic);
        int id = stringToInt(strs[0]);
        info.SetValue(plant, id);
        int _id = (int)info.GetValue(plant);

        info = typeof(Plant).GetField("plantName", BindingFlags.Instance | BindingFlags.NonPublic);
        info.SetValue(plant, strs[strs.Length - 1]);


        if(nameFoldout)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            EditorGUILayout.LabelField("Plant Number", labelStyle);
            EditorGUILayout.IntField(_id);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            EditorGUILayout.LabelField("Plant Name");
            string name = EditorGUILayout.TextField((string)info.GetValue(plant));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10f);
        //info = typeof(List<GrowthData>).GetField("growthProcedure", BindingFlags.Instance | BindingFlags.NonPublic);
        EditorGUILayout.BeginHorizontal();
        growthProcedure = serializedObject.FindProperty("growthProcedure");
        GUIStyle headerStyle = EditorStyles.foldoutHeader;
        headerStyle.alignment = TextAnchor.MiddleLeft;
        headerStyle.normal = new GUIStyleState() { textColor = Color.yellow };
        headerStyle.onNormal = new GUIStyleState() { textColor = Color.magenta };
        growthProcedure.isExpanded = EditorGUILayout.Foldout(growthProcedure.isExpanded, "Growth Procedure", headerStyle);

        EditorGUILayout.PropertyField(growthProcedure.FindPropertyRelative("Array.size"), nullLabel, option);
        EditorGUILayout.EndHorizontal();
        if (growthProcedure.isExpanded)
        {
            EditorGUILayout.Space(5f);
            growthProcedure_list.DoLayoutList();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.LabelField("plantSize", labelStyle);
        SerializedProperty plantSize = serializedObject.FindProperty("plantSize");
        EditorGUILayout.PropertyField(plantSize, nullLabel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        //SerializedProperty harvestMultipleTimes = serializedObject.FindProperty("harvestMultipleTimes");
       // EditorGUILayout.LabelField("HarvestMultipleTimes", labelStyle);
      //  EditorGUILayout.PropertyField(harvestMultipleTimes, nullLabel);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        harvestDrop = serializedObject.FindProperty("harvestDrop");
        harvestDrop.isExpanded = EditorGUILayout.Foldout(harvestDrop.isExpanded, "Harvest Drop", headerStyle);
        EditorGUILayout.PropertyField(harvestDrop.FindPropertyRelative("Array.size"), nullLabel, option);
        EditorGUILayout.EndHorizontal();
        if (harvestDrop.isExpanded)
        {
            EditorGUILayout.Space(5f);
            harvestDrop_list.DoLayoutList();
        }


        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5f);
        EditorGUILayout.LabelField("Tree", labelStyle);
        isTree = serializedObject.FindProperty("isTree");
        EditorGUILayout.PropertyField(isTree, nullLabel);
        EditorGUILayout.EndHorizontal();
        if (isTree.boolValue == true)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Harvest Term", labelStyle);
                harvestTerm = serializedObject.FindProperty("harvestTerm");
                EditorGUILayout.PropertyField(harvestTerm, nullLabel);
            }

            treeSizeTransition = serializedObject.FindProperty("treeSizeTransition");

            using (new EditorGUILayout.HorizontalScope())
            {
                treeChopDrop = serializedObject.FindProperty("treeChopDrop");
                treeChopDrop.isExpanded = EditorGUILayout.Foldout(treeChopDrop.isExpanded, "treeChopDrop", headerStyle);
                EditorGUILayout.PropertyField(treeChopDrop.FindPropertyRelative("Array.size"), nullLabel, option);
            }
            //EditorGUILayout.PropertyField(treeChopDrop, nullLabel);

            if (treeChopDrop.isExpanded)
            {
                EditorGUILayout.Space(5f);
                treeChopDrop_list.DoLayoutList();
            }
        }

        //base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void Initialize()
    {
        initialized = true;

        //캐싱
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 12;
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
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
