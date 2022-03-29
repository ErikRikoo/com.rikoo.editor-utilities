using UnityEditor;
using UnityEngine;

namespace EditorUtilities.Editor.Extensions
{
    public static class RectExtensions
    {
        public static Rect SubRect(this Rect _instance, int _index, int _columnCount, int _span = 1)
        {
            float width = _instance.width / _columnCount;
            return new Rect(
                _instance.x + width * _index, _instance.y,
                width * Mathf.Min(_span, _columnCount - _index), _instance.height
            );
        }

        public static void SplitRectHorizontally(this Rect _instance, out Rect _left, out Rect _right, float _ratio = 0.5f)
        {
            float splitWidth = _instance.width * _ratio;
            _left = new Rect(_instance)
            {
                width = splitWidth
            };

            _right = new Rect(
                _instance.x + splitWidth, _instance.y,
                _instance.width - splitWidth, _instance.height
            );
        }

        public static Rect AddPadding(ref this Rect _instance, float _padding)
        {
            var paddingVector = new Vector2(_padding, _padding);
            _instance.min += paddingVector;
            _instance.max -= paddingVector;

            return _instance;
        }

        public static Rect ExpandLeft(ref this Rect _instance, float expansion)
        {
            return _instance.ShrinkLeft(-expansion);
        }
        
        public static Rect ShrinkLeft(ref this Rect _instance, float shrink)
        {
            _instance.xMin += shrink;
            
            return _instance;
        }
        
        public static Rect ShrinkRight(ref this Rect _instance, float shrink)
        {
            _instance.xMax -= shrink;
            
            return _instance;
        }

        public static Rect IgnorePaddingLeft(ref this Rect _instance)
        {
            _instance.ExpandLeft(EditorStyles.inspectorDefaultMargins.padding.left);
            return _instance;
        }

        public static void AddLine(ref this Rect _instance)
        {
            _instance.AddLine(EditorGUIUtility.singleLineHeight);
        }
        
        public static void AddLine(ref this Rect _instance, float _lineHeight)
        {
            _instance.yMin += _lineHeight;
        }

        public static void NextLine(ref this Rect _instance)
        {
            _instance.AddLine();
            _instance.height = EditorGUIUtility.singleLineHeight;
        }

        public static void Indent(ref this Rect _instance)
        {
            _instance = EditorGUI.IndentedRect(_instance);
        }
    }
}