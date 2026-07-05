// Responsibility: Verifies first playable domain, fixture, rule, audit, and outcome behavior.
using System.Linq;
using EpicProjectR.Application;
using EpicProjectR.Content;
using EpicProjectR.Domain;
using EpicProjectR.Presentation;
using NUnit.Framework;

namespace EpicProjectR.Tests.EditMode
{
    public sealed class FirstPlayableDomainTests
    {
        [Test]
        public void SourceIdsPreserveExactStrings()
        {
            Assert.AreEqual("C001", new ContractId("C001").ToString());
            Assert.AreEqual("AR01", new RuleId("AR01").ToString());
            Assert.AreEqual("CR01", new RuleId("CR01").ToString());
            Assert.AreEqual("SC01", new SpecialContractorId("SC01").ToString());
            Assert.AreEqual("R0005", new RouteId("R0005").ToString());
            Assert.AreEqual(new ContractId("C001"), new ContractId("C001"));
        }

        [Test]
        public void SourceIdsRejectInvalidSourceForms()
        {
            Assert.Throws<System.ArgumentException>(() => new ContractId("1"));
            Assert.Throws<System.ArgumentException>(() => new RuleId("AR1"));
            Assert.Throws<System.ArgumentException>(() => new SpecialContractorId("SpecialContractor1"));
            Assert.Throws<System.ArgumentException>(() => new RouteId("Route5"));
        }

