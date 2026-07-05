// Responsibility: Provides clearly marked in-memory fixture content for the first playable loop.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Domain;

namespace EpicProjectR.Content
{
    public enum RuleConditionType
    {
        MissingDocument,
        FieldEquals,
        FieldMismatch,
        DateBefore,
        NumberAtLeast
    }

    public sealed class RuleActivationWindow
    {
        public RuleActivationWindow(TurnNumber startTurn, TurnNumber endTurn, BundleId? bundleId = null, RouteId? routeId = null)
        {
            StartTurn = startTurn;
            EndTurn = endTurn;
            BundleId = bundleId;
            RouteId = routeId;
        }

        public TurnNumber StartTurn { get; }
        public TurnNumber EndTurn { get; }
        public BundleId? BundleId { get; }
        public RouteId? RouteId { get; }

        public bool IsActiveFor(ContractCase contractCase)
        {
            if (contractCase.Turn.Value < StartTurn.Value || contractCase.Turn.Value > EndTurn.Value)
            {
                return false;
            }

            if (BundleId.HasValue && !BundleId.Value.Equals(contractCase.BundleId))
            {
                return false;
            }

            if (RouteId.HasValue && !RouteId.Value.Equals(contractCase.RouteId))
            {
                return false;
            }

            return true;
        }
    }

    public sealed class RuleCondition
    {
        private RuleCondition(RuleConditionType type, DocumentKind documentKind, DocumentFieldId? fieldId, string expectedValue, string comparisonFieldValue)
        {
            Type = type;
            DocumentKind = documentKind;
            FieldId = fieldId;
            ExpectedValue = expectedValue;
            ComparisonFieldValue = comparisonFieldValue;
        }

        public RuleConditionType Type { get; }
        public DocumentKind DocumentKind { get; }
        public DocumentFieldId? FieldId { get; }
        public string ExpectedValue { get; }
        public string ComparisonFieldValue { get; }

        public static RuleCondition MissingDocument(DocumentKind documentKind)
        {
            return new RuleCondition(RuleConditionType.MissingDocument, documentKind, null, null, null);
        }

        public static RuleCondition FieldEquals(DocumentKind documentKind, DocumentFieldId fieldId, string expectedValue)
        {
            return new RuleCondition(RuleConditionType.FieldEquals, documentKind, fieldId, expectedValue, null);
        }

        public static RuleCondition FieldMismatch(DocumentKind documentKind, DocumentFieldId fieldId, string comparisonFieldValue)
        {
            return new RuleCondition(RuleConditionType.FieldMismatch, documentKind, fieldId, null, comparisonFieldValue);
        }

        public static RuleCondition DateBefore(DocumentKind documentKind, DocumentFieldId fieldId, string yyyyMmDd)
        {
            return new RuleCondition(RuleConditionType.DateBefore, documentKind, fieldId, yyyyMmDd, null);
        }

        public static RuleCondition NumberAtLeast(DocumentKind documentKind, DocumentFieldId fieldId, string minimum)
        {
            return new RuleCondition(RuleConditionType.NumberAtLeast, documentKind, fieldId, minimum, null);
        }
    }

    public sealed class RuleDefinition
    {
        public RuleDefinition(
            RuleId id,
            bool isFixture,
            RuleSeverity severity,
            string title,
            string explanationKey,
            RuleCondition condition,
            IEnumerable<RuleActivationWindow> windows)
        {
            Id = id;
            IsFixture = isFixture;
            Severity = severity;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ExplanationKey = explanationKey ?? throw new ArgumentNullException(nameof(explanationKey));
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            Windows = (windows ?? throw new ArgumentNullException(nameof(windows))).ToList().AsReadOnly();
        }

        public RuleId Id { get; }
        public bool IsFixture { get; }
        public RuleSeverity Severity { get; }
        public string Title { get; }
        public string ExplanationKey { get; }
        public RuleCondition Condition { get; }
        public IReadOnlyList<RuleActivationWindow> Windows { get; }
    }

    public interface IContractRepository
    {
        IReadOnlyList<ContractCase> GetAll();
        ContractCase Get(ContractId id);
    }

