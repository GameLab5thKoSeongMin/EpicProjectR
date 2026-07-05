using System;
using System.Collections.Generic;

[Serializable]
public class MarineInsuranceRejectionEvaluationResult
{
    public List<MarineInsuranceRejectionReasonDefinition> triggeredReasons =
        new List<MarineInsuranceRejectionReasonDefinition>();

    public bool HasRejectReason
    {
        get
        {
            return HasDecision(MarineInsuranceRejectionDecision.Reject);
        }
    }

    public bool HasConsiderRejectReason
    {
        get
        {
            return HasDecision(MarineInsuranceRejectionDecision.ConsiderReject);
        }
    }

    public UnderwritingDecision SuggestedDecision
    {
        get
        {
            return HasRejectReason
                ? UnderwritingDecision.Rejected
                : UnderwritingDecision.Approved;
        }
    }

    public List<string> GetTriggeredReasonNames()
    {
        List<string> names = new List<string>();

        if (triggeredReasons == null)
            return names;

        for (int i = 0; i < triggeredReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = triggeredReasons[i];
            if (reason != null)
                names.Add(reason.DisplayName);
        }

        return names;
    }

    private bool HasDecision(MarineInsuranceRejectionDecision decision)
    {
        if (triggeredReasons == null)
            return false;

        for (int i = 0; i < triggeredReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = triggeredReasons[i];
            if (reason != null && reason.Decision == decision)
                return true;
        }

        return false;
    }
}

public static class MarineInsuranceRejectionEvaluator
{
    public static MarineInsuranceRejectionEvaluationResult Evaluate(UnderwritingCase underwritingCase)
    {
        MarineInsuranceRejectionEvaluationResult result =
            new MarineInsuranceRejectionEvaluationResult();

        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons =
            underwritingCase?.insuranceType?.RejectionReasons;

        if (reasons == null)
            return result;

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];
            if (reason != null && reason.IsTriggered(underwritingCase))
                result.triggeredReasons.Add(reason);
        }

        return result;
    }
}
