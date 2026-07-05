using UnityEngine;

public class MousePositionFollower : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 offset;

    private RectTransform Target
    {
        get
        {
            if (target == null)
                target = transform as RectTransform;

            return target;
        }
    }

    private void LateUpdate()
    {
        RectTransform rectTransform = Target;

        if (rectTransform == null)
            return;

        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (parentRect == null)
        {
            rectTransform.position = (Vector2)Input.mousePosition + offset;
            return;
        }

        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        Camera eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? canvas.worldCamera
            : null;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                Input.mousePosition,
                eventCamera,
                out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint + offset;
        }
    }
}
