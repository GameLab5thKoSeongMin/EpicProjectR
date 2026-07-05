using System;

[Serializable]
public class DialogueClaim
{
    public int adventurerIndex = -1;
    public DialogueClaimKind kind = DialogueClaimKind.None;
    public DefinitionBase actualDefinition;
    public DefinitionBase claimedDefinition;
    public string actualText;
    public string claimedText;
    public bool isLie;
    public string evidenceKey;

    public string ActualId => GetDefinitionId(actualDefinition, actualText);
    public string ClaimedId => GetDefinitionId(claimedDefinition, claimedText);

    public string ActualDisplayName => GetDisplayName(actualDefinition, actualText);
    public string ClaimedDisplayName => GetDisplayName(claimedDefinition, claimedText);

    public bool MatchesAdventurer(int index)
    {
        return adventurerIndex < 0 || adventurerIndex == index;
    }

    private static string GetDefinitionId(DefinitionBase definition, string fallback)
    {
        if (definition != null)
            return definition.Id;

        return fallback ?? string.Empty;
    }

    private static string GetDisplayName(DefinitionBase definition, string fallback)
    {
        if (definition != null)
            return definition.DisplayName;

        return fallback ?? string.Empty;
    }
}
