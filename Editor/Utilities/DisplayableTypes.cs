using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditorUtilities.Editor.Attributes.AbstractReference.Utilities;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Utilities
{
    [Serializable]
    public struct DisplayableTypes : IEnumerable<(Type, GUIContent)>
    {
        private Type[] m_Types;

        public Type[] Types => m_Types;
        
        private GUIContent[] m_TypeNames;

        public GUIContent[] TypeNames => m_TypeNames;

        public DisplayableTypes(Type[] _types)
        {
            m_Types = _types;
            m_TypeNames = m_Types.Select(t => t.GetGUIContent()).ToArray();
        }

        public static DisplayableTypes CreateFromSubClass<BaseClass>(bool _includeAbstract = false)
        {
            IEnumerable<Type> types = TypeCache.GetTypesDerivedFrom<BaseClass>();
            return CreateFromEnumerable(types, _includeAbstract);
        }
        
        public static DisplayableTypes CreateFromSubClass(Type _base, bool _includeAbstract = false)
        {
            IEnumerable<Type> types = TypeCache.GetTypesDerivedFrom(_base);
            return CreateFromEnumerable(types, _includeAbstract);
        }
        
        private static DisplayableTypes CreateFromEnumerable(IEnumerable<Type> _types, bool _includeAbstract)
        {
            if (!_includeAbstract)
            {
                _types = _types.Where(t => !t.IsAbstract);
            }

            return new DisplayableTypes(_types.ToArray());
        }

        public IEnumerator<(Type, GUIContent)> GetEnumerator()
        {
            for (var i = 0; i < m_Types.Length; i++)
            {
                yield return (m_Types[i], m_TypeNames[i]);
            }
        }
        
        public IEnumerable<(Type, GUIContent)> GetEnumerable()
        {
            for (var i = 0; i < m_Types.Length; i++)
            {
                yield return (m_Types[i], m_TypeNames[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<(Type, GUIContent)> ToTupleList()
        {
            return new List<(Type, GUIContent)>(GetEnumerable());
        }
    }
}