        [Test]
        public void TurnAndSlotValidationWorks()
        {
            Assert.AreEqual(1, new TurnNumber(1).Value);
            Assert.AreEqual(7, new TurnSlot(7).Value);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new TurnNumber(0));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new TurnSlot(8));
            Assert.AreEqual(PlayerDecision.ConditionalApprove, PlayerDecision.ConditionalApprove);
        }

        [Test]
        public void FixturesValidateAndRemainMarkedAsFixtures()
        {
            var contracts = FirstPlayableFixtures.Contracts();
            var rules = FirstPlayableFixtures.Rules();
            var result = new FixtureContentValidator().Validate(contracts, rules);
            Assert.IsTrue(result.IsValid, string.Join("\n", result.Issues.Select(i => i.Message)));
            Assert.IsTrue(contracts.All(c => c.IsFixture));
            Assert.IsTrue(rules.All(r => r.IsFixture));
        }

        [Test]
        public void RuleEngineProducesExpectedFixtureResults()
        {
            var service = new RuleReviewService(FirstPlayableFixtures.RuleRepository());
            var contracts = FirstPlayableFixtures.Contracts();

            var clean = service.Review(contracts[0]);
            Assert.AreEqual(0, clean.Triggers.Count);

            var reject = service.Review(contracts[1]);
            Assert.IsTrue(reject.Triggers.Any(t => t.RuleId.Equals(new RuleId("AR01"))));
            Assert.IsTrue(reject.HasAbsoluteRejection);

            var consideration = service.Review(contracts[2]);
            Assert.IsFalse(consideration.HasAbsoluteRejection);
            Assert.IsTrue(consideration.Triggers.Any(t => t.RuleId.Equals(new RuleId("CR01"))));
            Assert.IsTrue(consideration.Triggers.Any(t => t.RuleId.Equals(new RuleId("CR02"))));
        }

        [Test]
        public void DecisionAuditAndPremiumAreDeterministic()
        {
            var service = new RuleReviewService(FirstPlayableFixtures.RuleRepository());
            var audit = new DecisionAuditService();
            var contract = FirstPlayableFixtures.Contracts()[2];
            var review = service.Review(contract);
            var submission = new ReviewSubmission(contract.Id, PlayerDecision.Approve, review.Triggers.Select(t => t.RuleId));

            var result = audit.Audit(review, submission);
            var premium = audit.QuotePremium(review);

            Assert.IsTrue(result.IsCorrectAction);
            Assert.AreEqual(2, result.CorrectCheckedReasons.Count);
            Assert.AreEqual(150, premium.MultiplierPercent);
            Assert.IsFalse(premium.RejectRecommended);
        }

        [Test]
        public void SessionCreatesActiveContractsAndRejectedFlaggedContractDoesNotAccident()
        {
            var session = FirstPlayableSessionFactory.Create();
            var first = session.CurrentReview();
            var firstResult = session.Submit(PlayerDecision.Approve, first.Triggers.Select(t => t.RuleId));
            Assert.IsTrue(firstResult.ActiveContractCreated);
            Assert.IsFalse(firstResult.Outcome.AccidentOccurred);

            var second = session.CurrentReview();
            var secondResult = session.Submit(PlayerDecision.Reject, second.Triggers.Select(t => t.RuleId));
            Assert.IsFalse(secondResult.ActiveContractCreated);
            Assert.IsFalse(secondResult.Outcome.AccidentOccurred);

            var third = session.CurrentReview();
            var thirdResult = session.Submit(PlayerDecision.Approve, third.Triggers.Select(t => t.RuleId));
            Assert.IsTrue(thirdResult.ActiveContractCreated);
            Assert.IsTrue(thirdResult.Outcome.AccidentOccurred);
        }

        [Test]
        public void FirstPlayableSessionCanAdvanceThroughRequiredNextAndFinishFlow()
        {
            var session = FirstPlayableSessionFactory.Create();

            Assert.AreEqual("C001", session.CurrentContract.Id.ToString());
            var firstResult = session.Submit(PlayerDecision.Approve, Enumerable.Empty<RuleId>());
            Assert.IsTrue(firstResult.Audit.IsCorrectAction);
            Assert.AreEqual("C002", session.CurrentContract.Id.ToString());

            var secondReview = session.CurrentReview();
            var secondResult = session.Submit(PlayerDecision.Reject, secondReview.Triggers.Select(t => t.RuleId));
            Assert.IsTrue(secondResult.Audit.IsCorrectAction);
            Assert.IsFalse(secondResult.ActiveContractCreated);
            Assert.IsFalse(secondResult.Outcome.AccidentOccurred);
            Assert.AreEqual("C003", session.CurrentContract.Id.ToString());

            var thirdReview = session.CurrentReview();
            var thirdResult = session.Submit(PlayerDecision.Approve, thirdReview.Triggers.Select(t => t.RuleId));
            Assert.IsTrue(thirdResult.Audit.IsCorrectAction);
            Assert.AreEqual(150, thirdResult.Premium.MultiplierPercent);
            Assert.IsTrue(thirdResult.Outcome.AccidentOccurred);
            Assert.IsTrue(session.IsComplete);
        }

        [Test]
        public void KoreanTextCatalogKeepsSourceIdsExact()
        {
            var header = FirstPlayableKoreanText.HeaderMeta(new TurnNumber(1), new GameDate(1599, 1, 15), new ContractId("C001"), FirstPlayableKoreanText.ReviewingStatus());
            Assert.IsTrue(header.Contains("C001"));
            Assert.IsTrue(FirstPlayableKoreanText.ApproveButton.Contains("승인"));
            Assert.IsTrue(FirstPlayableKoreanText.RejectButton.Contains("거절"));
            Assert.AreEqual("AR", FirstPlayableKoreanText.RuleSeverityMarker(RuleSeverity.AbsoluteRejection));
            Assert.AreEqual("CR", FirstPlayableKoreanText.RuleSeverityMarker(RuleSeverity.RejectionConsideration));
        }

        [Test]
        public void KoreanOutcomeTextDoesNotExposeHiddenAccidentFlagName()
        {
            var outcome = new OutcomeResult(new OutcomeId("OUT-C003"), new ContractId("C003"), true, "Accident occurred on return. The hidden fixture AccidentFlag resolved deterministically.");
            var text = FirstPlayableKoreanText.OutcomeSummary(outcome);

            Assert.IsFalse(text.Contains("AccidentFlag"));
            Assert.IsFalse(text.Contains("WillAccidentIfApproved"));
            Assert.IsTrue(text.Contains("사고"));
        }
    }
}
