using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor.Attributes.AbstractReference.PropertyHandler
{
    public class DirectPropertyHandler : APropertyHandler
    {
        public DirectPropertyHandler(SerializedProperty property, FieldInfo fieldInfo) : base(property, fieldInfo) {}

        public override string GetDisplayLabel(GUIContent label)
        {
            string typeName = m_FieldInfo.GetValue(m_Property.serializedObject.targetObject).GetType().Name;
            return $"{label.text} ({typeName})";
        }

        public override bool IsPropertyNull()
        {
            return m_FieldInfo.GetValue(m_Property.serializedObject.targetObject) == null;
        }

        public override void SetPropertyValue(object instance)
        {
            m_FieldInfo.SetValue(m_Property.serializedObject.targetObject, instance);
        }

        public override bool ShouldDisplayLabelWhenNull => true;
    }
}