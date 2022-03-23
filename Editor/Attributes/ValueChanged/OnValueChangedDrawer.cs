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
        private OnValueChangedAttribute Attr => attribute as OnValueChangedAttribute;
        
        private ChangeType m_ChangeType;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EventType currentEventType = Event.current.type;
            KeyCode currentKeyCode = Event.current.keyCode;
            EditorGUI.PropertyField(position, property, label);

            if (EditorGUI.EndChangeCheck())
            {
                if (!Attr.CallOnlyWhenModificationOver)
                {
                    CallMethod(property);
                } else {
                    m_ChangeType = currentEventType switch
                    {
                        EventType.MouseDrag => ChangeType.Mouse,
                        EventType.KeyDown => ChangeType.Keyboard,
                        _ => ChangeType.None
                    };
                }
            }

            if (Attr.CallOnlyWhenModificationOver && m_ChangeType != ChangeType.None)
            {
                switch (m_ChangeType)
                {
                    case ChangeType.Keyboard when currentKeyCode == KeyCode.Return || currentKeyCode == KeyCode.Escape:
                        m_ChangeType = ChangeType.None;
                        CallMethod(property);
                        break;
                    case ChangeType.Mouse when currentEventType == EventType.MouseUp:
                        m_ChangeType = ChangeType.None;
                        CallMethod(property);
                        break;
                }
            }
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