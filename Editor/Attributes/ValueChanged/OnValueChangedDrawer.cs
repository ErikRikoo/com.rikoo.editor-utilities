using EditorUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Attributes.ValueChanged
{
    enum ChangeType
    {
        None,
        Keyboard,
        Mouse,
    }
    
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
    public class OnValueChangedDrawer : PropertyDrawer
    {
        private class Data
        {
            public ChangeType m_ChangeType;
        }
        
        private OnValueChangedAttribute Attr => attribute as OnValueChangedAttribute;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool canBeDelayed = CanBeDelayed(property);
            EventType currentEventType = Event.current.type;
            KeyCode currentKeyCode = Event.current.keyCode;
            EditorGUI.PropertyField(position, property, label);

            int lastCOntrolId = VariousEditorGUIUtilities.LastControlID;
            var data = GUIUtility.GetStateObject(typeof(Data), lastCOntrolId) as Data;
            if (EditorGUI.EndChangeCheck())
            {
                if (!canBeDelayed || !Attr.CallOnlyWhenModificationOver)
                {
                    CallMethod(property);
                } else
                {
                    data.m_ChangeType = currentEventType switch
                    {
                        EventType.MouseDrag => ChangeType.Mouse,
                        EventType.KeyDown => ChangeType.Keyboard,
                        _ => ChangeType.None
                    };
                }
            }

            if (canBeDelayed && Attr.CallOnlyWhenModificationOver && data.m_ChangeType != ChangeType.None)
            {
                
                switch (data.m_ChangeType)
                {
                    case ChangeType.Keyboard when currentKeyCode == KeyCode.Return || currentKeyCode == KeyCode.Escape:
                        data.m_ChangeType = ChangeType.None;
                        CallMethod(property);
                        break;
                    case ChangeType.Mouse when currentEventType == EventType.MouseUp:
                        data.m_ChangeType = ChangeType.None;
                        CallMethod(property);
                        break;
                }
            }
        }

        private bool CanBeDelayed(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Float ||
                   property.propertyType == SerializedPropertyType.Integer ||
                   property.propertyType == SerializedPropertyType.String;
        }

        private void CallMethod(SerializedProperty _property)
        {                    
            _property.serializedObject.ApplyModifiedProperties();
            var method = _property.serializedObject.targetObject.GetType().GetMethod(Attr.Method);
            method.Invoke(_property.serializedObject.targetObject, null);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property);
    }
}