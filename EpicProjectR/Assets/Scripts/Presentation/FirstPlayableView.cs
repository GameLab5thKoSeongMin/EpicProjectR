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
        private Text reviewStatusText;
        private Text premiumText;
        private Text resultText;
        private Text outcomeText;
        private Text settlementText;
        private Text nextButtonText;
        private Text drawerApproveText;
        private Text drawerCompensationText;
        private Text drawerPremiumText;
        private Transform documentRoot;
        private Transform absoluteRulesRoot;
        private Transform considerationRulesRoot;
        private GameObject absoluteRulesSection;
        private GameObject considerationRulesSection;
        private RectTransform documentBoardRect;
        private RectTransform workbenchRect;
        private RectTransform shelfRect;
        private RectTransform dialogueRect;
        private RectTransform contractorRect;
        private RectTransform entryContractorRect;
        private RectTransform decisionDrawerRect;
        private Button entryButton;
        private Button entryBellButton;
        private Button reviewDocumentButton;
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
        private GameObject reviewDocumentObject;
        private GameObject reviewBellObject;
        private Image contractorImage;
        private CanvasGroup contractorCanvasGroup;
        private Image entryContractorImage;
        private CanvasGroup entryContractorCanvasGroup;
        private CanvasGroup entryBellCanvasGroup;
        private CanvasGroup reviewBellCanvasGroup;
        private ScrollRect dialogueScroll;
        private Transform dialogueContentRoot;
        private IReadOnlyList<string> dialogueLines = Array.Empty<string>();
        private int dialogueIndex;
        private int decisionDrawerTweenGeneration;
        private string currentCompensationText = string.Empty;

        public FirstPlayableView(FirstPlayableUiTheme theme, FirstPlayableUiFactory factory)
        {
            this.theme = theme ?? throw new ArgumentNullException(nameof(theme));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            if (factory.Assets == null)
            {
                throw new ArgumentException("FirstPlayableUiFactory must expose a non-null asset catalog.", nameof(factory));
            }
        }

        public event Action BellRang;
        public event Action PresentedDocumentClicked;
        public event Action CharacterArrivalCompleted;
        public event Action CharacterExitCompleted;
        public event Action DecisionDrawerRequested;
        public event Action<PlayerDecision, IReadOnlyList<RuleId>> DecisionSubmitted;

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
            if (backgroundImage.sprite != null)
            {
                backgroundImage.color = Color.white;
            }

            factory.CreatePanel(background.transform, "Main Scene Dark Overlay", theme.MainOverlay, false);
            entryRoot = CreateRoot(background.transform, "Entry Contract Root");
            reviewRoot = CreateRoot(background.transform, "Review Sequence Root");
            BuildEntry(entryRoot.transform);
            BuildCharacterDialogue(reviewRoot.transform);
            BuildWorkbench(reviewRoot.transform);
            BuildShelfRules(reviewRoot.transform);
            BuildDecisionBox(reviewRoot.transform);
            BuildDecisionDrawer(reviewRoot.transform);
            BuildTopStrip(background.transform);
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
            entryButton.interactable = false;
            entryBellButton.interactable = true;
            ResetEntryComposition(showCharacter: false, documentInteractable: false, bellInteractable: true);
            entryRoot.SetActive(true);
            reviewRoot.SetActive(false);
            decisionDrawer.SetActive(false);
        }

        public void PlayEntryCharacterArrival()
        {
            ResetEntryComposition(showCharacter: true, documentInteractable: false, bellInteractable: false);
            tweenRunner.MoveTo(entryContractorRect, FirstPlayableMainSceneRectSpec.EntryContractorStartPosition, FirstPlayableMainSceneRectSpec.EntryContractorPosition, 0.46f);
            tweenRunner.ScaleTo(entryContractorRect, new Vector3(0.42f, 0.42f, 1f), Vector3.one, 0.46f);
            tweenRunner.FadeTo(entryContractorCanvasGroup, 0.12f, 1f, 0.46f);
            tweenRunner.TintTo(entryContractorImage, new Color(0.44f, 0.40f, 0.35f, 1f), factory.Assets.MainContractorSprite != null ? Color.white : theme.Gold, 0.46f);
            tweenRunner.InvokeAfter(0.48f, () => CharacterArrivalCompleted?.Invoke());
        }

        public void RenderDocumentPresented()
        {
            ClearReviewRoots();
            entryPromptText.text = FirstPlayableKoreanText.DocumentPresentedPrompt;
            entryRoot.SetActive(true);
            reviewRoot.SetActive(false);
            decisionDrawer.SetActive(false);
            ResetEntryComposition(showCharacter: true, documentInteractable: true, bellInteractable: false);
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

            reviewStatusText.text = state.ReviewStatus;
            currentCompensationText = state.Compensation;
            if (drawerCompensationText != null)
            {
                drawerCompensationText.text = currentCompensationText;
            }

            if (premiumText != null)
            {
                premiumText.text = FirstPlayableKoreanText.MainLoopPremium(0);
            }

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

            absoluteRulesSection.SetActive(absoluteRulesRoot.childCount > 0);
            considerationRulesSection.SetActive(considerationRulesRoot.childCount > 0);
            SetDecisionButtonsInteractable(true);
            nextRow.SetActive(false);
            RefreshDecisionDrawer();
            PlayReviewOpening();
        }

        public void OpenDecisionDrawer()
        {
            decisionDrawerTweenGeneration++;
            decisionDrawer.SetActive(true);
            RefreshDecisionDrawer();
            tweenRunner.MoveTo(decisionDrawerRect, FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerOpenPosition, 0.28f);
        }

        public void CloseDecisionDrawer()
        {
            decisionDrawerTweenGeneration++;
            var closeGeneration = decisionDrawerTweenGeneration;
            RefreshDecisionDrawer();
            tweenRunner.MoveTo(decisionDrawerRect, FirstPlayableMainSceneRectSpec.DecisionDrawerOpenPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, 0.22f);
            tweenRunner.InvokeAfter(0.24f, () =>
            {
                if (decisionDrawerTweenGeneration == closeGeneration)
                {
                    decisionDrawer.SetActive(false);
                }
            });
        }

        public void RenderResult(FirstPlayableResultScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            AppendDialogueLine(state.AuditCorrect || !state.OutcomeWarning
                ? FirstPlayableKoreanText.ContractorThanksLine
                : FirstPlayableKoreanText.ContractorRejectLine);
            decisionBox.SetActive(false);
            decisionDrawer.SetActive(false);
            reviewDocumentObject.SetActive(false);
            SetDecisionButtonsInteractable(false);
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
            ResetEntryComposition(showCharacter: false, documentInteractable: false, bellInteractable: false);
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
            var letter = factory.CreateAnchoredPanel(
                header.transform,
                "Letter Prop Mark",
                new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f),
                new Vector2(58f, 0f),
                new Vector2(74f, 54f),
                theme.Gold,
                false,
                factory.Assets.MainLetterSprite ?? factory.Assets.ShipSprite);
            letter.GetComponent<Image>().preserveAspect = true;

            hudTitleText = factory.CreateText(header.transform, "Office Title", theme.TitleFont, 28, FontStyle.Bold, theme.LightText, TextAnchor.MiddleLeft);
            hudTitleText.text = FirstPlayableKoreanText.OfficeTitle;
            hudTitleText.resizeTextForBestFit = true;
            hudTitleText.resizeTextMinSize = 20;
            hudTitleText.resizeTextMaxSize = 28;
            SetTextRect(hudTitleText, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(430f, 0f), new Vector2(650f, 0f));

            hudDateText = factory.CreateText(header.transform, "Header Date", theme.TitleFont, 24, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            hudDateText.resizeTextForBestFit = true;
            hudDateText.resizeTextMinSize = 18;
            hudDateText.resizeTextMaxSize = 24;
            SetTextRect(hudDateText, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(420f, 0f));

            hudLedgerText = factory.CreateText(header.transform, "Header Ledger", theme.BodyFont, 18, FontStyle.Bold, theme.MutedText, TextAnchor.MiddleRight);
            hudLedgerText.resizeTextForBestFit = true;
            hudLedgerText.resizeTextMinSize = 14;
            hudLedgerText.resizeTextMaxSize = 18;
            SetTextRect(hudLedgerText, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-174f, 0f), new Vector2(320f, 0f));
        }

        private void BuildEntry(Transform parent)
        {
            var contractor = factory.CreateAnchoredPanel(parent, "Entry Contractor Preview", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryContractorPosition, FirstPlayableMainSceneRectSpec.EntryContractorSize, Color.white, false, factory.Assets.MainContractorSprite ?? factory.Assets.ShipSprite);
            entryContractorRect = contractor.GetComponent<RectTransform>();
            entryContractorCanvasGroup = contractor.AddComponent<CanvasGroup>();
            entryContractorImage = contractor.GetComponent<Image>();
            entryContractorImage.preserveAspect = true;

            var bell = factory.CreateAnchoredPanel(parent, "Entry Bell", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryBellPosition, FirstPlayableMainSceneRectSpec.EntryBellSize, Color.white, true, factory.Assets.MainBellSprite ?? factory.Assets.MainSmallButtonSprite);
            bell.GetComponent<Image>().preserveAspect = true;
            entryBellCanvasGroup = bell.AddComponent<CanvasGroup>();
            entryBellButton = bell.AddComponent<Button>();
            entryBellButton.targetGraphic = bell.GetComponent<Image>();
            theme.ApplyButton(entryBellButton, FirstPlayableButtonTone.Neutral);
            entryBellButton.onClick.AddListener(() => BellRang?.Invoke());

            var panel = factory.CreateAnchoredPanel(parent, "Clickable Entry Contract", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), FirstPlayableMainSceneRectSpec.EntryDocumentPosition, FirstPlayableMainSceneRectSpec.EntryDocumentSize, theme.Paper, true, factory.Assets.MainPaperSprite ?? factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite);
            panel.GetComponent<Image>().preserveAspect = true;
            entryButton = panel.AddComponent<Button>();
            entryButton.targetGraphic = panel.GetComponent<Image>();
            theme.ApplyButton(entryButton, FirstPlayableButtonTone.Neutral);
            entryButton.onClick.AddListener(() => PresentedDocumentClicked?.Invoke());

            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 16, 14);
            layout.spacing = 6;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            entryTitleText = factory.CreateText(panel.transform, "Entry Title", theme.TitleFont, 15, FontStyle.Bold, theme.Ink, TextAnchor.MiddleCenter);
            entryTitleText.GetComponent<LayoutElement>().preferredHeight = 42;
            entryPromptText = factory.CreateText(panel.transform, "Entry Prompt", theme.BodyFont, 10, FontStyle.Bold, theme.MutedInk, TextAnchor.MiddleCenter);
            entryPromptText.GetComponent<LayoutElement>().preferredHeight = 36;
        }

        private void BuildCharacterDialogue(Transform parent)
        {
            var contractor = factory.CreateAnchoredPanel(parent, "Contractor Visual", new Vector2(0f, 0f), new Vector2(0f, 0f), FirstPlayableMainSceneRectSpec.ContractorOpenPosition, FirstPlayableMainSceneRectSpec.ContractorSize, Color.white, false, factory.Assets.MainContractorSprite ?? factory.Assets.ShipSprite);
            contractorRect = contractor.GetComponent<RectTransform>();
            contractorCanvasGroup = contractor.AddComponent<CanvasGroup>();
            contractorImage = contractor.GetComponent<Image>();
            contractorImage.preserveAspect = true;
            contractorImage.color = factory.Assets.MainContractorSprite != null ? Color.white : theme.Gold;

            reviewBellObject = factory.CreateAnchoredPanel(parent, "Review Inactive Bell", new Vector2(0f, 0f), new Vector2(0f, 0f), FirstPlayableMainSceneRectSpec.ReviewBellPosition, FirstPlayableMainSceneRectSpec.ReviewBellSize, Color.white, false, factory.Assets.MainBellSprite ?? factory.Assets.MainSmallButtonSprite);
            reviewBellObject.GetComponent<Image>().preserveAspect = true;
            reviewBellCanvasGroup = reviewBellObject.AddComponent<CanvasGroup>();
            reviewBellCanvasGroup.alpha = 0.42f;

            reviewDocumentObject = factory.CreateAnchoredPanel(parent, "Presented Review Document", new Vector2(0f, 0f), new Vector2(0f, 0f), FirstPlayableMainSceneRectSpec.ReviewDocumentPosition, FirstPlayableMainSceneRectSpec.ReviewDocumentSize, theme.Paper, true, factory.Assets.MainPaperSprite ?? factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite);
            reviewDocumentObject.GetComponent<Image>().preserveAspect = true;
            reviewDocumentButton = reviewDocumentObject.AddComponent<Button>();
            reviewDocumentButton.targetGraphic = reviewDocumentObject.GetComponent<Image>();
            theme.ApplyButton(reviewDocumentButton, FirstPlayableButtonTone.Neutral);
            reviewDocumentButton.onClick.AddListener(() => PresentedDocumentClicked?.Invoke());

            var dialogue = factory.CreateAnchoredPanel(parent, "Dialogue Log Panel", new Vector2(0f, 1f), new Vector2(0f, 1f), FirstPlayableMainSceneRectSpec.DialogueOpenPosition, FirstPlayableMainSceneRectSpec.DialogueSize, theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.MainSpeechBubbleSprite);
            dialogueRect = dialogue.GetComponent<RectTransform>();
            var button = dialogue.AddComponent<Button>();
            button.targetGraphic = dialogue.GetComponent<Image>();
            theme.ApplyButton(button, FirstPlayableButtonTone.Neutral);
            button.onClick.AddListener(AdvanceDialogue);
            var layout = dialogue.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 14, 14);
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;
            dialogueScroll = factory.CreateScroll(dialogue.transform, "Dialogue Scroll", new Color(1f, 1f, 1f, 0.02f), out dialogueContentRoot);
            var scrollElement = dialogueScroll.gameObject.AddComponent<LayoutElement>();
            scrollElement.flexibleHeight = 1;
            dialogueScroll.movementType = ScrollRect.MovementType.Clamped;
            dialogueScroll.scrollSensitivity = 28f;
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
            documentRoot = documentBoard.transform;
        }

        private void BuildShelfRules(Transform parent)
        {
            var panel = factory.CreateAnchoredPanel(parent, "Right Shelf Checklist", new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), FirstPlayableMainSceneRectSpec.ShelfOpenPosition, FirstPlayableMainSceneRectSpec.ShelfSize, theme.WorkPanelAlt, true, factory.Assets.PanelSprite);
            shelfRect = panel.GetComponent<RectTransform>();
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 16, 14);
            layout.spacing = 8;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            reviewStatusText = factory.CreateText(panel.transform, "Review Status", theme.BodyFont, 14, FontStyle.Bold, theme.LightText, TextAnchor.UpperLeft);
            reviewStatusText.gameObject.SetActive(false);
            BuildRuleSection(panel.transform, FirstPlayableKoreanText.AbsoluteRuleSectionTitle, out absoluteRulesSection, out absoluteRulesRoot);
            BuildRuleSection(panel.transform, FirstPlayableKoreanText.ConsiderationRuleSectionTitle, out considerationRulesSection, out considerationRulesRoot);
        }

        private void BuildRuleSection(Transform parent, string title, out GameObject section, out Transform root)
        {
            section = new GameObject(title + " Section", typeof(RectTransform));
            section.transform.SetParent(parent, false);
            var sectionLayout = section.AddComponent<VerticalLayoutGroup>();
            sectionLayout.spacing = 6;
            sectionLayout.childForceExpandWidth = true;
            sectionLayout.childForceExpandHeight = false;
            var sectionElement = section.AddComponent<LayoutElement>();
            sectionElement.preferredHeight = title == FirstPlayableKoreanText.AbsoluteRuleSectionTitle ? 284f : 372f;
            sectionElement.flexibleHeight = 1;

            factory.CreateSectionTitle(section.transform, title);
            var scroll = factory.CreateScroll(section.transform, title + " Scroll", new Color(1f, 1f, 1f, 0.04f), out root);
            var element = scroll.gameObject.AddComponent<LayoutElement>();
            element.preferredHeight = title == FirstPlayableKoreanText.AbsoluteRuleSectionTitle ? 226f : 314f;
            element.flexibleHeight = 1;
        }

        private void BuildDecisionBox(Transform parent)
        {
            decisionBox = factory.CreateAnchoredPanel(parent, "Final Decision Paper Button", new Vector2(1f, 0f), new Vector2(1f, 0f), FirstPlayableMainSceneRectSpec.FinalDecisionBoxPosition, FirstPlayableMainSceneRectSpec.FinalDecisionBoxSize, theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            decisionBoxButton = decisionBox.AddComponent<Button>();
            decisionBoxButton.targetGraphic = decisionBox.GetComponent<Image>();
            theme.ApplyButton(decisionBoxButton, FirstPlayableButtonTone.Neutral);
            decisionBoxButton.onClick.AddListener(() => DecisionDrawerRequested?.Invoke());

            var layout = decisionBox.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 12, 10);
            layout.spacing = 5;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var title = factory.CreateText(decisionBox.transform, "Decision Box Title", theme.TitleFont, 17, FontStyle.Bold, theme.Ink, TextAnchor.MiddleCenter);
            title.text = FirstPlayableKoreanText.DecisionBoxTitle;
            title.GetComponent<LayoutElement>().preferredHeight = 46;
        }

        private void BuildDecisionDrawer(Transform parent)
        {
            decisionDrawer = factory.CreateAnchoredPanel(parent, "Bottom Right Decision Paper", new Vector2(1f, 0f), new Vector2(1f, 0f), FirstPlayableMainSceneRectSpec.DecisionDrawerClosedPosition, FirstPlayableMainSceneRectSpec.DecisionDrawerSize, theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            decisionDrawerRect = decisionDrawer.GetComponent<RectTransform>();
            var layout = decisionDrawer.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 18, 18);
            layout.spacing = 12;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var drawerTitle = factory.CreateText(decisionDrawer.transform, "Decision Drawer Title", theme.TitleFont, 22, FontStyle.Bold, theme.Ink, TextAnchor.MiddleCenter);
            drawerTitle.text = FirstPlayableKoreanText.DecisionDrawerTitle;
            drawerTitle.GetComponent<LayoutElement>().preferredHeight = 58;

            var middlePaper = factory.CreatePanel(decisionDrawer.transform, "Decision Amounts", Color.white, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite);
            middlePaper.AddComponent<LayoutElement>().preferredHeight = 150;
            var middleLayout = middlePaper.AddComponent<VerticalLayoutGroup>();
            middleLayout.padding = new RectOffset(16, 16, 18, 18);
            middleLayout.spacing = 18;
            middleLayout.childForceExpandWidth = true;
            middleLayout.childForceExpandHeight = false;

            drawerCompensationText = factory.CreateText(middlePaper.transform, "Compensation Amount", theme.TitleFont, 18, FontStyle.Bold, theme.Ink, TextAnchor.MiddleLeft);
            drawerCompensationText.GetComponent<LayoutElement>().preferredHeight = 42;
            premiumText = factory.CreateText(middlePaper.transform, "Premium Percent", theme.TitleFont, 18, FontStyle.Bold, theme.Ink, TextAnchor.MiddleLeft);
            premiumText.GetComponent<LayoutElement>().preferredHeight = 42;
            drawerPremiumText = premiumText;

            decisionRow = new GameObject("Decision Buttons", typeof(RectTransform));
            decisionRow.transform.SetParent(decisionDrawer.transform, false);
            decisionRow.AddComponent<LayoutElement>().preferredHeight = 72;
            var decisionLayout = decisionRow.AddComponent<HorizontalLayoutGroup>();
            decisionLayout.spacing = 10;
            decisionLayout.childForceExpandWidth = false;
            decisionLayout.childAlignment = TextAnchor.MiddleCenter;
            rejectButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.RejectButton, FirstPlayableButtonTone.Reject, () => Submit(PlayerDecision.Reject));
            approveButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.ApproveButton, FirstPlayableButtonTone.Primary, () => Submit(PlayerDecision.Approve));
            ApplyDecisionButtonSize(rejectButton);
            ApplyDecisionButtonSize(approveButton);
            drawerApproveText = approveButton.GetComponentInChildren<Text>();

            nextRow = new GameObject("Continue Row", typeof(RectTransform));
            nextRow.transform.SetParent(decisionDrawer.transform, false);
            nextButton = factory.CreateButton(nextRow.transform, FirstPlayableKoreanText.NextContractButton, FirstPlayableButtonTone.Neutral, () => { });
            nextButtonText = nextButton.GetComponentInChildren<Text>();
            nextRow.SetActive(false);
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
            var row = factory.CreatePanel(parent, "Applicable Reason", theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite);
            var rowElement = row.AddComponent<LayoutElement>();
            rowElement.minHeight = FirstPlayableMainSceneRectSpec.RuleRowHeight;
            rowElement.preferredHeight = FirstPlayableMainSceneRectSpec.RuleRowHeight;
            rowElement.flexibleWidth = 1;
            var image = row.GetComponent<Image>();
            ruleRows[rule.Id] = image;
            ruleSeverities[rule.Id] = rule.Severity;

            var toggle = row.AddComponent<Toggle>();
            toggle.targetGraphic = image;
            theme.ApplyToggle(toggle);
            ruleToggles[rule.Id] = toggle;

            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 8, 8);
            layout.spacing = 10;
            layout.childForceExpandWidth = false;
            layout.childAlignment = TextAnchor.MiddleLeft;

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
            title.resizeTextForBestFit = true;
            title.resizeTextMinSize = 11;
            title.resizeTextMaxSize = 14;
            title.GetComponent<LayoutElement>().preferredHeight = 38;

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

        private void SetDialogueLines(IReadOnlyList<string> lines)
        {
            dialogueLines = lines != null && lines.Count > 0
                ? lines
                : new[] { FirstPlayableKoreanText.DialogueReviewIntro };
            dialogueIndex = 0;
            ClearDialogueRows();
            AppendDialogueLine(dialogueLines[dialogueIndex]);
        }

        private void AdvanceDialogue()
        {
            if (dialogueLines == null || dialogueLines.Count == 0)
            {
                return;
            }

            dialogueIndex = Mathf.Min(dialogueIndex + 1, dialogueLines.Count - 1);
            if (dialogueContentRoot.childCount < dialogueIndex + 1)
            {
                AppendDialogueLine(dialogueLines[dialogueIndex]);
            }
        }

        private void AppendDialogueLine(string line)
        {
            if (dialogueContentRoot == null || string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            var row = new GameObject("Dialogue Message Row", typeof(RectTransform));
            row.transform.SetParent(dialogueContentRoot, false);
            var rowLayout = row.AddComponent<HorizontalLayoutGroup>();
            rowLayout.childAlignment = TextAnchor.MiddleLeft;
            rowLayout.childForceExpandWidth = false;
            rowLayout.childForceExpandHeight = false;
            var rowElement = row.AddComponent<LayoutElement>();
            rowElement.minHeight = 76f;

            var bubble = factory.CreatePanel(row.transform, "Dialogue Bubble", Color.white, true, factory.Assets.MainSpeechBubbleSprite ?? factory.Assets.MainPaperSprite);
            var bubbleElement = bubble.AddComponent<LayoutElement>();
            bubbleElement.minWidth = 180f;
            bubbleElement.preferredWidth = 500f;
            bubbleElement.minHeight = 76f;
            var bubbleLayout = bubble.AddComponent<VerticalLayoutGroup>();
            bubbleLayout.padding = new RectOffset(16, 16, 10, 10);
            bubbleLayout.childForceExpandWidth = true;
            bubbleLayout.childForceExpandHeight = false;

            var text = factory.CreateText(bubble.transform, "Message", theme.BodyFont, 17, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            text.text = line;
            text.GetComponent<LayoutElement>().preferredHeight = 54f;

            Canvas.ForceUpdateCanvases();
            if (dialogueScroll != null)
            {
                dialogueScroll.verticalNormalizedPosition = 0f;
            }
        }

        private void ClearDialogueRows()
        {
            if (dialogueContentRoot == null)
            {
                return;
            }

            ClearRoot(dialogueContentRoot);
        }

        private void ResetEntryComposition(bool showCharacter, bool documentInteractable, bool bellInteractable)
        {
            if (entryContractorRect != null)
            {
                entryContractorRect.anchoredPosition = showCharacter
                    ? FirstPlayableMainSceneRectSpec.EntryContractorPosition
                    : FirstPlayableMainSceneRectSpec.EntryContractorStartPosition;
                entryContractorRect.localScale = showCharacter ? Vector3.one : new Vector3(0.42f, 0.42f, 1f);
            }

            if (entryContractorCanvasGroup != null)
            {
                entryContractorCanvasGroup.alpha = showCharacter ? 1f : 0f;
            }

            if (entryContractorImage != null)
            {
                entryContractorImage.color = factory.Assets.MainContractorSprite != null
                    ? (showCharacter ? Color.white : new Color(0.44f, 0.40f, 0.35f, 1f))
                    : theme.Gold;
            }

            if (entryButton != null)
            {
                entryButton.interactable = documentInteractable;
            }

            if (entryBellButton != null)
            {
                entryBellButton.interactable = bellInteractable;
            }

            if (entryBellCanvasGroup != null)
            {
                entryBellCanvasGroup.alpha = bellInteractable ? 1f : 0.42f;
            }
        }

        private void PlayCharacterExit()
        {
            tweenRunner.MoveTo(contractorRect, FirstPlayableMainSceneRectSpec.ContractorOpenPosition, FirstPlayableMainSceneRectSpec.ContractorClosedPosition, 0.32f);
            tweenRunner.ScaleTo(contractorRect, Vector3.one, new Vector3(0.58f, 0.58f, 1f), 0.32f);
            tweenRunner.FadeTo(contractorCanvasGroup, 1f, 0.25f, 0.32f);
            tweenRunner.TintTo(contractorImage, contractorImage != null ? contractorImage.color : Color.white, new Color(0.34f, 0.3f, 0.28f, 0.9f), 0.32f);
            tweenRunner.InvokeAfter(0.36f, () => CharacterExitCompleted?.Invoke());
        }

        private void PlayReviewOpening()
        {
            reviewDocumentObject.SetActive(true);
            reviewBellObject.SetActive(true);
            if (reviewBellCanvasGroup != null)
            {
                reviewBellCanvasGroup.alpha = 0.42f;
            }

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
            var label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.resizeTextForBestFit = true;
                label.resizeTextMinSize = 11;
                label.resizeTextMaxSize = 17;
            }
        }

        private static void SetTextRect(Text text, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            if (text == null)
            {
                return;
            }

            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            rect.offsetMin = new Vector2(rect.offsetMin.x, 0f);
            rect.offsetMax = new Vector2(rect.offsetMax.x, 0f);
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
            var selectedConsiderations = CountSelectedConsiderations();
            if (drawerPremiumText != null)
            {
                drawerPremiumText.text = FirstPlayableKoreanText.MainLoopPremium(selectedConsiderations);
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
