using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class RectTransformContactPointFollower : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private RectTransform upperRect;
    [SerializeField] private RectTransform lowerRect;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool followInLateUpdate = true;
    [SerializeField] private bool forceUpdateCanvases;

    private readonly Vector3[] upperCorners = new Vector3[4];
    private readonly Vector3[] lowerCorners = new Vector3[4];

    private void Reset()
    {
        targetRect = transform as RectTransform;
    }

    private void Awake()
    {
        EnsureTargetRect();
    }

    private void OnEnable()
    {
        EnsureTargetRect();
        SnapToContactPoint();
    }

    private void LateUpdate()
    {
        if (followInLateUpdate)
            SnapToContactPoint();
    }

    private void OnValidate()
    {
        EnsureTargetRect();

        if (!Application.isPlaying)
            SnapToContactPoint();
    }

    [ContextMenu("Snap To Contact Point")]
    public void SnapToContactPoint()
    {
        if (targetRect == null || upperRect == null || lowerRect == null)
            return;

        if (forceUpdateCanvases)
            Canvas.ForceUpdateCanvases();

        Vector3 worldPoint = GetWorldContactPoint();
        RectTransform parentRect = targetRect.parent as RectTransform;

        if (parentRect == null)
        {
            targetRect.position = worldPoint;
            return;
        }

        Vector3 localPoint = parentRect.InverseTransformPoint(worldPoint);
        Vector3 currentLocalPosition = targetRect.localPosition;

        targetRect.localPosition = new Vector3(
            localPoint.x + offset.x,
            localPoint.y + offset.y,
            currentLocalPosition.z);
    }

    public Vector3 GetWorldContactPoint()
    {
        if (upperRect == null || lowerRect == null)
            return targetRect != null ? targetRect.position : transform.position;

        upperRect.GetWorldCorners(upperCorners);
        lowerRect.GetWorldCorners(lowerCorners);

        Vector3 upperBottomCenter = (upperCorners[0] + upperCorners[3]) * 0.5f;
        Vector3 lowerTopCenter = (lowerCorners[1] + lowerCorners[2]) * 0.5f;

        return (upperBottomCenter + lowerTopCenter) * 0.5f;
    }

    private void EnsureTargetRect()
    {
        if (targetRect == null)
            targetRect = transform as RectTransform;
    }
}
