using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnderwritingCase
{
    [Header("NPC")]
    public string npcName;
    public Sprite npcSprite;

    [Header("Contract")]
    public MarineInsuranceTypeDefinition insuranceType;
    public ContractorDialogueProfile dialogueProfile;
    [Min(0)] public int premium;
    [Min(0)] public int compensationAmount;
    [Min(1)] public int contractDurationTurns = 3;
    [Range(0f, 1f)] public float accidentOccurrenceProbability;
    [Min(0)] public int patience = 1;
    [NonSerialized] public int conditionalApprovalPatienceTriggerCount;
    public List<GameObject> monthEndDocumentPrefabs = new List<GameObject>();
    public List<CaseDataEntry> data = new List<CaseDataEntry>();
    public List<CaseDataSpriteEntry> spriteData = new List<CaseDataSpriteEntry>();
    public List<AdventurerDocument> adventurers = new List<AdventurerDocument>();

    public string GetText(string key)
    {
        if (string.IsNullOrEmpty(key) || data == null)
            return string.Empty;

        for (int i = 0; i < data.Count; i++)
        {
            CaseDataEntry entry = data[i];
            if (entry != null && entry.key == key)
                return entry.value != null ? entry.value.Resolve(null) : string.Empty;
        }

        return string.Empty;
    }

    public string GetCode(string key)
    {
        if (string.IsNullOrEmpty(key) || data == null)
            return string.Empty;

        for (int i = 0; i < data.Count; i++)
        {
            CaseDataEntry entry = data[i];
            if (entry != null && entry.key == key)
            {
                string code = entry.codeValue != null
                    ? entry.codeValue.Resolve(null)
                    : string.Empty;

                return !string.IsNullOrEmpty(code)
                    ? code
                    : GetText(key);
            }
        }

        return string.Empty;
    }

    public void SetText(string key, string value)
    {
        SetValue(key, value, null);
    }

    public void SetCode(string key, string code)
    {
        SetValue(key, GetText(key), code);
    }

    public void SetValue(string key, string value, string code)
    {
        if (string.IsNullOrEmpty(key))
            return;

        if (data == null)
            data = new List<CaseDataEntry>();

        for (int i = 0; i < data.Count; i++)
        {
            CaseDataEntry entry = data[i];
            if (entry != null && entry.key == key)
            {
                entry.value = DocumentTextSource.Direct(value);
                entry.codeValue = DocumentTextSource.Direct(code ?? string.Empty);
                return;
            }
        }

        data.Add(new CaseDataEntry
        {
            key = key,
            value = DocumentTextSource.Direct(value),
            codeValue = DocumentTextSource.Direct(code ?? string.Empty)
        });
    }

    public Sprite GetSprite(string key)
    {
        if (string.IsNullOrEmpty(key) || spriteData == null)
            return null;

        for (int i = 0; i < spriteData.Count; i++)
        {
            CaseDataSpriteEntry entry = spriteData[i];
            if (entry != null && entry.key == key)
                return entry.value != null ? entry.value.Resolve(null) : null;
        }

        return null;
    }

    public UnderwritingCase CreateResolved(System.Random random)
    {
        UnderwritingCase result = new UnderwritingCase();
        result.npcName = npcName;
        result.npcSprite = npcSprite;
        result.insuranceType = insuranceType;
        result.dialogueProfile = dialogueProfile;
        result.premium = premium;
        result.compensationAmount = compensationAmount;
        result.contractDurationTurns = Mathf.Max(1, contractDurationTurns);
        result.accidentOccurrenceProbability = accidentOccurrenceProbability;
        result.patience = patience;

        if (monthEndDocumentPrefabs != null)
        {
            for (int i = 0; i < monthEndDocumentPrefabs.Count; i++)
                result.monthEndDocumentPrefabs.Add(monthEndDocumentPrefabs[i]);
        }

        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                CaseDataEntry entry = data[i];
                if (entry == null) continue;
                result.data.Add(new CaseDataEntry
                {
                    key = entry.key,
                    value = entry.value != null
                        ? entry.value.CreateResolved(random)
                        : DocumentTextSource.Direct(string.Empty),
                    codeValue = entry.codeValue != null
                        ? entry.codeValue.CreateResolved(random)
                        : DocumentTextSource.Direct(string.Empty)
                });
            }
        }

        if (spriteData != null)
        {
            for (int i = 0; i < spriteData.Count; i++)
            {
                CaseDataSpriteEntry entry = spriteData[i];
                if (entry == null) continue;
                result.spriteData.Add(new CaseDataSpriteEntry
                {
                    key = entry.key,
                    value = entry.value != null
                        ? entry.value.CreateResolved(random)
                        : DocumentSpriteSource.Direct(null)
                });
            }
        }

        result.adventurers = adventurers;
        return result;
    }

    public IEnumerable<TaggedContentDefinition> EnumerateAllContents()
    {
        if (adventurers == null)
            yield break;

        for (int i = 0; i < adventurers.Count; i++)
        {
            if (adventurers[i] == null)
                continue;

            foreach (TaggedContentDefinition content in adventurers[i].EnumerateContents())
                yield return content;
        }
    }
}