    public interface IRuleRepository
    {
        IReadOnlyList<RuleDefinition> GetAll();
    }

    public sealed class InMemoryContractRepository : IContractRepository
    {
        private readonly IReadOnlyList<ContractCase> contracts;
        private readonly IReadOnlyDictionary<ContractId, ContractCase> contractsById;

        public InMemoryContractRepository(IEnumerable<ContractCase> contracts)
        {
            var orderedContracts = (contracts ?? throw new ArgumentNullException(nameof(contracts))).ToList();
            this.contracts = orderedContracts.AsReadOnly();
            contractsById = orderedContracts.ToDictionary(contract => contract.Id);
        }

        public IReadOnlyList<ContractCase> GetAll()
        {
            return contracts;
        }

        public ContractCase Get(ContractId id)
        {
            return contractsById.TryGetValue(id, out var contractCase) ? contractCase : null;
        }
    }

    public sealed class InMemoryRuleRepository : IRuleRepository
    {
        private readonly IReadOnlyList<RuleDefinition> rules;

        public InMemoryRuleRepository(IEnumerable<RuleDefinition> rules)
        {
            this.rules = (rules ?? throw new ArgumentNullException(nameof(rules))).ToList().AsReadOnly();
        }

        public IReadOnlyList<RuleDefinition> GetAll()
        {
            return rules;
        }
    }

    public sealed class ContentValidationIssue
    {
        public ContentValidationIssue(string code, string message)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Code { get; }
        public string Message { get; }
    }

    public sealed class ContentValidationResult
    {
        public ContentValidationResult(IEnumerable<ContentValidationIssue> issues)
        {
            Issues = (issues ?? throw new ArgumentNullException(nameof(issues))).ToList().AsReadOnly();
        }

        public IReadOnlyList<ContentValidationIssue> Issues { get; }
        public bool IsValid => Issues.Count == 0;
    }

    public sealed class FixtureContentValidator
    {
        public ContentValidationResult Validate(IReadOnlyList<ContractCase> contracts, IReadOnlyList<RuleDefinition> rules)
        {
            var issues = new List<ContentValidationIssue>();
            AddDuplicateContractIssues(contracts, issues);
            AddDuplicateRuleIssues(rules, issues);
            AddContractIdFormulaIssues(contracts, issues);
            AddSubmittedStateIssues(contracts, issues);
            AddRuleReferenceIssues(contracts, rules, issues);
            return new ContentValidationResult(issues);
        }

        private static void AddDuplicateContractIssues(IReadOnlyList<ContractCase> contracts, List<ContentValidationIssue> issues)
        {
            foreach (var group in contracts.GroupBy(c => c.Id.ToString()).Where(g => g.Count() > 1))
            {
                issues.Add(new ContentValidationIssue("DUPLICATE_CONTRACT_ID", $"Duplicate contract ID {group.Key}."));
            }
        }

        private static void AddDuplicateRuleIssues(IReadOnlyList<RuleDefinition> rules, List<ContentValidationIssue> issues)
        {
            foreach (var group in rules.GroupBy(r => r.Id.ToString()).Where(g => g.Count() > 1))
            {
                issues.Add(new ContentValidationIssue("DUPLICATE_RULE_ID", $"Duplicate rule ID {group.Key}."));
            }
        }

        private static void AddContractIdFormulaIssues(IReadOnlyList<ContractCase> contracts, List<ContentValidationIssue> issues)
        {
            foreach (var contract in contracts)
            {
                var expected = $"C{((contract.Turn.Value - 1) * 7 + contract.Slot.Value):D3}";
                if (contract.Id.ToString() != expected)
                {
                    issues.Add(new ContentValidationIssue("CONTRACT_ID_FORMULA", $"{contract.Id} should be {expected} for turn {contract.Turn} slot {contract.Slot}."));
                }
            }
        }

