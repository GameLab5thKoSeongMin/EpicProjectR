using System.Collections.Generic;
using UnityEngine;

public class ApprovedContractListView : MonoBehaviour
{
    [SerializeField] private Transform itemRoot;
    [SerializeField] private GameObject itemPrefab;

    private readonly List<GameObject> spawnedItems = new List<GameObject>();

    public void Bind(IReadOnlyList<ApprovedContractRecord> contracts)
    {
        ClearItems();

        if (contracts == null || itemPrefab == null)
            return;

        Transform root = itemRoot != null ? itemRoot : transform;

        for (int i = 0; i < contracts.Count; i++)
        {
            ApprovedContractRecord contract = contracts[i];

            if (contract == null)
                continue;

            GameObject instance = Instantiate(itemPrefab, root);
            spawnedItems.Add(instance);

            ApprovedContractListItemView itemView =
                instance.GetComponentInChildren<ApprovedContractListItemView>(true);

            if (itemView != null)
                itemView.Bind(contract);
        }
    }

    public void ClearItems()
    {
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            GameObject item = spawnedItems[i];

            if (item != null)
                Destroy(item);
        }

        spawnedItems.Clear();
    }
}
