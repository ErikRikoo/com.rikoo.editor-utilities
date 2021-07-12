using System.Text.RegularExpressions;

namespace EditorUtilities.Editor.Extensions
{
    public static class StringExtension
    {
        public static string ComputeNiceName(this string instance)
        {
            return new Regex("([A-Z]|[0-9])").Replace(instance, " $1").Trim();
        }
    }
 }