        private static void AddSubmittedStateIssues(IReadOnlyList<ContractCase> contracts, List<ContentValidationIssue> issues)
        {
            foreach (var document in contracts.SelectMany(c => c.Documents))
            {
                if (!document.Submitted && document.Fields.Count > 0)
                {
                    issues.Add(new ContentValidationIssue("MISSING_DOCUMENT_HAS_FIELDS", $"{document.Id} is not submitted but has fields."));
                }
            }
        }

        private static void AddRuleReferenceIssues(IReadOnlyList<ContractCase> contracts, IReadOnlyList<RuleDefinition> rules, List<ContentValidationIssue> issues)
        {
            var documentKinds = new HashSet<DocumentKind>(contracts.SelectMany(c => c.Documents).Select(d => d.Kind));
            foreach (var rule in rules)
            {
                if (!documentKinds.Contains(rule.Condition.DocumentKind))
                {
                    issues.Add(new ContentValidationIssue("RULE_DOCUMENT_KIND_MISSING", $"{rule.Id} references unavailable document kind {rule.Condition.DocumentKind}."));
                }
            }
        }
    }

    public static class FirstPlayableFixtures
    {
        public static readonly BundleId Bundle1 = new BundleId("BUNDLE_1", SourceIdKind.Fixture);
        public static readonly BundleId Bundle2 = new BundleId("BUNDLE_2", SourceIdKind.Fixture);
        public static readonly BundleId Bundle3 = new BundleId("BUNDLE_3", SourceIdKind.Fixture);
        public static readonly RouteId RissebraToVelmora = new RouteId("R0005");

        public static IReadOnlyList<DocumentBundle> Bundles()
        {
            return new[]
            {
                new DocumentBundle(Bundle1, "Fixture Bundle 1: application + registration", new[] { DocumentKind.ShipApplication, DocumentKind.ShipRegistration }),
                new DocumentBundle(Bundle2, "Fixture Bundle 2: bundle 1 + hull inspection", new[] { DocumentKind.ShipApplication, DocumentKind.ShipRegistration, DocumentKind.HullInspection }),
                new DocumentBundle(Bundle3, "Fixture Bundle 3: bundle 2 + route declaration", new[] { DocumentKind.ShipApplication, DocumentKind.ShipRegistration, DocumentKind.HullInspection, DocumentKind.RouteDeclaration })
            };
        }

        public static IReadOnlyList<ContractCase> Contracts()
        {
            return new[]
            {
                CleanApprovalContract(),
                AbsoluteRejectionContract(),
                ConsiderationContract()
            };
        }

