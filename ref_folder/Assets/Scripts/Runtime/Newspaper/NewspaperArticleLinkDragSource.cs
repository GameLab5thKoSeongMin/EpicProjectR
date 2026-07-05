using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class NewspaperArticleLinkDragSource : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private string[] acceptedPrefixes =
    {
        "news:",
        "newspaper:",
        "effect:",
        "newspaper-effect:"
    };

    [SerializeField] private Vector2 dragVisualSize = new Vector2(220f, 40f);
    [SerializeField] private Vector2 dragIconSize = new Vector2(32f, 32f);
    [SerializeField] private Color dragBackgroundColor = new Color(0.08f, 0.07f, 0.06f, 0.88f);
    [SerializeField] private Color dragTextColor = new Color(1f, 0.94f, 0.78f, 1f);
    [SerializeField] private GameObject dragPreviewPrefab;

    private TMP_Text text;
    private string pendingLinkId;
    private string pendingLinkText;
    private GameObject dragVisual;
    private RectTransform dragVisualRect;
    private RectTransform dragLayerRect;
    private Camera dragEventCamera;
    private bool externalDragActive;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pendingLinkId = string.Empty;
        pendingLinkText = string.Empty;

        TMP_LinkInfo? linkInfo = FindLink(eventData);
        if (!linkInfo.HasValue)
            return;

        pendingLinkId = linkInfo.Value.GetLinkID();
        pendingLinkText = linkInfo.Value.GetLinkText();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AuditDragPayload payload = CreatePayload(pendingLinkId);

        if (payload == null || !payload.HasData)
            return;

        AuditDragSource.BeginExternalDrag(payload);
        externalDragActive = true;
        CreateDragVisual(eventData, payload);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveDragVisual(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
            Destroy(dragVisual);

        dragVisual = null;
        dragVisualRect = null;
        dragLayerRect = null;
        dragEventCamera = null;

        if (externalDragActive)
            AuditDragSource.EndExternalDrag();

        externalDragActive = false;
        pendingLinkId = string.Empty;
        pendingLinkText = string.Empty;
    }

    private TMP_LinkInfo? FindLink(PointerEventData eventData)
    {
        if (text == null || eventData == null)
            return null;

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            text,
            eventData.position,
            GetEventCamera(eventData));

        if (linkIndex < 0 || linkIndex >= text.textInfo.linkCount)
            return null;

        return text.textInfo.linkInfo[linkIndex];
    }

    private AuditDragPayload CreatePayload(string linkId)
    {
        string effectKey = StripKnownPrefix(linkId);

        if (string.IsNullOrWhiteSpace(effectKey))
            return null;

        NewspaperRuntimeState state = NewspaperRuntimeContext.GetState();
        NewspaperEffectDefinition effect = state != null
            ? state.FindPublishedEffect(effectKey)
            : null;

        if (effect == null)
            return null;

        AuditDragPayload payload = new AuditDragPayload();
        payload.newspaperEffect = effect;
        return payload;
    }

    private string StripKnownPrefix(string linkId)
    {
        if (string.IsNullOrWhiteSpace(linkId))
            return string.Empty;

        string trimmed = linkId.Trim();

        for (int i = 0; i < acceptedPrefixes.Length; i++)
        {
            string prefix = acceptedPrefixes[i];

            if (!string.IsNullOrWhiteSpace(prefix) &&
                trimmed.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                return trimmed.Substring(prefix.Length).Trim();
            }
        }

        return trimmed;
    }

    private void CreateDragVisual(
        PointerEventData eventData,
        AuditDragPayload payload)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Transform parent = canvas != null ? canvas.transform : transform.root;
        GameObject previewPrefab = dragPreviewPrefab;

        dragVisual = previewPrefab != null
            ? Instantiate(previewPrefab, parent)
            : CreateFallbackDragVisual(parent, payload);

        dragVisual.name = "Newspaper Link Drag Visual";
        dragVisual.transform.SetAsLastSibling();

        dragVisualRect = dragVisual.transform as RectTransform;

        if (dragVisualRect != null)
        {
            dragVisualRect.anchorMin = new Vector2(0.5f, 0.5f);
            dragVisualRect.anchorMax = new Vector2(0.5f, 0.5f);
            dragVisualRect.pivot = new Vector2(0.5f, 0.5f);

            if (previewPrefab == null)
                dragVisualRect.sizeDelta = dragVisualSize;
        }

        BindPreview(payload);

        CanvasGroup canvasGroup = dragVisual.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = dragVisual.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.85f;

        DisablePreviewRaycasts(dragVisual);

        dragLayerRect = parent as RectTransform;
        dragEventCamera = GetEventCamera(eventData);
        MoveDragVisual(eventData);
    }

    private GameObject CreateFallbackDragVisual(
        Transform parent,
        AuditDragPayload payload)
    {
        GameObject visual = new GameObject("Newspaper Link Drag Visual");
        visual.transform.SetParent(parent, false);

        RectTransform visualRect = visual.AddComponent<RectTransform>();
        visualRect.anchorMin = new Vector2(0.5f, 0.5f);
        visualRect.anchorMax = new Vector2(0.5f, 0.5f);
        visualRect.pivot = new Vector2(0.5f, 0.5f);
        visualRect.sizeDelta = dragVisualSize;

        Image background = visual.AddComponent<Image>();
        background.color = dragBackgroundColor;
        background.raycastTarget = false;

        HorizontalLayoutGroup layout = visual.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.spacing = 8f;
        layout.padding = new RectOffset(8, 10, 4, 4);

        Sprite iconSprite = GetPayloadIcon(payload);
        if (iconSprite != null)
            CreateIconVisual(visual.transform, iconSprite);

        TextMeshProUGUI visualText = CreateTextVisual(visual.transform);
        visualText.color = dragTextColor;
        visualText.fontSize = 18f;
        visualText.raycastTarget = false;
        visualText.alignment = TextAlignmentOptions.Center;
        visualText.textWrappingMode = TextWrappingModes.NoWrap;
        visualText.overflowMode = TextOverflowModes.Ellipsis;
        return visual;
    }

    private void BindPreview(AuditDragPayload payload)
    {
        NewspaperDragPreviewView previewView = dragVisual != null
            ? dragVisual.GetComponentInChildren<NewspaperDragPreviewView>(true)
            : null;

        if (previewView != null)
            previewView.Bind(GetPayloadIcon(payload), GetPayloadDisplayName(payload));
    }

    private string GetPayloadDisplayName(AuditDragPayload payload)
    {
        if (payload != null &&
            payload.newspaperEffect != null &&
            !string.IsNullOrWhiteSpace(payload.newspaperEffect.DisplayName))
        {
            return payload.newspaperEffect.DisplayName;
        }

        if (payload != null && payload.newspaperEffect != null)
            return payload.newspaperEffect.name;

        return string.IsNullOrWhiteSpace(pendingLinkText)
            ? pendingLinkId
            : pendingLinkText;
    }

    private void DisablePreviewRaycasts(GameObject preview)
    {
        if (preview == null)
            return;

        Graphic[] graphics = preview.GetComponentsInChildren<Graphic>(true);

        for (int i = 0; i < graphics.Length; i++)
            graphics[i].raycastTarget = false;
    }

    private void CreateIconVisual(Transform parent, Sprite iconSprite)
    {
        GameObject iconObject = new GameObject("Icon");
        iconObject.transform.SetParent(parent, false);

        RectTransform iconRect = iconObject.AddComponent<RectTransform>();
        iconRect.sizeDelta = dragIconSize;

        LayoutElement layoutElement = iconObject.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = dragIconSize.x;
        layoutElement.preferredHeight = dragIconSize.y;
        layoutElement.minWidth = dragIconSize.x;
        layoutElement.minHeight = dragIconSize.y;

        Image image = iconObject.AddComponent<Image>();
        image.sprite = iconSprite;
        image.preserveAspect = true;
        image.raycastTarget = false;
    }

    private TextMeshProUGUI CreateTextVisual(Transform parent)
    {
        GameObject textObject = new GameObject("Label");
        textObject.transform.SetParent(parent, false);

        RectTransform textRect = textObject.AddComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(
            Mathf.Max(40f, dragVisualSize.x - dragIconSize.x - 28f),
            dragVisualSize.y);

        LayoutElement layoutElement = textObject.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = textRect.sizeDelta.x;
        layoutElement.preferredHeight = dragVisualSize.y;
        layoutElement.flexibleWidth = 1f;

        return textObject.AddComponent<TextMeshProUGUI>();
    }

    private Sprite GetPayloadIcon(AuditDragPayload payload)
    {
        if (payload == null)
            return null;

        if (payload.newspaperEffect != null &&
            payload.newspaperEffect.IconSprite != null)
        {
            return payload.newspaperEffect.IconSprite;
        }

        return null;
    }

    private void MoveDragVisual(PointerEventData eventData)
    {
        if (dragVisual == null || eventData == null)
            return;

        if (dragVisualRect != null &&
            dragLayerRect != null &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragLayerRect,
                eventData.position,
                dragEventCamera,
                out Vector2 localPoint))
        {
            dragVisualRect.anchoredPosition = localPoint;
            return;
        }

        dragVisual.transform.position = eventData.position;
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
