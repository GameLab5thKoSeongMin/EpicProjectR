using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

[Serializable]
public class DialogueLocalizedTextResolver
{
    private const string PrefixWildcardSuffix = ".*";
    private const int PaddedIndexDigits = 2;
    private static readonly string[] IndexedKeySuffixes =
    {
        string.Empty,
        ".npc",
        ".contractor",
        ".player"
    };

    [SerializeField] private bool useLocalization = true;
    [SerializeField] private string dialogueTable = "Dialogue";

    public string DialogueTable => dialogueTable;

    public string Resolve(string keyOrText)
    {
        if (!useLocalization ||
            string.IsNullOrEmpty(dialogueTable) ||
            string.IsNullOrEmpty(keyOrText))
        {
            return keyOrText ?? string.Empty;
        }

        try
        {
            StringTable table = LocalizationSettings.StringDatabase
                .GetTable(dialogueTable);

            StringTableEntry entry = table != null ? table.GetEntry(keyOrText) : null;
            return entry != null ? entry.Value : keyOrText;
        }
        catch (Exception)
        {
            return keyOrText;
        }
    }

    public IReadOnlyList<string> ResolveMany(string keyOrText)
    {
        IReadOnlyList<DialogueLine> lines = ResolveManyLines(keyOrText);
        List<string> values = new List<string>(lines.Count);

        for (int i = 0; i < lines.Count; i++)
            values.Add(lines[i] != null ? lines[i].Text : string.Empty);

        return values;
    }

    public IReadOnlyList<DialogueLine> ResolveManyLines(string keyOrText)
    {
        if (!useLocalization ||
            string.IsNullOrEmpty(dialogueTable) ||
            string.IsNullOrEmpty(keyOrText))
        {
            return new[] { new DialogueLine(keyOrText, keyOrText) };
        }

        try
        {
            StringTable table = LocalizationSettings.StringDatabase
                .GetTable(dialogueTable);

            if (table == null)
                return new[] { new DialogueLine(keyOrText, keyOrText) };

            if (keyOrText.EndsWith(PrefixWildcardSuffix, StringComparison.Ordinal))
            {
                string prefix = keyOrText.Substring(
                    0,
                    keyOrText.Length - PrefixWildcardSuffix.Length);
                return ResolvePrefix(table, prefix, keyOrText);
            }

            StringTableEntry entry = table.GetEntry(keyOrText);
            if (entry != null)
                return new[] { new DialogueLine(keyOrText, entry.Value) };

            return ResolvePrefix(table, keyOrText, keyOrText);
        }
        catch (Exception)
        {
            return new[] { new DialogueLine(keyOrText, keyOrText) };
        }
    }

    private static IReadOnlyList<DialogueLine> ResolvePrefix(
        StringTable table,
        string prefix,
        string fallback)
    {
        if (string.IsNullOrEmpty(prefix))
            return new[] { new DialogueLine(fallback, fallback) };

        List<DialogueLine> values = new List<DialogueLine>();

        for (int index = 0; ; index++)
        {
            string key;
            StringTableEntry entry = GetIndexedEntry(table, prefix, index, out key);

            if (entry == null)
                break;

            values.Add(new DialogueLine(key, entry.Value));
        }

        return values.Count > 0 ? values : new[] { new DialogueLine(fallback, fallback) };
    }

    private static StringTableEntry GetIndexedEntry(
        StringTable table,
        string prefix,
        int index,
        out string key)
    {
        string paddedKey = prefix + "." + index.ToString("D" + PaddedIndexDigits);
        StringTableEntry entry = GetFirstMatchingEntry(table, paddedKey, out key);

        if (entry != null)
            return entry;

        string unpaddedKey = prefix + "." + index;
        return GetFirstMatchingEntry(table, unpaddedKey, out key);
    }

    private static StringTableEntry GetFirstMatchingEntry(
        StringTable table,
        string baseKey,
        out string key)
    {
        for (int i = 0; i < IndexedKeySuffixes.Length; i++)
        {
            key = baseKey + IndexedKeySuffixes[i];
            StringTableEntry entry = table.GetEntry(key);

            if (entry != null)
                return entry;
        }

        key = baseKey;
        return null;
    }
}