        public static IReadOnlyList<RuleDefinition> Rules()
        {
            var bundle1To3 = Windows(Bundle1, Bundle2, Bundle3);
            var bundle2To3 = Windows(Bundle2, Bundle3);
            var bundle3Only = Windows(Bundle3);
            return new[]
            {
                new RuleDefinition(new RuleId("AR01"), true, RuleSeverity.AbsoluteRejection, "Missing registration", "rule.ar01.missing_registration", RuleCondition.MissingDocument(DocumentKind.ShipRegistration), bundle1To3),
                new RuleDefinition(new RuleId("AR02"), true, RuleSeverity.AbsoluteRejection, "Ship name mismatch", "rule.ar02.ship_name_mismatch", RuleCondition.FieldMismatch(DocumentKind.ShipRegistration, Field("ShipName"), "ShipApplication.ShipName"), bundle1To3),
                new RuleDefinition(new RuleId("AR03"), true, RuleSeverity.AbsoluteRejection, "Owner mismatch", "rule.ar03.owner_mismatch", RuleCondition.FieldMismatch(DocumentKind.ShipRegistration, Field("OwnerName"), "ShipApplication.OwnerName"), bundle1To3),
                new RuleDefinition(new RuleId("AR04"), true, RuleSeverity.AbsoluteRejection, "Expired registration", "rule.ar04.registration_expired", RuleCondition.DateBefore(DocumentKind.ShipRegistration, Field("ExpiryDate"), "1599-01-15"), bundle1To3),
                new RuleDefinition(new RuleId("AR05"), true, RuleSeverity.AbsoluteRejection, "Missing hull inspection", "rule.ar05.missing_hull", RuleCondition.MissingDocument(DocumentKind.HullInspection), bundle2To3),
                new RuleDefinition(new RuleId("AR06"), true, RuleSeverity.AbsoluteRejection, "Failed hull inspection", "rule.ar06.failed_hull", RuleCondition.FieldEquals(DocumentKind.HullInspection, Field("InspectionResult"), "Failed"), bundle2To3),
                new RuleDefinition(new RuleId("AR07"), true, RuleSeverity.AbsoluteRejection, "Missing route declaration", "rule.ar07.missing_route", RuleCondition.MissingDocument(DocumentKind.RouteDeclaration), bundle3Only),
                new RuleDefinition(new RuleId("AR08"), true, RuleSeverity.AbsoluteRejection, "Departure date mismatch", "rule.ar08.departure_mismatch", RuleCondition.FieldMismatch(DocumentKind.RouteDeclaration, Field("DepartureDate"), "ShipApplication.DepartureDate"), bundle3Only),
                new RuleDefinition(new RuleId("CR01"), true, RuleSeverity.RejectionConsideration, "Old hull", "rule.cr01.old_hull", RuleCondition.NumberAtLeast(DocumentKind.ShipRegistration, Field("HullAge"), "15"), bundle1To3),
                new RuleDefinition(new RuleId("CR02"), true, RuleSeverity.RejectionConsideration, "Accident history", "rule.cr02.accident_history", RuleCondition.FieldEquals(DocumentKind.HullInspection, Field("AccidentHistory"), "Recent"), bundle2To3),
                new RuleDefinition(new RuleId("CR03"), true, RuleSeverity.RejectionConsideration, "Repair history", "rule.cr03.repair_history", RuleCondition.FieldEquals(DocumentKind.HullInspection, Field("RepairHistory"), "Major"), bundle2To3),
                new RuleDefinition(new RuleId("CR04"), true, RuleSeverity.RejectionConsideration, "Hull defect", "rule.cr04.hull_defect", RuleCondition.FieldEquals(DocumentKind.HullInspection, Field("HullDefect"), "Present"), bundle2To3),
                new RuleDefinition(new RuleId("CR05"), true, RuleSeverity.RejectionConsideration, "Bad weather", "rule.cr05.bad_weather", RuleCondition.FieldEquals(DocumentKind.RouteDeclaration, Field("WeatherForecast"), "Storm"), bundle3Only),
                new RuleDefinition(new RuleId("CR06"), true, RuleSeverity.RejectionConsideration, "Uncharted route", "rule.cr06.uncharted_route", RuleCondition.FieldEquals(DocumentKind.RouteDeclaration, Field("RouteRisk"), "Uncharted"), bundle3Only)
            };
        }

        private static IReadOnlyList<RuleActivationWindow> Windows(params BundleId[] bundles)
        {
            return bundles.Select(bundle => new RuleActivationWindow(new TurnNumber(1), new TurnNumber(24), bundle)).ToList();
        }

        public static InMemoryContractRepository ContractRepository()
        {
            return new InMemoryContractRepository(Contracts());
        }

        public static InMemoryRuleRepository RuleRepository()
        {
            return new InMemoryRuleRepository(Rules());
        }

        private static ContractCase CleanApprovalContract()
        {
            return new ContractCase(
                new ContractId("C001"),
                true,
                new TurnNumber(1),
                new TurnSlot(1),
                ContractType.Ship,
                Bundle1,
                RissebraToVelmora,
                "Firs de Alvarenga",
                new MoneyAmount(24),
                new MoneyAmount(600),
                PlayerDecision.Approve,
                new AccidentFlag(false),
                new GameDate(1599, 2, 15),
                new[] { ShipApplication("C001-APP", "Santa Luzia", "Firs de Alvarenga", "1599-01-20"), ShipRegistration("C001-REG", "Santa Luzia", "Firs de Alvarenga", "8", "1601-01-01") });
        }

