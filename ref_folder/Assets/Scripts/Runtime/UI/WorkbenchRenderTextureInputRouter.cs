using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WorkbenchRenderTextureInputRouter : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IScrollHandler,
    IPointerExitHandler
{
    [SerializeField] private Camera workbenchCamera;
    [SerializeField] private GraphicRaycaster[] workbenchRaycasters;
    [SerializeField] private bool autoFindRaycasters = true;
    [SerializeField] private bool enableCameraDrag = true;
    [SerializeField] private float cameraDragSpeed = 1f;
    [SerializeField] private bool invertCameraDrag;
    [SerializeField] private RectTransform cameraBoundsRect;
    [SerializeField] private bool enableCameraZoomScroll = true;
    [SerializeField] private float cameraZoomScrollSpeed = 1f;
    [SerializeField] private float minOrthographicSize = 1f;
    [SerializeField] private float maxOrthographicSize = 20f;

    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
    private RawImage rawImage;
    private PointerEventData workbenchEventData;
    private GameObject pointerEnter;
    private GameObject pointerPress;
    private GameObject rawPointerPress;
    private GameObject pointerDrag;
    private RaycastResult pointerPressRaycast;
    private AuditDragPayload forwardedAuditPayload;
    private Coroutine clearForwardedAuditPayloadCoroutine;
    private Vector2 pressPosition;
    private Vector2 lastWorkbenchPosition;
    private bool isPointerDown;
    private bool isDragging;
    private bool beginDragSent;
    private bool isCameraDragging;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();

        if (autoFindRaycasters && (workbenchRaycasters == null || workbenchRaycasters.Length == 0))
            workbenchRaycasters = FindWorkbenchRaycasters();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!TryPreparePointer(eventData, out GameObject currentTarget))
            return;

        if (currentTarget == null || !HasInputTarget(currentTarget))
        {
            BeginCameraDrag(eventData);
            return;
        }

        isPointerDown = true;
        isDragging = false;
        beginDragSent = false;
        pressPosition = workbenchEventData.position;
        lastWorkbenchPosition = workbenchEventData.position;
        pointerPressRaycast = workbenchEventData.pointerCurrentRaycast;
        rawPointerPress = currentTarget;

        pointerPress = ExecuteEvents.ExecuteHierarchy(
            currentTarget,
            workbenchEventData,
            ExecuteEvents.pointerDownHandler);

        if (pointerPress == null)
            pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentTarget);

        pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentTarget);
        CacheForwardedAuditPayload(pointerDrag);
        workbenchEventData.pointerPress = pointerPress;
        workbenchEventData.rawPointerPress = rawPointerPress;
        workbenchEventData.pointerDrag = pointerDrag;
        workbenchEventData.pointerPressRaycast = pointerPressRaycast;
        workbenchEventData.pressPosition = pressPosition;

        if (pointerDrag != null)
        {
            ExecuteEvents.Execute(
                pointerDrag,
                workbenchEventData,
                ExecuteEvents.initializePotentialDrag);
        }

        eventData.Use();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCameraDragging)
        {
            eventData.Use();
            return;
        }

        BeginForwardedDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCameraDragging)
        {
            DragCamera(eventData);
            eventData.Use();
            return;
        }

        if (!isPointerDown || pointerDrag == null)
            return;

        if (!beginDragSent)
            BeginForwardedDrag(eventData);

        if (!TryPreparePointer(eventData, out _))
            return;

        workbenchEventData.pointerPress = pointerPress;
        workbenchEventData.rawPointerPress = rawPointerPress;
        workbenchEventData.pointerDrag = pointerDrag;
        workbenchEventData.pointerPressRaycast = pointerPressRaycast;
        workbenchEventData.pressPosition = pressPosition;
        workbenchEventData.dragging = true;

        ExecuteEvents.Execute(pointerDrag, workbenchEventData, ExecuteEvents.dragHandler);
        lastWorkbenchPosition = workbenchEventData.position;
        eventData.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown)
            return;

        if (isCameraDragging)
        {
            ClearPointerState();
            eventData.Use();
            return;
        }

        if (!TryPreparePointer(eventData, out GameObject currentTarget))
        {
            EndDragWithoutDrop();
            return;
        }

        workbenchEventData.pointerPress = pointerPress;
        workbenchEventData.rawPointerPress = rawPointerPress;
        workbenchEventData.pointerDrag = pointerDrag;
        workbenchEventData.pointerPressRaycast = pointerPressRaycast;
        workbenchEventData.pressPosition = pressPosition;
        workbenchEventData.dragging = isDragging;

        if (pointerPress != null)
            ExecuteEvents.Execute(pointerPress, workbenchEventData, ExecuteEvents.pointerUpHandler);

        if (isDragging)
        {
            if (currentTarget != null)
                ExecuteEvents.ExecuteHierarchy(currentTarget, workbenchEventData, ExecuteEvents.dropHandler);

            if (pointerDrag != null)
                ExecuteEvents.Execute(pointerDrag, workbenchEventData, ExecuteEvents.endDragHandler);
        }
        else
        {
            GameObject clickHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentTarget);

            if (pointerPress != null && clickHandler == pointerPress)
                ExecuteEvents.Execute(pointerPress, workbenchEventData, ExecuteEvents.pointerClickHandler);
        }

        ClearPointerState();
        eventData.Use();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCameraDragging)
        {
            ClearPointerState();
            eventData.Use();
            return;
        }

        if (!isPointerDown || !isDragging)
            return;

        if (TryPreparePointer(eventData, out GameObject currentTarget))
        {
            if (currentTarget != null)
                ExecuteEvents.ExecuteHierarchy(currentTarget, workbenchEventData, ExecuteEvents.dropHandler);
        }

        if (pointerDrag != null)
            ExecuteEvents.Execute(pointerDrag, workbenchEventData, ExecuteEvents.endDragHandler);

        ClearPointerState();
        eventData.Use();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!TryPreparePointer(eventData, out GameObject currentTarget))
            return;

        if (currentTarget != null && HasScrollTarget(currentTarget))
        {
            ExecuteEvents.ExecuteHierarchy(currentTarget, workbenchEventData, ExecuteEvents.scrollHandler);
            eventData.Use();
            return;
        }

        ZoomCamera(eventData.scrollDelta.y);
        eventData.Use();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPointerDown)
            return;

        ClearPointerEnter();
    }

    private void BeginForwardedDrag(PointerEventData eventData)
    {
        if (!isPointerDown || pointerDrag == null || beginDragSent)
            return;

        if (!TryPreparePointer(eventData, out _))
            return;

        workbenchEventData.pointerPress = pointerPress;
        workbenchEventData.rawPointerPress = rawPointerPress;
        workbenchEventData.pointerDrag = pointerDrag;
        workbenchEventData.pointerPressRaycast = pointerPressRaycast;
        workbenchEventData.pressPosition = pressPosition;
        workbenchEventData.dragging = true;

        ExecuteEvents.Execute(pointerDrag, workbenchEventData, ExecuteEvents.beginDragHandler);
        beginDragSent = true;
        isDragging = true;
        lastWorkbenchPosition = workbenchEventData.position;
        eventData.Use();
    }

    private bool TryPreparePointer(PointerEventData sourceEventData, out GameObject currentTarget)
    {
        currentTarget = null;

        if (!TryGetWorkbenchPosition(sourceEventData, out Vector2 workbenchPosition))
            return false;

        PrepareEventData(sourceEventData, workbenchPosition);
        RaycastWorkbench();

        RaycastResult currentRaycast = raycastResults.Count > 0
            ? raycastResults[0]
            : new RaycastResult();

        currentTarget = currentRaycast.gameObject;
        workbenchEventData.pointerCurrentRaycast = currentRaycast;
        UpdatePointerEnter(currentTarget);

        return true;
    }

    private void PrepareEventData(PointerEventData sourceEventData, Vector2 workbenchPosition)
    {
        if (workbenchEventData == null)
            workbenchEventData = new PointerEventData(EventSystem.current);
        else
            workbenchEventData.Reset();

        workbenchEventData.pointerId = sourceEventData.pointerId;
        workbenchEventData.position = workbenchPosition;
        workbenchEventData.delta = isPointerDown ? workbenchPosition - lastWorkbenchPosition : Vector2.zero;
        workbenchEventData.button = sourceEventData.button;
        workbenchEventData.clickCount = sourceEventData.clickCount;
        workbenchEventData.clickTime = sourceEventData.clickTime;
        workbenchEventData.scrollDelta = sourceEventData.scrollDelta;
        workbenchEventData.useDragThreshold = sourceEventData.useDragThreshold;
    }

    private void RaycastWorkbench()
    {
        raycastResults.Clear();

        if (workbenchRaycasters == null)
            return;

        for (int i = 0; i < workbenchRaycasters.Length; i++)
        {
            GraphicRaycaster raycaster = workbenchRaycasters[i];

            if (raycaster == null || !raycaster.isActiveAndEnabled)
                continue;

            raycaster.Raycast(workbenchEventData, raycastResults);
        }
    }

    private bool TryGetWorkbenchPosition(PointerEventData eventData, out Vector2 workbenchPosition)
    {
        workbenchPosition = Vector2.zero;

        if (rawImage == null || eventData == null)
            return false;

        RectTransform rectTransform = rawImage.rectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            return false;
        }

        Rect rect = rectTransform.rect;

        if (rect.width <= 0f || rect.height <= 0f)
            return false;

        float normalizedX = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
        float normalizedY = Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y);

        if (normalizedX < 0f || normalizedX > 1f || normalizedY < 0f || normalizedY > 1f)
            return false;

        Rect uvRect = rawImage.uvRect;
        float textureX = uvRect.x + normalizedX * uvRect.width;
        float textureY = uvRect.y + normalizedY * uvRect.height;
        Texture texture = rawImage.texture;

        if (texture == null && workbenchCamera != null)
            texture = workbenchCamera.targetTexture;

        if (texture == null)
            return false;

        workbenchPosition = new Vector2(textureX * texture.width, textureY * texture.height);
        return true;
    }

    private GraphicRaycaster[] FindWorkbenchRaycasters()
    {
        if (workbenchCamera == null)
            return new GraphicRaycaster[0];

        GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        List<GraphicRaycaster> matches = new List<GraphicRaycaster>();

        for (int i = 0; i < raycasters.Length; i++)
        {
            GraphicRaycaster raycaster = raycasters[i];

            if (raycaster == null)
                continue;

            if (raycaster.eventCamera == workbenchCamera)
                matches.Add(raycaster);
        }

        return matches.ToArray();
    }

    private void UpdatePointerEnter(GameObject currentTarget)
    {
        if (pointerEnter == currentTarget)
            return;

        ClearPointerEnter();

        pointerEnter = currentTarget;

        if (pointerEnter != null)
            ExecuteEvents.ExecuteHierarchy(pointerEnter, workbenchEventData, ExecuteEvents.pointerEnterHandler);
    }

    private void ClearPointerEnter()
    {
        if (pointerEnter == null)
            return;

        ExecuteEvents.ExecuteHierarchy(pointerEnter, workbenchEventData, ExecuteEvents.pointerExitHandler);
        pointerEnter = null;
    }

    private void ClearPointerState()
    {
        ScheduleClearForwardedAuditPayload();
        isPointerDown = false;
        isDragging = false;
        beginDragSent = false;
        isCameraDragging = false;
        pointerPress = null;
        rawPointerPress = null;
        pointerDrag = null;
        pointerPressRaycast = new RaycastResult();
        pressPosition = Vector2.zero;
        lastWorkbenchPosition = Vector2.zero;
    }

    private void CacheForwardedAuditPayload(GameObject dragTarget)
    {
        if (clearForwardedAuditPayloadCoroutine != null)
        {
            StopCoroutine(clearForwardedAuditPayloadCoroutine);
            clearForwardedAuditPayloadCoroutine = null;
        }

        forwardedAuditPayload = null;

        AuditDragSource source = dragTarget != null
            ? dragTarget.GetComponentInParent<AuditDragSource>()
            : null;

        if (source == null)
            return;

        AuditDragPayload payload = source.CreatePayload();

        if (payload == null || !payload.HasData)
            return;

        forwardedAuditPayload = payload;
        AuditDragSource.BeginExternalDrag(forwardedAuditPayload);
    }

    private void ScheduleClearForwardedAuditPayload()
    {
        if (forwardedAuditPayload == null)
            return;

        AuditDragSource.BeginExternalDrag(forwardedAuditPayload);

        if (clearForwardedAuditPayloadCoroutine != null)
            StopCoroutine(clearForwardedAuditPayloadCoroutine);

        clearForwardedAuditPayloadCoroutine = StartCoroutine(ClearForwardedAuditPayloadAfterFrame());
    }

    private IEnumerator ClearForwardedAuditPayloadAfterFrame()
    {
        yield return null;

        AuditDragSource.EndExternalDrag();
        forwardedAuditPayload = null;
        clearForwardedAuditPayloadCoroutine = null;
    }

    private void EndDragWithoutDrop()
    {
        if (workbenchEventData != null)
        {
            workbenchEventData.pointerPress = pointerPress;
            workbenchEventData.rawPointerPress = rawPointerPress;
            workbenchEventData.pointerDrag = pointerDrag;
            workbenchEventData.pointerPressRaycast = pointerPressRaycast;
            workbenchEventData.pressPosition = pressPosition;
            workbenchEventData.position = lastWorkbenchPosition;
            workbenchEventData.dragging = isDragging;

            if (pointerPress != null)
                ExecuteEvents.Execute(pointerPress, workbenchEventData, ExecuteEvents.pointerUpHandler);

            if (isDragging && pointerDrag != null)
                ExecuteEvents.Execute(pointerDrag, workbenchEventData, ExecuteEvents.endDragHandler);
        }

        ClearPointerState();
    }

    private void BeginCameraDrag(PointerEventData eventData)
    {
        if (!enableCameraDrag || workbenchCamera == null)
            return;

        isPointerDown = true;
        isDragging = false;
        beginDragSent = false;
        isCameraDragging = true;
        lastWorkbenchPosition = workbenchEventData.position;
        eventData.Use();
    }

    private void DragCamera(PointerEventData eventData)
    {
        if (!enableCameraDrag || workbenchCamera == null)
            return;

        if (!TryGetWorkbenchPosition(eventData, out Vector2 currentWorkbenchPosition))
            return;

        Vector2 delta = currentWorkbenchPosition - lastWorkbenchPosition;

        if (invertCameraDrag)
            delta = -delta;

        float unitsPerPixel = GetCameraPanUnitsPerPixel();
        Transform cameraTransform = workbenchCamera.transform;
        Vector3 movement =
            (-cameraTransform.right * delta.x - cameraTransform.up * delta.y) *
            unitsPerPixel;

        cameraTransform.position += movement;
        ClampCameraToBounds();
        lastWorkbenchPosition = currentWorkbenchPosition;
    }

    private void ZoomCamera(float scrollDelta)
    {
        if (!enableCameraZoomScroll ||
            workbenchCamera == null ||
            !workbenchCamera.orthographic ||
            Mathf.Approximately(scrollDelta, 0f))
        {
            return;
        }

        float size = workbenchCamera.orthographicSize;
        size -= scrollDelta * cameraZoomScrollSpeed;
        workbenchCamera.orthographicSize = Mathf.Clamp(size, minOrthographicSize, maxOrthographicSize);
        ClampOrthographicSizeToBounds();
        ClampCameraToBounds();
    }

    private float GetCameraPanUnitsPerPixel()
    {
        if (workbenchCamera == null)
            return cameraDragSpeed;

        if (!workbenchCamera.orthographic)
            return cameraDragSpeed;

        Texture texture = rawImage != null ? rawImage.texture : null;

        if (texture == null && workbenchCamera.targetTexture != null)
            texture = workbenchCamera.targetTexture;

        if (texture == null || texture.height <= 0)
            return cameraDragSpeed;

        return (workbenchCamera.orthographicSize * 2f / texture.height) * cameraDragSpeed;
    }

    private bool HasInputTarget(GameObject target)
    {
        if (target == null)
            return false;

        return ExecuteEvents.GetEventHandler<IPointerDownHandler>(target) != null ||
               ExecuteEvents.GetEventHandler<IPointerClickHandler>(target) != null ||
               ExecuteEvents.GetEventHandler<IDragHandler>(target) != null ||
               ExecuteEvents.GetEventHandler<IDropHandler>(target) != null ||
               ExecuteEvents.GetEventHandler<IScrollHandler>(target) != null;
    }

    private bool HasScrollTarget(GameObject target)
    {
        return target != null && ExecuteEvents.GetEventHandler<IScrollHandler>(target) != null;
    }

    private void ClampOrthographicSizeToBounds()
    {
        if (workbenchCamera == null || !workbenchCamera.orthographic || cameraBoundsRect == null)
            return;

        if (!TryGetBoundsInCameraAxes(out float minX, out float maxX, out float minY, out float maxY))
            return;

        float boundsWidth = maxX - minX;
        float boundsHeight = maxY - minY;

        if (boundsWidth <= 0f || boundsHeight <= 0f || workbenchCamera.aspect <= 0f)
            return;

        float maxSizeByHeight = boundsHeight * 0.5f;
        float maxSizeByWidth = boundsWidth / (2f * workbenchCamera.aspect);
        float effectiveMaxSize = Mathf.Min(maxOrthographicSize, maxSizeByHeight, maxSizeByWidth);
        float effectiveMinSize = Mathf.Min(minOrthographicSize, effectiveMaxSize);

        workbenchCamera.orthographicSize = Mathf.Clamp(
            workbenchCamera.orthographicSize,
            effectiveMinSize,
            effectiveMaxSize);
    }

    private void ClampCameraToBounds()
    {
        if (workbenchCamera == null || !workbenchCamera.orthographic || cameraBoundsRect == null)
            return;

        if (!TryGetBoundsInCameraAxes(out float minX, out float maxX, out float minY, out float maxY))
            return;

        float halfHeight = workbenchCamera.orthographicSize;
        float halfWidth = halfHeight * workbenchCamera.aspect;
        float cameraX = Vector3.Dot(workbenchCamera.transform.position, workbenchCamera.transform.right);
        float cameraY = Vector3.Dot(workbenchCamera.transform.position, workbenchCamera.transform.up);
        float clampedX = ClampCameraAxis(cameraX, minX, maxX, halfWidth);
        float clampedY = ClampCameraAxis(cameraY, minY, maxY, halfHeight);
        Vector3 offset =
            workbenchCamera.transform.right * (clampedX - cameraX) +
            workbenchCamera.transform.up * (clampedY - cameraY);

        workbenchCamera.transform.position += offset;
    }

    private float ClampCameraAxis(float value, float min, float max, float halfSize)
    {
        float minCenter = min + halfSize;
        float maxCenter = max - halfSize;

        if (minCenter > maxCenter)
            return (min + max) * 0.5f;

        return Mathf.Clamp(value, minCenter, maxCenter);
    }

    private bool TryGetBoundsInCameraAxes(
        out float minX,
        out float maxX,
        out float minY,
        out float maxY)
    {
        minX = 0f;
        maxX = 0f;
        minY = 0f;
        maxY = 0f;

        if (workbenchCamera == null || cameraBoundsRect == null)
            return false;

        Vector3[] corners = new Vector3[4];
        cameraBoundsRect.GetWorldCorners(corners);
        Vector3 right = workbenchCamera.transform.right;
        Vector3 up = workbenchCamera.transform.up;

        minX = maxX = Vector3.Dot(corners[0], right);
        minY = maxY = Vector3.Dot(corners[0], up);

        for (int i = 1; i < corners.Length; i++)
        {
            float x = Vector3.Dot(corners[i], right);
            float y = Vector3.Dot(corners[i], up);

            minX = Mathf.Min(minX, x);
            maxX = Mathf.Max(maxX, x);
            minY = Mathf.Min(minY, y);
            maxY = Mathf.Max(maxY, y);
        }

        return true;
    }
}
