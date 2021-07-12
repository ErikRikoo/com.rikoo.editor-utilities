using System;
using System.Linq;
using System.Reflection;
using EditorUtilities.Editor.Attributes.AbstractReference.Utilities;
using EditorUtilities.Editor.Extensions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public abstract class APropertyHandler
    {
        protected SerializedProperty m_Property;
        protected FieldInfo m_FieldInfo;

        protected APropertyHandler(SerializedProperty property, FieldInfo fieldInfo)
        {
            m_Property = property;
            m_FieldInfo = fieldInfo;
        }

        public void HandleProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckAttributeAttachment(property);
            if (IsPropertyNull())
            {
                Type baseType = property.GetManagedReferenceType();
                if (ShouldDisplayLabelWhenNull)
                {
                    EditorGUI.LabelField(position, property.displayName);
                    position.x = position.width * 0.5f;
                    position.width *= 0.5f;
                }
                
                Type chosen = DisplayImplementationsPopup(baseType, position);
                if (chosen != null)
                {
                    SetPropertyValueAndSave(chosen.GetDefaultInstance());
                }
            }
            else
            {
                position.width -= EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, property, new GUIContent(GetDisplayLabel(label)), true);
                Rect buttonRect = position;
                buttonRect.height = EditorGUIUtility.singleLineHeight;
                buttonRect.width = EditorGUIUtility.singleLineHeight;
                buttonRect.x = position.x + position.width;
                if (GUI.Button(buttonRect, "x"))
                {
                    SetPropertyValueAndSave(null);
                }
            }
        }

        public abstract string GetDisplayLabel(GUIContent label);

        public virtual bool ShouldDisplayLabelWhenNull => false;

        public abstract bool IsPropertyNull();
        public abstract void SetPropertyValue(object instance);
        
        private string AttributeInvalidMessage => "The attribute should be on a SerializeReference property";
        private void CheckAttributeAttachment(SerializedProperty _property)
        {
            Assert.AreEqual(_property.propertyType, SerializedPropertyType.ManagedReference, AttributeInvalidMessage);
        }
        
        private Type DisplayImplementationsPopup(Type _baseType, Rect position)
        {
            var types = TypeCache.GetTypesDerivedFrom(_baseType);
            var names = types.Select(type => type.GetGUIContent()).ToArray();
            int newIndex = EditorGUI.Popup(position, -1, names);
            return newIndex != -1 ? types[newIndex] : null;
        }

        private void SetPropertyValueAndSave(object instance)
        {
            SetPropertyValue(instance);
            m_Property.serializedObject.ApplyModifiedProperties();
        }
    }
}