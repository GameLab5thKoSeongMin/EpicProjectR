// Responsibility: Defines data-only screen state objects passed from presenter to the first playable view.
using System;
using System.Collections.Generic;
using System.Linq;
using EpicProjectR.Content;
using EpicProjectR.Domain;

namespace EpicProjectR.Presentation
{
    public enum FirstPlayableScreenMode
    {
        MainWaitingForBell,
        CharacterArriving,
        DocumentPresented,
        OpeningReview,
        Reviewing,
        DecisionPaperClosed,
        DecisionPaperOpen,
        DecisionDrawerOpening,
        DecisionSubmitted,
        CharacterExiting,
        DecisionReady,
        Result,
        Completed
    }

    public sealed class FirstPlayableRuleRowState
    {
        public FirstPlayableRuleRowState(RuleDefinition rule, bool isTriggered)
        {
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
            IsTriggered = isTriggered;
        }

        public RuleDefinition Rule { get; }
        public bool IsTriggered { get; }
    }

    public sealed class FirstPlayableHudState
    {
        public FirstPlayableHudState(string title, string date, int ducats, int reputation)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Date = date ?? throw new ArgumentNullException(nameof(date));
            Ducats = ducats;
            Reputation = reputation;
        }

        public string Title { get; }
        public string Date { get; }
        public int Ducats { get; }
        public int Reputation { get; }
    }

    public sealed class FirstPlayableEntryScreenState
    {
        public FirstPlayableEntryScreenState(
            string headerMeta,
            string docket,
            string caseSummary,
            string entryTitle,
            string entryPrompt)
        {
            Mode = FirstPlayableScreenMode.MainWaitingForBell;
            HeaderMeta = headerMeta ?? throw new ArgumentNullException(nameof(headerMeta));
            Docket = docket ?? throw new ArgumentNullException(nameof(docket));
            CaseSummary = caseSummary ?? throw new ArgumentNullException(nameof(caseSummary));
            EntryTitle = entryTitle ?? throw new ArgumentNullException(nameof(entryTitle));
            EntryPrompt = entryPrompt ?? throw new ArgumentNullException(nameof(entryPrompt));
        }

        public FirstPlayableScreenMode Mode { get; }
        public string HeaderMeta { get; }
        public string Docket { get; }
        public string CaseSummary { get; }
        public string EntryTitle { get; }
        public string EntryPrompt { get; }
    }

    public sealed class FirstPlayableReviewScreenState
    {
        public FirstPlayableReviewScreenState(
            string headerMeta,
            string docket,
            string caseSummary,
            string reviewStatus,
            string premium,
            string compensation,
            IEnumerable<string> dialogueLines,
            IEnumerable<DocumentRecord> documents,
            IEnumerable<FirstPlayableRuleRowState> rules)
        {
            Mode = FirstPlayableScreenMode.Reviewing;
            HeaderMeta = headerMeta ?? throw new ArgumentNullException(nameof(headerMeta));
            Docket = docket ?? throw new ArgumentNullException(nameof(docket));
            CaseSummary = caseSummary ?? throw new ArgumentNullException(nameof(caseSummary));
            ReviewStatus = reviewStatus ?? throw new ArgumentNullException(nameof(reviewStatus));
            Premium = premium ?? throw new ArgumentNullException(nameof(premium));
            Compensation = compensation ?? throw new ArgumentNullException(nameof(compensation));
            DialogueLines = (dialogueLines ?? throw new ArgumentNullException(nameof(dialogueLines))).ToList().AsReadOnly();
            Documents = (documents ?? throw new ArgumentNullException(nameof(documents))).ToList().AsReadOnly();
            Rules = (rules ?? throw new ArgumentNullException(nameof(rules))).ToList().AsReadOnly();
        }

        public FirstPlayableScreenMode Mode { get; }
        public string HeaderMeta { get; }
        public string Docket { get; }
        public string CaseSummary { get; }
        public string ReviewStatus { get; }
        public string Premium { get; }
        public string Compensation { get; }
        public IReadOnlyList<string> DialogueLines { get; }
        public IReadOnlyList<DocumentRecord> Documents { get; }
        public IReadOnlyList<FirstPlayableRuleRowState> Rules { get; }
    }

    public sealed class FirstPlayableResultScreenState
    {
        public FirstPlayableResultScreenState(
            string headerMeta,
            string premium,
            string result,
            string outcome,
            string settlement,
            string nextButton,
            bool auditCorrect,
            bool outcomeWarning)
        {
            Mode = FirstPlayableScreenMode.Result;
            HeaderMeta = headerMeta ?? throw new ArgumentNullException(nameof(headerMeta));
            Premium = premium ?? throw new ArgumentNullException(nameof(premium));
            Result = result ?? throw new ArgumentNullException(nameof(result));
            Outcome = outcome ?? throw new ArgumentNullException(nameof(outcome));
            Settlement = settlement ?? throw new ArgumentNullException(nameof(settlement));
            NextButton = nextButton ?? throw new ArgumentNullException(nameof(nextButton));
            AuditCorrect = auditCorrect;
            OutcomeWarning = outcomeWarning;
        }

        public FirstPlayableScreenMode Mode { get; }
        public string HeaderMeta { get; }
        public string Premium { get; }
        public string Result { get; }
        public string Outcome { get; }
        public string Settlement { get; }
        public string NextButton { get; }
        public bool AuditCorrect { get; }
        public bool OutcomeWarning { get; }
    }

    public sealed class FirstPlayableCompleteScreenState
    {
        public FirstPlayableCompleteScreenState(
            string headerMeta,
            string docket,
            string caseSummary,
            string reviewStatus,
            string premium,
            string result,
            string outcome,
            string settlement)
        {
            Mode = FirstPlayableScreenMode.Completed;
            HeaderMeta = headerMeta ?? throw new ArgumentNullException(nameof(headerMeta));
            Docket = docket ?? throw new ArgumentNullException(nameof(docket));
            CaseSummary = caseSummary ?? throw new ArgumentNullException(nameof(caseSummary));
            ReviewStatus = reviewStatus ?? throw new ArgumentNullException(nameof(reviewStatus));
            Premium = premium ?? throw new ArgumentNullException(nameof(premium));
            Result = result ?? throw new ArgumentNullException(nameof(result));
            Outcome = outcome ?? throw new ArgumentNullException(nameof(outcome));
            Settlement = settlement ?? throw new ArgumentNullException(nameof(settlement));
        }

        public FirstPlayableScreenMode Mode { get; }
        public string HeaderMeta { get; }
        public string Docket { get; }
        public string CaseSummary { get; }
        public string ReviewStatus { get; }
        public string Premium { get; }
        public string Result { get; }
        public string Outcome { get; }
        public string Settlement { get; }
    }
}
