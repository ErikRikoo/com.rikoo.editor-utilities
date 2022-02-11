using UnityEngine;

namespace Utilities
{
    public static class RectExtensions
    {
        public static Rect SubRect(this Rect _instance, int _index, int _columnCount)
        {
            float width = _instance.width / _columnCount;
            return new Rect(
                _instance.x + width * _index, _instance.y,
                width, _instance.height
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
    }
}