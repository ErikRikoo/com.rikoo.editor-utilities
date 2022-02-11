using System.Text.RegularExpressions;
using UnityEditor;

namespace EditorUtilities.Editor.Extensions
{
    public static class StringExtension
    {
        public static string ComputeNiceName(this string instance)
        {
            return new Regex("([A-Z]|[0-9])").Replace(instance, " $1").Trim();
        }
        
        public static string ToNiceName(this string _instance, string _suffix)
        {
            string ret = _instance;
            if (ret.EndsWith(_suffix))
            {
                ret = _instance.Substring(0, ret.Length - _suffix.Length);
            }
            
            return ret.ToNiceName();
        }

        public static string ToNiceName(this string _instance)
        {
            return ObjectNames.NicifyVariableName(_instance);
        }
    }
 }