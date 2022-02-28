using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using EditorUtilities.Editor.Attributes.AbstractReference.Utilities;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public class DirectPropertyHandler : APropertyHandler
    {
        public DirectPropertyHandler(SerializedProperty property) : base(property) {}

        public override string GetDisplayLabel(GUIContent label)
        {
            // string typeName = m_Value.Value.GetType().GetGUIContent().text;
            // return $"{label.text} ({typeName})";
            return label.text;
        }

        public override bool ShouldDisplayLabelWhenNull => true;
    }
}