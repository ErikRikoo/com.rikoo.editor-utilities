using System;
using System.Linq;
using EditorUtilities.Editor.Attributes.AbstractReference.Utilities;
using EditorUtilities.Editor.Extensions;
using EditorUtilities.Editor.Extensions.TypeSystemUtilities;
using EditorUtilities.Editor.Utilities;
using Logic.Scripts.Utilities.Extensions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public abstract class APropertyHandler
    {
        protected InstanceField m_Value;
        protected SerializedProperty m_Property;

        protected APropertyHandler(SerializedProperty property)
        {
            m_Value = property.GetInstanceField();
            m_Property = property;
        }

        public void HandleProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckAttributeAttachment(property);
            Type baseType = property.GetManagedReferenceType();
            bool isPropertyNull = IsPropertyNull();
            if (isPropertyNull)
            {
                if (!baseType.IsAbstract)
                {
                    Debug.LogError("This type should be abstract to use that attribute");
                }
                
                if (ShouldDisplayLabelWhenNull)
                {
                    EditorGUI.LabelField(position, property.displayName);
                }
            }
            else
            {
                DrawPropertiesAndLabel(position, property);

                Rect buttonRect = position;
                buttonRect.height = EditorGUIUtility.singleLineHeight;
                buttonRect.width = EditorGUIUtility.singleLineHeight;
                buttonRect.x = position.x + position.width - EditorGUIUtility.singleLineHeight;
                if (GUI.Button(buttonRect, "x"))
                {
                    SetPropertyValueAndSave(null);
                }
            }

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;
            if (!isPropertyNull)
            {
                position.width -= EditorGUIUtility.singleLineHeight;
            }
            if ( DisplayImplementationsPopup(baseType, position, out Type chosen))
            {
                SetPropertyValueAndSave(chosen.GetDefaultInstance());
            }
        }

        private void DrawPropertiesAndLabel(Rect _position, SerializedProperty _property)
        {
            Rect foldoutPosition = _position;
            foldoutPosition.width = EditorGUIUtility.labelWidth;
            foldoutPosition.height = EditorGUIUtility.singleLineHeight;
            string label = ShouldDisplayLabelWhenNull ? GetDisplayLabel(new GUIContent(_property.displayName)) : "";
            _property.isExpanded = EditorGUI.Foldout(foldoutPosition, _property.isExpanded, label);
            _position.AddLine();

            if (!_property.isExpanded)
            {
                return;
            }
            
            ++EditorGUI.indentLevel;
            foreach (var child in _property.GetChildren())
            {
                float propertyHeight = EditorGUI.GetPropertyHeight(child);
                Rect propertyRect = _position;
                propertyRect.height = propertyHeight;
                propertyRect.Indent();
                EditorGUI.PropertyField(propertyRect, child, true);
                
                _position.AddLine(propertyHeight);
            }
            
            --EditorGUI.indentLevel;
        }

        public abstract string GetDisplayLabel(GUIContent label);

        public virtual bool ShouldDisplayLabelWhenNull => false;

        public bool IsPropertyNull()
        {
            return m_Value.Value == null;
        }

        public void SetPropertyValue(object instance)
        {
            m_Value.SetValue(instance);
        }
        
        private string AttributeInvalidMessage => "The attribute should be on a SerializeReference property";
        private void CheckAttributeAttachment(SerializedProperty _property)
        {
            Assert.AreEqual(_property.propertyType, SerializedPropertyType.ManagedReference, AttributeInvalidMessage);
        }
        
        private bool DisplayImplementationsPopup(Type _baseType, Rect position, out Type _newType)
        {
            DisplayableTypes types = DisplayableTypes.CreateFromSubClass(_baseType);
            EditorGUI.BeginChangeCheck();
            int index = IsPropertyNull()? -1 : types.Types.FindIndex(m_Value.Value.GetType());
            int newIndex = EditorGUI.Popup(position, index, types.TypeNames);
            _newType = newIndex != -1 ? types.Types[newIndex] : null;
            return EditorGUI.EndChangeCheck() && newIndex != index;
        }

        private void SetPropertyValueAndSave(object instance)
        {
            SetPropertyValue(instance);
            m_Property.serializedObject.ApplyModifiedProperties();
        }
    }
}