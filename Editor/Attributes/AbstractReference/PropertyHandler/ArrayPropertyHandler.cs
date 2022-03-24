using System;
using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Attributes.AbstractReference.Utilities;
using EditorUtilities.Editor.Extensions;
using Tutorial.Wizard.LoadingScreen;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public class ArrayPropertyHandler : APropertyHandler
    {
        private static string m_RemoveLastOn;
        
        public override bool HasBackground => false; 
        
        public override string GetDisplayLabel(GUIContent label)
        {
            return m_Value.Value.GetType().GetGUIContent().text;
        }

        public override bool PreDrawing(SerializedProperty _property)
        {
            if (_property.propertyPath == m_RemoveLastOn)
            {
                var parent = _property.GetParent();
                parent.DeleteArrayElementAtIndex(parent.arraySize - 1);
                m_RemoveLastOn = String.Empty;
                return false;
            }

            return true;
        }

        protected override void OnRemove()
        {
            int index = m_Property.GetPropertyIndexInArray();
            var parentProperty = m_Property.GetParent();
            parentProperty.MoveArrayElement(index, parentProperty.arraySize - 1);

            m_RemoveLastOn = parentProperty.GetArrayElementAtIndex(parentProperty.arraySize - 1).propertyPath;
        }
    }
}