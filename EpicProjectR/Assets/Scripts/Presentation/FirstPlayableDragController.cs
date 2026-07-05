// Responsibility: Adds Main-scene-style mouse dragging to generated first playable document cards.
using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicProjectR.Presentation
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class FirstPlayableDragController : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        private RectTransform rectTransform;
        private RectTransform dragPlane;
        private RectTransform bounds;
        private Camera eventCamera;
        private Vector2 pointerOffset;

        public void Initialize(RectTransform clampBounds)
        {
            rectTransform = transform as RectTransform;
            bounds = clampBounds;
            dragPlane = clampBounds != null ? clampBounds : rectTransform != null ? rectTransform.parent as RectTransform : null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (rectTransform == null)
            {
                rectTransform = transform as RectTransform;
            }

            if (rectTransform == null)
            {
                return;
            }

            transform.SetAsLastSibling();
            dragPlane = rectTransform.parent as RectTransform;
            eventCamera = GetEventCamera(eventData);

            if (dragPlane == null)
            {
                return;
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragPlane, eventData.position, eventCamera, out var localPointer))
            {
                pointerOffset = rectTransform.anchoredPosition - localPointer;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (rectTransform == null || dragPlane == null)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(dragPlane, eventData.position, eventCamera, out var localPointer))
            {
                return;
            }

            rectTransform.anchoredPosition = localPointer + pointerOffset;
            ClampToBounds();
        }

        private void ClampToBounds()
        {
            if (bounds == null || rectTransform == null)
            {
                return;
            }

            var objectBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(bounds, rectTransform);
            var boundsRect = bounds.rect;
            var correction = Vector3.zero;
            correction.x = AxisCorrection(objectBounds.min.x, objectBounds.max.x, objectBounds.center.x, boundsRect.xMin, boundsRect.xMax, boundsRect.center.x);
            correction.y = AxisCorrection(objectBounds.min.y, objectBounds.max.y, objectBounds.center.y, boundsRect.yMin, boundsRect.yMax, boundsRect.center.y);

            if (correction == Vector3.zero)
            {
                return;
            }

            var worldCorrection = bounds.TransformVector(correction);
            var planeCorrection = dragPlane.InverseTransformVector(worldCorrection);
            rectTransform.anchoredPosition += new Vector2(planeCorrection.x, planeCorrection.y);
        }

        private static float AxisCorrection(float objectMin, float objectMax, float objectCenter, float boundsMin, float boundsMax, float boundsCenter)
        {
            var objectSize = objectMax - objectMin;
            var boundsSize = boundsMax - boundsMin;

            if (objectSize > boundsSize)
            {
                return boundsCenter - objectCenter;
            }

            if (objectMin < boundsMin)
            {
                return boundsMin - objectMin;
            }

            if (objectMax > boundsMax)
            {
                return boundsMax - objectMax;
            }

            return 0f;
        }

        private Camera GetEventCamera(PointerEventData eventData)
        {
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return null;
            }

            if (canvas != null && canvas.worldCamera != null)
            {
                return canvas.worldCamera;
            }

            return eventData != null ? eventData.pressEventCamera : null;
        }
    }
}
