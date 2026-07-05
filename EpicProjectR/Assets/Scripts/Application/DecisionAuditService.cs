// Responsibility: Audits player decisions against review results and creates premium recommendations.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Domain;

namespace EpicProjectR.Application
{
    public sealed class DecisionAuditResult
    {
        public DecisionAuditResult(
            ContractId contractId,
            PlayerDecision decision,
            PlayerDecision correctAction,
            bool isCorrectAction,
            IEnumerable<RuleId> correctCheckedReasons,
            IEnumerable<RuleId> missedReasons,
            IEnumerable<RuleId> falsePositiveReasons,
            EvaluationScore scoreDelta)
        {
            ContractId = contractId;
            Decision = decision;
            CorrectAction = correctAction;
            IsCorrectAction = isCorrectAction;
            CorrectCheckedReasons = correctCheckedReasons.ToList().AsReadOnly();
            MissedReasons = missedReasons.ToList().AsReadOnly();
            FalsePositiveReasons = falsePositiveReasons.ToList().AsReadOnly();
            ScoreDelta = scoreDelta;
        }

        public ContractId ContractId { get; }
        public PlayerDecision Decision { get; }
        public PlayerDecision CorrectAction { get; }
        public bool IsCorrectAction { get; }
        public IReadOnlyList<RuleId> CorrectCheckedReasons { get; }
        public IReadOnlyList<RuleId> MissedReasons { get; }
        public IReadOnlyList<RuleId> FalsePositiveReasons { get; }
        public EvaluationScore ScoreDelta { get; }
    }

    public sealed class PremiumQuoteResult
    {
        public PremiumQuoteResult(ContractId contractId, int multiplierPercent, bool rejectRecommended, IEnumerable<RuleId> considerationRuleIds)
        {
            ContractId = contractId;
            MultiplierPercent = multiplierPercent;
            RejectRecommended = rejectRecommended;
            ConsiderationRuleIds = considerationRuleIds.ToList().AsReadOnly();
        }

        public ContractId ContractId { get; }
        public int MultiplierPercent { get; }
        public bool RejectRecommended { get; }
        public IReadOnlyList<RuleId> ConsiderationRuleIds { get; }
    }

    public sealed class DecisionAuditService
    {
        public DecisionAuditResult Audit(ReviewResult reviewResult, ReviewSubmission submission)
        {
            if (!reviewResult.ContractId.Equals(submission.ContractId))
            {
                throw new InvalidOperationException("Review result and submission contract IDs must match exactly.");
            }

            var triggered = new HashSet<RuleId>(reviewResult.Triggers.Select(t => t.RuleId));
            var checkedIds = new HashSet<RuleId>(submission.CheckedRuleIds);
            var correctChecked = checkedIds.Where(triggered.Contains).ToList();
            var missed = triggered.Where(id => !checkedIds.Contains(id)).ToList();
            var falsePositive = checkedIds.Where(id => !triggered.Contains(id)).ToList();
            var correctAction = reviewResult.HasAbsoluteRejection ? PlayerDecision.Reject : PlayerDecision.Approve;
            var isCorrectAction = submission.Decision == correctAction || (correctAction == PlayerDecision.Approve && submission.Decision == PlayerDecision.ConditionalApprove);
            var score = checkedIds.Count * 2 + (isCorrectAction ? 5 : -10);

            return new DecisionAuditResult(
                submission.ContractId,
                submission.Decision,
                correctAction,
                isCorrectAction,
                correctChecked,
                missed,
                falsePositive,
                new EvaluationScore(score));
        }

        public PremiumQuoteResult QuotePremium(ReviewResult reviewResult)
        {
            var crIds = reviewResult.Triggers
                .Where(t => t.Severity == RuleSeverity.RejectionConsideration)
                .Select(t => t.RuleId)
                .ToList();

            var multiplier = crIds.Count == 0 ? 100 : crIds.Count == 1 ? 125 : 150;
            return new PremiumQuoteResult(reviewResult.ContractId, multiplier, crIds.Count >= 3, crIds);
        }
    }
}
