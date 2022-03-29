using System;
using System.Reflection;
using EditorUtilities.Editor.Extensions;
using EditorUtilities.Editor.Extensions.TypeSystemUtilities;
using EditorUtilities.Editor.Utilities;
using Logic.Scripts.Utilities.Extensions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Utilities.DrawerFactory;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public abstract class APropertyHandler
    {
        private static ObjectDrawerFactory<AObjectDrawer> s_Drawers = new ObjectDrawerFactory<AObjectDrawer>();

        private static readonly Color DarkerColor = new Color(0.19f, 0.19f, 0.19f);
        private static readonly float FoldoutArrowSize = 12;
        public static readonly int Padding = 3;

        protected InstanceField m_Value;
        protected SerializedProperty m_Property;
        protected FieldInfo m_FieldInfo;

        public Type PropertyType => m_Value.Value?.GetType();

        public void Init(SerializedProperty property, FieldInfo fieldInfo)
        {
            m_Value = property.GetInstanceField();
            m_Property = property;
            m_FieldInfo = fieldInfo;
        }

        public virtual bool HasBackground => true;

        public void HandleProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!PreDrawing(property))
            {
                return;
            }
            
            if (HasBackground)
            {
                var backGroundRect = new Rect(position);
                backGroundRect.Indent();
                backGroundRect.ExpandLeft(FoldoutArrowSize);
                EditorGUI.DrawRect(backGroundRect, DarkerColor);
            }
            position.AddPadding(Padding);

            
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
                    OnRemove();
                }
            }

            if (ShouldDisplayLabelWhenNull)
            {
                position.x += EditorGUIUtility.labelWidth;
                position.width -= EditorGUIUtility.labelWidth;
            }
            if (!isPropertyNull)
            {
                position.width -= EditorGUIUtility.singleLineHeight;
            }

            position.height = EditorGUIUtility.singleLineHeight;
            if ( DisplayImplementationsPopup(baseType, position, out Type chosen))
            {
                SetPropertyValueAndSave(chosen.GetDefaultInstance());
            }
        }

        protected virtual void OnRemove() {}

        private void DrawPropertiesAndLabel(Rect _position, SerializedProperty _property)
        {
            Rect foldoutPosition = _position;
            foldoutPosition.width = EditorGUIUtility.labelWidth;
            foldoutPosition.height = EditorGUIUtility.singleLineHeight;
            string label = ShouldDisplayLabelWhenNull ? GetDisplayLabel(new GUIContent(_property.displayName)) : "";
            if (_property.hasVisibleChildren)
            {
                _property.isExpanded = EditorGUI.Foldout(foldoutPosition, _property.isExpanded, label);
            }
            else
            {
                EditorGUI.LabelField(foldoutPosition, label);
            }
            _position.AddLine();

            if (!_property.isExpanded)
            {
                return;
            }
            _position.AddLine(PostDropdownMarginValue);
            ++EditorGUI.indentLevel;
            _position.Indent();
            if (s_Drawers.TryGetDrawer(PropertyType, out AObjectDrawer _drawer))
            {
                _drawer.OnGUI(_position, _property, null);
            }
            else
            {
                EditorDrawingUtilities.DrawPropertyChildren(_position, _property);
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
            m_Property.managedReferenceValue = instance;
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

        public static float PostDropdownMarginValue => 4;
        
        public static float PostDropdownMargin(SerializedProperty property)
        {
            return property.isExpanded && property.hasVisibleChildren? PostDropdownMarginValue : 0;
        }

        public virtual bool PreDrawing(SerializedProperty _property)
        {
            return true;
        }

        public float GetBodyHeight(SerializedProperty property, GUIContent label)
        {
            return s_Drawers.TryGetDrawer(PropertyType, out AObjectDrawer _drawer)
                ? _drawer.GetPropertyHeight(property, null)
                : EditorGUI.GetPropertyHeight(property) - EditorGUIUtility.singleLineHeight;
        }
    }
}