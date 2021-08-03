using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference
{
    [CustomPropertyDrawer(typeof(AbstractReferenceAttribute))]
    public class AbstractReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            APropertyHandler handler = GetRightPropertyHandler(property);
            handler.HandleProperty(position, property, label);
        }

        private APropertyHandler GetRightPropertyHandler(SerializedProperty property)
        {
            if (fieldInfo.FieldType.IsArray)
            {
                return new ArrayPropertyHandler(property);
            }

            return new DirectPropertyHandler(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.GetInstanceField().Value == null
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(property);
        }
    }
}