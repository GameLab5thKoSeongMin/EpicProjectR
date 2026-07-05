using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueResponseEntry
{
    [SerializeField] private string key;
    [SerializeField] private List<string> responseKeys = new List<string>();
    [TextArea]
    [SerializeField] private List<string> templates = new List<string>();
    [SerializeField] private GameObject additionalSubmissionDocumentPrefab;

    public string Key => key;
    public IReadOnlyList<string> ResponseKeys => responseKeys;
    public string EntryKey
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(key))
                return key;

            if (responseKeys != null)
            {
                for (int i = 0; i < responseKeys.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(responseKeys[i]))
                        return responseKeys[i];
                }
            }

            return string.Empty;
        }
    }
    public IReadOnlyList<string> Templates => templates;
    public GameObject AdditionalSubmissionDocumentPrefab => additionalSubmissionDocumentPrefab;

    public bool Matches(string responseKey)
    {
        if (MatchesKey(key, responseKey))
            return true;

        if (responseKeys == null)
            return false;

        for (int i = 0; i < responseKeys.Count; i++)
        {
            if (MatchesKey(responseKeys[i], responseKey))
                return true;
        }

        return false;
    }

    public string PickTemplate(System.Random random)
    {
        if (templates == null || templates.Count == 0)
            return string.Empty;

        int index = random != null ? random.Next(templates.Count) : 0;
        return templates[index] ?? string.Empty;
    }

    private static bool MatchesKey(string candidate, string responseKey)
    {
        return !string.IsNullOrWhiteSpace(candidate) &&
               !string.IsNullOrWhiteSpace(responseKey) &&
               string.Equals(
                   candidate,
                   responseKey,
                   StringComparison.OrdinalIgnoreCase);
    }
}
