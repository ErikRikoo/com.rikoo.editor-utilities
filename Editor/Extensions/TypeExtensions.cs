using System;

namespace EditorUtilities.Editor.Extensions
{
    public static class TypeExtensions
    {
        public static Object GetDefaultInstance(this Type instance)
        {
            return Activator.CreateInstance(instance);
        }
    }
}