using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AuditDragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private DefinitionBase definition;
    [SerializeField] private bool fadeSourceWhileDragging = true;
    [SerializeField] private Transform dragRoot;
    [SerializeField] private bool autoFindDragRoot = true;

    private GameObject dragVisual;
    private RectTransform dragVisualRect;
    private RectTransform dragLayerRect;
    private Camera dragEventCamera;
    private CanvasGroup sourceCanvasGroup;
    private bool previousBlocksRaycasts;
    private float previousAlpha;

    public static AuditDragSource CurrentSource { get; private set; }
    public static AuditDragPayload CurrentPayload { get; private set; }

    private void Start()
    {
        if (autoFindDragRoot && dragRoot == null)
            dragRoot = FindDragRoot();
    }

    public static void BeginExternalDrag(AuditDragPayload payload)
    {
        CurrentSource = null;
        CurrentPayload = payload;
    }

    public static void EndExternalDrag()
    {
        if (CurrentSource != null)
            return;

        CurrentPayload = null;
    }

    public void BindDefinition(DefinitionBase value)
    {
        definition = value;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentSource = this;
        CurrentPayload = CreatePayload();
        PrepareSourceForDrag();
        CreateDragVisual(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveDragVisual(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
            Destroy(dragVisual);

        dragVisualRect = null;
        dragLayerRect = null;
        dragEventCamera = null;

        RestoreSourceAfterDrag();

        if (CurrentSource == this)
        {
            CurrentSource = null;
            CurrentPayload = null;
        }
    }

    public AuditDragPayload CreatePayload()
    {
        AuditDragPayload payload = new AuditDragPayload();
        payload.definition = definition;
        return payload;
    }

    private void PrepareSourceForDrag()
    {
        sourceCanvasGroup = GetComponent<CanvasGroup>();

        if (sourceCanvasGroup == null)
            sourceCanvasGroup = gameObject.AddComponent<CanvasGroup>();

        previousBlocksRaycasts = sourceCanvasGroup.blocksRaycasts;
        previousAlpha = sourceCanvasGroup.alpha;
        sourceCanvasGroup.blocksRaycasts = false;

        if (fadeSourceWhileDragging)
            sourceCanvasGroup.alpha = 0.45f;
    }

    private void RestoreSourceAfterDrag()
    {
        if (sourceCanvasGroup == null)
            return;

        sourceCanvasGroup.blocksRaycasts = previousBlocksRaycasts;
        sourceCanvasGroup.alpha = previousAlpha;
    }

    private void CreateDragVisual(PointerEventData eventData)
    {
        Transform parent = GetDragLayer();

        dragVisual = Instantiate(gameObject, parent);
        dragVisual.name = gameObject.name + " Drag Visual";
        dragVisual.transform.SetAsLastSibling();
        PrepareDragVisual(eventData);

        DisableDragVisualInteraction(dragVisual);
        MoveDragVisual(eventData);
    }

    private Transform GetDragLayer()
    {
        if (dragRoot != null)
            return dragRoot;

        if (autoFindDragRoot)
        {
            dragRoot = FindDragRoot();

            if (dragRoot != null)
                return dragRoot;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.transform : transform.root;
    }

    private Transform FindDragRoot()
    {
        AuditDragRoot root = Object.FindFirstObjectByType<AuditDragRoot>();
        return root != null ? root.Root : null;
    }

    private void PrepareDragVisual(PointerEventData eventData)
    {
        dragVisualRect = dragVisual.transform as RectTransform;
        dragLayerRect = dragVisual.transform.parent as RectTransform;
        dragEventCamera = GetDragEventCamera(eventData);

        RectTransform sourceRect = transform as RectTransform;

        if (dragVisualRect == null || sourceRect == null)
            return;

        dragVisualRect.anchorMin = new Vector2(0.5f, 0.5f);
        dragVisualRect.anchorMax = new Vector2(0.5f, 0.5f);
        dragVisualRect.pivot = new Vector2(0.5f, 0.5f);
        dragVisualRect.sizeDelta = sourceRect.rect.size;
        dragVisualRect.localScale = Vector3.one;
        dragVisualRect.localRotation = Quaternion.identity;
    }

    private Camera GetDragEventCamera(PointerEventData eventData)
    {
        Canvas canvas = dragVisual != null
            ? dragVisual.GetComponentInParent<Canvas>()
            : GetComponentInParent<Canvas>();

        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        if (canvas != null && canvas.worldCamera != null)
            return canvas.worldCamera;

        return eventData != null ? eventData.pressEventCamera : null;
    }

    private void DisableDragVisualInteraction(GameObject visual)
    {
        AuditDragSource[] sources = visual.GetComponentsInChildren<AuditDragSource>(true);
        for (int i = 0; i < sources.Length; i++)
            sources[i].enabled = false;

        Selectable[] selectables = visual.GetComponentsInChildren<Selectable>(true);
        for (int i = 0; i < selectables.Length; i++)
            selectables[i].interactable = false;

        CanvasGroup canvasGroup = visual.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = visual.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.8f;
    }

    private void MoveDragVisual(PointerEventData eventData)
    {
        if (dragVisual == null)
            return;

        Vector2 pointerPosition = GetDragPointerPosition(eventData);

        if (dragVisualRect != null &&
            dragLayerRect != null &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragLayerRect,
                pointerPosition,
                dragEventCamera,
                out Vector2 localPoint))
        {
            dragVisualRect.anchoredPosition = localPoint;
            return;
        }

        dragVisual.transform.position = pointerPosition;
    }

    private Vector2 GetDragPointerPosition(PointerEventData eventData)
    {
        if (dragRoot != null)
            return Input.mousePosition;

        return eventData != null ? eventData.position : (Vector2)Input.mousePosition;
    }
}
