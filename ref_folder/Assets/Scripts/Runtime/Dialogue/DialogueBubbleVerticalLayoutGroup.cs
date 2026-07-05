using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Dialogue Bubble Vertical Layout Group")]
public class DialogueBubbleVerticalLayoutGroup : LayoutGroup
{
    [SerializeField] private float spacing;
    [SerializeField] private bool childControlWidth = true;
    [SerializeField] private bool childControlHeight = true;
    [SerializeField] private bool childForceExpandWidth = true;
    [SerializeField] private bool childForceExpandHeight;

    public float Spacing
    {
        get => spacing;
        set => SetProperty(ref spacing, value);
    }

    public bool ChildControlWidth
    {
        get => childControlWidth;
        set => SetProperty(ref childControlWidth, value);
    }

    public bool ChildControlHeight
    {
        get => childControlHeight;
        set => SetProperty(ref childControlHeight, value);
    }

    public bool ChildForceExpandWidth
    {
        get => childForceExpandWidth;
        set => SetProperty(ref childForceExpandWidth, value);
    }

    public bool ChildForceExpandHeight
    {
        get => childForceExpandHeight;
        set => SetProperty(ref childForceExpandHeight, value);
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        CalculateLayoutInputForAxis(0);
    }

    public override void CalculateLayoutInputVertical()
    {
        CalculateLayoutInputForAxis(1);
    }

    public override void SetLayoutHorizontal()
    {
        SetChildrenAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        SetChildrenAlongAxis(1);
    }

    private void CalculateLayoutInputForAxis(int axis)
    {
        float totalMin = axis == 0 ? padding.horizontal : padding.vertical;
        float totalPreferred = totalMin;
        float totalFlexible = 0f;
        bool isVertical = axis == 1;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];
            float min = LayoutUtility.GetMinSize(child, axis);
            float preferred = LayoutUtility.GetPreferredSize(child, axis);
            float flexible = LayoutUtility.GetFlexibleSize(child, axis);

            if ((axis == 0 && childForceExpandWidth) ||
                (axis == 1 && childForceExpandHeight))
            {
                flexible = Mathf.Max(flexible, 1f);
            }

            if (isVertical)
            {
                if (i > 0)
                {
                    totalMin += spacing;
                    totalPreferred += spacing;
                }

                totalMin += min;
                totalPreferred += preferred;
                totalFlexible += flexible;
            }
            else
            {
                totalMin = Mathf.Max(totalMin, min + padding.horizontal);
                totalPreferred = Mathf.Max(totalPreferred, preferred + padding.horizontal);
                totalFlexible = Mathf.Max(totalFlexible, flexible);
            }
        }

        totalPreferred = Mathf.Max(totalMin, totalPreferred);
        SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
    }

    private void SetChildrenAlongAxis(int axis)
    {
        if (axis == 0)
        {
            float innerWidth = rectTransform.rect.width - padding.horizontal;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                float width = GetChildSize(child, axis, innerWidth);
                float x = GetChildStartOffset(child, width, innerWidth);

                if (childControlWidth)
                    SetChildAlongAxis(child, axis, x, width);
                else
                    SetChildAlongAxis(child, axis, x);
            }

            return;
        }

        float totalPreferredHeight = GetTotalPreferredHeight();
        float y = GetStartOffset(1, totalPreferredHeight - padding.vertical);
        float extraHeight = Mathf.Max(
            0f,
            rectTransform.rect.height - totalPreferredHeight);
        float extraPerChild = childForceExpandHeight && rectChildren.Count > 0
            ? extraHeight / rectChildren.Count
            : 0f;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];
            float height = childControlHeight
                ? LayoutUtility.GetPreferredHeight(child) + extraPerChild
                : child.sizeDelta.y;

            if (childControlHeight)
                SetChildAlongAxis(child, axis, y, height);
            else
                SetChildAlongAxis(child, axis, y);

            y += height + spacing;
        }
    }

    private float GetChildSize(RectTransform child, int axis, float availableSize)
    {
        if ((axis == 0 && !childControlWidth) ||
            (axis == 1 && !childControlHeight))
        {
            return child.sizeDelta[axis];
        }

        float preferred = LayoutUtility.GetPreferredSize(child, axis);

        if (axis == 0 && childForceExpandWidth)
            return Mathf.Max(0f, availableSize);

        if (axis == 1 && childForceExpandHeight)
            return Mathf.Max(preferred, availableSize);

        return Mathf.Min(preferred, Mathf.Max(0f, availableSize));
    }

    private float GetChildStartOffset(
        RectTransform child,
        float childWidth,
        float innerWidth)
    {
        if (childForceExpandWidth)
            return padding.left;

        float surplus = Mathf.Max(0f, innerWidth - childWidth);
        TextAnchor alignment = GetPerChildAlignment(child);
        float alignmentFactor = GetHorizontalAlignmentFactor(alignment);
        return padding.left + surplus * alignmentFactor;
    }

    private TextAnchor GetPerChildAlignment(RectTransform child)
    {
        HorizontalLayoutGroup rowLayout = child.GetComponent<HorizontalLayoutGroup>();
        return rowLayout != null ? rowLayout.childAlignment : childAlignment;
    }

    private float GetHorizontalAlignmentFactor(TextAnchor alignment)
    {
        switch (alignment)
        {
            case TextAnchor.UpperCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.LowerCenter:
                return 0.5f;

            case TextAnchor.UpperRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.LowerRight:
                return 1f;

            default:
                return 0f;
        }
    }

    private float GetTotalPreferredHeight()
    {
        float total = padding.vertical;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            if (i > 0)
                total += spacing;

            total += LayoutUtility.GetPreferredHeight(rectChildren[i]);
        }

        return total;
    }
}
