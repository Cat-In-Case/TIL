using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GrowthData))]
public class GrowthDrawer : PropertyDrawer
{
    private SerializedProperty plantState;
    private SerializedProperty sprite;
    private SerializedProperty growingPeriod;
    private SerializedProperty growChance;
    GUIStyle labelStyle;
    private bool initialized = false;

    private Color lineColor;
    private Color propertyBGColor;
    private Color bgColor;
    private Color originColor;

    private float totalHeight = 0;
    private void Initialize(SerializedProperty property)
    {
        initialized = true;

        //Ä³½Ì
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 10;
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;

        propertyBGColor = new Color(0.25f, 0.25f, 0.25f, 1f);
        bgColor = new Color(0.5f, 0.5f, 0.5f, 1);
        lineColor = new Color(0.5f, 0.5f, 0.5f);
        originColor = GUI.color;

        sprite = property.FindPropertyRelative("stepSprite");
        if(sprite.objectReferenceValue == null)
        {
            Debug.LogAssertion("Sprite is Null");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount = 4;
        int spriteCount = 1;

        if (property.isExpanded == true)
        {
            return totalHeight = EditorGUIUtility.singleLineHeight * lineCount +
                    EditorGUIUtility.standardVerticalSpacing * (lineCount + spriteCount - 1) +
                    60f * spriteCount;
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

        Rect box = position;
        box.y += EditorGUIUtility.singleLineHeight;
        box.height = totalHeight - EditorGUIUtility.singleLineHeight;
        EditorGUI.DrawRect(box, propertyBGColor);
        float height = 0;
        EditorGUI.BeginProperty(position, label, property);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        position.y += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;
        property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, property.displayName);

        Rect line = position;
        line.size = new Vector2(position.size.x, 0.5f);

        if (property.isExpanded == true)
        {
            //EditorGUIUtility.labelWidth = 60;

            plantState = property.FindPropertyRelative("plantState");
            growingPeriod = property.FindPropertyRelative("growingPeriod");
            sprite = property.FindPropertyRelative("stepSprite");
            growChance = property.FindPropertyRelative("growChance");

            GUI.backgroundColor = bgColor;

            Rect rectState_label = new Rect(position.position + new Vector2(5, 0), new Vector2(100, 20));
            Rect rectState = new Rect(position.position + new Vector2(70, 0), new Vector2(position.size.x / 2, 20));
            GUI.color = Color.yellow;
            EditorGUI.LabelField(rectState_label, "Plant State", labelStyle);
            GUI.color = originColor;
            EditorGUI.PropertyField(rectState, plantState, GUIContent.none, true);

            line.position = new Vector2(line.position.x, line.position.y + 20);
            EditorGUI.DrawRect(line, lineColor);


            Rect rectPeriod_string = new Rect(position.position + new Vector2(position.size.x / 8, 41f), new Vector2(position.size.x /3, 20));
            EditorGUI.PropertyField(rectPeriod_string, growingPeriod, GUIContent.none, true);
            line.position = new Vector2(line.position.x, line.position.y + 20);
            EditorGUI.DrawRect(line, lineColor);
            if(growingPeriod.intValue <= 0)
            {
                growingPeriod.intValue = 1;
            }

            Rect rectChance = new Rect(position.position + new Vector2(position.size.x / 2, 41f), new Vector2(position.size.x / 3, 20));
            EditorGUI.PropertyField(rectChance, growChance, GUIContent.none, true);
            line.position = new Vector2(line.position.x, line.position.y + 20);
            EditorGUI.DrawRect(line, lineColor);

            Rect rectPeriod_label = new Rect(position.position + new Vector2(position.size.x / 8, 20.5f), new Vector2(position.size.x / 3, 20));
            Rect rectChance_label = new Rect(position.position + new Vector2(position.size.x / 2, 20.5f), new Vector2(position.size.x / 3, 20));
            EditorGUI.LabelField(rectPeriod_label, "Growing Period", labelStyle);
            EditorGUI.LabelField(rectChance_label, "Grow Chance", labelStyle);




            Rect rectSprite_label = new Rect(position.position + new Vector2(5, 65f), new Vector2(80, 20));
            Rect rectSprite = new Rect(position.position + new Vector2(70, 61f), new Vector2(60, 60));
            GUI.color = Color.cyan;
            EditorGUI.LabelField(rectSprite_label, "Step Sprite", labelStyle);
            GUI.color = originColor;
            if(sprite.objectReferenceValue == null)
            {
                GUI.backgroundColor = Color.red;
            }
            EditorGUI.ObjectField(rectSprite, sprite, typeof(Sprite), GUIContent.none);
            GUI.backgroundColor = bgColor;
            line.position = new Vector2(line.position.x, line.position.y + 60);
            EditorGUI.DrawRect(line, lineColor);
        }

        line.position = new Vector2(line.position.x, line.position.y);
        EditorGUI.DrawRect(line, propertyBGColor);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
