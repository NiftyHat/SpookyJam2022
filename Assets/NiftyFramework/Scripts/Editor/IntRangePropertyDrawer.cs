using UnityEditor;
using UnityEngine;

namespace NiftyFramework.Scripts
{
    [CustomPropertyDrawer(typeof(IntRange))]
    public class IntRangePropertyDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float labelWidth = 40;
            float valueRectWidth = (position.width - labelWidth) * 0.5f;
            
            
            // Calculate rects
            var minRect = new Rect(position.x, position.y, valueRectWidth, position.height);
            var labelRect = new Rect(minRect.max.x, position.y, labelWidth, position.height);
            var maxRect = new Rect(labelRect.max.x, position.y, valueRectWidth, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("_min"), GUIContent.none);
            EditorGUI.LabelField(labelRect, "->");
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("_max"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}