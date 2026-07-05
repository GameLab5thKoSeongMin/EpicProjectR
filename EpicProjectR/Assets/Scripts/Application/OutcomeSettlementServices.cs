// Responsibility: Resolves deterministic contract outcomes and minimal traceable settlement feedback.
using System.Collections.Generic;
using EpicProjectR.Domain;

namespace EpicProjectR.Application
{
    public sealed class OutcomeResolver
    {
        public OutcomeResult Resolve(ActiveContractState activeContract, GameDate currentDate)
        {
            if (currentDate >= activeContract.ReturnDate && activeContract.AccidentFlag.WillAccidentIfApproved)
            {
                return new OutcomeResult(
                    new OutcomeId($"OUT-{activeContract.ContractId}", SourceIdKind.RuntimeGenerated),
                    activeContract.ContractId,
                    true,
                    "Accident occurred on return. The hidden fixture AccidentFlag resolved deterministically.");
            }

            return new OutcomeResult(
                new OutcomeId($"OUT-{activeContract.ContractId}", SourceIdKind.RuntimeGenerated),
                activeContract.ContractId,
                false,
                "Contract returned safely.");
        }

        public OutcomeResult ResolveRejected(ContractCase contractCase)
        {
            return new OutcomeResult(
                new OutcomeId($"OUT-{contractCase.Id}", SourceIdKind.RuntimeGenerated),
                contractCase.Id,
                false,
                "Rejected contract created no regular accident.");
        }
    }

    public sealed class SettlementService
    {
        public SettlementResult Settle(ContractCase contractCase, DecisionAuditResult audit, PremiumQuoteResult premium, OutcomeResult outcome)
        {
            var lines = new List<SettlementLineItem>
            {
                new SettlementLineItem("Objective audit score", contractCase.Id, new MoneyAmount(0), audit.ScoreDelta)
            };

            if (audit.Decision == PlayerDecision.Approve || audit.Decision == PlayerDecision.ConditionalApprove)
            {
                var commission = contractCase.BasePremium.Ducats * premium.MultiplierPercent / 100;
                lines.Add(new SettlementLineItem($"Fixture commission at {premium.MultiplierPercent}%", contractCase.Id, new MoneyAmount(commission), new EvaluationScore(0)));
            }

            if (outcome.AccidentOccurred)
            {
                lines.Add(new SettlementLineItem("Fixture accident responsibility placeholder", contractCase.Id, new MoneyAmount(-15), new EvaluationScore(0)));
            }

            return new SettlementResult(lines);
        }
    }
}
