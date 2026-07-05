using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissedRejectionReasonPaperView : MonoBehaviour
{
    [SerializeField] private Transform itemRoot;
    [SerializeField] private GameObject reasonTextPrefab;
    [SerializeField] private bool clearExistingItemsOnBind = true;

    private readonly List<GameObject> spawnedItems = new List<GameObject>();

    public void Bind(IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons)
    {
        if (clearExistingItemsOnBind)
            ClearItems();

        if (reasons == null || reasonTextPrefab == null)
            return;

        Transform root = itemRoot != null ? itemRoot : transform;

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];

            if (reason == null)
                continue;

            GameObject instance = Instantiate(reasonTextPrefab, root);
            spawnedItems.Add(instance);
            SetReasonText(instance, GetPaperText(reason));
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

    private string GetPaperText(MarineInsuranceRejectionReasonDefinition reason)
    {
        if (!string.IsNullOrWhiteSpace(reason.MissedReasonPaperText))
            return reason.MissedReasonPaperText;

        if (!string.IsNullOrWhiteSpace(reason.Description))
            return reason.Description;

        return reason.DisplayName;
    }

    private void SetReasonText(GameObject instance, string text)
    {
        TMP_Text textView = instance != null
            ? instance.GetComponentInChildren<TMP_Text>(true)
            : null;

        if (textView != null)
            textView.text = text ?? string.Empty;
    }
}
