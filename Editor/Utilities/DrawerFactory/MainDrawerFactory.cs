using System;
using System.Collections.Generic;
using System.Linq;
using EditorUtilities.Editor.Utilities;
using UnityEngine;

namespace Utilities.DrawerFactory
{
    public class MainDrawerFactory<DrawerType>
        where DrawerType : ADrawer
    {
        protected DisplayableTypes m_Types;
        protected DrawerType[] m_Instances;

        public MainDrawerFactory()
        {
            m_Types = DisplayableTypes.CreateFromSubClass<DrawerType>();
            m_Instances = m_Types.Types
                .Select(t => (DrawerType)Activator.CreateInstance(t))
                .ToArray();
        }

        public DrawerType GetInstance(int _index)
        {
            var pair = m_Instances.ElementAtOrDefault(_index);
            return pair;
        }
        
        public Type[] Types => m_Types.Types;
        public GUIContent[] Names => m_Types.TypeNames;
        public int Count => Types.Length;
    }
}