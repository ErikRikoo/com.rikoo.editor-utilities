using EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.PropertyHandler
{
    public class DirectPropertyHandler : APropertyHandler
    {
        public DirectPropertyHandler(SerializedProperty property) : base(property) {}

        public override string GetDisplayLabel(GUIContent label)
        {
            string typeName = m_Value.Value.GetType().Name;
            return $"{label.text} ({typeName})";
        }

        public override bool ShouldDisplayLabelWhenNull => true;
    }
}