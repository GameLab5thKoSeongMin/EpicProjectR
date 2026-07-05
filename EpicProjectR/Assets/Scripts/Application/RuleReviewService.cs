// Responsibility: Evaluates fixture underwriting rules into explainable review results.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EpicProjectR.Content;
using EpicProjectR.Domain;

namespace EpicProjectR.Application
{
    public sealed class RuleReviewService
    {
        private readonly IRuleRepository ruleRepository;

        public RuleReviewService(IRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository ?? throw new ArgumentNullException(nameof(ruleRepository));
        }

        public ReviewResult Review(ContractCase contractCase)
        {
            var triggers = new List<RuleTrigger>();
            foreach (var rule in ruleRepository.GetAll())
            {
                if (!rule.Windows.Any(window => window.IsActiveFor(contractCase)))
                {
                    continue;
                }

                var trigger = EvaluateRule(contractCase, rule);
                if (trigger != null)
                {
                    triggers.Add(trigger);
                }
            }

            return new ReviewResult(contractCase.Id, triggers);
        }

        private static RuleTrigger EvaluateRule(ContractCase contractCase, RuleDefinition rule)
        {
            var condition = rule.Condition;
            var document = contractCase.Documents.FirstOrDefault(d => d.Kind == condition.DocumentKind);

            if (condition.Type == RuleConditionType.MissingDocument)
            {
                if (document == null || !document.Submitted)
                {
                    return Trigger(rule, document, null);
                }

                return null;
            }

            if (document == null || !document.Submitted || !condition.FieldId.HasValue)
            {
                return null;
            }

            var value = document.GetFieldValue(condition.FieldId.Value);
            if (value == null)
            {
                return null;
            }

            switch (condition.Type)
            {
                case RuleConditionType.FieldEquals:
                    return value == condition.ExpectedValue ? Trigger(rule, document, condition.FieldId) : null;
                case RuleConditionType.FieldMismatch:
                    return IsMismatch(contractCase, value, condition.ComparisonFieldValue) ? Trigger(rule, document, condition.FieldId) : null;
                case RuleConditionType.DateBefore:
                    return IsDateBefore(value, condition.ExpectedValue) ? Trigger(rule, document, condition.FieldId) : null;
                case RuleConditionType.NumberAtLeast:
                    return IsNumberAtLeast(value, condition.ExpectedValue) ? Trigger(rule, document, condition.FieldId) : null;
                default:
                    return null;
            }
        }

        private static bool IsMismatch(ContractCase contractCase, string value, string comparison)
        {
            if (comparison != null && comparison.Contains("."))
            {
                var parts = comparison.Split('.');
                if (parts.Length == 2 && TryParseDocumentKind(parts[0], out var kind))
                {
                    var doc = contractCase.Documents.FirstOrDefault(d => d.Kind == kind);
                    var compareValue = doc != null ? doc.GetFieldValue(new DocumentFieldId(parts[1], SourceIdKind.Fixture)) : null;
                    return compareValue != null && value != compareValue;
                }
            }

            return value != comparison;
        }

        private static bool TryParseDocumentKind(string value, out DocumentKind kind)
        {
            if (value == "ShipApplication")
            {
                kind = DocumentKind.ShipApplication;
                return true;
            }

            kind = default;
            return false;
        }

        private static bool IsDateBefore(string value, string limit)
        {
            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var actual)
                && DateTime.TryParseExact(limit, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var cutoff)
                && actual < cutoff;
        }

        private static bool IsNumberAtLeast(string value, string minimum)
        {
            return int.TryParse(value, out var actual) && int.TryParse(minimum, out var min) && actual >= min;
        }

        private static RuleTrigger Trigger(RuleDefinition rule, DocumentRecord document, DocumentFieldId? fieldId)
        {
            return new RuleTrigger(rule.Id, rule.Severity, document != null ? document.Id : (DocumentId?)null, fieldId, rule.ExplanationKey, rule.Title);
        }
    }
}
