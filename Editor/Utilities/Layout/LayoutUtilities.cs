using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utilities.Layout.LayoutUtilities
{
    public static class HorizontalGridLayout
    {
        public static IEnumerator<Rect> GetRects(int _columnCount, int _cellCount, float _height)
        {
            for (int i = 0; i < _cellCount; i += _columnCount)
            {
                var lineRect = EditorGUILayout.GetControlRect(false, _height);
                int bound = Mathf.Min(_columnCount, _cellCount - i);
                for (int j = 0; j < bound; ++j)
                {
                    yield return lineRect.SubRect(j, _columnCount);
                }
            }
        }

        public static IEnumerator<Rect> GetRects(int _columnCount, int _cellCount)
        {
            return GetRects(_columnCount, _cellCount, EditorGUIUtility.singleLineHeight);
        }
    }
}