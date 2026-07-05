using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MarineInsuranceRejectionReasonListView : MonoBehaviour
{
    [FormerlySerializedAs("itemRoot")]
    [SerializeField] private Transform rejectItemRoot;
    [SerializeField] private Transform considerRejectItemRoot;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private bool includeConsiderRejectReasons = true;

    private readonly List<GameObject> spawnedItems = new List<GameObject>();
    private readonly Dictionary<string, MarineInsuranceRejectionReasonItemView> itemViewsByReasonId =
        new Dictionary<string, MarineInsuranceRejectionReasonItemView>();
    private readonly Dictionary<string, MarineInsuranceRejectionReasonDefinition> reasonsById =
        new Dictionary<string, MarineInsuranceRejectionReasonDefinition>();
    private readonly HashSet<string> checkedReasonIds = new HashSet<string>();

    public event Action CheckedReasonsChanged;

    public void Bind(UnderwritingCase underwritingCase)
    {
        ClearItems();

        if (underwritingCase?.insuranceType == null ||
            itemPrefab == null)
        {
            return;
        }

        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons =
            underwritingCase.insuranceType.RejectionReasons;

        if (reasons == null)
            return;

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];

            if (reason == null || !ShouldShow(reason))
                continue;

            Transform root = GetRoot(reason.Decision);
            GameObject instance = Instantiate(itemPrefab, root);
            spawnedItems.Add(instance);

            MarineInsuranceRejectionReasonItemView itemView =
                instance.GetComponent<MarineInsuranceRejectionReasonItemView>();

            if (itemView != null)
            {
                itemViewsByReasonId[reason.Id] = itemView;
                reasonsById[reason.Id] = reason;
                itemView.Bind(underwritingCase, reason, this);
            }
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
        itemViewsByReasonId.Clear();
        reasonsById.Clear();
        checkedReasonIds.Clear();
        CheckedReasonsChanged?.Invoke();
    }

    public bool IsReasonChecked(MarineInsuranceRejectionReasonDefinition reason)
    {
        return reason != null && checkedReasonIds.Contains(reason.Id);
    }

    public void SetReasonChecked(
        MarineInsuranceRejectionReasonDefinition reason,
        bool isChecked)
    {
        if (reason == null || string.IsNullOrEmpty(reason.Id))
            return;

        if (isChecked)
            checkedReasonIds.Add(reason.Id);
        else
            checkedReasonIds.Remove(reason.Id);

        if (itemViewsByReasonId.TryGetValue(reason.Id, out MarineInsuranceRejectionReasonItemView itemView))
            itemView.SetChecked(isChecked);

        CheckedReasonsChanged?.Invoke();
    }

    public bool HasCheckedReasonWithDecision(MarineInsuranceRejectionDecision decision)
    {
        foreach (string reasonId in checkedReasonIds)
        {
            if (!reasonsById.TryGetValue(reasonId, out MarineInsuranceRejectionReasonDefinition reason))
                continue;

            if (reason != null && reason.Decision == decision)
                return true;
        }

        return false;
    }

    public int CountCheckedReasonsWithDecision(MarineInsuranceRejectionDecision decision)
    {
        int count = 0;

        foreach (string reasonId in checkedReasonIds)
        {
            if (!reasonsById.TryGetValue(reasonId, out MarineInsuranceRejectionReasonDefinition reason))
                continue;

            if (reason != null && reason.Decision == decision)
                count++;
        }

        return count;
    }

    public bool HasCheckedUntriggeredReasonWithDecision(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionDecision decision)
    {
        foreach (string reasonId in checkedReasonIds)
        {
            if (!reasonsById.TryGetValue(reasonId, out MarineInsuranceRejectionReasonDefinition reason))
                continue;

            if (reason == null || reason.Decision != decision)
                continue;

            if (!reason.IsTriggered(underwritingCase))
                return true;
        }

        return false;
    }

    public bool AreAllTriggeredReasonsChecked(MarineInsuranceRejectionEvaluationResult evaluation)
    {
        if (evaluation?.triggeredReasons == null || evaluation.triggeredReasons.Count == 0)
            return false;

        for (int i = 0; i < evaluation.triggeredReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = evaluation.triggeredReasons[i];

            if (reason == null)
                continue;

            if (!checkedReasonIds.Contains(reason.Id))
                return false;
        }

        return true;
    }

    public List<MarineInsuranceRejectionReasonDefinition> GetUncheckedTriggeredReasons(
        MarineInsuranceRejectionEvaluationResult evaluation,
        bool includeConsiderRejectReasons)
    {
        List<MarineInsuranceRejectionReasonDefinition> uncheckedReasons =
            new List<MarineInsuranceRejectionReasonDefinition>();

        if (evaluation?.triggeredReasons == null)
            return uncheckedReasons;

        for (int i = 0; i < evaluation.triggeredReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = evaluation.triggeredReasons[i];

            if (reason == null)
                continue;

            if (!includeConsiderRejectReasons &&
                reason.Decision == MarineInsuranceRejectionDecision.ConsiderReject)
            {
                continue;
            }

            if (!checkedReasonIds.Contains(reason.Id))
                uncheckedReasons.Add(reason);
        }

        return uncheckedReasons;
    }

    private Transform GetRoot(MarineInsuranceRejectionDecision decision)
    {
        if (decision == MarineInsuranceRejectionDecision.ConsiderReject &&
            considerRejectItemRoot != null)
        {
            return considerRejectItemRoot;
        }

        return rejectItemRoot != null ? rejectItemRoot : transform;
    }

    private bool ShouldShow(MarineInsuranceRejectionReasonDefinition reason)
    {
        return includeConsiderRejectReasons ||
               reason.Decision == MarineInsuranceRejectionDecision.Reject;
    }
}
