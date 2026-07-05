using System.Collections.Generic;

public static class DialogueSequenceBuilder
{
    public static DialogueSequence BuildGreetingSequence(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        IReadOnlyList<DialogueClaim> claims)
    {
        return BuildGreetingSequence(
            profile,
            underwritingCase,
            adventurerIndex,
            claims,
            null);
    }

    public static DialogueSequence BuildGreetingSequence(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        IReadOnlyList<DialogueClaim> claims,
        int? seed)
    {
        List<DialogueLine> lines = new List<DialogueLine>();
        AdventurerDocument adventurer = GetAdventurer(underwritingCase, adventurerIndex);
        DialogueRuntimeContext context = new DialogueRuntimeContext(
            underwritingCase,
            adventurer,
            adventurerIndex,
            null);

        AddLines(lines, profile, DialogueTemplateKind.Greeting, context);
        AddLines(lines, profile, DialogueTemplateKind.QuestIntroduction, context);
        AddClaimLines(lines, profile, context, claims);
        DialogueSequence sequence = CreateRuntimeSequence(lines, underwritingCase, adventurerIndex, adventurer);
        SetTimelineCues(sequence, profile);

        if (profile != null)
        {
            sequence.SetAdditionalSubmissionDocument(
                profile.GreetingAdditionalSubmissionDocumentPrefab,
                DialogueAdditionalSubmissionTiming.OnEnd);
        }

        return sequence;
    }

    public static DialogueSequence BuildPressureResponse(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        DialogueClaim claim)
    {
        return BuildPressureResponse(
            profile,
            underwritingCase,
            adventurerIndex,
            claim,
            null);
    }

    public static DialogueSequence BuildPressureResponse(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        DialogueClaim claim,
        int? seed)
    {
        List<DialogueLine> lines = new List<DialogueLine>();
        AdventurerDocument adventurer = GetAdventurer(underwritingCase, adventurerIndex);
        DialogueRuntimeContext context = new DialogueRuntimeContext(
            underwritingCase,
            adventurer,
            adventurerIndex,
            claim);

        DialogueTemplateKind kind = claim != null && claim.isLie
            ? DialogueTemplateKind.LiePressureReaction
            : DialogueTemplateKind.TruthPressureReaction;

        AddLines(lines, profile, kind, context);
        DialogueSequence sequence = CreateRuntimeSequence(lines, underwritingCase, adventurerIndex, adventurer);
        SetTimelineCues(sequence, profile);
        return sequence;
    }

    public static DialogueSequence BuildKeyedResponse(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        string responseKey,
        DialogueClaim claim)
    {
        return BuildKeyedResponse(
            profile,
            underwritingCase,
            adventurerIndex,
            responseKey,
            claim,
            null);
    }

    public static DialogueSequence BuildKeyedResponse(
        ContractorDialogueProfile profile,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        string responseKey,
        DialogueClaim claim,
        int? seed)
    {
        List<DialogueLine> lines = new List<DialogueLine>();
        AdventurerDocument adventurer = GetAdventurer(underwritingCase, adventurerIndex);
        DialogueRuntimeContext context = new DialogueRuntimeContext(
            underwritingCase,
            adventurer,
            adventurerIndex,
            claim,
            responseKey);

        DialogueTemplateKind fallbackKind = claim != null && claim.isLie
            ? DialogueTemplateKind.LiePressureReaction
            : DialogueTemplateKind.TruthPressureReaction;

        AddKeyedLines(lines, profile, responseKey, fallbackKind, context);
        DialogueSequence sequence = CreateRuntimeSequence(lines, underwritingCase, adventurerIndex, adventurer);
        SetTimelineCues(sequence, profile);
        sequence.SetAdditionalSubmissionDocument(
            profile.GetAdditionalSubmissionDocumentPrefab(responseKey));
        return sequence;
    }

    private static DialogueSequence CreateRuntimeSequence(
        IEnumerable<DialogueLine> lines,
        UnderwritingCase underwritingCase,
        int adventurerIndex,
        AdventurerDocument adventurer)
    {
        return DialogueSequence.CreateRuntime(
            lines,
            adventurerIndex,
            GetConversationName(underwritingCase, adventurer));
    }

