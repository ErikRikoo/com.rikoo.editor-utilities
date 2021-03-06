using Codice.Utils;
using EditorUtilities.Editor.Extensions;
using EditorUtilities.Editor.Utilities.Layout.LayoutUtilities;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Utilities
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

        public static void DrawAsHeader(this string _instance, Rect _position, float _padding = 20f)
        {
            float lineSize = 1;
            float margin = 15;
            float textWidth = GUI.skin.label.CalcSize(new GUIContent(_instance)).x;
            
            EditorGUI.LabelField(_position, _instance, GUI.skin.label.CenterText().Bold());
            Rect fullDrawingRect = new Rect(
                _position.x + _padding,
                _position.y + (_position.height) * 0.5f,
                _position.width - _padding * 2f,
                lineSize);
            float lineWidth = (fullDrawingRect.width - textWidth) * 0.5f - margin;

            Rect leftPart = new Rect(fullDrawingRect);
            leftPart.width = lineWidth; 
            EditorGUI.DrawRect(leftPart, Color.white);
            
            Rect rightPart = new Rect(fullDrawingRect);
            rightPart.xMin = rightPart.xMax - lineWidth;
            EditorGUI.DrawRect(rightPart, Color.white);
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
                EditorGUI.LabelField(rect.AddPadding(_padding), labels[i], _style);
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

        public static bool TryComputeMouseWorldPosition(out RaycastHit _hit)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            return Physics.Raycast(ray, out _hit);
        }
        
        public static bool TryComputeMouseWorldPosition(out Vector3 _point, out Vector3 _normal)
        {
            if (TryComputeMouseWorldPosition(out RaycastHit _hit))
            {
                _point = _hit.point;
                _normal = _hit.normal;
                return true;
            }

            _point = default;
            _normal = default;
            return false;
        }

        public static void DrawPropertyChildren(Rect position, SerializedProperty property)
        {
            foreach (var child in property.GetChildren())
            {
                float propertyHeight = EditorGUI.GetPropertyHeight(child);
                Rect propertyRect = position;
                propertyRect.height = propertyHeight;
                EditorGUI.PropertyField(propertyRect, child, true);
                
                position.AddLine(propertyHeight);
            }  
        }

        public static void DrawLineInTheCenter(this Rect _space, float _height = 1f)
        {
            _space.y = _space.y + _space.height * 0.5f;
            _space.height = _height;
            EditorGUI.DrawRect(_space, Color.white);
        }
    }
}