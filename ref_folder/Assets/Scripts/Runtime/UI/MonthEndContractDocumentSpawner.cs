using System.Collections.Generic;
using UnityEngine;

public class MonthEndContractDocumentSpawner : MonoBehaviour
{
    [SerializeField] private Transform documentRoot;
    [SerializeField] private bool clearBeforeSpawn = true;

    private readonly List<GameObject> spawnedDocuments = new List<GameObject>();

    public void Bind(MonthlySettlementSummary summary)
    {
        if (clearBeforeSpawn)
            Clear();

        if (summary?.records == null)
            return;

        Transform root = documentRoot != null ? documentRoot : transform;

        for (int i = 0; i < summary.records.Count; i++)
            SpawnRecordDocuments(summary.records[i], root);
    }

    public void Clear()
    {
        for (int i = spawnedDocuments.Count - 1; i >= 0; i--)
        {
            GameObject document = spawnedDocuments[i];

            if (document != null)
                Destroy(document);
        }

        spawnedDocuments.Clear();
    }

    private void SpawnRecordDocuments(
        MonthlySettlementRecord record,
        Transform root)
    {
        if (record?.underwritingCase == null ||
            record.underwritingCase.monthEndDocumentPrefabs == null)
        {
            return;
        }

        IReadOnlyList<GameObject> prefabs = record.underwritingCase.monthEndDocumentPrefabs;

        for (int i = 0; i < prefabs.Count; i++)
        {
            GameObject prefab = prefabs[i];

            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, root);
            spawnedDocuments.Add(instance);
            BindDocument(instance, record);
        }
    }

    private void BindDocument(
        GameObject instance,
        MonthlySettlementRecord record)
    {
        if (instance == null)
            return;

        IUnderwritingCaseDocumentView[] views =
            instance.GetComponentsInChildren<IUnderwritingCaseDocumentView>(true);

        for (int i = 0; i < views.Length; i++)
            views[i]?.Bind(record?.underwritingCase);

        IMonthEndContractDocumentView[] monthEndViews =
            instance.GetComponentsInChildren<IMonthEndContractDocumentView>(true);

        for (int i = 0; i < monthEndViews.Length; i++)
            monthEndViews[i]?.Bind(record);
    }
}
