using System;
using System.Collections.Generic;
using System.Linq;
using EditorUtilities.Editor.Utilities;

namespace Utilities.DrawerFactory
{
    public class DisplayableDrawerFactory<DrawerType>
    where DrawerType : ABaseDrawer
    {
        private DisplayableTypes m_Types;
        private Dictionary<Type, DrawerType> m_Instances;

        public DisplayableDrawerFactory()
        {
            m_Types = DisplayableTypes.CreateFromSubClass<DrawerType>();
            m_Instances = m_Types.Types
                .Select(t => (DrawerType)Activator.CreateInstance(t))
                .ToDictionary(drawer => drawer.HandledType);
        }

        public bool TryGetDrawer(Type _handledType, out DrawerType _drawer)
        {
            return m_Instances.TryGetValue(_handledType, out _drawer);
        }
    }
}