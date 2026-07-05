using System.Collections.Generic;
using UnityEngine;

public class MissedRejectionReasonPaperSpawner : MonoBehaviour
{
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperRoot;
    [SerializeField] private bool spawnOnApproved = true;
    [SerializeField] private bool spawnOnRejected = true;
    [SerializeField] private bool includeConsiderRejectReasons;
    [SerializeField] private bool replaceExistingPaper = true;
    [SerializeField] private bool logDebug;

    private GameLoopController registeredGameLoopController;
    private GameObject spawnedPaper;

    private void OnEnable()
    {
        RegisterGameLoopController();
    }

    private void OnDisable()
    {
        UnregisterGameLoopController();
    }

    public void ClearPaper()
    {
        if (spawnedPaper != null)
            Destroy(spawnedPaper);

        spawnedPaper = null;
    }

    private void OnVisitorResolvedWithDecision(
        UnderwritingDecision decision,
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionEvaluationResult evaluation,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> uncheckedTriggeredReasons)
    {
        if (!ShouldSpawnForDecision(decision))
            return;

        List<MarineInsuranceRejectionReasonDefinition> paperReasons =
            FilterReasons(uncheckedTriggeredReasons);

        if (paperReasons.Count == 0)
        {
            Log("Missed rejection reason paper skipped: no missed reasons.");
            return;
        }

        SpawnPaper(paperReasons);
    }

    private void SpawnPaper(IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons)
    {
        if (paperPrefab == null)
        {
            Log("Missed rejection reason paper skipped: paper prefab is missing.");
            return;
        }

        if (replaceExistingPaper)
            ClearPaper();

        Transform root = paperRoot != null ? paperRoot : transform;
        spawnedPaper = Instantiate(paperPrefab, root);

        MissedRejectionReasonPaperView view =
            spawnedPaper.GetComponentInChildren<MissedRejectionReasonPaperView>(true);

        if (view == null)
        {
            Log("Missed rejection reason paper spawned without MissedRejectionReasonPaperView.");
            return;
        }

        view.Bind(reasons);
    }

    private List<MarineInsuranceRejectionReasonDefinition> FilterReasons(
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons)
    {
        List<MarineInsuranceRejectionReasonDefinition> filteredReasons =
            new List<MarineInsuranceRejectionReasonDefinition>();

        if (reasons == null)
            return filteredReasons;

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];

            if (reason == null)
                continue;

            if (!includeConsiderRejectReasons &&
                reason.Decision == MarineInsuranceRejectionDecision.ConsiderReject)
            {
                continue;
            }

            filteredReasons.Add(reason);
        }

        return filteredReasons;
    }

    private bool ShouldSpawnForDecision(UnderwritingDecision decision)
    {
        if (decision == UnderwritingDecision.Approved)
            return spawnOnApproved;

        if (decision == UnderwritingDecision.Rejected)
            return spawnOnRejected;

        return false;
    }

    private void RegisterGameLoopController()
    {
        if (gameLoopController == null)
            gameLoopController = Object.FindFirstObjectByType<GameLoopController>();

        if (registeredGameLoopController == gameLoopController)
            return;

        UnregisterGameLoopController();
        registeredGameLoopController = gameLoopController;

        if (registeredGameLoopController != null)
        {
            registeredGameLoopController.VisitorArrivedEvent += ClearPaper;
            registeredGameLoopController.VisitorResolvedWithDecisionEvent +=
                OnVisitorResolvedWithDecision;
        }
    }

    private void UnregisterGameLoopController()
    {
        if (registeredGameLoopController == null)
            return;

        registeredGameLoopController.VisitorArrivedEvent -= ClearPaper;
        registeredGameLoopController.VisitorResolvedWithDecisionEvent -=
            OnVisitorResolvedWithDecision;
        registeredGameLoopController = null;
    }

    private void Log(string message)
    {
        if (logDebug)
            Debug.Log(message, this);
    }
}
