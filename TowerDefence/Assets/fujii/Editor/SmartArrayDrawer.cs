using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SmartArrayAttribute))]
public class SmartArrayDrawer : PropertyDrawer
{
    /// <summary>インデント段階を保存</summary>
    private int lastIndentLevel;


    /// <summary>1行におく要素の数</summary>
    private const int ItemsInLine = 10;

    /// <summary>ラベルの幅</summary>
    private const int LabelWidth = 30;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BeginProperty(position, property, label);

        int index = GetIndex(property);
        var targetRect = position;
        if (index % ItemsInLine == 0)
        {
            var labelRect = targetRect;
            labelRect.width = LabelWidth;
            labelRect.height = 18;
            EditorGUI.LabelField(labelRect, index.ToString());
        }
        targetRect.y += -(index % ItemsInLine) * 2;
        targetRect.x += LabelWidth + index % ItemsInLine * ((position.width - LabelWidth) / ItemsInLine);
        targetRect.width = 26;
        targetRect.height = 18;
        EditorGUI.PropertyField(targetRect, property, new GUIContent());


        EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }

    int GetIndex(SerializedProperty property)
    {
        var elem = property;
        var path = elem.propertyPath;
        var num = path.Substring(path.LastIndexOf('[') + 1);
        num = num.Substring(0, num.Length - 1);
        int index = 0;
        if (!int.TryParse(num, out index))
        {
            Debug.LogWarningFormat("Failed to parse int from: {0}, originally: {1}", num, path);
        }

        return index;
    }

    /// <summary>
    /// プロパティの開始
    /// </summary>
    /// <param name="position">OnGUIと同じ</param>
    /// <param name="property">OnGUIと同じ</param>
    /// <param name="label">OnGUIと同じ</param>
    protected void BeginProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUIUtility.labelWidth = 80 + EditorGUI.indentLevel * 20;
        label = EditorGUI.BeginProperty(position, label, property);

        this.lastIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
    }

    /// <summary>
    /// プロパティの終了
    /// </summary>
    protected void EndProperty()
    {
        EditorGUI.indentLevel = this.lastIndentLevel;
        EditorGUI.EndProperty();
    }
}