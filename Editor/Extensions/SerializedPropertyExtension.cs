using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using EditorUtilities.Editor.Extensions.TypeSystemUtilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorUtilities.Editor.Extensions
{
    public static class SerializedPropertyExtension
    {
        public static Type GetManagedReferenceType(this SerializedProperty instance)
        {
            string[] splittedTypename = instance.managedReferenceFieldTypename.Split(' ');
            return Type.GetType($"{splittedTypename[1]}, {splittedTypename[0]}");
        }

        private static Regex regex = new Regex(@".*\[(\d+)\]");
        public static int GetPropertyIndexInArray(this string instance)
        {
            MatchCollection match = regex.Matches(instance);
            
            return Convert.ToInt32(match[match.Count - 1].Groups[1].Value);
        }
        
        public static int GetPropertyIndexInArray(this SerializedProperty instance)
        {
            return instance.propertyPath.GetPropertyIndexInArray();
        }
        
        public static InstanceField GetInstanceField(this SerializedProperty instance)
        {
            string cleanedPath = instance.propertyPath.Replace(".Array.data[", "[");
            InstanceField obj = new InstanceWithNoField()
            {
                Instance = instance.serializedObject.targetObject,
            };
            string[] pathToProperty = cleanedPath.Split('.');
            foreach (var path in pathToProperty)
            {
                if (path.Contains("["))
                {
                    string pathWithoutIndex = Regex.Replace(path, "\\[\\d*\\]", "");
                    int index = path.GetPropertyIndexInArray();
                    obj = GetFieldWithPath(obj, pathWithoutIndex, index);
                }
                else
                {
                    obj = GetFieldWithPath(obj, path);
                }
            }
            
            return obj;
        }

        private static InstanceField GetFieldWithPath(this InstanceField instance, string path)
        {
            Type type = instance.GetValue<object>().GetType();

            while (type != null)
            {
                FieldInfo info = type.GetField(path,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (info != null)
                {
                    return new DirectInstanceField
                    {
                        Info = info,
                        Instance = instance.GetValue<object>(),
                    };
                }

                type = type.BaseType;
            }

            return null;
        }
        
        
        private static InstanceField GetFieldWithPath(this InstanceField instance, string path, int index)
        {
            var array = GetFieldWithPath(instance, path).GetValue<Array>();
            if (array == null)
            {
                return null;
            }

            return new ArrayInstanceField
            {
                Instance = array,
                Index = index,
            };
        }

        public static IEnumerable<SerializedProperty> GetChildren(
            this SerializedProperty _instance, bool _enterChildren = false
        )
        {
            SerializedProperty current = _instance.Copy();
            SerializedProperty end = _instance.Copy();
            end.NextVisible(false);

            bool shouldContinue = current.NextVisible(true);
            while (shouldContinue && !SerializedProperty.EqualContents(current, end))
            {
                yield return current.Copy();
                shouldContinue = current.NextVisible(_enterChildren);
            }
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty _instance)
        {
            string path = _instance.propertyPath;
            string[] splittedPath = path.Split('.');
            Type objectType = _instance.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = null;
            Type currentLookedType = objectType;
            object obj = _instance.serializedObject.targetObject;
            
            for (int i = 0; i < splittedPath.Length; ++i)
            {
                Type typeIteration = currentLookedType;
                do
                {
                    fieldInfo = typeIteration.GetField(splittedPath[i], (BindingFlags) (-1));
                    typeIteration = typeIteration.BaseType;

                } while (fieldInfo == null && typeIteration != null);
                
                if (fieldInfo == null)
                {
                    return null;
                }

                obj = fieldInfo.GetValue(obj);
                if (obj == null)
                {
                    return null;
                }
                currentLookedType = obj.GetType();
            }

            return fieldInfo;
        }
        

        public static GUIContent GetGUIContent(this SerializedProperty _instance)
        {
            return new GUIContent(_instance.displayName, _instance.tooltip);
        }

        public static void ChangeType(this SerializedProperty _instance, Type _newType)
        {
            _instance.managedReferenceValue = Activator.CreateInstance(_newType);
            _instance.serializedObject.ApplyModifiedProperties();
        }

        public static void AppendArrayProperty(this SerializedProperty _instance, Object _elementToAdd)
        {
            int size = _instance.arraySize;
            _instance.InsertArrayElementAtIndex(size);
            _instance.GetArrayElementAtIndex(size).objectReferenceValue = _elementToAdd;
        }
        
        public static void AppendArrayProperty(this SerializedProperty _instance, float _elementToAdd)
        {
            int size = _instance.arraySize;
            _instance.InsertArrayElementAtIndex(size);
            _instance.GetArrayElementAtIndex(size).floatValue = _elementToAdd;
        }

        public static IEnumerable<SerializedProperty> GetArrayElement(this SerializedProperty _instance)
        {
            for (int i = 0; i < _instance.arraySize; ++i)
            {
                yield return _instance.GetArrayElementAtIndex(i);
            }
        }

        public static bool FindObjectIndex<T>(this SerializedProperty _instance, T _object, out int _index)
            where T : UnityEngine.Object
        {
            if (_instance == null || !_instance.isArray)
            {
                _index = -1;
                return false;
            }
            
            for (int i = 0; i < _instance.arraySize; i++)
            {
                var currentVar = _instance.GetArrayElementAtIndex(i).objectReferenceValue as T;
                if (_object == currentVar)
                {
                    _index = i;
                    return true;
                }
            }

            _index = -1;
            return false;
        }

        public static SerializedProperty GetParent(this SerializedProperty _instance)
        {
            string propertyPath = _instance.propertyPath;
            int lastDotOccurence = propertyPath.LastIndexOf('.');
            if (lastDotOccurence == -1)
            {
                return null;
            }
            
            int lastArrayDataOccurence = propertyPath.LastIndexOf("data");
            int substrEnd = lastArrayDataOccurence > lastDotOccurence
                ? propertyPath.LastIndexOf(".Array.data")
                : lastDotOccurence;
            string parentPath = _instance.propertyPath.Substring(0, substrEnd);

            return _instance.serializedObject.FindProperty(parentPath);
        }
    }
}