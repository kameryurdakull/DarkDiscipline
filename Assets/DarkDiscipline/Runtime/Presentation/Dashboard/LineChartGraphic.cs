using System;
using UnityEngine;
using UnityEngine.UI;

namespace DarkDiscipline.Presentation.Dashboard
{
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class LineChartGraphic : Graphic
    {
        private const float MinimumRange = 1f;
        private const float LineThickness = 5f;

        private float[] _values = Array.Empty<float>();

        public void SetValues(float[] values)
        {
            _values = values ?? Array.Empty<float>();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();

            if (_values.Length == 0)
            {
                return;
            }

            var rect = rectTransform.rect;
            var maxValue = Mathf.Max(MinimumRange, Max(_values));

            if (_values.Length == 1)
            {
                AddDot(vertexHelper, new Vector2(rect.center.x, rect.yMin + rect.height * Mathf.Clamp01(_values[0] / maxValue)), LineThickness * 1.6f);
                return;
            }

            var previous = GetPoint(rect, 0, maxValue);

            for (var index = 1; index < _values.Length; index++)
            {
                var current = GetPoint(rect, index, maxValue);
                AddLine(vertexHelper, previous, current, LineThickness);
                previous = current;
            }
        }

        private Vector2 GetPoint(Rect rect, int index, float maxValue)
        {
            var xStep = rect.width / (_values.Length - 1);
            var normalized = Mathf.Clamp01(_values[index] / maxValue);
            return new Vector2(rect.xMin + xStep * index, rect.yMin + rect.height * normalized);
        }

        private static float Max(float[] values)
        {
            var max = 0f;

            for (var index = 0; index < values.Length; index++)
            {
                max = Mathf.Max(max, values[index]);
            }

            return max;
        }

        private void AddLine(VertexHelper vertexHelper, Vector2 start, Vector2 end, float thickness)
        {
            var direction = (end - start).normalized;
            var normal = new Vector2(-direction.y, direction.x) * (thickness * 0.5f);
            var startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(start - normal, color, Vector2.zero);
            vertexHelper.AddVert(start + normal, color, Vector2.zero);
            vertexHelper.AddVert(end + normal, color, Vector2.zero);
            vertexHelper.AddVert(end - normal, color, Vector2.zero);
            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex, startIndex + 2, startIndex + 3);
        }

        private void AddDot(VertexHelper vertexHelper, Vector2 center, float size)
        {
            var halfSize = size * 0.5f;
            var startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(center + new Vector2(-halfSize, -halfSize), color, Vector2.zero);
            vertexHelper.AddVert(center + new Vector2(-halfSize, halfSize), color, Vector2.zero);
            vertexHelper.AddVert(center + new Vector2(halfSize, halfSize), color, Vector2.zero);
            vertexHelper.AddVert(center + new Vector2(halfSize, -halfSize), color, Vector2.zero);
            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex, startIndex + 2, startIndex + 3);
        }
    }
}
