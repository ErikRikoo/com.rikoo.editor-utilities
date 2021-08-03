using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using EditorUtilities.Editor.Extensions.TypeSystemUtilities;
using UnityEditor;

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
            Match match = regex.Match(instance);
            return Convert.ToInt32(match.Groups[1].Value);
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
    }
}