using UnityEngine;

namespace EditorUtilities.Editor.Extensions
{
    public static class GUIStyleExtension
    {
        public static GUIStyle LabelStyle => new GUIStyle(GUI.skin.label);

        public static GUIStyle CenteredLabelStyle
        {
            get
            {
                var ret = new GUIStyle(LabelStyle);
                ret.alignment = TextAnchor.MiddleCenter;
                return ret;
            }
        }

        public static GUIStyle CenterText(this GUIStyle _style)
        {
            _style.alignment = TextAnchor.MiddleCenter;
            return _style;
        }

        public static GUIStyle Bold(this GUIStyle _style)
        {
            _style.fontStyle = FontStyle.Bold;
            return _style;
        }

        public static GUIStyle ClearInteractivity(this GUIStyle _style)
        {
            _style.hover = _style.normal;
            _style.active = _style.normal;
            _style.focused = _style.normal;
            _style.onHover = _style.normal;

            return _style;
        }
    }
}