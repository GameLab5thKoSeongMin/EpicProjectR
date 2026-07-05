using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableImageObject : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private bool moveToBottomOnBeginDrag = true;
    [SerializeField] private bool clampToCanvasBounds = true;
    [SerializeField] private RectTransform canvasBoundsOverride;

    private RectTransform rectTransform;
    private RectTransform dragPlane;
    private RectTransform canvasBounds;
    private Camera eventCamera;
    private Vector2 pointerOffset;
    private bool wasDragged;
    private int lastDragFrame = -1;

    public bool ConsumeRecentDrag()
    {
        if (!wasDragged)
            return false;

        if (Time.frameCount - lastDragFrame > 1)
        {
            wasDragged = false;
            return false;
        }

        wasDragged = false;
        return true;
    }

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        canvasBounds = FindCanvasBounds();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MarkDragged();

        if (rectTransform == null)
            rectTransform = transform as RectTransform;

        if (rectTransform == null)
            return;

        if (moveToBottomOnBeginDrag)
            transform.SetAsLastSibling();

        dragPlane = rectTransform.parent as RectTransform;
        eventCamera = GetEventCamera(eventData);
        canvasBounds = FindCanvasBounds();

        if (dragPlane == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragPlane,
            eventData.position,
            eventCamera,
            out Vector2 localPointerPosition))
        {
            pointerOffset = rectTransform.anchoredPosition - localPointerPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        MarkDragged();

        if (rectTransform == null || dragPlane == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragPlane,
            eventData.position,
            eventCamera,
            out Vector2 localPointerPosition))
        {
            rectTransform.anchoredPosition = localPointerPosition + pointerOffset;
            ClampToCanvasBounds();
        }
    }

    private void MarkDragged()
    {
        wasDragged = true;
        lastDragFrame = Time.frameCount;
    }

    private void ClampToCanvasBounds()
    {
        if (!clampToCanvasBounds ||
            rectTransform == null ||
            dragPlane == null ||
            canvasBounds == null)
        {
            return;
        }

        Bounds objectBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(
            canvasBounds,
            rectTransform);

        Rect boundsRect = canvasBounds.rect;
        Vector3 correction = Vector3.zero;
        correction.x = CalculateAxisCorrection(
            objectBounds.min.x,
            objectBounds.max.x,
            objectBounds.center.x,
            boundsRect.xMin,
            boundsRect.xMax,
            boundsRect.center.x);
        correction.y = CalculateAxisCorrection(
            objectBounds.min.y,
            objectBounds.max.y,
            objectBounds.center.y,
            boundsRect.yMin,
            boundsRect.yMax,
            boundsRect.center.y);

        if (correction == Vector3.zero)
            return;

        Vector3 worldCorrection = canvasBounds.TransformVector(correction);
        Vector3 dragPlaneCorrection = dragPlane.InverseTransformVector(worldCorrection);
        rectTransform.anchoredPosition += new Vector2(dragPlaneCorrection.x, dragPlaneCorrection.y);
    }

    private float CalculateAxisCorrection(
        float objectMin,
        float objectMax,
        float objectCenter,
        float boundsMin,
        float boundsMax,
        float boundsCenter)
    {
        float objectSize = objectMax - objectMin;
        float boundsSize = boundsMax - boundsMin;

        if (objectSize > boundsSize)
            return boundsCenter - objectCenter;

        if (objectMin < boundsMin)
            return boundsMin - objectMin;

        if (objectMax > boundsMax)
            return boundsMax - objectMax;

        return 0f;
    }

    private RectTransform FindCanvasBounds()
    {
        if (canvasBoundsOverride != null)
            return canvasBoundsOverride;

        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.transform as RectTransform : null;
    }

    private Camera GetEventCamera(PointerEventData eventData)
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        if (canvas != null && canvas.worldCamera != null)
            return canvas.worldCamera;

        return eventData != null ? eventData.pressEventCamera : null;
    }
}
