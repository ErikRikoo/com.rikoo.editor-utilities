using System;
using System.Collections.Generic;
using System.Linq;
using EditorUtilities.Editor.Utilities;
using UnityEngine;

namespace Utilities.DrawerFactory
{
    public class ObjectDrawerFactory<DrawerType> : MainDrawerFactory<DrawerType>
    where DrawerType : AObjectDrawer
    {

        public bool TryGetDrawer(Type _handledType, out DrawerType _drawer)
        {
            _drawer = m_Instances.FirstOrDefault(drawer => drawer.HandledType == _handledType);
            return _drawer != null;
        }
    }
}