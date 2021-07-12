using System;
using System.Reflection;
using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public class ArrayPropertyHandler : APropertyHandler
    {
        private Array m_PropertyAsArray;
        private int m_ValueIndex;

        public ArrayPropertyHandler(SerializedProperty property, FieldInfo fieldInfo) : base(property, fieldInfo)
        {
            m_PropertyAsArray = (Array) fieldInfo.GetValue(property.serializedObject.targetObject);
            m_ValueIndex = m_Property.GetPropertyIndexInArray();
        }

        public override string GetDisplayLabel(GUIContent label)
        {
            return m_PropertyAsArray.GetValue(m_ValueIndex).GetType().Name;
        }
        
        public override bool IsPropertyNull()
        {
           
            return m_PropertyAsArray.GetValue(m_ValueIndex) == null;
        }

        public override void SetPropertyValue(object instance)
        {
            m_PropertyAsArray.SetValue(instance, m_ValueIndex);
        }
    }
}