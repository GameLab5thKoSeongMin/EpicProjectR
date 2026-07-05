// Responsibility: Defines source ID value objects that preserve exact authored strings.
using System;
using System.Text.RegularExpressions;

namespace EpicProjectR.Domain
{
    public enum SourceIdKind
    {
        SourceAuthored,
        Fixture,
        RuntimeGenerated,
        DisplayKey
    }

    public readonly struct SourceId : IEquatable<SourceId>
    {
        public SourceId(string value, SourceIdKind kind)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Source ID cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(SourceId other)
        {
            return Value == other.Value && Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            return obj is SourceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0) * 397) ^ (int)Kind;
            }
        }
    }

    public readonly struct ContractId : IEquatable<ContractId>
    {
        private static readonly Regex SourcePattern = new Regex("^C[0-9]{3}$", RegexOptions.Compiled);

        public ContractId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            Validate(value, kind);
            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(ContractId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is ContractId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;

        private static void Validate(string value, SourceIdKind kind)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ContractId cannot be empty.", nameof(value));
            }

            if (kind == SourceIdKind.SourceAuthored && !SourcePattern.IsMatch(value))
            {
                throw new ArgumentException("ContractId must preserve source form such as C001.", nameof(value));
            }
        }
    }

    public readonly struct RuleId : IEquatable<RuleId>
    {
        private static readonly Regex SourcePattern = new Regex("^(AR|CR)[0-9]{2}$", RegexOptions.Compiled);

        public RuleId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            Validate(value, kind);
            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(RuleId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is RuleId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;

        private static void Validate(string value, SourceIdKind kind)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("RuleId cannot be empty.", nameof(value));
            }

            if (kind == SourceIdKind.SourceAuthored && !SourcePattern.IsMatch(value))
            {
                throw new ArgumentException("RuleId must preserve source form such as AR01 or CR01.", nameof(value));
            }
        }
    }

    public readonly struct DocumentId : IEquatable<DocumentId>
    {
        public DocumentId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("DocumentId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(DocumentId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is DocumentId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct DocumentFieldId : IEquatable<DocumentFieldId>
    {
        public DocumentFieldId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("DocumentFieldId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(DocumentFieldId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is DocumentFieldId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct BundleId : IEquatable<BundleId>
    {
        public BundleId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("BundleId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(BundleId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is BundleId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct RouteId : IEquatable<RouteId>
    {
        private static readonly Regex SourcePattern = new Regex("^R[0-9]{4}$", RegexOptions.Compiled);

        public RouteId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("RouteId cannot be empty.", nameof(value));
            }

            if (kind == SourceIdKind.SourceAuthored && !SourcePattern.IsMatch(value))
            {
                throw new ArgumentException("RouteId must preserve source form such as R0005.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(RouteId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is RouteId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct NewsId : IEquatable<NewsId>
    {
        public NewsId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("NewsId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(NewsId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is NewsId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct SpecialContractorId : IEquatable<SpecialContractorId>
    {
        private static readonly Regex SourcePattern = new Regex("^SC[0-9]{2}$", RegexOptions.Compiled);

        public SpecialContractorId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("SpecialContractorId cannot be empty.", nameof(value));
            }

            if (kind == SourceIdKind.SourceAuthored && !SourcePattern.IsMatch(value))
            {
                throw new ArgumentException("SpecialContractorId must preserve source form such as SC01.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(SpecialContractorId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is SpecialContractorId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct SubjectiveInfoId : IEquatable<SubjectiveInfoId>
    {
        public SubjectiveInfoId(string value, SourceIdKind kind = SourceIdKind.SourceAuthored)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("SubjectiveInfoId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(SubjectiveInfoId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is SubjectiveInfoId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }

    public readonly struct OutcomeId : IEquatable<OutcomeId>
    {
        public OutcomeId(string value, SourceIdKind kind = SourceIdKind.RuntimeGenerated)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("OutcomeId cannot be empty.", nameof(value));
            }

            Value = value;
            Kind = kind;
        }

        public string Value { get; }
        public SourceIdKind Kind { get; }
        public override string ToString() => Value;
        public bool Equals(OutcomeId other) => Value == other.Value && Kind == other.Kind;
        public override bool Equals(object obj) => obj is OutcomeId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode() ^ (int)Kind;
    }
}
