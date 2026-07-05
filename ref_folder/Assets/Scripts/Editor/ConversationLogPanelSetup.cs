using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class ConversationLogPanelSetup
{
    private const string MenuPath = "Tools/Conversation UI/Build On Selected Panel";

    [MenuItem(MenuPath)]
    private static void BuildOnSelectedPanel()
    {
        RectTransform panel = Selection.activeGameObject != null
            ? Selection.activeGameObject.GetComponent<RectTransform>()
            : null;

        if (panel == null)
        {
            EditorUtility.DisplayDialog(
                "Conversation UI",
                "대화 패널로 사용할 UI Image 오브젝트를 선택해 주세요.",
                "확인");
            return;
        }

        ConversationLogPanelView existingView =
            panel.GetComponent<ConversationLogPanelView>();

        if (existingView != null)
        {
            EditorUtility.DisplayDialog(
                "Conversation UI",
                "선택한 패널에 대화 레이아웃이 이미 구성되어 있습니다.",
                "확인");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(
            panel.gameObject,
            "Build Conversation Log Panel");

        GameObject scrollObject = CreateUiObject(
            "ConversationScrollView",
            panel,
            typeof(Image),
            typeof(ScrollRect));

        RectTransform scrollRectTransform =
            scrollObject.GetComponent<RectTransform>();
        Stretch(scrollRectTransform, 12f, 12f, 12f, 12f);

        Image scrollBackground = scrollObject.GetComponent<Image>();
        scrollBackground.color = new Color(1f, 1f, 1f, 0f);
        scrollBackground.raycastTarget = true;

        GameObject viewportObject = CreateUiObject(
            "Viewport",
            scrollRectTransform,
            typeof(Image),
            typeof(RectMask2D));

        RectTransform viewportRect =
            viewportObject.GetComponent<RectTransform>();
        Stretch(viewportRect, 0f, 0f, 0f, 0f);

        Image viewportImage = viewportObject.GetComponent<Image>();
        viewportImage.color = new Color(1f, 1f, 1f, 0.001f);
        viewportImage.raycastTarget = true;

        GameObject contentObject = CreateUiObject(
            "Content",
            viewportRect,
            typeof(VerticalLayoutGroup),
            typeof(ContentSizeFitter));

        RectTransform contentRect =
            contentObject.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = Vector2.zero;

        VerticalLayoutGroup verticalLayout =
            contentObject.GetComponent<VerticalLayoutGroup>();
        verticalLayout.padding = new RectOffset(12, 12, 12, 12);
        verticalLayout.spacing = 8f;
        verticalLayout.childAlignment = TextAnchor.UpperLeft;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = true;
        verticalLayout.childForceExpandWidth = true;
        verticalLayout.childForceExpandHeight = false;

        ContentSizeFitter contentSizeFitter =
            contentObject.GetComponent<ContentSizeFitter>();
        contentSizeFitter.horizontalFit =
            ContentSizeFitter.FitMode.Unconstrained;
        contentSizeFitter.verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect scrollRect = scrollObject.GetComponent<ScrollRect>();
        scrollRect.content = contentRect;
        scrollRect.viewport = viewportRect;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.inertia = true;
        scrollRect.scrollSensitivity = 24f;

        ConversationLogPanelView panelView =
            Undo.AddComponent<ConversationLogPanelView>(panel.gameObject);

        SerializedObject serializedView = new SerializedObject(panelView);
        serializedView.FindProperty("scrollRect").objectReferenceValue = scrollRect;
        serializedView.FindProperty("contentRoot").objectReferenceValue = contentRect;
        serializedView.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(panel.gameObject);
        Selection.activeGameObject = panel.gameObject;

        EditorUtility.DisplayDialog(
            "Conversation UI",
            "스크롤 대화 레이아웃을 생성했습니다.\n" +
            "ConversationLogPanelView에서 말풍선 Sprite와 색상을 지정할 수 있습니다.",
            "확인");
    }

    [MenuItem(MenuPath, true)]
    private static bool ValidateBuildOnSelectedPanel()
    {
        return Selection.activeGameObject != null &&
               Selection.activeGameObject.GetComponent<RectTransform>() != null;
    }

    private static GameObject CreateUiObject(
        string objectName,
        RectTransform parent,
        params System.Type[] components)
    {
        GameObject gameObject = new GameObject(
            objectName,
            typeof(RectTransform));

        Undo.RegisterCreatedObjectUndo(
            gameObject,
            "Create " + objectName);

        RectTransform rectTransform =
            gameObject.GetComponent<RectTransform>();
        rectTransform.SetParent(parent, false);

        for (int i = 0; i < components.Length; i++)
            Undo.AddComponent(gameObject, components[i]);

        return gameObject;
    }

    private static void Stretch(
        RectTransform rect,
        float left,
        float right,
        float top,
        float bottom)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = new Vector2(left, bottom);
        rect.offsetMax = new Vector2(-right, -top);
    }
}
