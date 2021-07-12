using System;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Editor.Extensions
{
    public static class SerializedPropertyExtension
    {
        public static Type GetManagedReferenceType(this SerializedProperty instance)
        {
            string[] splittedTypename = instance.managedReferenceFieldTypename.Split(' ');
            return Type.GetType($"{splittedTypename[1]}, {splittedTypename[0]}");
        }

        private static Regex regex = new Regex(@".*\[(\d+)\]");
        public static int GetPropertyIndexInArray(this SerializedProperty instance)
        {
            Match match = regex.Match(instance.propertyPath);
            return Convert.ToInt32(match.Groups[1].Value);
        }
    }
}