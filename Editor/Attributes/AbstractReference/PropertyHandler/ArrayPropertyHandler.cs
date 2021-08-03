using System;
using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public class ArrayPropertyHandler : APropertyHandler
    {
        public ArrayPropertyHandler(SerializedProperty property) : base(property) {}

        public override string GetDisplayLabel(GUIContent label)
        {
            return m_Value.Value.GetType().Name;
        }
    }
}