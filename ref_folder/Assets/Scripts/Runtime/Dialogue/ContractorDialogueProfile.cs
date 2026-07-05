using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Contractor Dialogue Profile")]
public class ContractorDialogueProfile : ScriptableObject
{
    [SerializeField] private DialogueLocalizedTextResolver localizedTextResolver =
        new DialogueLocalizedTextResolver();

    [SerializeField] private List<string> greetingTemplates = new List<string>();
    [SerializeField] private GameObject greetingAdditionalSubmissionDocumentPrefab;
    [SerializeField] private List<string> questIntroductionTemplates = new List<string>();
    [SerializeField] private List<string> truthPressureReactionTemplates = new List<string>();
    [SerializeField] private List<string> liePressureReactionTemplates = new List<string>();
    [SerializeField] private List<DialogueResponseEntry> keyedResponses =
        new List<DialogueResponseEntry>();
    [SerializeField] private List<DialogueTimelineCue> timelineCues =
        new List<DialogueTimelineCue>();

    public string PickTemplate(DialogueTemplateKind kind, System.Random random)
    {
        return Pick(ResolveList(GetTemplates(kind)), random);
    }

    public IReadOnlyList<string> GetTemplateList(DialogueTemplateKind kind)
    {
        return ResolveList(GetTemplates(kind));
    }

    public IReadOnlyList<DialogueLine> GetTemplateLineList(DialogueTemplateKind kind)
    {
        return ResolveLineList(GetTemplates(kind));
    }

    public IReadOnlyList<DialogueTimelineCue> TimelineCues => timelineCues;

    public GameObject GreetingAdditionalSubmissionDocumentPrefab =>
        greetingAdditionalSubmissionDocumentPrefab;

    public string PickResponseTemplate(
        string responseKey,
        DialogueTemplateKind fallbackKind,
        System.Random random)
    {
        IReadOnlyList<string> keyedTemplates = GetKeyedTemplateList(responseKey);

        if (keyedTemplates != null && keyedTemplates.Count > 0)
            return Pick(ResolveList(keyedTemplates), random);

        IReadOnlyList<string> resolvedResponse;
        if (TryResolveResponseKey(responseKey, out resolvedResponse))
            return Pick(resolvedResponse, random);

        return PickTemplate(fallbackKind, random);
    }

    public IReadOnlyList<string> GetResponseTemplateList(
        string responseKey,
        DialogueTemplateKind fallbackKind)
    {
        IReadOnlyList<string> keyedTemplates = GetKeyedTemplateList(responseKey);

        if (keyedTemplates != null && keyedTemplates.Count > 0)
            return ResolveList(keyedTemplates);

        IReadOnlyList<string> resolvedResponse;
        if (TryResolveResponseKey(responseKey, out resolvedResponse))
            return resolvedResponse;

        return ResolveList(GetTemplates(fallbackKind));
    }

    public IReadOnlyList<DialogueLine> GetResponseTemplateLineList(
        string responseKey,
        DialogueTemplateKind fallbackKind)
    {
        IReadOnlyList<string> keyedTemplates = GetKeyedTemplateList(responseKey);

        if (keyedTemplates != null && keyedTemplates.Count > 0)
            return ResolveLineList(keyedTemplates);

        IReadOnlyList<DialogueLine> resolvedResponse;
        if (TryResolveResponseKeyLines(responseKey, out resolvedResponse))
            return resolvedResponse;

        return ResolveLineList(GetTemplates(fallbackKind));
    }

    public GameObject GetAdditionalSubmissionDocumentPrefab(string responseKey)
    {
        DialogueResponseEntry entry = GetKeyedResponse(responseKey);
        return entry != null ? entry.AdditionalSubmissionDocumentPrefab : null;
    }

    public string GetKeyedResponseEntryKey(string responseKey)
    {
        DialogueResponseEntry entry = GetKeyedResponse(responseKey);
        return entry != null ? entry.EntryKey : string.Empty;
    }

