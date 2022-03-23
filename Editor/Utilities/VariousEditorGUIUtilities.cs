using System.Reflection;
using UnityEditor;

namespace EditorUtilities.Editor.Utilities
{
    public static class VariousEditorGUIUtilities
    {
        private static FieldInfo s_LastControlIDField;

        
        private static FieldInfo LastControlIDField
        {
            get
            {
                return s_LastControlIDField ??= typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
            }
        }

        public static int LastControlID => (int) LastControlIDField.GetValue(null);
    }
}