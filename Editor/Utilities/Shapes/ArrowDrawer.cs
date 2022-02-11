using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utilities.Shapes
{
    public static class ArrowDrawer
    {
        public static Vector3[] s_ArrowShape = {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1.5f, 3),
            new Vector2(3, 1),
            new Vector2(2, 1),
            new Vector2(2, 0),
        };

        public static float s_InverseArrowScale = 0.33f;

        public static void DrawCenteredArrow(Rect _rect, float _width, Color _color)
        {
            DrawArrow(_rect, _width, _color, (_rect.width - _width) * 0.5f);
        }
        
        public static void DrawArrow(Rect _rect, float _width, float _padding = 0f)
        {
            DrawArrow(_rect, _width, Color.white, _padding);
        }

        public static void DrawArrow(Rect _rect, float _width, Color _color, float _padding = 0f)
        {
            Handles.BeginGUI();
            Handles.color = _color;
            
            var transform = Matrix4x4.TRS(
                new Vector3(_rect.x + _padding, _rect.y, 0),
                Quaternion.identity, 
                new Vector3(_width, _rect.height, 0f) * s_InverseArrowScale
            );
            var newShape = s_ArrowShape.Select(p => transform.MultiplyPoint3x4(p)).ToArray();
            Handles.DrawPolyLine(newShape);
            Handles.EndGUI();
        }
    }
}