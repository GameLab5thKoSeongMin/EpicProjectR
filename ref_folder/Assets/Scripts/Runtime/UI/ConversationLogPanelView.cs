using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationLogPanelView : MonoBehaviour
{
    public enum Speaker
    {
        Contractor,
        Player
    }

    [Header("Layout")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentRoot;
    [SerializeField, Min(1f)] private float minBubbleWidth = 120f;
    [SerializeField, Min(1f)] private float maxBubbleWidth = 400f;
    [SerializeField, Min(1f)] private float minBubbleHeight = 52f;
    [SerializeField, Min(0f)] private float horizontalTextPadding = 20f;
    [SerializeField, Min(0f)] private float verticalTextPadding = 10f;
    [SerializeField, Min(0f)] private float preferredSizeSafetyPadding = 4f;
    [SerializeField, Range(1, 10)] private int maximumVisibleLines = 2;

    [Header("Contractor Bubble")]
    [SerializeField] private Sprite contractorBubbleSprite;
    [SerializeField] private Color contractorBubbleColor = Color.white;
    [SerializeField] private Color contractorTextColor = Color.black;

    [Header("Player Bubble")]
    [SerializeField] private Sprite playerBubbleSprite;
    [SerializeField] private Color playerBubbleColor = new Color(0.75f, 0.86f, 1f, 1f);
    [SerializeField] private Color playerTextColor = Color.black;

    [Header("Text")]
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField, Min(1f)] private float fontSize = 24f;

    [Header("Scroll Test")]
    [SerializeField] private bool addScrollTestConversationOnStart = true;
    [SerializeField, Min(1)] private int scrollTestPairCount = 5;

    private readonly List<GameObject> messageRows = new List<GameObject>();
    private Coroutine scrollRoutine;

    private void Start()
    {
        if (addScrollTestConversationOnStart)
            AddScrollTestConversation();
    }

    public void AddContractorMessage(string message)
    {
        AddMessage(Speaker.Contractor, message);
    }

    public void AddPlayerMessage(string message)
    {
        AddMessage(Speaker.Player, message);
    }

    public void AddMessage(Speaker speaker, string message)
    {
        if (contentRoot == null || string.IsNullOrWhiteSpace(message))
            return;

        GameObject rowObject = CreateMessageRow(speaker);
        CreateBubble(rowObject.transform, speaker, message);
        messageRows.Add(rowObject);
        ScrollToLatestMessage();
    }

    public void ClearMessages()
    {
        if (contentRoot == null)
            return;

        for (int i = contentRoot.childCount - 1; i >= 0; i--)
            Destroy(contentRoot.GetChild(i).gameObject);

        messageRows.Clear();
        ScrollToLatestMessage();
    }

    [ContextMenu("Add Scroll Test Conversation")]
    public void AddScrollTestConversation()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Scroll test conversation can be added while the game is running.", this);
            return;
        }

        string[] contractorMessages =
        {
            "이번 계약은 북부 지역의 위험 구역을 통과하는 의뢰이며 준비 상태를 자세히 확인해 주셔야 합니다.",
            "모험가 전원이 추위에 대비한 장비를 준비했고 비상 상황에 사용할 물품도 함께 챙겼습니다.",
            "제출한 증명서와 장비 목록은 모두 최신 정보이며 필요한 사항이 있다면 바로 답변드리겠습니다.",
            "현지에서는 강한 바람과 낮은 기온이 예상되므로 이동 계획을 평소보다 신중하게 세웠습니다.",
            "계약 조건을 확인하신 뒤 추가로 설명이 필요한 항목이 있다면 어떤 내용이든 질문해 주세요."
        };

        string[] playerMessages =
        {
            "제출된 서류와 장비 목록을 먼저 대조한 뒤 위험 요소에 대응할 수 있는지 확인하겠습니다.",
            "모든 인원이 같은 수준으로 보호받는지와 비상 물품의 수량이 충분한지도 검토하겠습니다.",
            "증명서의 발급 기관과 유효 기간을 확인하고 기록된 내용이 실제 정보와 같은지 살펴보겠습니다.",
            "이동 경로와 예상 기후를 기준으로 현재 준비한 장비가 적절한지 다시 계산해 보겠습니다.",
            "검토가 끝날 때까지 잠시 기다려 주시고 누락된 내용이 발견되면 추가 설명을 요청하겠습니다."
        };

        int pairCount = Mathf.Max(1, scrollTestPairCount);

        for (int i = 0; i < pairCount; i++)
        {
            AddContractorMessage(contractorMessages[i % contractorMessages.Length]);
            AddPlayerMessage(playerMessages[i % playerMessages.Length]);
        }
    }

    private GameObject CreateMessageRow(Speaker speaker)
    {
        GameObject rowObject = new GameObject(
            speaker == Speaker.Contractor ? "ContractorBubbleRow" : "PlayerBubbleRow",
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement));

        RectTransform rowRect = rowObject.GetComponent<RectTransform>();
        rowRect.SetParent(contentRoot, false);
        rowRect.anchorMin = new Vector2(0f, 1f);
        rowRect.anchorMax = new Vector2(1f, 1f);
        rowRect.pivot = new Vector2(0.5f, 1f);
        rowRect.sizeDelta = Vector2.zero;

        HorizontalLayoutGroup layout = rowObject.GetComponent<HorizontalLayoutGroup>();
        layout.childAlignment = speaker == Speaker.Contractor
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

    private void CreateBubble(
        Transform row,
        Speaker speaker,
        string message)
    {
        GameObject bubbleObject = new GameObject(
            speaker == Speaker.Contractor ? "ContractorBubble" : "PlayerBubble",
            typeof(RectTransform),
            typeof(Image),
            typeof(RectMask2D),
            typeof(LayoutElement));

        RectTransform bubbleRect = bubbleObject.GetComponent<RectTransform>();
        bubbleRect.SetParent(row, false);

        Image bubbleImage = bubbleObject.GetComponent<Image>();
        bubbleImage.sprite = speaker == Speaker.Contractor
            ? contractorBubbleSprite
            : playerBubbleSprite;
        bubbleImage.color = speaker == Speaker.Contractor
            ? contractorBubbleColor
            : playerBubbleColor;
        bubbleImage.type = bubbleImage.sprite != null
            ? Image.Type.Sliced
            : Image.Type.Simple;
        bubbleImage.raycastTarget = false;

        TMP_Text text = CreateMessageText(bubbleRect, speaker, message);
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

    private TMP_Text CreateMessageText(
        RectTransform bubbleRect,
        Speaker speaker,
        string message)
    {
        GameObject textObject = new GameObject(
            "MessageText",
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
        text.color = speaker == Speaker.Contractor
            ? contractorTextColor
            : playerTextColor;
        text.alignment = TextAlignmentOptions.MidlineLeft;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Ellipsis;
        text.maxVisibleLines = maximumVisibleLines;
        text.raycastTarget = false;

        if (fontAsset != null)
            text.font = fontAsset;

        return text;
    }

    private string BuildLineHeightSample(int lineCount)
    {
        int count = Mathf.Max(1, lineCount);
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < count; i++)
        {
            if (i > 0)
                builder.Append('\n');

            builder.Append("가Ag");
        }

        return builder.ToString();
    }

    private void ScrollToLatestMessage()
    {
        if (!isActiveAndEnabled || scrollRect == null)
            return;

        if (scrollRoutine != null)
            StopCoroutine(scrollRoutine);

        scrollRoutine = StartCoroutine(ScrollToLatestMessageNextFrame());
    }

    private IEnumerator ScrollToLatestMessageNextFrame()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot);
        scrollRect.verticalNormalizedPosition = 0f;
        scrollRoutine = null;
    }
}
