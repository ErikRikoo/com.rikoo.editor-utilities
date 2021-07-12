using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Editor.Attributes.AbstractReference.PropertyHandler;
using Editor.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
using Object = System.Object;

namespace Packages.com.rikoo.editor.attributes.Editor.Attributes.AbstractReference
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
                return new ArrayPropertyHandler(property, fieldInfo);
            }

            return new DirectPropertyHandler(property, fieldInfo);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return fieldInfo.GetValue(property.serializedObject.targetObject) == null
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(property);
        }
    }
}