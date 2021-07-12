using System;
using System.Reflection;
using EditorUtilities.Editor.Extensions;
using UnityEngine;

namespace EditorUtilities.Editor.Attributes.AbstractReference.Utilities
{
    public static class AbstractReferenceTypeExtension
    {
        public static GUIContent GetGUIContent(this Type instance)
        {
            AbstractNamingAttribute attr = instance.GetCustomAttribute<AbstractNamingAttribute>();
            if (attr == null)
            {
                return new GUIContent(instance.Name.ComputeNiceName());
            }
            
            return new GUIContent(
                attr.DisplayName ?? instance.Name.ComputeNiceName()
                );
        }
    }
}