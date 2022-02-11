using System.Linq;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using Utilities.Layout;
using Utilities.Layout.LayoutUtilities;

namespace Utilities
{
    public static class EditorDrawingUtilities
    {
        public static void DrawAsHeader(this string _instance, GUIStyle _style, float _padding = 10f,
            float _topMargin = 10f)
        {
            float textWidth = _style.CalcSize(new GUIContent(_instance)).x;
            float _halfPadding = _padding * 0.5f;
            GUIStyle centeredStyle = new GUIStyle(_style);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            
            EditorGUILayout.Space(_topMargin);
            var rect = EditorGUILayout.BeginHorizontal();
            {
                float barSize = (rect.width - textWidth) / 2f - _padding;

                Rect barRect = new Rect(rect.x + _halfPadding, rect.y + rect.height * 0.5f, barSize, 1f); 
                EditorGUI.DrawRect(barRect, Color.white);
                EditorGUILayout.LabelField(_instance, centeredStyle);

                barRect.x = rect.x + rect.width - barSize - _halfPadding;
                EditorGUI.DrawRect(barRect, Color.white);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        public static void DrawAsHeader(this string _instance, float _padding = 10f, float _topMargin = 10f)
        {
            _instance.DrawAsHeader(GUI.skin.label, _padding, _topMargin);
        }

        public static void DrawAsMaxWidthLabelIfNotEmpty(this string _instance)
        {
            if (_instance != "")
            {
                _instance.DrawAsMaxWidthLabel();
            }
        }
        
        public static void DrawAsMaxWidthLabel(this string _instance)
        {
            GUIContent content = new GUIContent(_instance);
            float calcWidth = GUI.skin.label.CalcSize(content).x + 15f;
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.padding = new RectOffset();
            style.margin = new RectOffset();
            EditorGUILayout.LabelField(_instance, style, GUILayout.MinWidth(calcWidth), GUILayout.MaxWidth(calcWidth));
        }

        public static bool DrawIndentedButton(GUIContent _content, GUIStyle _buttonStyle)
        {
            Rect r = EditorGUILayout.BeginHorizontal();
            Rect indented = EditorGUI.IndentedRect(r);
            EditorGUILayout.Space(indented.x - r.x, false);
            bool ret = GUILayout.Button(_content, _buttonStyle);
            EditorGUILayout.EndHorizontal();
            return ret;
        }

        public static bool DrawIndentedButton(string _content, GUIStyle _buttonStyle)
        {
            return DrawIndentedButton(new GUIContent(_content), _buttonStyle);
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

        public static bool DisplayFloatFieldAsPercentage(Rect r, float _current, out float newFloatValue)
        {
            string display = (_current * 100) + "%";
            string newValue = EditorGUI.TextField(r, display);
            if (newValue.EndsWith("%"))
            {
                newValue = newValue.Substring(0, newValue.Length - 1);
            }

            if (float.TryParse(newValue, out newFloatValue))
            {
                newFloatValue *= 0.01f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DrawLineSeparator(float _height, Color _color)
        {
            var lineRect = EditorGUILayout.GetControlRect(false, _height);
            EditorGUI.DrawRect(lineRect, _color);
        }
        
        public static void DrawLineSeparator(float _height)
        {
            DrawLineSeparator(_height, Color.white);
        }

        public static void DrawLabelsAsGrid(
            int _columnCount, float _padding,  GUIStyle _style, string[] labels)
        {
            var rects = HorizontalGridLayout.GetRects(
                _columnCount, labels.Length, 
                EditorGUIUtility.singleLineHeight + _padding * 2
                );

            int i = 0;
            while (rects.MoveNext())
            {
                var rect = rects.Current;
                DrawBorders(rect, 1);
                EditorGUI.LabelField(rect.Padding(_padding), labels[i], _style);
                ++i;
            }
        }

        public static void DrawBorders(Rect _space, float _width, Color _color)
        {
            Rect top = _space;
            top.height = _width;
            EditorGUI.DrawRect(top, _color);

            Rect left = _space;
            left.width = _width;
            EditorGUI.DrawRect(left, _color);

            Rect right = _space;
            right.width = _width;
            right.x = _space.x + _space.width - _width;
            EditorGUI.DrawRect(right, _color);

            Rect bottom = _space;
            bottom.height = _width ;
            bottom.y = _space.y + _space.height - _width;
            EditorGUI.DrawRect(bottom, _color);
        }
        
        
        public static void DrawBorders(Rect _space, float _width)
        {
            DrawBorders(_space, _width, Color.white);
        }
    }
}