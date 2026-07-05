// Responsibility: Owns first playable Unity UI references and renders presenter-provided screen states.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Content;
using EpicProjectR.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayableView
    {
        private readonly FirstPlayableUiTheme theme;
        private readonly FirstPlayableUiFactory factory;
        private readonly Dictionary<RuleId, Toggle> ruleToggles = new Dictionary<RuleId, Toggle>();
        private readonly Dictionary<RuleId, Image> ruleRows = new Dictionary<RuleId, Image>();
        private readonly Dictionary<RuleId, RuleSeverity> ruleSeverities = new Dictionary<RuleId, RuleSeverity>();

        private FirstPlayableTweenRunner tweenRunner;
        private Text hudTitleText;
        private Text hudDateText;
        private Text hudLedgerText;
        private Text entryTitleText;
        private Text entryPromptText;
        private Text dialogueText;
        private Text reviewStatusText;
        private Text premiumText;
        private Text resultText;
        private Text outcomeText;
        private Text settlementText;
        private Text nextButtonText;
        private Text drawerApproveText;
        private Text drawerPremiumText;
        private Text drawerSummaryText;
        private Transform documentRoot;
        private Transform absoluteRulesRoot;
        private Transform considerationRulesRoot;
        private RectTransform documentBoardRect;
        private RectTransform workbenchRect;
        private RectTransform shelfRect;
        private RectTransform dialogueRect;
        private RectTransform contractorRect;
        private RectTransform entryContractorRect;
        private RectTransform decisionDrawerRect;
        private Button entryButton;
        private Button entryBellButton;
        private Button documentBoardButton;
        private Button decisionBoxButton;
        private Button approveButton;
        private Button rejectButton;
        private Button nextButton;
        private GameObject entryRoot;
        private GameObject reviewRoot;
        private GameObject decisionBox;
        private GameObject decisionDrawer;
        private GameObject decisionRow;
        private GameObject nextRow;
        private Image contractorImage;
        private CanvasGroup contractorCanvasGroup;
        private Image entryContractorImage;
        private CanvasGroup entryContractorCanvasGroup;
        private IReadOnlyList<string> dialogueLines = Array.Empty<string>();
        private int dialogueIndex;
        private bool resultAwaitingDocumentClick;

        public FirstPlayableView(FirstPlayableUiTheme theme, FirstPlayableUiFactory factory)
        {
            this.theme = theme ?? throw new ArgumentNullException(nameof(theme));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            if (factory.Assets == null)
            {
                throw new ArgumentException("FirstPlayableUiFactory must expose a non-null asset catalog.", nameof(factory));
            }
        }

        public event Action ReviewStarted;
        public event Action DecisionDrawerRequested;
        public event Action ReviewClosedRequested;
        public event Action<PlayerDecision, IReadOnlyList<RuleId>> DecisionSubmitted;
        public event Action ResultAcknowledged;

        public void Build(Transform parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var canvasObject = new GameObject("First Playable Canvas");
            canvasObject.transform.SetParent(parent, false);
            tweenRunner = canvasObject.AddComponent<FirstPlayableTweenRunner>();
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = FirstPlayableMainSceneRectSpec.ReferenceResolution;
            scaler.matchWidthOrHeight = 0f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var background = factory.CreatePanel(canvasObject.transform, "Main Scene Background", theme.MainBackdrop, false, factory.Assets.MainBackgroundSprite);
            var backgroundImage = background.GetComponent<Image>();
            backgroundImage.preserveAspect = false;

            factory.CreatePanel(background.transform, "Main Scene Dark Overlay", theme.MainOverlay, false);
            BuildTopStrip(background.transform);
            entryRoot = CreateRoot(background.transform, "Entry Contract Root");
            reviewRoot = CreateRoot(background.transform, "Review Sequence Root");
            BuildEntry(entryRoot.transform);
            BuildCharacterDialogue(reviewRoot.transform);
            BuildWorkbench(reviewRoot.transform);
            BuildShelfRules(reviewRoot.transform);
            BuildDecisionBox(reviewRoot.transform);
            BuildDecisionDrawer(reviewRoot.transform);
            reviewRoot.SetActive(false);
            decisionDrawer.SetActive(false);
        }

        public void RenderEntry(FirstPlayableEntryScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ClearReviewRoots();
            entryTitleText.text = state.EntryTitle;
            entryPromptText.text = state.EntryPrompt;
            entryButton.interactable = true;
            entryBellButton.interactable = true;
            resultAwaitingDocumentClick = false;
            ResetEntryContractorPreview();
            entryRoot.SetActive(true);
            reviewRoot.SetActive(false);
            decisionDrawer.SetActive(false);
        }

        public void RenderHud(FirstPlayableHudState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            hudTitleText.text = state.Title;
            hudDateText.text = state.Date;
            hudLedgerText.text = FirstPlayableKoreanText.HudLedger(state.Ducats, state.Reputation);
        }

        public void RenderReview(FirstPlayableReviewScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ClearReviewRoots();
            ruleToggles.Clear();
            ruleRows.Clear();
            ruleSeverities.Clear();

            entryRoot.SetActive(false);
            reviewRoot.SetActive(true);
            decisionDrawer.SetActive(false);
            decisionBox.SetActive(true);
            decisionBoxButton.interactable = true;
            resultAwaitingDocumentClick = false;

            reviewStatusText.text = state.ReviewStatus;
            premiumText.text = state.Premium;
            resultText.text = FirstPlayableKoreanText.ReviewWaitingAudit;
            resultText.color = theme.Ink;
            outcomeText.text = FirstPlayableKoreanText.ReviewWaitingOutcome;
            outcomeText.color = theme.Ink;
            settlementText.text = FirstPlayableKoreanText.ReviewWaitingSettlement;
            settlementText.color = theme.MutedInk;
            SetDialogueLines(state.DialogueLines);

            for (var i = 0; i < state.Documents.Count; i++)
            {
                CreateDocumentCard(documentRoot, state.Documents[i], i);
            }

            foreach (var rule in state.Rules)
            {
                var parent = rule.Rule.Severity == RuleSeverity.AbsoluteRejection
                    ? absoluteRulesRoot
                    : considerationRulesRoot;
                CreateRuleRow(parent, rule);
            }

            SetDecisionButtonsInteractable(true);
            nextRow.SetActive(false);
            RefreshDecisionDrawer();
            PlayReviewOpening();
        }

        public void OpenDecisionDrawer()
        {
            decisionDrawer.SetActive(true);
            RefreshDecisionDrawer();
            tweenRunner.MoveTo(decisionDrawerRect, FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerOpenPosition, 0.28f);
        }

        public void CloseDecisionDrawer()
        {
            RefreshDecisionDrawer();
            tweenRunner.MoveTo(decisionDrawerRect, FirstPlayableMainSceneRectSpec.DecisionDrawerOpenPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, 0.22f);
        }

        public void RenderResult(FirstPlayableResultScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            premiumText.text = state.Premium;
            resultText.text = state.Result;
            resultText.color = state.AuditCorrect ? theme.Correct : theme.Reject;
            outcomeText.text = state.Outcome;
            outcomeText.color = state.OutcomeWarning ? theme.Warning : theme.Ink;
            settlementText.text = state.Settlement;
            settlementText.color = theme.MutedInk;

            decisionBox.SetActive(false);
            decisionDrawer.SetActive(true);
            decisionDrawerRect.anchoredPosition = FirstPlayableMainSceneRectSpec.DecisionDrawerOpenPosition;
            SetDecisionButtonsInteractable(false);
            nextRow.SetActive(true);
            nextButton.interactable = true;
            nextButtonText.text = state.NextButton;
            resultAwaitingDocumentClick = true;
            PlayCharacterExit();
        }

        public void RenderComplete(FirstPlayableCompleteScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ClearReviewRoots();
            ruleToggles.Clear();
            ruleRows.Clear();
            ruleSeverities.Clear();

            entryTitleText.text = state.Result;
            entryPromptText.text = state.Outcome + "\n" + state.Settlement;
            entryButton.interactable = false;
            entryBellButton.interactable = false;
            entryRoot.SetActive(true);
            reviewRoot.SetActive(false);
            decisionDrawer.SetActive(false);
        }

        private void BuildTopStrip(Transform parent)
        {
            var header = factory.CreateAnchoredPanel(
                parent,
                "Top Date Status Strip",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                FirstPlayableMainSceneRectSpec.HeaderPosition,
                FirstPlayableMainSceneRectSpec.HeaderSize,
                theme.TopStrip,
                false,
                factory.Assets.PanelSprite);
            var layout = header.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(26, 26, 10, 10);
            layout.spacing = 18;
            layout.childForceExpandWidth = false;
            layout.childAlignment = TextAnchor.MiddleLeft;

            var letter = factory.CreateSpriteImage(header.transform, "Letter Prop Mark", factory.Assets.MainLetterSprite ?? factory.Assets.ShipSprite, theme.Gold, true);
            var letterElement = letter.GetComponent<LayoutElement>();
            letterElement.preferredWidth = 74;
            letterElement.preferredHeight = 54;

            hudTitleText = factory.CreateText(header.transform, "Office Title", theme.TitleFont, 30, FontStyle.Bold, theme.LightText, TextAnchor.MiddleLeft);
            hudTitleText.text = FirstPlayableKoreanText.OfficeTitle;
            hudTitleText.GetComponent<LayoutElement>().preferredWidth = 620;

            hudDateText = factory.CreateText(header.transform, "Header Date", theme.TitleFont, 24, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            hudDateText.GetComponent<LayoutElement>().flexibleWidth = 1;

            hudLedgerText = factory.CreateText(header.transform, "Header Ledger", theme.BodyFont, 18, FontStyle.Bold, theme.MutedText, TextAnchor.MiddleRight);
            hudLedgerText.GetComponent<LayoutElement>().preferredWidth = 250;
        }

        private void BuildEntry(Transform parent)
        {
            var contractor = factory.CreateAnchoredPanel(parent, "Entry Contractor Preview", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryContractorPosition, FirstPlayableMainSceneRectSpec.EntryContractorSize, Color.white, false, factory.Assets.MainContractorSprite ?? factory.Assets.ShipSprite);
            entryContractorRect = contractor.GetComponent<RectTransform>();
            entryContractorCanvasGroup = contractor.AddComponent<CanvasGroup>();
            entryContractorImage = contractor.GetComponent<Image>();
            entryContractorImage.preserveAspect = true;

            var bell = factory.CreateAnchoredPanel(parent, "Entry Bell", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryBellPosition, FirstPlayableMainSceneRectSpec.EntryBellSize, Color.white, true, factory.Assets.MainBellSprite ?? factory.Assets.MainSmallButtonSprite);
            entryBellButton = bell.AddComponent<Button>();
            entryBellButton.targetGraphic = bell.GetComponent<Image>();
            theme.ApplyButton(entryBellButton, FirstPlayableButtonTone.Neutral);
            entryBellButton.onClick.AddListener(() => ReviewStarted?.Invoke());

            var panel = factory.CreateAnchoredPanel(parent, "Clickable Entry Contract", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryDocumentPosition, FirstPlayableMainSceneRectSpec.EntryDocumentSize, theme.Paper, true, factory.Assets.MainPaperSprite ?? factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite);
            entryButton = panel.AddComponent<Button>();
            entryButton.targetGraphic = panel.GetComponent<Image>();
            theme.ApplyButton(entryButton, FirstPlayableButtonTone.Neutral);
            entryButton.onClick.AddListener(() => ReviewStarted?.Invoke());

            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(34, 34, 34, 34);
            layout.spacing = 14;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            entryTitleText = factory.CreateText(panel.transform, "Entry Title", theme.TitleFont, 24, FontStyle.Bold, theme.Ink, TextAnchor.MiddleCenter);
            entryTitleText.GetComponent<LayoutElement>().preferredHeight = 96;
            entryPromptText = factory.CreateText(panel.transform, "Entry Prompt", theme.BodyFont, 14, FontStyle.Bold, theme.MutedInk, TextAnchor.MiddleCenter);
            entryPromptText.GetComponent<LayoutElement>().preferredHeight = 72;
        }

        private void BuildCharacterDialogue(Transform parent)
        {
            var contractor = factory.CreateAnchoredPanel(parent, "Contractor Visual", new Vector2(0f, 0f), new Vector2(0f, 0f), FirstPlayableMainSceneRectSpec.ContractorOpenPosition, FirstPlayableMainSceneRectSpec.ContractorSize, Color.white, false, factory.Assets.MainContractorSprite ?? factory.Assets.ShipSprite);
            contractorRect = contractor.GetComponent<RectTransform>();
            contractorCanvasGroup = contractor.AddComponent<CanvasGroup>();
            contractorImage = contractor.GetComponent<Image>();
            contractorImage.preserveAspect = true;
            contractorImage.color = factory.Assets.MainContractorSprite != null ? Color.white : theme.Gold;

            var dialogue = factory.CreateAnchoredPanel(parent, "Dialogue Panel", new Vector2(0f, 1f), new Vector2(0f, 1f), FirstPlayableMainSceneRectSpec.DialogueOpenPosition, FirstPlayableMainSceneRectSpec.DialogueSize, theme.Paper, true, factory.Assets.MainSpeechBubbleSprite ?? factory.Assets.MainPaperSprite);
            dialogueRect = dialogue.GetComponent<RectTransform>();
            var button = dialogue.AddComponent<Button>();
            button.targetGraphic = dialogue.GetComponent<Image>();
            theme.ApplyButton(button, FirstPlayableButtonTone.Neutral);
            button.onClick.AddListener(AdvanceDialogue);
            var layout = dialogue.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(34, 34, 28, 24);
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;
            dialogueText = factory.CreateText(dialogue.transform, "Dialogue Text", theme.BodyFont, 24, FontStyle.Bold, theme.Ink, TextAnchor.MiddleLeft);
            dialogueText.text = FirstPlayableKoreanText.DialogueReviewIntro;
        }

        private void BuildWorkbench(Transform parent)
        {
            var workbench = factory.CreateAnchoredPanel(parent, "Central Workbench Surface", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.WorkbenchOpenPosition, FirstPlayableMainSceneRectSpec.WorkbenchSize, theme.WorkPanel, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.PanelSprite);
            workbenchRect = workbench.GetComponent<RectTransform>();
            var layout = workbench.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(22, 22, 18, 20);
            layout.spacing = 12;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var shipRow = new GameObject("Ship And Documents Header");
            shipRow.transform.SetParent(workbench.transform, false);
            shipRow.AddComponent<LayoutElement>().preferredHeight = 120;
            var shipLayout = shipRow.AddComponent<HorizontalLayoutGroup>();
            shipLayout.spacing = 12;
            shipLayout.childForceExpandWidth = false;
            shipLayout.childAlignment = TextAnchor.MiddleLeft;

            var ship = factory.CreateSpriteImage(shipRow.transform, "Workbench Ship", factory.Assets.ShipSprite, theme.Gold, true);
            var shipElement = ship.GetComponent<LayoutElement>();
            shipElement.preferredWidth = 132;
            shipElement.preferredHeight = 82;

            var heading = factory.CreateText(shipRow.transform, "Document Workbench Title", theme.TitleFont, 24, FontStyle.Bold, theme.LightText, TextAnchor.MiddleLeft);
            heading.text = FirstPlayableKoreanText.DocumentBundleTitle;
            heading.GetComponent<LayoutElement>().flexibleWidth = 1;

            var letter = factory.CreateSpriteImage(shipRow.transform, "Workbench Letter", factory.Assets.MainLetterSprite, theme.Paper, true);
            var letterElement = letter.GetComponent<LayoutElement>();
            letterElement.preferredWidth = 112;
            letterElement.preferredHeight = 78;

            var documentBoard = factory.CreatePanel(workbench.transform, "Document Clamp Board", theme.PaperInset, true);
            var boardElement = documentBoard.AddComponent<LayoutElement>();
            boardElement.preferredHeight = FirstPlayableMainSceneRectSpec.DocumentBoardSize.y;
            boardElement.flexibleHeight = 1;
            documentBoardRect = documentBoard.GetComponent<RectTransform>();
            documentBoardButton = documentBoard.AddComponent<Button>();
            documentBoardButton.targetGraphic = documentBoard.GetComponent<Image>();
            theme.ApplyButton(documentBoardButton, FirstPlayableButtonTone.Neutral);
            documentBoardButton.onClick.AddListener(OnDocumentBoardClicked);
            documentRoot = documentBoard.transform;
        }

        private void BuildShelfRules(Transform parent)
        {
            var panel = factory.CreateAnchoredPanel(parent, "Right Shelf Checklist", new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), FirstPlayableMainSceneRectSpec.ShelfOpenPosition, FirstPlayableMainSceneRectSpec.ShelfSize, theme.WorkPanelAlt, true, factory.Assets.PanelSprite);
            shelfRect = panel.GetComponent<RectTransform>();
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 18, 16);
            layout.spacing = 8;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            reviewStatusText = factory.CreateText(panel.transform, "Review Status", theme.BodyFont, 14, FontStyle.Bold, theme.LightText, TextAnchor.UpperLeft);
            reviewStatusText.GetComponent<LayoutElement>().preferredHeight = 38;
            BuildRuleSection(panel.transform, FirstPlayableKoreanText.AbsoluteRuleSectionTitle, out absoluteRulesRoot);
            BuildRuleSection(panel.transform, FirstPlayableKoreanText.ConsiderationRuleSectionTitle, out considerationRulesRoot);
        }

        private void BuildRuleSection(Transform parent, string title, out Transform root)
        {
            factory.CreateSectionTitle(parent, title);
            var scroll = factory.CreateScroll(parent, title + " Scroll", theme.PaperInset, out root);
            var element = scroll.gameObject.AddComponent<LayoutElement>();
            element.preferredHeight = title == FirstPlayableKoreanText.AbsoluteRuleSectionTitle ? 280f : 360f;
            element.flexibleHeight = 1;
        }

        private void BuildDecisionBox(Transform parent)
        {
            decisionBox = factory.CreateAnchoredPanel(parent, "Final Decision Box", new Vector2(1f, 0f), new Vector2(1f, 0f), FirstPlayableMainSceneRectSpec.FinalDecisionBoxPosition, FirstPlayableMainSceneRectSpec.FinalDecisionBoxSize, Color.white, true, factory.Assets.MainBellSprite ?? factory.Assets.MainSmallButtonSprite);
            decisionBoxButton = decisionBox.AddComponent<Button>();
            decisionBoxButton.targetGraphic = decisionBox.GetComponent<Image>();
            theme.ApplyButton(decisionBoxButton, FirstPlayableButtonTone.Neutral);
            decisionBoxButton.onClick.AddListener(() => DecisionDrawerRequested?.Invoke());

            var layout = decisionBox.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 12, 10);
            layout.spacing = 5;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var title = factory.CreateText(decisionBox.transform, "Decision Box Title", theme.TitleFont, 14, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            title.text = FirstPlayableKoreanText.DecisionBoxTitle;
            title.GetComponent<LayoutElement>().preferredHeight = 28;
            var prompt = factory.CreateText(decisionBox.transform, "Decision Box Prompt", theme.BodyFont, 10, FontStyle.Bold, theme.MutedText, TextAnchor.MiddleCenter);
            prompt.text = FirstPlayableKoreanText.DecisionBoxPrompt;
            prompt.GetComponent<LayoutElement>().preferredHeight = 32;
        }

        private void BuildDecisionDrawer(Transform parent)
        {
            decisionDrawer = factory.CreateAnchoredPanel(parent, "Bottom Right Decision Paper", new Vector2(1f, 0f), new Vector2(1f, 0f), FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerSize, theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            decisionDrawerRect = decisionDrawer.GetComponent<RectTransform>();
            var layout = decisionDrawer.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 16, 16);
            layout.spacing = 14;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            var resultColumn = new GameObject("Result Column");
            resultColumn.transform.SetParent(decisionDrawer.transform, false);
            resultColumn.AddComponent<LayoutElement>().flexibleWidth = 2;
            var resultLayout = resultColumn.AddComponent<VerticalLayoutGroup>();
            resultLayout.spacing = 7;
            resultLayout.childForceExpandWidth = true;
            resultLayout.childForceExpandHeight = false;

            var drawerTitle = factory.CreateText(resultColumn.transform, "Decision Drawer Title", theme.TitleFont, 20, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            drawerTitle.text = FirstPlayableKoreanText.DecisionDrawerTitle;
            drawerTitle.GetComponent<LayoutElement>().preferredHeight = 30;

            var summaryPaper = factory.CreatePanel(resultColumn.transform, "Selected Reason Paper", Color.white, true, factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            summaryPaper.AddComponent<LayoutElement>().preferredHeight = 92;
            var summaryLayout = summaryPaper.AddComponent<VerticalLayoutGroup>();
            summaryLayout.padding = new RectOffset(12, 12, 10, 14);
            summaryLayout.spacing = 5;
            summaryLayout.childForceExpandWidth = true;
            summaryLayout.childForceExpandHeight = false;
            drawerPremiumText = factory.CreateText(summaryPaper.transform, "Drawer Premium", theme.TitleFont, 17, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            drawerSummaryText = factory.CreateText(summaryPaper.transform, "Drawer Summary", theme.BodyFont, 14, FontStyle.Bold, theme.MutedInk, TextAnchor.UpperLeft);
            premiumText = factory.CreateText(resultColumn.transform, "Premium Quote", theme.TitleFont, 17, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            resultText = factory.CreateText(resultColumn.transform, "Audit Result", theme.BodyFont, 14, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            outcomeText = factory.CreateText(resultColumn.transform, "Outcome Result", theme.BodyFont, 14, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            settlementText = factory.CreateText(resultColumn.transform, "Settlement Result", theme.BodyFont, 13, FontStyle.Bold, theme.MutedInk, TextAnchor.UpperLeft);
            drawerPremiumText.GetComponent<LayoutElement>().preferredHeight = 26;
            drawerSummaryText.GetComponent<LayoutElement>().preferredHeight = 24;
            premiumText.GetComponent<LayoutElement>().preferredHeight = 26;
            resultText.GetComponent<LayoutElement>().preferredHeight = 70;
            outcomeText.GetComponent<LayoutElement>().preferredHeight = 42;
            settlementText.GetComponent<LayoutElement>().preferredHeight = 52;

            var actionColumn = new GameObject("Action Column");
            actionColumn.transform.SetParent(decisionDrawer.transform, false);
            actionColumn.AddComponent<LayoutElement>().preferredWidth = 245;
            var actionLayout = actionColumn.AddComponent<VerticalLayoutGroup>();
            actionLayout.spacing = 10;
            actionLayout.childForceExpandWidth = true;
            actionLayout.childForceExpandHeight = false;

            var prompt = factory.CreateText(actionColumn.transform, "Decision Drawer Prompt", theme.BodyFont, 14, FontStyle.Bold, theme.MutedInk, TextAnchor.MiddleCenter);
            prompt.text = FirstPlayableKoreanText.DecisionDrawerPrompt;
            prompt.GetComponent<LayoutElement>().preferredHeight = 42;

            decisionRow = new GameObject("Decision Buttons");
            decisionRow.transform.SetParent(actionColumn.transform, false);
            var decisionLayout = decisionRow.AddComponent<HorizontalLayoutGroup>();
            decisionLayout.spacing = 10;
            decisionLayout.childForceExpandWidth = false;
            decisionLayout.childAlignment = TextAnchor.MiddleCenter;
            rejectButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.RejectButton, FirstPlayableButtonTone.Reject, () => Submit(PlayerDecision.Reject));
            approveButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.ApproveButton, FirstPlayableButtonTone.Primary, () => Submit(PlayerDecision.Approve));
            ApplyDecisionButtonSize(rejectButton);
            ApplyDecisionButtonSize(approveButton);
            drawerApproveText = approveButton.GetComponentInChildren<Text>();

            nextRow = new GameObject("Continue Row");
            nextRow.transform.SetParent(actionColumn.transform, false);
            nextButton = factory.CreateButton(nextRow.transform, FirstPlayableKoreanText.NextContractButton, FirstPlayableButtonTone.Neutral, () => ResultAcknowledged?.Invoke());
            nextButtonText = nextButton.GetComponentInChildren<Text>();
        }

        private void CreateDocumentCard(Transform parent, DocumentRecord document, int index)
        {
            var sprite = factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite ?? factory.Assets.DocumentTileSprite;
            var position = FirstPlayableMainSceneRectSpec.DocumentPositions[index % FirstPlayableMainSceneRectSpec.DocumentPositions.Length];
            var submittedSize = FirstPlayableMainSceneRectSpec.DocumentSizes[index % FirstPlayableMainSceneRectSpec.DocumentSizes.Length];
            var size = document.Submitted ? submittedSize : new Vector2(submittedSize.x, Mathf.Min(submittedSize.y, 150f));
            var card = factory.CreateAnchoredPanel(parent, FirstPlayableKoreanText.DocumentTitle(document), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), position, size, theme.Paper, true, sprite);
            var drag = card.AddComponent<FirstPlayableDragController>();
            drag.Initialize(documentBoardRect);
            var layout = card.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 12, 12);
            layout.spacing = 5;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var title = factory.CreateText(card.transform, "Document Title", theme.TitleFont, 17, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            title.text = FirstPlayableKoreanText.DocumentTitle(document);
            title.GetComponent<LayoutElement>().preferredHeight = 30;

            if (!document.Submitted)
            {
                var missing = factory.CreateText(card.transform, "Missing Message", theme.BodyFont, 14, FontStyle.Bold, theme.Reject, TextAnchor.UpperLeft);
                missing.text = FirstPlayableKoreanText.MissingDocumentMessage();
                return;
            }

            foreach (var field in document.Fields.Take(4))
            {
                var fieldText = factory.CreateText(card.transform, field.Id.ToString(), theme.BodyFont, 13, FontStyle.Normal, theme.Ink, TextAnchor.UpperLeft);
                fieldText.text = $"{FirstPlayableKoreanText.FieldLabel(field)}: {FirstPlayableKoreanText.FieldValue(field.Value)}";
            }
        }

        private void CreateRuleRow(Transform parent, FirstPlayableRuleRowState rowState)
        {
            var rule = rowState.Rule;
            var row = factory.CreatePanel(parent, rule.Id.ToString(), theme.DisabledRow, true, factory.Assets.MainPaperSprite);
            row.AddComponent<LayoutElement>().minHeight = FirstPlayableMainSceneRectSpec.RuleRowHeight;
            var image = row.GetComponent<Image>();
            ruleRows[rule.Id] = image;
            ruleSeverities[rule.Id] = rule.Severity;

            var toggle = row.AddComponent<Toggle>();
            toggle.targetGraphic = image;
            theme.ApplyToggle(toggle);
            ruleToggles[rule.Id] = toggle;

            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.spacing = 10;
            layout.childForceExpandWidth = false;
            layout.childAlignment = TextAnchor.MiddleLeft;

            var marker = factory.CreatePanel(row.transform, "Severity Marker", RuleColor(rule), false);
            marker.AddComponent<LayoutElement>().preferredWidth = 46;
            var markerText = factory.CreateText(marker.transform, "Severity", theme.TitleFont, 15, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            markerText.text = FirstPlayableKoreanText.RuleSeverityMarker(rule.Severity);

            var check = factory.CreatePanel(row.transform, "Check", theme.Paper, true);
            var checkElement = check.AddComponent<LayoutElement>();
            checkElement.preferredWidth = 28;
            checkElement.preferredHeight = 28;
            var checkFill = factory.CreatePanel(check.transform, "Check Fill", theme.Gold, false);
            var checkFillRect = checkFill.GetComponent<RectTransform>();
            checkFillRect.anchorMin = new Vector2(0.22f, 0.22f);
            checkFillRect.anchorMax = new Vector2(0.78f, 0.78f);
            checkFillRect.offsetMin = Vector2.zero;
            checkFillRect.offsetMax = Vector2.zero;
            toggle.graphic = checkFill.GetComponent<Image>();

            var labelColumn = new GameObject("Rule Text");
            labelColumn.transform.SetParent(row.transform, false);
            labelColumn.AddComponent<LayoutElement>().flexibleWidth = 1;
            var textLayout = labelColumn.AddComponent<VerticalLayoutGroup>();
            textLayout.spacing = 3;
            textLayout.childForceExpandWidth = true;
            textLayout.childForceExpandHeight = false;

            var title = factory.CreateText(labelColumn.transform, "Title", theme.TitleFont, 14, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            title.text = FirstPlayableKoreanText.RuleTitle(rule);
            var detail = factory.CreateText(labelColumn.transform, "Detail", theme.BodyFont, 12, FontStyle.Bold, theme.MutedInk, TextAnchor.UpperLeft);
            detail.text = FirstPlayableKoreanText.RuleFindingText(rowState.IsTriggered);

            toggle.onValueChanged.AddListener(_ =>
            {
                RefreshRuleRow(rule.Id);
                RefreshDecisionDrawer();
            });
            RefreshRuleRow(rule.Id);
        }

        private void Submit(PlayerDecision decision)
        {
            var submittedDecision = decision == PlayerDecision.Approve && CountSelectedConsiderations() > 0
                ? PlayerDecision.ConditionalApprove
                : decision;
            DecisionSubmitted?.Invoke(submittedDecision, ruleToggles.Where(pair => pair.Value.isOn).Select(pair => pair.Key).ToList());
        }

        private void OnDocumentBoardClicked()
        {
            if (resultAwaitingDocumentClick)
            {
                resultAwaitingDocumentClick = false;
                ResultAcknowledged?.Invoke();
                return;
            }

            ReviewClosedRequested?.Invoke();
        }

        private void SetDialogueLines(IReadOnlyList<string> lines)
        {
            dialogueLines = lines != null && lines.Count > 0
                ? lines
                : new[] { FirstPlayableKoreanText.DialogueReviewIntro };
            dialogueIndex = 0;
            dialogueText.text = dialogueLines[dialogueIndex];
        }

        private void AdvanceDialogue()
        {
            if (dialogueLines == null || dialogueLines.Count == 0)
            {
                return;
            }

            dialogueIndex = Mathf.Min(dialogueIndex + 1, dialogueLines.Count - 1);
            dialogueText.text = dialogueLines[dialogueIndex];
        }

        private void ResetEntryContractorPreview()
        {
            if (entryContractorRect != null)
            {
                entryContractorRect.localScale = new Vector3(0.82f, 0.82f, 1f);
            }

            if (entryContractorCanvasGroup != null)
            {
                entryContractorCanvasGroup.alpha = 0.62f;
            }

            if (entryContractorImage != null)
            {
                entryContractorImage.color = factory.Assets.MainContractorSprite != null
                    ? new Color(0.72f, 0.68f, 0.62f, 1f)
                    : theme.Gold;
            }
        }

        private void PlayCharacterExit()
        {
            tweenRunner.MoveTo(contractorRect, FirstPlayableMainSceneRectSpec.ContractorOpenPosition, FirstPlayableMainSceneRectSpec.ContractorClosedPosition, 0.32f);
            tweenRunner.ScaleTo(contractorRect, Vector3.one, new Vector3(0.58f, 0.58f, 1f), 0.32f);
            tweenRunner.FadeTo(contractorCanvasGroup, 1f, 0.25f, 0.32f);
            tweenRunner.TintTo(contractorImage, contractorImage != null ? contractorImage.color : Color.white, new Color(0.34f, 0.3f, 0.28f, 0.9f), 0.32f);
        }

        private void PlayReviewOpening()
        {
            tweenRunner.MoveTo(contractorRect, FirstPlayableMainSceneRectSpec.ContractorClosedPosition, FirstPlayableMainSceneRectSpec.ContractorOpenPosition, 0.32f);
            tweenRunner.MoveTo(dialogueRect, FirstPlayableMainSceneRectSpec.DialogueClosedPosition, FirstPlayableMainSceneRectSpec.DialogueOpenPosition, 0.36f);
            tweenRunner.MoveTo(workbenchRect, FirstPlayableMainSceneRectSpec.WorkbenchClosedPosition, FirstPlayableMainSceneRectSpec.WorkbenchOpenPosition, 0.4f);
            tweenRunner.MoveTo(shelfRect, FirstPlayableMainSceneRectSpec.ShelfClosedPosition, FirstPlayableMainSceneRectSpec.ShelfOpenPosition, 0.4f);
            tweenRunner.ScaleTo(contractorRect, new Vector3(0.58f, 0.58f, 1f), Vector3.one, 0.42f);
            tweenRunner.FadeTo(contractorCanvasGroup, 0.35f, 1f, 0.42f);
            tweenRunner.TintTo(contractorImage, new Color(0.38f, 0.34f, 0.3f, 1f), factory.Assets.MainContractorSprite != null ? Color.white : theme.Gold, 0.42f);
            decisionDrawerRect.anchoredPosition = FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition;
        }

        private static void ApplyDecisionButtonSize(Button button)
        {
            if (button == null)
            {
                return;
            }

            var layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = button.gameObject.AddComponent<LayoutElement>();
            }

            layoutElement.preferredWidth = FirstPlayableMainSceneRectSpec.DecisionButtonSize.x;
            layoutElement.preferredHeight = FirstPlayableMainSceneRectSpec.DecisionButtonSize.y;
        }

        private void SetDecisionButtonsInteractable(bool interactable)
        {
            decisionRow.SetActive(interactable);
            approveButton.interactable = interactable;
            rejectButton.interactable = interactable;

            foreach (var toggle in ruleToggles.Values)
            {
                toggle.interactable = interactable;
            }
        }

        private void RefreshRuleRow(RuleId id)
        {
            if (!ruleRows.TryGetValue(id, out var row) || !ruleToggles.TryGetValue(id, out var toggle))
            {
                return;
            }

            row.color = toggle.isOn ? theme.SelectedRow : theme.DisabledRow;
        }

        private void RefreshDecisionDrawer()
        {
            var selectedAbsolute = CountSelectedAbsoluteRejections();
            var selectedConsiderations = CountSelectedConsiderations();
            if (drawerPremiumText != null)
            {
                drawerPremiumText.text = FirstPlayableKoreanText.MainLoopPremium(selectedConsiderations);
            }

            if (drawerSummaryText != null)
            {
                drawerSummaryText.text = FirstPlayableKoreanText.SelectedReasonSummary(selectedAbsolute, selectedConsiderations);
            }

            if (drawerApproveText != null)
            {
                drawerApproveText.text = FirstPlayableMainLoopState.ApprovalLabelForSelectedConsiderations(selectedConsiderations);
            }
        }

        private int CountSelectedAbsoluteRejections()
        {
            return CountSelectedRules(RuleSeverity.AbsoluteRejection);
        }

        private int CountSelectedConsiderations()
        {
            return CountSelectedRules(RuleSeverity.RejectionConsideration);
        }

        private int CountSelectedRules(RuleSeverity severity)
        {
            var count = 0;
            foreach (var pair in ruleToggles)
            {
                if (pair.Value.isOn && ruleSeverities.TryGetValue(pair.Key, out var ruleSeverity) && ruleSeverity == severity)
                {
                    count++;
                }
            }

            return count;
        }

        private Color RuleColor(RuleDefinition rule)
        {
            return rule.Severity == RuleSeverity.AbsoluteRejection ? theme.Reject : theme.Consideration;
        }

        private void ClearReviewRoots()
        {
            if (documentRoot != null)
            {
                ClearRoot(documentRoot);
            }

            if (absoluteRulesRoot != null)
            {
                ClearRoot(absoluteRulesRoot);
            }

            if (considerationRulesRoot != null)
            {
                ClearRoot(considerationRulesRoot);
            }
        }

        private static GameObject CreateRoot(Transform parent, string name)
        {
            var root = new GameObject(name, typeof(RectTransform));
            root.transform.SetParent(parent, false);
            var rect = root.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return root;
        }

        private static void ClearRoot(Transform root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            foreach (Transform child in root)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
    }
}
