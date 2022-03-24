﻿using System;
using EditorUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Utilities.DrawerFactory
{
    public abstract class ABaseDrawer
    {
        public abstract Type HandledType
        {
            get;
        }

        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (label == null)
            {
                EditorDrawingUtilities.DrawPropertyChildren(position, property);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public virtual float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) -
                   (label == null
                       ? EditorGUIUtility.singleLineHeight
                       : 0f
                   );
        }
    }
}