using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(DropItem))]
public class DropItemDrawer : PropertyDrawer
{
    private SerializedProperty id;
    private SerializedProperty amount;
    private SerializedProperty dropChance;

    private float totalHeight;

    GUIStyle labelStyle;
    private bool initialized = false;
    private void Initialize(SerializedProperty property)
    {
        initialized = true;

        //캐싱
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 10;
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;

       // propertyBGColor = new Color(0.25f, 0.25f, 0.25f, 1f);
     //   bgColor = new Color(0.5f, 0.5f, 0.5f, 1);
     //   lineColor = new Color(0.5f, 0.5f, 0.5f);
     //   originColor = GUI.color;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount = 4;
        int spriteCount = 1;

        if (property.isExpanded == true)
        {
            return totalHeight = EditorGUIUtility.singleLineHeight * lineCount +
                    EditorGUIUtility.standardVerticalSpacing * (lineCount + spriteCount - 1) +
                    64f * spriteCount;
        }
        else
        {
            return totalHeight = EditorGUIUtility.singleLineHeight;
        }
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
            Initialize(property);

        EditorGUI.BeginProperty(position, new GUIContent("DropItem"), property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, "Drop Item");
        position.y += EditorGUIUtility.singleLineHeight;


        Rect box = position;
        box.height = totalHeight - EditorGUIUtility.singleLineHeight; ;

        if (property.isExpanded == true)
        {
            EditorGUI.DrawRect(box, new Color(0.4f, 0.4f, 0.4f, 0.7f));

            Rect rectState = new Rect(position.position + new Vector2(70, 0), new Vector2(position.size.x / 2, 20));
            Rect rectState_label = new Rect(position.position + new Vector2(5, 0), new Vector2(100, 20));

            id = property.FindPropertyRelative("itemID");
            Item item =  ItemDataBase.GetItem(id.intValue);

            EditorGUI.LabelField(rectState_label, "Item ID", labelStyle);
            EditorGUI.PropertyField(rectState, id, GUIContent.none, true);
            if (item != null)
            {
                EditorGUI.LabelField(rectState_label, "Item ID", labelStyle);

                //EditorGUILayout.ObjectField(item.sprite, typeof(Sprite), true);
                Rect itemnameRect = new Rect(position.position + new Vector2(25, EditorGUIUtility.singleLineHeight + 5), new Vector2(80, 20));
                EditorGUI.LabelField(itemnameRect, item.ItemName, labelStyle);
                itemnameRect.position = new Vector2(itemnameRect.x, itemnameRect.y + EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(itemnameRect, item.itemType.ToString(), labelStyle);
                itemnameRect.position = new Vector2(itemnameRect.x, itemnameRect.y + EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(itemnameRect, item.ItemGrade.ToString(), labelStyle);
                Rect spriteRect = new Rect(position.position + new Vector2(105, EditorGUIUtility.singleLineHeight + 5), new Vector2(64, 64));
                EditorGUI.ObjectField(spriteRect, item.sprite, typeof(Sprite), false);

                itemnameRect.position = new Vector2(itemnameRect.x, itemnameRect.y + 10f);
                rectState_label.position = itemnameRect.position;
            }
            else
            {
                labelStyle.normal.textColor = Color.red;
                EditorGUI.LabelField(rectState_label, "Item ID", labelStyle);
                labelStyle.normal.textColor = Color.white;
            }

            rectState_label.position = new Vector2(position.x + 5f, rectState_label.y + EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rectState_label, "Amount", labelStyle);
            amount = property.FindPropertyRelative("amount");
            rectState = new Rect(rectState_label.position + new Vector2(70, 0), new Vector2(position.size.x / 2, 20));
            EditorGUI.PropertyField(rectState, amount, new GUIContent(""), true);

            rectState_label.position = new Vector2(rectState_label.x, rectState_label.y + EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rectState_label, "Drop Chance", labelStyle);
            dropChance = property.FindPropertyRelative("dropChance");
            rectState = new Rect(rectState_label.position + new Vector2(70, 0), new Vector2(position.size.x / 2, 20));
            EditorGUI.PropertyField(rectState, dropChance, new GUIContent(""), true);
        }


        EditorGUI.EndProperty();

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

}