        private static ContractCase AbsoluteRejectionContract()
        {
            return new ContractCase(
                new ContractId("C002"),
                true,
                new TurnNumber(1),
                new TurnSlot(2),
                ContractType.Ship,
                Bundle2,
                RissebraToVelmora,
                "Helena Duarte",
                new MoneyAmount(31),
                new MoneyAmount(700),
                PlayerDecision.Reject,
                new AccidentFlag(true),
                new GameDate(1599, 2, 20),
                new[] { ShipApplication("C002-APP", "Mar Azul", "Helena Duarte", "1599-01-22"), Missing(DocumentKind.ShipRegistration, "C002-REG"), HullInspection("C002-HULL", "Passed", "None", "None", "None") });
        }

        private static ContractCase ConsiderationContract()
        {
            return new ContractCase(
                new ContractId("C003"),
                true,
                new TurnNumber(1),
                new TurnSlot(3),
                ContractType.Ship,
                Bundle3,
                RissebraToVelmora,
                "Mateo Reis",
                new MoneyAmount(36),
                new MoneyAmount(850),
                PlayerDecision.Approve,
                new AccidentFlag(true),
                new GameDate(1599, 2, 28),
                new[]
                {
                    ShipApplication("C003-APP", "Estrela Norte", "Mateo Reis", "1599-01-20"),
                    ShipRegistration("C003-REG", "Estrela Norte", "Mateo Reis", "16", "1600-12-01"),
                    HullInspection("C003-HULL", "Passed", "Recent", "None", "None"),
                    RouteDeclaration("C003-ROUTE", "1599-01-20", "Clear", "Known")
                });
        }

        private static DocumentRecord ShipApplication(string id, string shipName, string owner, string departure)
        {
            return new DocumentRecord(new DocumentId(id, SourceIdKind.Fixture), DocumentKind.ShipApplication, "Ship Insurance Application", true, new[]
            {
                FieldValue("ShipName", "Ship name", shipName),
                FieldValue("OwnerName", "Owner", owner),
                FieldValue("DepartureDate", "Departure date", departure)
            });
        }

        private static DocumentRecord ShipRegistration(string id, string shipName, string owner, string hullAge, string expiry)
        {
            return new DocumentRecord(new DocumentId(id, SourceIdKind.Fixture), DocumentKind.ShipRegistration, "Ship Registration Certificate", true, new[]
            {
                FieldValue("ShipName", "Ship name", shipName),
                FieldValue("OwnerName", "Owner", owner),
                FieldValue("HullAge", "Hull age", hullAge),
                FieldValue("ExpiryDate", "Expiry date", expiry)
            });
        }

        private static DocumentRecord HullInspection(string id, string result, string accidentHistory, string repairHistory, string defect)
        {
            return new DocumentRecord(new DocumentId(id, SourceIdKind.Fixture), DocumentKind.HullInspection, "Hull Inspection Certificate", true, new[]
            {
                FieldValue("InspectionResult", "Inspection result", result),
                FieldValue("AccidentHistory", "Accident history", accidentHistory),
                FieldValue("RepairHistory", "Repair history", repairHistory),
                FieldValue("HullDefect", "Hull defect", defect)
            });
        }

        private static DocumentRecord RouteDeclaration(string id, string departure, string weather, string routeRisk)
        {
            return new DocumentRecord(new DocumentId(id, SourceIdKind.Fixture), DocumentKind.RouteDeclaration, "Route Declaration", true, new[]
            {
                FieldValue("DepartureDate", "Departure date", departure),
                FieldValue("WeatherForecast", "Weather forecast", weather),
                FieldValue("RouteRisk", "Route risk", routeRisk)
            });
        }

        private static DocumentRecord Missing(DocumentKind kind, string id)
        {
            return new DocumentRecord(new DocumentId(id, SourceIdKind.Fixture), kind, $"{kind} (missing)", false, Array.Empty<DocumentField>());
        }

        private static DocumentField FieldValue(string id, string label, string value)
        {
            return new DocumentField(Field(id), label, value);
        }

        private static DocumentFieldId Field(string id)
        {
            return new DocumentFieldId(id, SourceIdKind.Fixture);
        }
    }
}
