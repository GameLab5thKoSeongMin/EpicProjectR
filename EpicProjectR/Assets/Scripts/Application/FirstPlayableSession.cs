// Responsibility: Coordinates fixture contract review, decisions, active contracts, outcomes, and settlement.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Content;
using EpicProjectR.Domain;

namespace EpicProjectR.Application
{
    public sealed class TurnDefinition
    {
        public TurnDefinition(TurnNumber turnNumber, GameDate date, IEnumerable<ContractId> contractIds)
        {
            TurnNumber = turnNumber;
            Date = date;
            ContractIds = contractIds.ToList().AsReadOnly();
        }

        public TurnNumber TurnNumber { get; }
        public GameDate Date { get; }
        public IReadOnlyList<ContractId> ContractIds { get; }
    }

    public sealed class SubmissionResult
    {
        public SubmissionResult(DecisionAuditResult audit, PremiumQuoteResult premium, OutcomeResult outcome, SettlementResult settlement, bool activeContractCreated)
        {
            Audit = audit;
            Premium = premium;
            Outcome = outcome;
            Settlement = settlement;
            ActiveContractCreated = activeContractCreated;
        }

        public DecisionAuditResult Audit { get; }
        public PremiumQuoteResult Premium { get; }
        public OutcomeResult Outcome { get; }
        public SettlementResult Settlement { get; }
        public bool ActiveContractCreated { get; }
    }

    public sealed class FirstPlayableSession
    {
        private readonly IContractRepository contractRepository;
        private readonly RuleReviewService reviewService;
        private readonly DecisionAuditService decisionAuditService;
        private readonly OutcomeResolver outcomeResolver;
        private readonly SettlementService settlementService;
        private readonly IReadOnlyList<ContractId> contractQueue;
        private readonly List<ActiveContractState> activeContracts = new List<ActiveContractState>();
        private int currentIndex;

        public FirstPlayableSession(
            IContractRepository contractRepository,
            RuleReviewService reviewService,
            DecisionAuditService decisionAuditService,
            OutcomeResolver outcomeResolver,
            SettlementService settlementService,
            TurnDefinition turnDefinition)
        {
            this.contractRepository = contractRepository ?? throw new ArgumentNullException(nameof(contractRepository));
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.decisionAuditService = decisionAuditService ?? throw new ArgumentNullException(nameof(decisionAuditService));
            this.outcomeResolver = outcomeResolver ?? throw new ArgumentNullException(nameof(outcomeResolver));
            this.settlementService = settlementService ?? throw new ArgumentNullException(nameof(settlementService));
            TurnDefinition = turnDefinition ?? throw new ArgumentNullException(nameof(turnDefinition));
            contractQueue = turnDefinition.ContractIds;
        }

        public TurnDefinition TurnDefinition { get; }
        public int CurrentIndex => currentIndex;
        public bool IsComplete => currentIndex >= contractQueue.Count;
        public IReadOnlyList<ActiveContractState> ActiveContracts => activeContracts.AsReadOnly();

        public ContractCase CurrentContract => IsComplete ? null : contractRepository.Get(contractQueue[currentIndex]);

        public ReviewResult CurrentReview()
        {
            var contract = CurrentContract;
            return contract == null ? null : reviewService.Review(contract);
        }

        public SubmissionResult Submit(PlayerDecision decision, IEnumerable<RuleId> checkedRuleIds)
        {
            var contract = CurrentContract;
            if (contract == null)
            {
                throw new InvalidOperationException("No current contract to submit.");
            }

            var review = reviewService.Review(contract);
            var submission = new ReviewSubmission(contract.Id, decision, checkedRuleIds ?? Array.Empty<RuleId>());
            var audit = decisionAuditService.Audit(review, submission);
            var premium = decisionAuditService.QuotePremium(review);
            var activeCreated = false;
            OutcomeResult outcome = null;

            if (decision == PlayerDecision.Approve || decision == PlayerDecision.ConditionalApprove)
            {
                var active = new ActiveContractState(contract.Id, contract.ReturnDate, contract.AccidentFlag, decision);
                activeContracts.Add(active);
                activeCreated = true;
                outcome = outcomeResolver.Resolve(active, contract.ReturnDate);
            }
            else
            {
                outcome = outcomeResolver.ResolveRejected(contract);
            }

            var settlement = settlementService.Settle(contract, audit, premium, outcome);
            currentIndex++;
            return new SubmissionResult(audit, premium, outcome, settlement, activeCreated);
        }
    }

    public static class FirstPlayableSessionFactory
    {
        public static FirstPlayableSession Create()
        {
            var contracts = FirstPlayableFixtures.Contracts();
            var contractRepository = new InMemoryContractRepository(contracts);
            var ruleRepository = FirstPlayableFixtures.RuleRepository();
            var reviewService = new RuleReviewService(ruleRepository);
            var auditService = new DecisionAuditService();
            var outcomeResolver = new OutcomeResolver();
            var settlementService = new SettlementService();
            var turn = new TurnDefinition(new TurnNumber(1), new GameDate(1599, 1, 15), contracts.Select(c => c.Id));
            return new FirstPlayableSession(contractRepository, reviewService, auditService, outcomeResolver, settlementService, turn);
        }
    }
}
