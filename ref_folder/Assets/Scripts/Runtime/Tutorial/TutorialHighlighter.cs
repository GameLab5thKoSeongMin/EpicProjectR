using UnityEngine;

public class TutorialHighlighter : MonoBehaviour
{
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private RectTransform coordinateRoot;
    [SerializeField] private Vector2 padding = new Vector2(16f, 16f);

    private void Awake()
    {
        if (highlightRect == null)
            highlightRect = transform as RectTransform;

        if (coordinateRoot == null && highlightRect != null)
            coordinateRoot = highlightRect.parent as RectTransform;

        Clear();
    }

    public void Focus(RectTransform target)
    {
        if (highlightRect == null || coordinateRoot == null || target == null)
            return;

        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(
            coordinateRoot,
            target);

        highlightRect.gameObject.SetActive(true);
        highlightRect.anchorMin = new Vector2(0.5f, 0.5f);
        highlightRect.anchorMax = new Vector2(0.5f, 0.5f);
        highlightRect.pivot = new Vector2(0.5f, 0.5f);
        highlightRect.anchoredPosition = bounds.center;
        highlightRect.sizeDelta = new Vector2(
            bounds.size.x + padding.x,
            bounds.size.y + padding.y);
        highlightRect.SetAsLastSibling();
    }

    public void Clear()
    {
        if (highlightRect != null)
            highlightRect.gameObject.SetActive(false);
    }
}
