using System;
using System.Collections.Generic;
using UnityEngine;

public enum MarineInsuranceCaseGoalMode
{
    Uncontrolled,
    Approved,
    Rejected,
    SpecificRejectionReasons
}

[Serializable]
public class MarineInsuranceCaseGoal
{
    [SerializeField] private MarineInsuranceCaseGoalMode mode;
    [SerializeField] private bool applySafeValuesForOtherReasons = true;
    [SerializeField, HideInInspector] private List<string> targetRejectionReasonIds =
        new List<string>();

    [SerializeField, HideInInspector] private List<MarineInsuranceRejectionReasonDefinition> targetRejectionReasons =
        new List<MarineInsuranceRejectionReasonDefinition>();

    public MarineInsuranceCaseGoalMode Mode => mode;
    public bool ApplySafeValuesForOtherReasons => applySafeValuesForOtherReasons;
    public IReadOnlyList<string> TargetRejectionReasonIds => targetRejectionReasonIds;
    public IReadOnlyList<MarineInsuranceRejectionReasonDefinition> TargetRejectionReasons => targetRejectionReasons;
}

public static class MarineInsuranceCaseGoalApplier
{
    public static void Apply(
        UnderwritingCase underwritingCase,
        MarineInsuranceCaseGoal goal,
        System.Random random)
    {
        if (underwritingCase?.insuranceType == null ||
            goal == null ||
            goal.Mode == MarineInsuranceCaseGoalMode.Uncontrolled)
        {
            return;
        }

        List<MarineInsuranceRejectionReasonDefinition> targetReasons =
            SelectTargetReasons(underwritingCase.insuranceType, goal, random);

        Apply(underwritingCase, goal, targetReasons);
    }

    public static void Apply(
        UnderwritingCase underwritingCase,
        MarineInsuranceCaseGoal goal,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> targetReasons)
    {
        if (underwritingCase?.insuranceType == null ||
            goal == null ||
            goal.Mode == MarineInsuranceCaseGoalMode.Uncontrolled)
        {
            return;
        }

        if (goal.Mode == MarineInsuranceCaseGoalMode.Approved ||
            goal.ApplySafeValuesForOtherReasons)
        {
            ApplySafeValues(underwritingCase, underwritingCase.insuranceType.RejectionReasons, targetReasons);
        }

        ApplyTriggerValues(underwritingCase, targetReasons);
    }

    public static List<MarineInsuranceRejectionReasonDefinition> SelectTargetReasons(
        MarineInsuranceTypeDefinition insuranceType,
        MarineInsuranceCaseGoal goal,
        System.Random random)
    {
        List<MarineInsuranceRejectionReasonDefinition> result =
            new List<MarineInsuranceRejectionReasonDefinition>();

        if (goal.Mode == MarineInsuranceCaseGoalMode.Approved)
            return result;

        IReadOnlyList<string> configuredTargetIds =
            goal.TargetRejectionReasonIds;

        if (goal.Mode == MarineInsuranceCaseGoalMode.SpecificRejectionReasons &&
            configuredTargetIds != null &&
            configuredTargetIds.Count > 0)
        {
            AddReasonsMatchingIds(insuranceType, configuredTargetIds, result);

            if (result.Count > 0)
                return result;
        }

        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> configuredTargets =
            goal.TargetRejectionReasons;

        if (goal.Mode == MarineInsuranceCaseGoalMode.SpecificRejectionReasons &&
            configuredTargets != null &&
            configuredTargets.Count > 0)
        {
            AddReasonsOwnedByType(insuranceType, configuredTargets, result);

            if (result.Count > 0)
                return result;
        }

        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> availableReasons =
            insuranceType != null ? insuranceType.RejectionReasons : null;

        if (availableReasons == null || availableReasons.Count == 0)
            return result;

        List<MarineInsuranceRejectionReasonDefinition> rejectReasons =
            new List<MarineInsuranceRejectionReasonDefinition>();

        for (int i = 0; i < availableReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = availableReasons[i];
            if (reason != null && reason.Decision == MarineInsuranceRejectionDecision.Reject)
                rejectReasons.Add(reason);
        }

        if (rejectReasons.Count == 0)
            return result;

        int index = random != null
            ? random.Next(0, rejectReasons.Count)
            : UnityEngine.Random.Range(0, rejectReasons.Count);

        result.Add(rejectReasons[index]);
        return result;
    }

    private static void AddReasonsMatchingIds(
        MarineInsuranceTypeDefinition insuranceType,
        IReadOnlyList<string> targetIds,
        List<MarineInsuranceRejectionReasonDefinition> result)
    {
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> availableReasons =
            insuranceType != null ? insuranceType.RejectionReasons : null;

        if (availableReasons == null || targetIds == null)
            return;

        for (int i = 0; i < availableReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = availableReasons[i];
            if (reason != null && ContainsId(targetIds, reason.Id))
                result.Add(reason);
        }
    }

    private static void AddReasonsOwnedByType(
        MarineInsuranceTypeDefinition insuranceType,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> targetReasons,
        List<MarineInsuranceRejectionReasonDefinition> result)
    {
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> availableReasons =
            insuranceType != null ? insuranceType.RejectionReasons : null;

        if (availableReasons == null || targetReasons == null)
            return;

        for (int i = 0; i < availableReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition availableReason = availableReasons[i];
            if (availableReason != null && ContainsReason(targetReasons, availableReason))
                result.Add(availableReason);
        }
    }

    private static void ApplySafeValues(
        UnderwritingCase underwritingCase,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> excludedReasons)
    {
        if (reasons == null)
            return;

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];
            if (reason == null || ContainsReason(excludedReasons, reason))
                continue;

            ApplyReasonValues(underwritingCase, reason, false);
        }
    }

    private static void ApplyTriggerValues(
        UnderwritingCase underwritingCase,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons)
    {
        if (reasons == null)
            return;

        for (int i = 0; i < reasons.Count; i++)
            ApplyReasonValues(underwritingCase, reasons[i], true);
    }

    private static void ApplyReasonValues(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionReasonDefinition reason,
        bool shouldTrigger)
    {
        IReadOnlyList<MarineInsuranceRejectionCondition> conditions = reason != null ? reason.Conditions : null;

        if (conditions == null)
            return;

        for (int i = 0; i < conditions.Count; i++)
        {
            MarineInsuranceRejectionCondition condition = conditions[i];
            if (condition == null)
                continue;

            bool created = shouldTrigger
                ? condition.TryCreateTriggerValue(underwritingCase, out string triggerValue, out string triggerCode)
                : condition.TryCreateSafeValue(underwritingCase, out triggerValue, out triggerCode);

            if (created)
                underwritingCase.SetValue(condition.DataKey, triggerValue, triggerCode);
        }
    }

    private static bool ContainsReason(
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons,
        MarineInsuranceRejectionReasonDefinition target)
    {
        if (reasons == null)
            return false;

        for (int i = 0; i < reasons.Count; i++)
        {
            if (reasons[i] == target)
                return true;
        }

        return false;
    }

    private static bool ContainsId(IReadOnlyList<string> ids, string targetId)
    {
        if (ids == null || string.IsNullOrEmpty(targetId))
            return false;

        for (int i = 0; i < ids.Count; i++)
        {
            if (ids[i] == targetId)
                return true;
        }

        return false;
    }
}