    private IReadOnlyList<string> GetKeyedTemplateList(string responseKey)
    {
        DialogueResponseEntry entry = GetKeyedResponse(responseKey);
        return entry != null ? entry.Templates : null;
    }

    private DialogueResponseEntry GetKeyedResponse(string responseKey)
    {
        if (string.IsNullOrWhiteSpace(responseKey) || keyedResponses == null)
            return null;

        for (int i = 0; i < keyedResponses.Count; i++)
        {
            DialogueResponseEntry entry = keyedResponses[i];

            if (entry != null && entry.Matches(responseKey))
                return entry;
        }

        return null;
    }

    private bool TryResolveResponseKey(
        string responseKey,
        out IReadOnlyList<string> resolvedTemplates)
    {
        resolvedTemplates = null;

        if (localizedTextResolver == null || string.IsNullOrWhiteSpace(responseKey))
            return false;

        IReadOnlyList<string> resolved = localizedTextResolver.ResolveMany(responseKey);

        if (resolved == null || resolved.Count == 0)
            return false;

        if (resolved.Count == 1 &&
            string.Equals(resolved[0], responseKey, StringComparison.Ordinal))
        {
            return false;
        }

        resolvedTemplates = resolved;
        return true;
    }

    private bool TryResolveResponseKeyLines(
        string responseKey,
        out IReadOnlyList<DialogueLine> resolvedTemplates)
    {
        resolvedTemplates = null;

        if (localizedTextResolver == null || string.IsNullOrWhiteSpace(responseKey))
            return false;

        IReadOnlyList<DialogueLine> resolved = localizedTextResolver.ResolveManyLines(responseKey);

        if (resolved == null || resolved.Count == 0)
            return false;

        if (resolved.Count == 1 &&
            resolved[0] != null &&
            string.Equals(resolved[0].Text, responseKey, StringComparison.Ordinal))
        {
            return false;
        }

        resolvedTemplates = resolved;
        return true;
    }

    private IReadOnlyList<string> GetTemplates(DialogueTemplateKind kind)
    {
        switch (kind)
        {
            case DialogueTemplateKind.Greeting:
                return greetingTemplates;

            case DialogueTemplateKind.QuestIntroduction:
                return questIntroductionTemplates;

            case DialogueTemplateKind.TruthPressureReaction:
                return truthPressureReactionTemplates;

            case DialogueTemplateKind.LiePressureReaction:
                return liePressureReactionTemplates;

            default:
                return greetingTemplates;
        }
    }

    private static string Pick(IReadOnlyList<string> templates, System.Random random)
    {
        if (templates == null || templates.Count == 0)
            return string.Empty;

        int index = random != null ? random.Next(templates.Count) : 0;
        return templates[index] ?? string.Empty;
    }

    private IReadOnlyList<string> ResolveList(IReadOnlyList<string> templates)
    {
        if (templates == null || templates.Count == 0)
            return templates;

        List<string> resolvedTemplates = new List<string>(templates.Count);

        for (int i = 0; i < templates.Count; i++)
            resolvedTemplates.AddRange(ResolveMany(templates[i]));

        return resolvedTemplates;
    }

    private IReadOnlyList<DialogueLine> ResolveLineList(IReadOnlyList<string> templates)
    {
        if (templates == null || templates.Count == 0)
            return new List<DialogueLine>();

        List<DialogueLine> resolvedTemplates = new List<DialogueLine>(templates.Count);

        for (int i = 0; i < templates.Count; i++)
            resolvedTemplates.AddRange(ResolveManyLines(templates[i]));

        return resolvedTemplates;
    }

    private IReadOnlyList<string> ResolveMany(string keyOrText)
    {
        return localizedTextResolver != null
            ? localizedTextResolver.ResolveMany(keyOrText)
            : new[] { keyOrText ?? string.Empty };
    }

    private IReadOnlyList<DialogueLine> ResolveManyLines(string keyOrText)
    {
        return localizedTextResolver != null
            ? localizedTextResolver.ResolveManyLines(keyOrText)
            : new[] { new DialogueLine(keyOrText, keyOrText) };
    }
}
