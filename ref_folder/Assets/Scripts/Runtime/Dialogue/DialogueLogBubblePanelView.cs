using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLogBubblePanelView : MonoBehaviour
{
    public enum BubbleAlignment
    {
        Left,
        Right
    }

    private class ConversationState
    {
        public int key;
        public string title;
        public RectTransform contentRoot;
        public Button tabButton;
        public Image tabImage;
        public TMP_Text tabText;
        public readonly List<GameObject> messageRows = new List<GameObject>();
        public readonly Dictionary<int, TextMeshProUGUI> activeLineTexts =
            new Dictionary<int, TextMeshProUGUI>();
    }

    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private bool clearOnDialogueStart = true;

    [Header("Tabs")]
    [SerializeField] private bool useConversationTabs = true;
    [SerializeField] private RectTransform tabRoot;
    [SerializeField, Min(1f)] private float tabHeight = 34f;
    [SerializeField, Min(0f)] private float tabSpacing = 4f;
    [SerializeField] private Color selectedTabColor = new Color(0.86f, 0.92f, 1f, 1f);
    [SerializeField] private Color normalTabColor = new Color(1f, 1f, 1f, 0.72f);
    [SerializeField] private Color tabTextColor = Color.black;
    [SerializeField] private string fallbackConversationTitle = "Visitor";

    [Header("Layout")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentRoot;
    [SerializeField] private BubbleAlignment bubbleAlignment = BubbleAlignment.Left;
    [SerializeField, Min(1f)] private float minBubbleWidth = 120f;
    [SerializeField, Min(1f)] private float maxBubbleWidth = 420f;
    [SerializeField, Min(1f)] private float minBubbleHeight = 52f;
    [SerializeField, Min(0f)] private float horizontalTextPadding = 20f;
    [SerializeField, Min(0f)] private float verticalTextPadding = 10f;
    [SerializeField, Min(0f)] private float preferredSizeSafetyPadding = 4f;
    [SerializeField, Min(0f)] private float lineSpacing = 6f;
    [SerializeField, Range(1, 10)] private int maximumVisibleLines = 3;

    [Header("Contractor Bubble")]
    [SerializeField] private Sprite bubbleSprite;
    [SerializeField] private Color bubbleColor = Color.white;
    [SerializeField] private Color textColor = Color.black;

    [Header("Player Bubble")]
    [SerializeField] private Sprite playerBubbleSprite;
    [SerializeField] private Color playerBubbleColor = new Color(0.75f, 0.86f, 1f, 1f);
    [SerializeField] private Color playerTextColor = Color.black;

    [Header("Progress Indicator")]
    [SerializeField] private Sprite progressIndicatorSprite;
    [SerializeField] private Color progressIndicatorColor = new Color(1f, 0.18f, 0.12f, 1f);
    [SerializeField, Min(1f)] private float progressIndicatorWidth = 18f;
    [SerializeField, Min(1f)] private float progressIndicatorHeight = 18f;
    [SerializeField] private Vector2 contractorProgressIndicatorOffset = new Vector2(-10f, 0f);
    [SerializeField] private Vector2 playerProgressIndicatorOffset = new Vector2(10f, 0f);
    [SerializeField, Min(0f)] private float progressIndicatorBlinkDuration = 0.45f;
    [SerializeField, Range(0f, 1f)] private float progressIndicatorMinAlpha = 0.2f;
    [SerializeField, Range(0f, 1f)] private float progressIndicatorMaxAlpha = 1f;

    [Header("Text")]
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField, Min(1f)] private float fontSize = 24f;
    [SerializeField, Min(1f)] private float tabFontSize = 20f;

    private readonly Dictionary<int, ConversationState> conversations =
        new Dictionary<int, ConversationState>();
    private readonly List<int> conversationOrder = new List<int>();
    private Coroutine scrollRoutine;
    private bool isSubscribed;
    private bool dialogueOutputEnabled = true;
    private bool createdTabRoot;
    private UnderwritingCase boundCase;
    private ConversationState activeConversation;
    private ConversationState sequenceConversation;
    private Image activeProgressIndicator;
    private Coroutine progressIndicatorBlinkRoutine;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = GetComponentInParent<DialogueManager>();

        if (dialogueManager == null)
            dialogueManager = FindFirstObjectByType<DialogueManager>();

        EnsureTabRoot();
        EnsureContentRootLayout(contentRoot);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void SetDialogueManager(DialogueManager value)
    {
        if (dialogueManager == value)
            return;

        Unsubscribe();
        dialogueManager = value;
        Subscribe();
    }

    public void SetDialogueOutputEnabled(bool value)
    {
        if (dialogueOutputEnabled == value)
            return;

        dialogueOutputEnabled = value;

        Subscribe();

    }

    public void ShowPanel()
    {
    }

    public void HidePanel()
    {
    }

    public void Bind(UnderwritingCase underwritingCase)
    {
        if (boundCase == underwritingCase)
            return;

        boundCase = underwritingCase;
        ClearMessages();

        if (underwritingCase == null ||
            underwritingCase.adventurers == null)
        {
            CreateCaseConversationIfNamed();
            return;
        }

        bool createdConversation = false;

        for (int i = 0; i < underwritingCase.adventurers.Count; i++)
        {
            AdventurerDocument adventurer = underwritingCase.adventurers[i];

            if (adventurer == null)
                continue;

            GetOrCreateConversation(
                i,
                GetConversationTitle(i, adventurer.adventurerName));
            createdConversation = true;
        }

        if (!createdConversation)
            CreateCaseConversationIfNamed();

        if (conversationOrder.Count > 0)
            SelectConversation(conversationOrder[0], false);
    }

    public void ClearMessages()
    {
        ClearProgressIndicator();

        for (int i = 0; i < conversationOrder.Count; i++)
        {
            if (conversations.TryGetValue(conversationOrder[i], out ConversationState state))
                DestroyConversation(state);
        }

        conversations.Clear();
        conversationOrder.Clear();
        activeConversation = null;
        sequenceConversation = null;

        if (contentRoot != null)
        {
            ClearContentRoot(contentRoot);
            contentRoot.gameObject.SetActive(true);

            if (scrollRect != null)
                scrollRect.content = contentRoot;
        }

        if (tabRoot != null)
        {
            for (int i = tabRoot.childCount - 1; i >= 0; i--)
                Destroy(tabRoot.GetChild(i).gameObject);
        }
    }

    private void Subscribe()
    {
        if (dialogueManager == null || isSubscribed)
            return;

        dialogueManager.DialogueStarted += OnDialogueStarted;
        dialogueManager.DialogueLineStarted += OnDialogueLineStarted;
        dialogueManager.DialogueLineVisibleCharactersChanged += OnDialogueLineVisibleCharactersChanged;
        dialogueManager.DialogueLineCompleted += OnDialogueLineCompleted;
        dialogueManager.DialogueEnded += OnDialogueEnded;
        isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (dialogueManager == null || !isSubscribed)
            return;

        dialogueManager.DialogueStarted -= OnDialogueStarted;
        dialogueManager.DialogueLineStarted -= OnDialogueLineStarted;
        dialogueManager.DialogueLineVisibleCharactersChanged -= OnDialogueLineVisibleCharactersChanged;
        dialogueManager.DialogueLineCompleted -= OnDialogueLineCompleted;
        dialogueManager.DialogueEnded -= OnDialogueEnded;
        isSubscribed = false;
    }

    private void OnDialogueStarted(DialogueSequence sequence)
    {
        sequenceConversation = null;

        if (sequence == null)
            return;

        int key = useConversationTabs
            ? GetConversationKey(sequence)
            : 0;

        sequenceConversation = GetOrCreateConversation(
            key,
            GetConversationTitle(sequence));
        sequenceConversation.activeLineTexts.Clear();

        if (clearOnDialogueStart && !useConversationTabs)
            ClearConversationMessages(sequenceConversation);

        SelectConversation(key, true);

    }

    private void OnDialogueLineStarted(int lineIndex, string line)
    {
        ConversationState state = sequenceConversation ?? activeConversation;

        if (state == null || state.contentRoot == null || string.IsNullOrWhiteSpace(line))
            return;

        if (state.activeLineTexts.ContainsKey(lineIndex))
            return;

        DialogueSpeaker speaker = GetLineSpeaker(lineIndex);
        GameObject rowObject = CreateMessageRow(state.contentRoot, speaker);
        TextMeshProUGUI lineText = CreateBubble(rowObject.transform, speaker, line);
        lineText.maxVisibleCharacters = 0;
        state.activeLineTexts.Add(lineIndex, lineText);
        state.messageRows.Add(rowObject);
        ScrollToLatestMessage();
    }

    private void OnDialogueLineVisibleCharactersChanged(
        int lineIndex,
        int visibleCharacterCount)
    {
        ConversationState state = sequenceConversation ?? activeConversation;

        if (state == null ||
            !state.activeLineTexts.TryGetValue(lineIndex, out TextMeshProUGUI lineText))
        {
            return;
        }

        lineText.maxVisibleCharacters = Mathf.Max(0, visibleCharacterCount);
    }

    private void OnDialogueLineCompleted(int lineIndex)
    {
        ConversationState state = sequenceConversation ?? activeConversation;

        if (state == null ||
            !state.activeLineTexts.TryGetValue(lineIndex, out TextMeshProUGUI lineText))
        {
            return;
        }

        lineText.maxVisibleCharacters = int.MaxValue;
    }

    private void OnDialogueEnded()
    {
        ClearProgressIndicator();
        sequenceConversation = null;

    }

    private ConversationState GetOrCreateConversation(int key, string title)
    {
        if (conversations.TryGetValue(key, out ConversationState state))
        {
            state.title = title;
            SetTabText(state, title);
            return state;
        }

        state = new ConversationState();
        state.key = key;
        state.title = title;
        state.contentRoot = CreateConversationContentRoot(title);
        conversations.Add(key, state);
        conversationOrder.Add(key);

        EnsureContentRootLayout(state.contentRoot);
        CreateConversationTab(state);

        if (activeConversation == null)
            SelectConversation(key, false);
        else
            state.contentRoot.gameObject.SetActive(false);

        return state;
    }

    private RectTransform CreateConversationContentRoot(string title)
    {
        if (contentRoot == null)
            return null;

        if (conversations.Count == 0)
        {
            ClearContentRoot(contentRoot);
            contentRoot.name = BuildContentRootName(title);
            return contentRoot;
        }

        GameObject contentObject = new GameObject(
            BuildContentRootName(title),
            typeof(RectTransform));
        RectTransform newRoot = contentObject.GetComponent<RectTransform>();
        RectTransform parent = contentRoot.parent as RectTransform;
        newRoot.SetParent(parent != null ? parent : contentRoot.parent, false);
        CopyRectTransform(contentRoot, newRoot);
        return newRoot;
    }

    private void CreateConversationTab(ConversationState state)
    {
        if (!useConversationTabs || tabRoot == null || state == null)
            return;

        GameObject tabObject = new GameObject(
            "Tab_" + state.title,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button),
            typeof(LayoutElement));
        RectTransform tabRect = tabObject.GetComponent<RectTransform>();
        tabRect.SetParent(tabRoot, false);

        LayoutElement layout = tabObject.GetComponent<LayoutElement>();
        layout.minHeight = tabHeight;
        layout.preferredHeight = tabHeight;
        layout.minWidth = 92f;
        layout.preferredWidth = 124f;

        state.tabImage = tabObject.GetComponent<Image>();
        state.tabImage.color = normalTabColor;

        state.tabButton = tabObject.GetComponent<Button>();
        int capturedKey = state.key;
        state.tabButton.onClick.AddListener(() => SelectConversation(capturedKey, true));

        GameObject textObject = new GameObject(
            "Label",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(tabRect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10f, 2f);
        textRect.offsetMax = new Vector2(-10f, -2f);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.fontSize = tabFontSize;
        text.color = tabTextColor;
        text.alignment = TextAlignmentOptions.Center;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Ellipsis;
        text.raycastTarget = false;

        if (fontAsset != null)
            text.font = fontAsset;

        state.tabText = text;
        SetTabText(state, state.title);
    }

    private void SelectConversation(int key, bool scrollToLatest)
    {
        if (!conversations.TryGetValue(key, out ConversationState selected))
            return;

        activeConversation = selected;

        for (int i = 0; i < conversationOrder.Count; i++)
        {
            ConversationState state = conversations[conversationOrder[i]];
            bool isSelected = state == selected;

            if (state.contentRoot != null)
                state.contentRoot.gameObject.SetActive(isSelected);

            if (state.tabImage != null)
                state.tabImage.color = isSelected ? selectedTabColor : normalTabColor;
        }

        if (scrollRect != null)
            scrollRect.content = selected.contentRoot;

        if (scrollToLatest)
            ScrollToLatestMessage();
    }

    private void EnsureTabRoot()
    {
        if (!useConversationTabs)
            return;

        if (tabRoot != null)
        {
            EnsureTabRootLayout();
            return;
        }

        RectTransform panelRect = transform as RectTransform;
        if (panelRect == null)
            return;

        GameObject tabObject = new GameObject(
            "ConversationTabs",
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup));
        tabRoot = tabObject.GetComponent<RectTransform>();
        tabRoot.SetParent(panelRect, false);
        tabRoot.anchorMin = new Vector2(0f, 1f);
        tabRoot.anchorMax = new Vector2(1f, 1f);
        tabRoot.pivot = new Vector2(0.5f, 1f);
        tabRoot.offsetMin = new Vector2(12f, -tabHeight - 8f);
        tabRoot.offsetMax = new Vector2(-12f, -8f);
        tabRoot.SetAsLastSibling();
        createdTabRoot = true;

        EnsureTabRootLayout();
        ReserveTabArea();
    }

    private void EnsureTabRootLayout()
    {
        if (tabRoot == null)
            return;

        HorizontalLayoutGroup layout = tabRoot.GetComponent<HorizontalLayoutGroup>();
        if (layout == null)
            layout = tabRoot.gameObject.AddComponent<HorizontalLayoutGroup>();

        layout.spacing = tabSpacing;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
    }

    private void ReserveTabArea()
    {
        if (!createdTabRoot || scrollRect == null)
            return;

        RectTransform scrollRectTransform = scrollRect.transform as RectTransform;
        if (scrollRectTransform == null || scrollRectTransform.parent != transform)
            return;

        float currentTop = Mathf.Abs(scrollRectTransform.offsetMax.y);
        float reservedTop = currentTop + tabHeight + tabSpacing + 8f;
        scrollRectTransform.offsetMax = new Vector2(
            scrollRectTransform.offsetMax.x,
            -reservedTop);
    }

    private void EnsureContentRootLayout(RectTransform targetRoot)
    {
        if (targetRoot == null)
            return;

        VerticalLayoutGroup existingLayout =
            targetRoot.GetComponent<VerticalLayoutGroup>();
        if (existingLayout != null)
            existingLayout.enabled = false;

        DialogueBubbleVerticalLayoutGroup layout =
            targetRoot.GetComponent<DialogueBubbleVerticalLayoutGroup>();
        if (layout == null)
            layout = targetRoot.gameObject.AddComponent<DialogueBubbleVerticalLayoutGroup>();

        layout.childAlignment = TextAnchor.UpperLeft;
        layout.Spacing = lineSpacing;
        layout.ChildControlWidth = true;
        layout.ChildControlHeight = true;
        layout.ChildForceExpandWidth = true;
        layout.ChildForceExpandHeight = false;

        ContentSizeFitter fitter = targetRoot.GetComponent<ContentSizeFitter>();
        if (fitter == null)
            fitter = targetRoot.gameObject.AddComponent<ContentSizeFitter>();

        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private GameObject CreateMessageRow(
        RectTransform targetContentRoot,
        DialogueSpeaker speaker)
    {
        GameObject rowObject = new GameObject(
            speaker == DialogueSpeaker.Player ? "PlayerBubbleRow" : "ContractorBubbleRow",
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement));

        RectTransform rowRect = rowObject.GetComponent<RectTransform>();
        rowRect.SetParent(targetContentRoot, false);
        rowRect.anchorMin = new Vector2(0f, 1f);
        rowRect.anchorMax = new Vector2(1f, 1f);
        rowRect.pivot = new Vector2(0.5f, 1f);
        rowRect.sizeDelta = Vector2.zero;

        HorizontalLayoutGroup layout = rowObject.GetComponent<HorizontalLayoutGroup>();
        BubbleAlignment alignment = GetBubbleAlignment(speaker);
        layout.childAlignment = alignment == BubbleAlignment.Left
            ? TextAnchor.MiddleLeft
            : TextAnchor.MiddleRight;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        LayoutElement layoutElement = rowObject.GetComponent<LayoutElement>();
        layoutElement.minHeight = minBubbleHeight;

        return rowObject;
    }

    private TextMeshProUGUI CreateBubble(
        Transform row,
        DialogueSpeaker speaker,
        string message)
    {
        GameObject bubbleObject = new GameObject(
            speaker == DialogueSpeaker.Player ? "PlayerBubble" : "ContractorBubble",
            typeof(RectTransform),
            typeof(Image),
            typeof(RectMask2D),
            typeof(LayoutElement));

        RectTransform bubbleRect = bubbleObject.GetComponent<RectTransform>();
        bubbleRect.SetParent(row, false);

        Image bubbleImage = bubbleObject.GetComponent<Image>();
        bubbleImage.sprite = GetBubbleSprite(speaker);
        bubbleImage.color = GetBubbleColor(speaker);
        bubbleImage.type = bubbleImage.sprite != null
            ? Image.Type.Sliced
            : Image.Type.Simple;
        bubbleImage.raycastTarget = false;

        TextMeshProUGUI text = CreateMessageText(bubbleRect, speaker, message);
        ResizeBubble(row, bubbleObject, text, message);
        SetActiveProgressIndicator(CreateProgressIndicator(bubbleRect, speaker));
        return text;
    }

    private Image CreateProgressIndicator(
        RectTransform bubbleRect,
        DialogueSpeaker speaker)
    {
        GameObject indicatorObject = new GameObject(
            "ProgressIndicator",
            typeof(RectTransform),
            typeof(Image));

        RectTransform indicatorRect = indicatorObject.GetComponent<RectTransform>();
        indicatorRect.SetParent(bubbleRect, false);

        bool isPlayer = speaker == DialogueSpeaker.Player;
        indicatorRect.anchorMin = isPlayer
            ? new Vector2(0f, 0f)
            : new Vector2(1f, 0f);
        indicatorRect.anchorMax = indicatorRect.anchorMin;
        indicatorRect.pivot = isPlayer
            ? new Vector2(0f, 0f)
            : new Vector2(1f, 0f);
        indicatorRect.sizeDelta = new Vector2(progressIndicatorWidth, progressIndicatorHeight);
        indicatorRect.anchoredPosition = isPlayer
            ? playerProgressIndicatorOffset
            : contractorProgressIndicatorOffset;

        Image indicatorImage = indicatorObject.GetComponent<Image>();
        indicatorImage.sprite = progressIndicatorSprite;
        indicatorImage.color = progressIndicatorColor;
        indicatorImage.type = indicatorImage.sprite != null
            ? Image.Type.Simple
            : Image.Type.Simple;
        indicatorImage.raycastTarget = false;

        return indicatorImage;
    }

    private void SetActiveProgressIndicator(Image indicator)
    {
        ClearProgressIndicator();
        activeProgressIndicator = indicator;

        if (activeProgressIndicator == null)
            return;

        progressIndicatorBlinkRoutine = StartCoroutine(BlinkProgressIndicator(activeProgressIndicator));
    }

    private void ClearProgressIndicator()
    {
        if (progressIndicatorBlinkRoutine != null)
        {
            StopCoroutine(progressIndicatorBlinkRoutine);
            progressIndicatorBlinkRoutine = null;
        }

        if (activeProgressIndicator != null)
            Destroy(activeProgressIndicator.gameObject);

        activeProgressIndicator = null;
    }

    private IEnumerator BlinkProgressIndicator(Image indicator)
    {
        if (indicator == null)
            yield break;

        if (progressIndicatorBlinkDuration <= 0f)
        {
            SetProgressIndicatorAlpha(indicator, progressIndicatorMaxAlpha);
            yield break;
        }

        while (indicator != null)
        {
            float phase = Mathf.PingPong(
                Time.unscaledTime / progressIndicatorBlinkDuration,
                1f);
            float alpha = Mathf.Lerp(
                progressIndicatorMaxAlpha,
                progressIndicatorMinAlpha,
                phase);

            SetProgressIndicatorAlpha(indicator, alpha);
            yield return null;
        }
    }

    private void SetProgressIndicatorAlpha(Image indicator, float alpha)
    {
        if (indicator == null)
            return;

        Color color = indicator.color;
        color.a = Mathf.Clamp01(alpha);
        indicator.color = color;
    }

    private TextMeshProUGUI CreateMessageText(
        RectTransform bubbleRect,
        DialogueSpeaker speaker,
        string message)
    {
        GameObject textObject = new GameObject(
            "DialogueText",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(bubbleRect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(horizontalTextPadding, verticalTextPadding);
        textRect.offsetMax = new Vector2(-horizontalTextPadding, -verticalTextPadding);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = message;
        text.fontSize = fontSize;
        text.color = GetTextColor(speaker);
        text.alignment = TextAlignmentOptions.MidlineLeft;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Ellipsis;
        text.maxVisibleLines = maximumVisibleLines;
        text.raycastTarget = false;

        if (fontAsset != null)
            text.font = fontAsset;

        return text;
    }

    private DialogueSpeaker GetLineSpeaker(int lineIndex)
    {
        if (dialogueManager == null || dialogueManager.CurrentSequence == null)
            return DialogueSpeaker.Contractor;

        DialogueLine line = dialogueManager.CurrentSequence.GetDialogueLine(lineIndex);
        return line != null ? line.Speaker : DialogueSpeaker.Contractor;
    }

    private BubbleAlignment GetBubbleAlignment(DialogueSpeaker speaker)
    {
        return speaker == DialogueSpeaker.Player
            ? BubbleAlignment.Right
            : bubbleAlignment;
    }

    private Sprite GetBubbleSprite(DialogueSpeaker speaker)
    {
        return speaker == DialogueSpeaker.Player && playerBubbleSprite != null
            ? playerBubbleSprite
            : bubbleSprite;
    }

    private Color GetBubbleColor(DialogueSpeaker speaker)
    {
        return speaker == DialogueSpeaker.Player
            ? playerBubbleColor
            : bubbleColor;
    }

    private Color GetTextColor(DialogueSpeaker speaker)
    {
        return speaker == DialogueSpeaker.Player
            ? playerTextColor
            : textColor;
    }

    private void ResizeBubble(
        Transform row,
        GameObject bubbleObject,
        TMP_Text text,
        string message)
    {
        float horizontalPadding = horizontalTextPadding * 2f;
        float verticalPadding = verticalTextPadding * 2f;
        float maxTextWidth = Mathf.Max(
            1f,
            maxBubbleWidth - horizontalPadding - preferredSizeSafetyPadding);

        Vector2 unwrappedTextSize = text.GetPreferredValues(
            message,
            float.PositiveInfinity,
            float.PositiveInfinity);

        float bubbleWidth = Mathf.Clamp(
            unwrappedTextSize.x + horizontalPadding + preferredSizeSafetyPadding,
            minBubbleWidth,
            maxBubbleWidth);

        float actualTextWidth = Mathf.Max(
            1f,
            bubbleWidth - horizontalPadding - preferredSizeSafetyPadding);

        Vector2 wrappedTextSize = text.GetPreferredValues(
            message,
            actualTextWidth,
            float.PositiveInfinity);

        Vector2 maximumLineSize = text.GetPreferredValues(
            BuildLineHeightSample(maximumVisibleLines),
            maxTextWidth,
            float.PositiveInfinity);

        float visibleTextHeight = Mathf.Min(
            wrappedTextSize.y,
            maximumLineSize.y);
        float bubbleHeight = Mathf.Max(
            minBubbleHeight,
            visibleTextHeight + verticalPadding + preferredSizeSafetyPadding);

        LayoutElement bubbleLayout = bubbleObject.GetComponent<LayoutElement>();
        bubbleLayout.minWidth = minBubbleWidth;
        bubbleLayout.preferredWidth = bubbleWidth;
        bubbleLayout.minHeight = minBubbleHeight;
        bubbleLayout.preferredHeight = bubbleHeight;

        LayoutElement rowLayout = row.GetComponent<LayoutElement>();
        if (rowLayout != null)
            rowLayout.preferredHeight = bubbleHeight;
    }

    private void DestroyConversation(ConversationState state)
    {
        if (state == null)
            return;

        ClearConversationMessages(state);

        if (state.tabButton != null)
            Destroy(state.tabButton.gameObject);

        if (state.contentRoot != null && state.contentRoot != contentRoot)
            Destroy(state.contentRoot.gameObject);
    }

    private void ClearConversationMessages(ConversationState state)
    {
        if (state == null)
            return;

        if (state.contentRoot != null)
            ClearContentRoot(state.contentRoot);

        state.messageRows.Clear();
        state.activeLineTexts.Clear();
    }

    private void ClearContentRoot(RectTransform targetRoot)
    {
        if (targetRoot == null)
            return;

        for (int i = targetRoot.childCount - 1; i >= 0; i--)
            Destroy(targetRoot.GetChild(i).gameObject);
    }

    private void CopyRectTransform(RectTransform source, RectTransform target)
    {
        if (source == null || target == null)
            return;

        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
        target.localScale = Vector3.one;
        target.localRotation = Quaternion.identity;
    }

    private int GetConversationKey(DialogueSequence sequence)
    {
        if (sequence != null && sequence.HasConversationOverride)
            return sequence.ConversationKey;

        if (sequence != null && sequence.AdventurerIndex >= 0)
            return sequence.AdventurerIndex;

        return 0;
    }

    private string GetConversationTitle(DialogueSequence sequence)
    {
        if (sequence != null &&
            sequence.HasConversationOverride &&
            !string.IsNullOrWhiteSpace(sequence.ConversationTitle))
        {
            return sequence.ConversationTitle;
        }

        if (sequence != null && !string.IsNullOrWhiteSpace(sequence.AdventurerName))
            return sequence.AdventurerName;

        string caseConversationTitle = GetCaseConversationTitle();
        if (!string.IsNullOrWhiteSpace(caseConversationTitle))
            return caseConversationTitle;

        int key = GetConversationKey(sequence);
        return GetConversationTitle(key, fallbackConversationTitle);
    }

    private string GetConversationTitle(int index, string adventurerName)
    {
        if (!string.IsNullOrWhiteSpace(adventurerName))
            return adventurerName;

        string caseConversationTitle = GetCaseConversationTitle();
        if (!string.IsNullOrWhiteSpace(caseConversationTitle))
            return caseConversationTitle;

        return fallbackConversationTitle + " " + (index + 1);
    }

    private void CreateCaseConversationIfNamed()
    {
        string caseConversationTitle = GetCaseConversationTitle();

        if (!string.IsNullOrWhiteSpace(caseConversationTitle))
            GetOrCreateConversation(0, caseConversationTitle);
    }

    private string GetCaseConversationTitle()
    {
        return boundCase != null ? boundCase.npcName : string.Empty;
    }

    private void SetTabText(ConversationState state, string text)
    {
        if (state != null && state.tabText != null)
            state.tabText.text = text;
    }

    private string BuildContentRootName(string title)
    {
        return "Content_" + (string.IsNullOrWhiteSpace(title) ? fallbackConversationTitle : title);
    }

    private string BuildLineHeightSample(int lineCount)
    {
        int count = Mathf.Max(1, lineCount);
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < count; i++)
        {
            if (i > 0)
                builder.Append('\n');

            builder.Append("Ag");
        }

        return builder.ToString();
    }

    private void ScrollToLatestMessage()
    {
        if (!isActiveAndEnabled || scrollRect == null || scrollRect.content == null)
            return;

        if (scrollRoutine != null)
            StopCoroutine(scrollRoutine);

        scrollRoutine = StartCoroutine(ScrollToLatestMessageNextFrame());
    }

    private IEnumerator ScrollToLatestMessageNextFrame()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();

        if (scrollRect != null && scrollRect.content != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;

        scrollRoutine = null;
    }

}
