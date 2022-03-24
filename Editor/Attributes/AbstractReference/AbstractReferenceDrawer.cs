using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference
{
    [CustomPropertyDrawer(typeof(AbstractReferenceAttribute))]
    public class AbstractReferenceDrawer : PropertyDrawer
    {
        private ArrayPropertyHandler m_ArrayProperty;

        private ArrayPropertyHandler ArrayProperty => m_ArrayProperty ??= new ArrayPropertyHandler();
        
        private DirectPropertyHandler m_DirectPropertyHandler;

        private DirectPropertyHandler DirectPropertyHandler => m_DirectPropertyHandler ??= new DirectPropertyHandler();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            APropertyHandler handler = GetRightPropertyHandler(property);
            handler.HandleProperty(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private APropertyHandler GetRightPropertyHandler(SerializedProperty property)
        {
            if (fieldInfo.FieldType.IsArray)
            {
                ArrayProperty.Init(property, fieldInfo);
                return ArrayProperty;
            }

            DirectPropertyHandler.Init(property, fieldInfo);
            return DirectPropertyHandler;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return APropertyHandler.Padding * 2 + EditorGUIUtility.singleLineHeight +
            
            (property.GetInstanceField().Value == null
                ? 0
                : GetRightPropertyHandler(property).GetBodyHeight(property, label)) + APropertyHandler.PostDropdownMargin(property);
        }
    }
}