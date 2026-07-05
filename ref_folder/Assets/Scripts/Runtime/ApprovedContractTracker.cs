using System;
using System.Collections.Generic;
using UnityEngine;

public class ApprovedContractTracker : MonoBehaviour
{
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField, Min(1)] private int defaultDurationTurns = 3;
    [SerializeField] private string durationTurnsKey = "route.duration.turns";
    [SerializeField] private string departurePortKey = "route.departure.port";
    [SerializeField] private string destinationPortKey = "route.destination.port";
    [SerializeField] private string[] contractorNameKeys =
    {
        "insurance.ship.owner.name",
        "ship.owner.name",
        "registration.ship.owner.name"
    };
    [SerializeField] private bool logDebug;

    private readonly List<ApprovedContractRecord> contracts =
        new List<ApprovedContractRecord>();

    private GameLoopController registeredGameLoopController;
    private int observedTurn = -1;

    public event Action<IReadOnlyList<ApprovedContractRecord>> ContractsChanged;
    public IReadOnlyList<ApprovedContractRecord> Contracts => contracts;

    private void OnEnable()
    {
        RegisterGameLoopController();
        observedTurn = GetCurrentTurn();
        NotifyChanged();
    }

    private void Update()
    {
        int currentTurn = GetCurrentTurn();

        if (observedTurn < 0)
        {
            observedTurn = currentTurn;
            return;
        }

        if (currentTurn <= observedTurn)
            return;

        AdvanceContracts(currentTurn - observedTurn);
        observedTurn = currentTurn;
    }

    private void OnDisable()
    {
        UnregisterGameLoopController();
    }

    public void ClearContracts()
    {
        contracts.Clear();
        NotifyChanged();
    }

    private void OnVisitorResolvedWithDecision(
        UnderwritingDecision decision,
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionEvaluationResult evaluation,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> uncheckedTriggeredReasons)
    {
        if (decision != UnderwritingDecision.Approved || underwritingCase == null)
            return;

        contracts.Add(CreateRecord(underwritingCase));
        NotifyChanged();
    }

    private ApprovedContractRecord CreateRecord(UnderwritingCase underwritingCase)
    {
        return new ApprovedContractRecord(
            GetFirstText(underwritingCase, contractorNameKeys, underwritingCase.npcName),
            underwritingCase.GetText(departurePortKey),
            underwritingCase.GetCode(departurePortKey),
            underwritingCase.GetText(destinationPortKey),
            underwritingCase.GetCode(destinationPortKey),
            underwritingCase.premium,
            underwritingCase.compensationAmount,
            GetCurrentTurn(),
            GetDurationTurns(underwritingCase));
    }

    private int GetDurationTurns(UnderwritingCase underwritingCase)
    {
        string value = underwritingCase.GetCode(durationTurnsKey);

        if (string.IsNullOrWhiteSpace(value))
            value = underwritingCase.GetText(durationTurnsKey);

        if (int.TryParse(value, out int turns))
            return Mathf.Max(1, turns);

        return underwritingCase.contractDurationTurns > 0
            ? underwritingCase.contractDurationTurns
            : defaultDurationTurns;
    }

    private string GetFirstText(
        UnderwritingCase underwritingCase,
        IReadOnlyList<string> keys,
        string fallback)
    {
        if (underwritingCase != null && keys != null)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                string value = underwritingCase.GetText(keys[i]);

                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
        }

        return !string.IsNullOrWhiteSpace(fallback) ? fallback : "계약자 미상";
    }

    private void AdvanceContracts(int turns)
    {
        bool changed = false;

        for (int i = 0; i < contracts.Count; i++)
        {
            ApprovedContractRecord contract = contracts[i];

            if (contract == null || contract.IsArrived)
                continue;

            contract.AdvanceTurns(turns);
            changed = true;
        }

        if (changed)
            NotifyChanged();
    }

    private int GetCurrentTurn()
    {
        GameLoopController controller = GetGameLoopController();
        return controller != null ? controller.CurrentTurn : 0;
    }

    private GameLoopController GetGameLoopController()
    {
        if (gameLoopController == null)
            gameLoopController = UnityEngine.Object.FindFirstObjectByType<GameLoopController>();

        return gameLoopController;
    }

    private void RegisterGameLoopController()
    {
        GameLoopController controller = GetGameLoopController();

        if (registeredGameLoopController == controller)
            return;

        UnregisterGameLoopController();
        registeredGameLoopController = controller;

        if (registeredGameLoopController != null)
        {
            registeredGameLoopController.VisitorResolvedWithDecisionEvent +=
                OnVisitorResolvedWithDecision;
        }
    }

    private void UnregisterGameLoopController()
    {
        if (registeredGameLoopController == null)
            return;

        registeredGameLoopController.VisitorResolvedWithDecisionEvent -=
            OnVisitorResolvedWithDecision;
        registeredGameLoopController = null;
    }

    private void NotifyChanged()
    {
        ContractsChanged?.Invoke(contracts);

        if (logDebug)
            Debug.Log("Approved contract count: " + contracts.Count, this);
    }
}
