using UnityEngine;

namespace GameEngine.UI.Extensions
{
    /// <summary>
    /// Utility functions to make working with RectTransforms a bit easier.
    /// </summary>
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Reformats GetWorldCorners into a Rect instead of a Vector3 array.
        /// </summary>
        public static Rect GetWorldRect(this RectTransform RT)
        {
            Vector3[] corners = new Vector3[4];
            RT.GetWorldCorners(corners);
            Vector2 Size = new Vector2(corners[2].x - corners[1].x, corners[1].y - corners[0].y);
            return new Rect(new Vector2(corners[1].x, -corners[1].y), Size);
        }


        /// <summary>
        /// Aligns the edges of two different RectTransforms.
        /// Returns the distance between the edges.
        /// </summary>
        public static Vector3 AlignEdges(this RectTransform RT, RectTransform other, RectTransform.Edge edge)
        {
            Vector3 offset = Vector3.zero;
            Rect myRect = RT.GetWorldRect();
            Rect otherRect = other.GetWorldRect();

            switch (edge)
            {
                case RectTransform.Edge.Top:
                    offset.y = (myRect.yMin - otherRect.yMin);
                    break;
                case RectTransform.Edge.Bottom:
                    offset.y = (myRect.yMax - otherRect.yMax);
                    break;
                case RectTransform.Edge.Left:
                    offset.x = -(myRect.xMin - otherRect.xMin);
                    break;
                case RectTransform.Edge.Right:
                    offset.x = -(myRect.xMax - otherRect.xMax);
                    break;
            }

            return offset;
        }

        /// <summary>
        /// Aligns the edges of two different RectTransforms.
        /// Returns the distance between the edges.
        /// </summary>
        public static Vector3 AlignEdges(this RectTransform RT, RectTransform other, RectTransform.Edge myEdge, RectTransform.Edge otherEdge)
        {
            Vector3 offset = Vector3.zero;
            Rect myRect = RT.GetWorldRect();
            Rect otherRect = other.GetWorldRect();

            float otherEdgePos = 0f;

            switch (otherEdge)
            {
                case RectTransform.Edge.Top:
                    otherEdgePos = otherRect.yMin;
                    break;
                case RectTransform.Edge.Bottom:
                    otherEdgePos = otherRect.yMax;
                    break;
                case RectTransform.Edge.Left:
                    otherEdgePos = otherRect.xMin;
                    break;
                case RectTransform.Edge.Right:
                    otherEdgePos = otherRect.xMax;
                    break;
            }

            switch (myEdge)
            {
                case RectTransform.Edge.Top:
                    offset.y = (myRect.yMin - otherEdgePos);
                    break;
                case RectTransform.Edge.Bottom:
                    offset.y = (myRect.yMax - otherEdgePos);
                    break;
                case RectTransform.Edge.Left:
                    offset.x = -(myRect.xMin - otherEdgePos);
                    break;
                case RectTransform.Edge.Right:
                    offset.x = -(myRect.xMax - otherEdgePos);
                    break;
            }

            return offset;
        }
    }
}
