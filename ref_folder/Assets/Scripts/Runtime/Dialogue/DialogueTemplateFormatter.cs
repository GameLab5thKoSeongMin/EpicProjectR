public static class DialogueTemplateFormatter
{
    public static string Format(string template, DialogueRuntimeContext context)
    {
        if (string.IsNullOrEmpty(template))
            return string.Empty;

        UnderwritingCase underwritingCase = context != null ? context.underwritingCase : null;
        AdventurerDocument adventurer = context != null ? context.adventurer : null;
        DialogueClaim claim = context != null ? context.claim : null;
        string result = template;
        result = Replace(result, "responseKey", context != null ? context.responseKey : string.Empty);
        result = Replace(result, "adventurerName", adventurer != null ? adventurer.adventurerName : string.Empty);
        result = Replace(result, "claimed", claim != null ? claim.ClaimedDisplayName : string.Empty);
        result = Replace(result, "actual", claim != null ? claim.ActualDisplayName : string.Empty);
        result = Replace(result, "claimKind", claim != null ? claim.kind.ToString() : string.Empty);

        if (underwritingCase?.data != null)
        {
            for (int i = 0; i < underwritingCase.data.Count; i++)
            {
                CaseDataEntry entry = underwritingCase.data[i];

                if (entry == null || string.IsNullOrEmpty(entry.key))
                    continue;

                result = Replace(result, entry.key, underwritingCase.GetText(entry.key));
            }
        }

        return result;
    }

    private static string Replace(string source, string token, string value)
    {
        if (source == null)
            return string.Empty;

        return source.Replace("{" + token + "}", value ?? string.Empty);
    }
}
