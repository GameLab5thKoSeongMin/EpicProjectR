// Responsibility: Connects first playable application/session state to the passive Unity view.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Application;
using EpicProjectR.Content;
using EpicProjectR.Domain;

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayablePresenter
    {
        private readonly FirstPlayableSession session;
        private readonly IReadOnlyList<ContractCase> allContracts;
        private readonly IReadOnlyList<RuleDefinition> allRules;
        private readonly FirstPlayableView view;
        private FirstPlayableScreenMode currentMode = FirstPlayableScreenMode.Entry;

        public FirstPlayablePresenter(
            FirstPlayableSession session,
            IReadOnlyList<ContractCase> allContracts,
            IReadOnlyList<RuleDefinition> allRules,
            FirstPlayableView view)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            this.allContracts = allContracts ?? throw new ArgumentNullException(nameof(allContracts));
            this.allRules = allRules ?? throw new ArgumentNullException(nameof(allRules));
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            view.ReviewStarted += StartReview;
            view.DecisionDrawerRequested += OpenDecisionDrawer;
            view.DecisionSubmitted += SubmitDecision;
            view.ResultAcknowledged += AdvanceAfterResult;
        }

        public FirstPlayableScreenMode CurrentMode => currentMode;

        public void ShowCurrent()
        {
            ShowCurrentCase();
        }

        public void ShowCurrentCase()
        {
            if (session.IsComplete)
            {
                ShowCompleted();
                return;
            }

            currentMode = FirstPlayableScreenMode.Entry;
            var current = session.CurrentContract;
            view.RenderEntry(new FirstPlayableEntryScreenState(
                FirstPlayableKoreanText.HeaderMeta(session.TurnDefinition.TurnNumber, session.TurnDefinition.Date, current.Id, FirstPlayableKoreanText.ReviewingStatus()),
                RenderDocket(),
                FirstPlayableKoreanText.CaseSummary(current),
                FirstPlayableKoreanText.EntryDocumentTitle,
                FirstPlayableKoreanText.EntryPrompt));
        }

        public void StartReview()
        {
            if (session.IsComplete || currentMode != FirstPlayableScreenMode.Entry)
            {
                return;
            }

            currentMode = FirstPlayableScreenMode.OpeningReview;
            RenderReview();
            currentMode = FirstPlayableScreenMode.Reviewing;
        }

        public void OpenDecisionDrawer()
        {
            if (session.IsComplete || currentMode != FirstPlayableScreenMode.Reviewing)
            {
                return;
            }

            currentMode = FirstPlayableScreenMode.DecisionDrawerOpening;
            view.OpenDecisionDrawer();
            currentMode = FirstPlayableScreenMode.DecisionReady;
        }

        private void RenderReview()
        {
            currentMode = FirstPlayableScreenMode.Reviewing;
            var current = session.CurrentContract;
            var review = session.CurrentReview();
            var triggered = new HashSet<RuleId>(review.Triggers.Select(trigger => trigger.RuleId));
            var activeRules = allRules
                .Where(rule => rule.Windows.Any(window => window.IsActiveFor(current)))
                .Select(rule => new FirstPlayableRuleRowState(rule, triggered.Contains(rule.Id)))
                .ToList();

            view.RenderReview(new FirstPlayableReviewScreenState(
                FirstPlayableKoreanText.HeaderMeta(session.TurnDefinition.TurnNumber, session.TurnDefinition.Date, current.Id, FirstPlayableKoreanText.ReviewingStatus()),
                RenderDocket(),
                FirstPlayableKoreanText.CaseSummary(current),
                FirstPlayableKoreanText.ActiveRuleStatus(activeRules.Count),
                FirstPlayableKoreanText.PreSubmitPremium(review.ConsiderationCount),
                current.Documents,
                activeRules));
        }

        public void SubmitDecision(PlayerDecision decision, IReadOnlyList<RuleId> checkedRuleIds)
        {
            if (session.IsComplete || currentMode != FirstPlayableScreenMode.DecisionReady)
            {
                return;
            }

            var result = session.Submit(decision, checkedRuleIds);
            ShowResult(result);
        }

        public void AdvanceAfterResult()
        {
            if (currentMode != FirstPlayableScreenMode.Result)
            {
                return;
            }

            ShowCurrentCase();
        }

        private void ShowResult(SubmissionResult result)
        {
            currentMode = FirstPlayableScreenMode.Result;

            view.RenderResult(new FirstPlayableResultScreenState(
                ResultHeaderMeta(result.Audit.ContractId),
                FirstPlayableKoreanText.SubmittedPremium(result.Premium.MultiplierPercent, result.Premium.RejectRecommended, JoinIds(result.Premium.ConsiderationRuleIds)),
                RenderAuditResult(result.Audit),
                FirstPlayableKoreanText.OutcomeResult(result),
                RenderSettlement(result.Settlement),
                session.IsComplete ? FirstPlayableKoreanText.FinishButton : FirstPlayableKoreanText.NextContractButton,
                result.Audit.IsCorrectAction,
                result.Outcome.AccidentOccurred));
        }

        private void ShowCompleted()
        {
            currentMode = FirstPlayableScreenMode.Completed;
            view.RenderComplete(new FirstPlayableCompleteScreenState(
                FirstPlayableKoreanText.CompleteHeader(),
                RenderDocket(),
                FirstPlayableKoreanText.CompleteCaseSummary(),
                FirstPlayableKoreanText.SessionCompleteStatus(),
                FirstPlayableKoreanText.CompletePremium,
                FirstPlayableKoreanText.CompleteResult,
                FirstPlayableKoreanText.CompleteOutcome,
                FirstPlayableKoreanText.CompleteSettlement));
        }

        private string ResultHeaderMeta(ContractId contractId)
        {
            return FirstPlayableKoreanText.HeaderMeta(session.TurnDefinition.TurnNumber, session.TurnDefinition.Date, contractId, FirstPlayableKoreanText.ResultPostedStatus());
        }

        private string RenderDocket()
        {
            var lines = new List<string>();
            for (var i = 0; i < allContracts.Count; i++)
            {
                var marker = i < session.CurrentIndex
                    ? FirstPlayableKoreanText.DocketMarkerSubmitted()
                    : i == session.CurrentIndex && !session.IsComplete
                        ? FirstPlayableKoreanText.DocketMarkerCurrent()
                        : FirstPlayableKoreanText.DocketMarkerPending();
                lines.Add(FirstPlayableKoreanText.DocketLine(allContracts[i].Id, marker));
            }

            return string.Join("\n", lines);
        }

        private static string RenderAuditResult(DecisionAuditResult audit)
        {
            return FirstPlayableKoreanText.AuditResult(
                audit,
                JoinIds(audit.CorrectCheckedReasons),
                JoinIds(audit.MissedReasons),
                JoinIds(audit.FalsePositiveReasons));
        }

        private static string RenderSettlement(SettlementResult settlement)
        {
            return FirstPlayableKoreanText.SettlementSummary(
                settlement,
                string.Join("  |  ", settlement.LineItems.Select(FirstPlayableKoreanText.SettlementLineItem)));
        }

        private static string JoinIds(IEnumerable<RuleId> ids)
        {
            var list = ids.Select(id => id.ToString()).ToList();
            return list.Count == 0 ? "-" : string.Join(", ", list);
        }
    }
}
