using System;
using EditorUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Utilities.DrawerFactory
{
    public abstract class AObjectDrawer : ADrawer
    {
        public abstract Type HandledType
        {
            get;
        }
    }
}