    private static string GetConversationName(
        UnderwritingCase underwritingCase,
        AdventurerDocument adventurer)
    {
        if (adventurer != null && !string.IsNullOrWhiteSpace(adventurer.adventurerName))
            return adventurer.adventurerName;

        return underwritingCase != null ? underwritingCase.npcName : string.Empty;
    }

    private static void SetTimelineCues(
        DialogueSequence sequence,
        ContractorDialogueProfile profile)
    {
        if (sequence != null && profile != null)
            sequence.SetTimelineCues(profile.TimelineCues);
    }

    private static void AddClaimLines(
        List<DialogueLine> lines,
        ContractorDialogueProfile profile,
        DialogueRuntimeContext baseContext,
        IReadOnlyList<DialogueClaim> claims)
    {
        if (claims == null)
            return;

        for (int i = 0; i < claims.Count; i++)
        {
            DialogueClaim claim = claims[i];

            if (claim == null || !claim.MatchesAdventurer(baseContext.adventurerIndex))
                continue;

            DialogueTemplateKind templateKind;
            if (!TryGetAnswerTemplateKind(claim.kind, out templateKind))
                continue;

            DialogueRuntimeContext context = new DialogueRuntimeContext(
                baseContext.underwritingCase,
                baseContext.adventurer,
                baseContext.adventurerIndex,
                claim);

            AddLines(lines, profile, templateKind, context);
        }
    }

    private static bool TryGetAnswerTemplateKind(
        DialogueClaimKind claimKind,
        out DialogueTemplateKind templateKind)
    {
        switch (claimKind)
        {
            default:
                templateKind = DialogueTemplateKind.Greeting;
                return false;
        }
    }

    private static void AddLines(
        List<DialogueLine> lines,
        ContractorDialogueProfile profile,
        DialogueTemplateKind kind,
        DialogueRuntimeContext context)
    {
        if (lines == null || profile == null)
            return;

        IReadOnlyList<DialogueLine> templates = profile.GetTemplateLineList(kind);

        if (templates == null)
            return;

        for (int i = 0; i < templates.Count; i++)
        {
            string line = DialogueTemplateFormatter.Format(
                templates[i] != null ? templates[i].Text : string.Empty,
                context);

            if (!string.IsNullOrWhiteSpace(line))
            {
                string key = templates[i] != null ? templates[i].Key : string.Empty;
                DialogueSpeaker speaker = templates[i] != null
                    ? templates[i].Speaker
                    : DialogueSpeaker.Contractor;
                lines.Add(new DialogueLine(key, line, speaker));
            }
        }
    }

    private static void AddKeyedLines(
        List<DialogueLine> lines,
        ContractorDialogueProfile profile,
        string responseKey,
        DialogueTemplateKind fallbackKind,
        DialogueRuntimeContext context)
    {
        if (lines == null || profile == null)
            return;

        IReadOnlyList<DialogueLine> templates = profile.GetResponseTemplateLineList(
            responseKey,
            fallbackKind);

        if (templates == null)
            return;

        for (int i = 0; i < templates.Count; i++)
        {
            string line = DialogueTemplateFormatter.Format(
                templates[i] != null ? templates[i].Text : string.Empty,
                context);

            if (!string.IsNullOrWhiteSpace(line))
            {
                string key = templates[i] != null ? templates[i].Key : string.Empty;
                DialogueSpeaker speaker = templates[i] != null
                    ? templates[i].Speaker
                    : DialogueSpeaker.Contractor;
                lines.Add(new DialogueLine(key, line, speaker));
            }
        }
    }

    private static AdventurerDocument GetAdventurer(
        UnderwritingCase underwritingCase,
        int index)
    {
        if (underwritingCase == null ||
            underwritingCase.adventurers == null ||
            index < 0 ||
            index >= underwritingCase.adventurers.Count)
        {
            return null;
        }

        return underwritingCase.adventurers[index];
    }
}
