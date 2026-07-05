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

        private Text headerMetaText;
        private Text docketText;
        private Text caseSummaryText;
        private Text reviewStatusText;
        private Text premiumText;
        private Text resultText;
        private Text outcomeText;
        private Text settlementText;
        private Text nextButtonText;
        private Transform documentRoot;
        private Transform rulesRoot;
        private Button approveButton;
        private Button rejectButton;
        private Button nextButton;
        private GameObject decisionRow;
        private GameObject nextRow;

        public FirstPlayableView(FirstPlayableUiTheme theme, FirstPlayableUiFactory factory)
        {
            this.theme = theme ?? throw new ArgumentNullException(nameof(theme));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

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
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var background = factory.CreatePanel(canvasObject.transform, "Main Scene Background", theme.MainBackdrop, false, factory.Assets.MainBackgroundSprite);
            var backgroundImage = background.GetComponent<Image>();
            backgroundImage.preserveAspect = false;

            factory.CreatePanel(background.transform, "Main Scene Dark Overlay", theme.MainOverlay, false);
            BuildTopStrip(background.transform);
            BuildDocketPaper(background.transform);
            BuildWorkbench(background.transform);
            BuildShelfRules(background.transform);
            BuildDecisionPaper(background.transform);
        }

        public void RenderReview(FirstPlayableReviewScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ClearRoot(documentRoot);
            ClearRoot(rulesRoot);
            ruleToggles.Clear();
            ruleRows.Clear();

            headerMetaText.text = state.HeaderMeta;
            docketText.text = state.Docket;
            caseSummaryText.text = state.CaseSummary;
            reviewStatusText.text = state.ReviewStatus;
            premiumText.text = state.Premium;
            resultText.text = FirstPlayableKoreanText.ReviewWaitingAudit;
            resultText.color = theme.Ink;
            outcomeText.text = FirstPlayableKoreanText.ReviewWaitingOutcome;
            outcomeText.color = theme.Ink;
            settlementText.text = FirstPlayableKoreanText.ReviewWaitingSettlement;
            settlementText.color = theme.MutedInk;

            foreach (var document in state.Documents)
            {
                CreateDocumentCard(documentRoot, document);
            }

            foreach (var rule in state.Rules)
            {
                CreateRuleRow(rulesRoot, rule);
            }

            SetDecisionInteractable(true);
            nextRow.SetActive(false);
        }

        public void RenderResult(FirstPlayableResultScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            headerMetaText.text = state.HeaderMeta;
            premiumText.text = state.Premium;
            resultText.text = state.Result;
            resultText.color = state.AuditCorrect ? theme.Correct : theme.Reject;
            outcomeText.text = state.Outcome;
            outcomeText.color = state.OutcomeWarning ? theme.Warning : theme.Ink;
            settlementText.text = state.Settlement;
            settlementText.color = theme.MutedInk;

            SetDecisionInteractable(false);
            nextRow.SetActive(true);
            nextButton.interactable = true;
            nextButtonText.text = state.NextButton;
        }

        public void RenderComplete(FirstPlayableCompleteScreenState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ClearRoot(documentRoot);
            ClearRoot(rulesRoot);
            ruleToggles.Clear();
            ruleRows.Clear();

            headerMetaText.text = state.HeaderMeta;
            docketText.text = state.Docket;
            caseSummaryText.text = state.CaseSummary;
            reviewStatusText.text = state.ReviewStatus;
            premiumText.text = state.Premium;
            resultText.text = state.Result;
            resultText.color = theme.Correct;
            outcomeText.text = state.Outcome;
            outcomeText.color = theme.Ink;
            settlementText.text = state.Settlement;
            settlementText.color = theme.MutedInk;
            SetDecisionInteractable(false);
            nextRow.SetActive(false);
        }

        private void BuildTopStrip(Transform parent)
        {
            var header = factory.CreateAnchoredPanel(
                parent,
                "Top Date Status Strip",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -39f),
                new Vector2(0f, 78f),
                theme.TopStrip,
                false,
                factory.Assets.PanelSprite);
            var layout = header.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(26, 26, 10, 10);
            layout.spacing = 14;
            layout.childForceExpandWidth = false;
            layout.childAlignment = TextAnchor.MiddleLeft;

            var letter = factory.CreateSpriteImage(header.transform, "Letter Prop Mark", factory.Assets.MainLetterSprite ?? factory.Assets.ShipSprite, theme.Gold, true);
            var letterElement = letter.GetComponent<LayoutElement>();
            letterElement.preferredWidth = 74;
            letterElement.preferredHeight = 54;

            var title = factory.CreateText(header.transform, "Office Title", theme.TitleFont, 30, FontStyle.Bold, theme.LightText, TextAnchor.MiddleLeft);
            title.text = FirstPlayableKoreanText.OfficeTitle;
            title.GetComponent<LayoutElement>().flexibleWidth = 1;

            headerMetaText = factory.CreateText(header.transform, "Header Meta", theme.BodyFont, 16, FontStyle.Bold, theme.MutedText, TextAnchor.MiddleRight);
            headerMetaText.GetComponent<LayoutElement>().preferredWidth = 720;
        }

        private void BuildDocketPaper(Transform parent)
        {
            var shadow = factory.CreateAnchoredPanel(parent, "Docket Paper Shadow", new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(205f, -28f), new Vector2(340f, 770f), theme.PaperShadow, false);
            shadow.transform.SetAsFirstSibling();
            var panel = factory.CreateAnchoredPanel(parent, "Left Contract Paper Stack", new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(190f, -18f), new Vector2(340f, 770f), theme.Paper, true, factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 18, 18);
            layout.spacing = 10;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            factory.CreateSectionTitle(panel.transform, FirstPlayableKoreanText.ContractDocketTitle);
            docketText = factory.CreateText(panel.transform, "Contract List", theme.BodyFont, 16, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            docketText.GetComponent<LayoutElement>().preferredHeight = 128;
            factory.CreateDivider(panel.transform);
            factory.CreateSectionTitle(panel.transform, FirstPlayableKoreanText.CurrentCaseTitle);
            caseSummaryText = factory.CreateText(panel.transform, "Case Summary", theme.BodyFont, 15, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            caseSummaryText.GetComponent<LayoutElement>().flexibleHeight = 1;
        }

        private void BuildWorkbench(Transform parent)
        {
            var workbench = factory.CreateAnchoredPanel(parent, "Central Workbench Surface", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-110f, -12f), new Vector2(880f, 790f), theme.WorkPanel, true, factory.Assets.PanelSprite);
            var layout = workbench.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(22, 22, 18, 20);
            layout.spacing = 12;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var shipRow = new GameObject("Ship And Documents Header");
            shipRow.transform.SetParent(workbench.transform, false);
            shipRow.AddComponent<LayoutElement>().preferredHeight = 110;
            var shipLayout = shipRow.AddComponent<HorizontalLayoutGroup>();
            shipLayout.padding = new RectOffset(0, 0, 0, 0);
            shipLayout.spacing = 12;
            shipLayout.childForceExpandWidth = false;
            shipLayout.childAlignment = TextAnchor.MiddleLeft;

            var ship = factory.CreateSpriteImage(shipRow.transform, "Workbench Ship", factory.Assets.ShipSprite, theme.Gold, true);
            var shipElement = ship.GetComponent<LayoutElement>();
            shipElement.preferredWidth = 150;
            shipElement.preferredHeight = 94;

            var heading = factory.CreateText(shipRow.transform, "Document Workbench Title", theme.TitleFont, 24, FontStyle.Bold, theme.LightText, TextAnchor.MiddleLeft);
            heading.text = FirstPlayableKoreanText.DocumentBundleTitle;
            heading.GetComponent<LayoutElement>().flexibleWidth = 1;

            var letter = factory.CreateSpriteImage(shipRow.transform, "Workbench Letter", factory.Assets.MainLetterSprite, theme.Paper, true);
            var letterElement = letter.GetComponent<LayoutElement>();
            letterElement.preferredWidth = 120;
            letterElement.preferredHeight = 82;

            var scroll = factory.CreateScroll(workbench.transform, "Document Prop Scroll", theme.PaperInset, out documentRoot);
            scroll.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1;
        }

        private void BuildShelfRules(Transform parent)
        {
            var panel = factory.CreateAnchoredPanel(parent, "Right Shelf Checklist", new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-238f, -18f), new Vector2(430f, 770f), theme.WorkPanelAlt, true, factory.Assets.PanelSprite);
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 18, 16);
            layout.spacing = 10;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            factory.CreateSectionTitle(panel.transform, FirstPlayableKoreanText.RuleChecklistTitle);
            reviewStatusText = factory.CreateText(panel.transform, "Review Status", theme.BodyFont, 14, FontStyle.Bold, theme.LightText, TextAnchor.UpperLeft);
            reviewStatusText.GetComponent<LayoutElement>().preferredHeight = 56;
            var scroll = factory.CreateScroll(panel.transform, "Rules Shelf Scroll", theme.PaperInset, out rulesRoot);
            scroll.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1;
        }

        private void BuildDecisionPaper(Transform parent)
        {
            var panel = factory.CreateAnchoredPanel(parent, "Bottom Right Decision Paper", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-552f, 156f), new Vector2(690f, 285f), theme.Paper, true, factory.Assets.MainPaperTextureSprite ?? factory.Assets.MainPaperSprite ?? factory.Assets.DocumentPaperSprite);
            var layout = panel.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 16, 16);
            layout.spacing = 14;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            var resultColumn = new GameObject("Result Column");
            resultColumn.transform.SetParent(panel.transform, false);
            resultColumn.AddComponent<LayoutElement>().flexibleWidth = 2;
            var resultLayout = resultColumn.AddComponent<VerticalLayoutGroup>();
            resultLayout.spacing = 8;
            resultLayout.childForceExpandWidth = true;
            resultLayout.childForceExpandHeight = false;

            premiumText = factory.CreateText(resultColumn.transform, "Premium Quote", theme.TitleFont, 18, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            resultText = factory.CreateText(resultColumn.transform, "Audit Result", theme.BodyFont, 15, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            outcomeText = factory.CreateText(resultColumn.transform, "Outcome Result", theme.BodyFont, 15, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            settlementText = factory.CreateText(resultColumn.transform, "Settlement Result", theme.BodyFont, 14, FontStyle.Bold, theme.MutedInk, TextAnchor.UpperLeft);
            premiumText.GetComponent<LayoutElement>().preferredHeight = 30;
            resultText.GetComponent<LayoutElement>().preferredHeight = 78;
            outcomeText.GetComponent<LayoutElement>().preferredHeight = 44;
            settlementText.GetComponent<LayoutElement>().preferredHeight = 62;

            var actionColumn = new GameObject("Action Column");
            actionColumn.transform.SetParent(panel.transform, false);
            actionColumn.AddComponent<LayoutElement>().preferredWidth = 245;
            var actionLayout = actionColumn.AddComponent<VerticalLayoutGroup>();
            actionLayout.spacing = 10;
            actionLayout.childForceExpandWidth = true;
            actionLayout.childForceExpandHeight = false;

            decisionRow = new GameObject("Decision Buttons");
            decisionRow.transform.SetParent(actionColumn.transform, false);
            var decisionLayout = decisionRow.AddComponent<HorizontalLayoutGroup>();
            decisionLayout.spacing = 10;
            decisionLayout.childForceExpandWidth = true;
            approveButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.ApproveButton, FirstPlayableButtonTone.Primary, () => Submit(PlayerDecision.Approve));
            rejectButton = factory.CreateButton(decisionRow.transform, FirstPlayableKoreanText.RejectButton, FirstPlayableButtonTone.Reject, () => Submit(PlayerDecision.Reject));

            nextRow = new GameObject("Continue Row");
            nextRow.transform.SetParent(actionColumn.transform, false);
            nextButton = factory.CreateButton(nextRow.transform, FirstPlayableKoreanText.NextContractButton, FirstPlayableButtonTone.Neutral, () => ResultAcknowledged?.Invoke());
            nextButtonText = nextButton.GetComponentInChildren<Text>();
        }

        private void CreateDocumentCard(Transform parent, DocumentRecord document)
        {
            var sprite = factory.Assets.MainPaperTextureSprite ?? factory.Assets.DocumentPaperSprite ?? factory.Assets.DocumentTileSprite;
            var card = factory.CreatePanel(parent, FirstPlayableKoreanText.DocumentTitle(document), theme.Paper, true, sprite);
            var layout = card.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 12, 12);
            layout.spacing = 6;
            layout.childForceExpandWidth = true;
            card.AddComponent<LayoutElement>().minHeight = document.Submitted ? 150 : 92;

            var titleRow = new GameObject("Document Title Row");
            titleRow.transform.SetParent(card.transform, false);
            var titleLayout = titleRow.AddComponent<HorizontalLayoutGroup>();
            titleLayout.spacing = 8;
            titleLayout.childForceExpandWidth = true;
            titleLayout.childAlignment = TextAnchor.MiddleLeft;

            var title = factory.CreateText(titleRow.transform, "Document Title", theme.TitleFont, 19, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            title.text = $"{FirstPlayableKoreanText.DocumentTitle(document)}  |  {document.Id}";
            title.GetComponent<LayoutElement>().flexibleWidth = 1;

            var seal = factory.CreateSpriteImage(titleRow.transform, "Document Seal", factory.Assets.SealSprite, theme.Paper, true);
            seal.GetComponent<LayoutElement>().preferredWidth = 38;

            var status = factory.CreateText(card.transform, "Document Status", theme.BodyFont, 14, FontStyle.Bold, document.Submitted ? theme.MutedInk : theme.Reject, TextAnchor.UpperLeft);
            status.text = FirstPlayableKoreanText.DocumentStatus(document.Submitted);

            if (!document.Submitted)
            {
                var missing = factory.CreateText(card.transform, "Missing Message", theme.BodyFont, 15, FontStyle.Bold, theme.Reject, TextAnchor.UpperLeft);
                missing.text = FirstPlayableKoreanText.MissingDocumentMessage();
                return;
            }

            foreach (var field in document.Fields)
            {
                var fieldText = factory.CreateText(card.transform, field.Id.ToString(), theme.BodyFont, 15, FontStyle.Normal, theme.Ink, TextAnchor.UpperLeft);
                fieldText.text = $"{FirstPlayableKoreanText.FieldLabel(field)}: {FirstPlayableKoreanText.FieldValue(field.Value)}";
            }
        }

        private void CreateRuleRow(Transform parent, FirstPlayableRuleRowState rowState)
        {
            var rule = rowState.Rule;
            var row = factory.CreatePanel(parent, rule.Id.ToString(), theme.DisabledRow, true, factory.Assets.MainPaperSprite);
            row.AddComponent<LayoutElement>().minHeight = 78;
            var image = row.GetComponent<Image>();
            ruleRows[rule.Id] = image;

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
            marker.AddComponent<LayoutElement>().preferredWidth = 52;
            var markerText = factory.CreateText(marker.transform, "Severity", theme.TitleFont, 15, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            markerText.text = FirstPlayableKoreanText.RuleSeverityMarker(rule.Severity);

            var check = factory.CreatePanel(row.transform, "Check", theme.Paper, true);
            var checkElement = check.AddComponent<LayoutElement>();
            checkElement.preferredWidth = 30;
            checkElement.preferredHeight = 30;
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

            var title = factory.CreateText(labelColumn.transform, "Title", theme.TitleFont, 15, FontStyle.Bold, theme.Ink, TextAnchor.UpperLeft);
            title.text = $"{rule.Id}  {FirstPlayableKoreanText.RuleTitle(rule)}";
            var detail = factory.CreateText(labelColumn.transform, "Detail", theme.BodyFont, 13, FontStyle.Bold, theme.MutedInk, TextAnchor.UpperLeft);
            detail.text = FirstPlayableKoreanText.RuleFindingText(rowState.IsTriggered);

            toggle.onValueChanged.AddListener(_ => RefreshRuleRow(rule.Id));
            RefreshRuleRow(rule.Id);
        }

        private void Submit(PlayerDecision decision)
        {
            DecisionSubmitted?.Invoke(decision, ruleToggles.Where(pair => pair.Value.isOn).Select(pair => pair.Key).ToList());
        }

        private void SetDecisionInteractable(bool interactable)
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

        private Color RuleColor(RuleDefinition rule)
        {
            return rule.Severity == RuleSeverity.AbsoluteRejection ? theme.Reject : theme.Consideration;
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
