// Responsibility: Defines pure domain models for contract review, decisions, outcomes, and settlement.
using System;
using System.Collections.Generic;
using System.Linq;

namespace EpicProjectR.Domain
{
    public enum ContractType
    {
        Ship,
        Cargo,
        Mixed
    }

    public enum PlayerDecision
    {
        Approve,
        Reject,
        ConditionalApprove
    }

    public enum RuleSeverity
    {
        AbsoluteRejection,
        RejectionConsideration
    }

    public enum DocumentKind
    {
        ShipApplication,
        ShipRegistration,
        HullInspection,
        RouteDeclaration,
        CargoApplication,
        CargoManifest,
        LoadingCertificate,
        QuarantineCertificate
    }

    public readonly struct GameDate : IComparable<GameDate>, IEquatable<GameDate>
    {
        public GameDate(int year, int month, int day)
        {
            if (year < 1) throw new ArgumentOutOfRangeException(nameof(year));
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month));
            if (day < 1 || day > DateTime.DaysInMonth(year, month)) throw new ArgumentOutOfRangeException(nameof(day));
            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int CompareTo(GameDate other) => ToDateTime().CompareTo(other.ToDateTime());
        public bool Equals(GameDate other) => Year == other.Year && Month == other.Month && Day == other.Day;
        public override bool Equals(object obj) => obj is GameDate other && Equals(other);
        public override int GetHashCode() => (Year * 397) ^ (Month * 31) ^ Day;
        public override string ToString() => $"{Year:D4}-{Month:D2}-{Day:D2}";
        private DateTime ToDateTime() => new DateTime(Year, Month, Day);
        public static bool operator <=(GameDate left, GameDate right) => left.CompareTo(right) <= 0;
        public static bool operator >=(GameDate left, GameDate right) => left.CompareTo(right) >= 0;
    }

    public readonly struct TurnNumber : IEquatable<TurnNumber>
    {
        public TurnNumber(int value)
        {
            if (value < 1 || value > 24) throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }

        public int Value { get; }
        public bool Equals(TurnNumber other) => Value == other.Value;
        public override bool Equals(object obj) => obj is TurnNumber other && Equals(other);
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();
    }

    public readonly struct TurnSlot : IEquatable<TurnSlot>
    {
        public TurnSlot(int value)
        {
            if (value < 1 || value > 7) throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }

        public int Value { get; }
        public bool Equals(TurnSlot other) => Value == other.Value;
        public override bool Equals(object obj) => obj is TurnSlot other && Equals(other);
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();
    }

    public readonly struct AccidentFlag
    {
        public AccidentFlag(bool willAccidentIfApproved)
        {
            WillAccidentIfApproved = willAccidentIfApproved;
        }

        public bool WillAccidentIfApproved { get; }
    }

    public readonly struct MoneyAmount
    {
        public MoneyAmount(int ducats)
        {
            Ducats = ducats;
        }

        public int Ducats { get; }
        public override string ToString() => $"{Ducats} ducats";
    }

    public readonly struct EvaluationScore
    {
        public EvaluationScore(int value)
        {
            Value = value;
        }

        public int Value { get; }
        public override string ToString() => Value.ToString();
    }

    public sealed class DocumentField
    {
        public DocumentField(DocumentFieldId id, string label, string value)
        {
            Id = id;
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Value = value ?? string.Empty;
        }

        public DocumentFieldId Id { get; }
        public string Label { get; }
        public string Value { get; }
    }

    public sealed class DocumentRecord
    {
        public DocumentRecord(DocumentId id, DocumentKind kind, string title, bool submitted, IEnumerable<DocumentField> fields)
        {
            Id = id;
            Kind = kind;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Submitted = submitted;
            Fields = (fields ?? throw new ArgumentNullException(nameof(fields))).ToList().AsReadOnly();
        }

        public DocumentId Id { get; }
        public DocumentKind Kind { get; }
        public string Title { get; }
        public bool Submitted { get; }
        public IReadOnlyList<DocumentField> Fields { get; }

        public string GetFieldValue(DocumentFieldId fieldId)
        {
            var field = Fields.FirstOrDefault(f => f.Id.Equals(fieldId));
            return field != null ? field.Value : null;
        }
    }

    public sealed class DocumentBundle
    {
        public DocumentBundle(BundleId id, string name, IEnumerable<DocumentKind> requiredDocuments)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RequiredDocuments = (requiredDocuments ?? throw new ArgumentNullException(nameof(requiredDocuments))).ToList().AsReadOnly();
        }

        public BundleId Id { get; }
        public string Name { get; }
        public IReadOnlyList<DocumentKind> RequiredDocuments { get; }
    }

    public sealed class ContractCase
    {
        public ContractCase(
            ContractId id,
            bool isFixture,
            TurnNumber turn,
            TurnSlot slot,
            ContractType contractType,
            BundleId bundleId,
            RouteId routeId,
            string applicantName,
            MoneyAmount basePremium,
            MoneyAmount coverage,
            PlayerDecision expectedDecision,
            AccidentFlag accidentFlag,
            GameDate returnDate,
            IEnumerable<DocumentRecord> documents,
            SubjectiveInfoId? subjectiveInfoId = null,
            SpecialContractorId? specialContractorId = null)
        {
            Id = id;
            IsFixture = isFixture;
            Turn = turn;
            Slot = slot;
            ContractType = contractType;
            BundleId = bundleId;
            RouteId = routeId;
            ApplicantName = applicantName ?? throw new ArgumentNullException(nameof(applicantName));
            BasePremium = basePremium;
            Coverage = coverage;
            ExpectedDecision = expectedDecision;
            AccidentFlag = accidentFlag;
            ReturnDate = returnDate;
            Documents = (documents ?? throw new ArgumentNullException(nameof(documents))).ToList().AsReadOnly();
            SubjectiveInfoId = subjectiveInfoId;
            SpecialContractorId = specialContractorId;
        }

        public ContractId Id { get; }
        public bool IsFixture { get; }
        public TurnNumber Turn { get; }
        public TurnSlot Slot { get; }
        public ContractType ContractType { get; }
        public BundleId BundleId { get; }
        public RouteId RouteId { get; }
        public string ApplicantName { get; }
        public MoneyAmount BasePremium { get; }
        public MoneyAmount Coverage { get; }
        public PlayerDecision ExpectedDecision { get; }
        public AccidentFlag AccidentFlag { get; }
        public GameDate ReturnDate { get; }
        public IReadOnlyList<DocumentRecord> Documents { get; }
        public SubjectiveInfoId? SubjectiveInfoId { get; }
        public SpecialContractorId? SpecialContractorId { get; }
    }

    public sealed class RuleTrigger
    {
        public RuleTrigger(RuleId ruleId, RuleSeverity severity, DocumentId? documentId, DocumentFieldId? fieldId, string explanationKey, string message)
        {
            RuleId = ruleId;
            Severity = severity;
            DocumentId = documentId;
            FieldId = fieldId;
            ExplanationKey = explanationKey ?? throw new ArgumentNullException(nameof(explanationKey));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public RuleId RuleId { get; }
        public RuleSeverity Severity { get; }
        public DocumentId? DocumentId { get; }
        public DocumentFieldId? FieldId { get; }
        public string ExplanationKey { get; }
        public string Message { get; }
    }

    public sealed class ReviewResult
    {
        public ReviewResult(ContractId contractId, IEnumerable<RuleTrigger> triggers)
        {
            ContractId = contractId;
            Triggers = (triggers ?? throw new ArgumentNullException(nameof(triggers))).ToList().AsReadOnly();
        }

        public ContractId ContractId { get; }
        public IReadOnlyList<RuleTrigger> Triggers { get; }
        public bool HasAbsoluteRejection => Triggers.Any(t => t.Severity == RuleSeverity.AbsoluteRejection);
        public int ConsiderationCount => Triggers.Count(t => t.Severity == RuleSeverity.RejectionConsideration);
    }

    public sealed class ReviewSubmission
    {
        public ReviewSubmission(ContractId contractId, PlayerDecision decision, IEnumerable<RuleId> checkedRuleIds)
        {
            ContractId = contractId;
            Decision = decision;
            CheckedRuleIds = (checkedRuleIds ?? Array.Empty<RuleId>()).ToList().AsReadOnly();
        }

        public ContractId ContractId { get; }
        public PlayerDecision Decision { get; }
        public IReadOnlyList<RuleId> CheckedRuleIds { get; }
    }

    public sealed class ActiveContractState
    {
        public ActiveContractState(ContractId contractId, GameDate returnDate, AccidentFlag accidentFlag, PlayerDecision acceptedDecision)
        {
            ContractId = contractId;
            ReturnDate = returnDate;
            AccidentFlag = accidentFlag;
            AcceptedDecision = acceptedDecision;
        }

        public ContractId ContractId { get; }
        public GameDate ReturnDate { get; }
        public AccidentFlag AccidentFlag { get; }
        public PlayerDecision AcceptedDecision { get; }
    }

    public sealed class OutcomeResult
    {
        public OutcomeResult(OutcomeId id, ContractId contractId, bool accidentOccurred, string summary)
        {
            Id = id;
            ContractId = contractId;
            AccidentOccurred = accidentOccurred;
            Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        public OutcomeId Id { get; }
        public ContractId ContractId { get; }
        public bool AccidentOccurred { get; }
        public string Summary { get; }
    }

    public sealed class SettlementLineItem
    {
        public SettlementLineItem(string label, ContractId? contractId, MoneyAmount moneyDelta, EvaluationScore scoreDelta)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            ContractId = contractId;
            MoneyDelta = moneyDelta;
            ScoreDelta = scoreDelta;
        }

        public string Label { get; }
        public ContractId? ContractId { get; }
        public MoneyAmount MoneyDelta { get; }
        public EvaluationScore ScoreDelta { get; }
    }

    public sealed class SettlementResult
    {
        public SettlementResult(IEnumerable<SettlementLineItem> lineItems)
        {
            LineItems = (lineItems ?? throw new ArgumentNullException(nameof(lineItems))).ToList().AsReadOnly();
        }

        public IReadOnlyList<SettlementLineItem> LineItems { get; }
        public int TotalMoneyDelta => LineItems.Sum(i => i.MoneyDelta.Ducats);
        public int TotalScoreDelta => LineItems.Sum(i => i.ScoreDelta.Value);
    }
}
