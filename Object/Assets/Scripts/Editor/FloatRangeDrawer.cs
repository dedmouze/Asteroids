using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int originalIndentLevel = EditorGUI.indentLevel;
        float originalLabelWidth = EditorGUIUtility.labelWidth;
        
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width /= 2f;
        EditorGUIUtility.labelWidth = position.width / 2;
        EditorGUI.indentLevel = 1;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("Min"));
        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("Max"));
        
        EditorGUI.EndProperty();
        
        EditorGUI.indentLevel = originalIndentLevel;
        EditorGUIUtility.labelWidth = originalLabelWidth;
    }
}
