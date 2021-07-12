using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Editor.Attributes.AbstractReference
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AbstractNamingAttribute : Attribute
    {
        [CanBeNull] public string DisplayName;
        /**
         * Not used yet.
         */
        [CanBeNull] public string Tooltip;
        
        public AbstractNamingAttribute([CanBeNull] string displayName = null, string tooltip = null)
        {
            DisplayName = displayName;
            Tooltip = tooltip;
        }
    }
}