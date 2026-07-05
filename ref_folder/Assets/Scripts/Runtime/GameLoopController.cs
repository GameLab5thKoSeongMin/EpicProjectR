using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameLoopState
{
    WaitingForBell,
    VisitorPresent,
    SettlementOpen,
    MonthTransitioning
}

public enum UnderwritingDialoguePresentationState
{
    Normal,
    Workbench
}

public class GameLoopController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int money;
    [SerializeField] private int performance;

    [Header("Date")]
    [SerializeField] private int currentYear;
    [SerializeField] private int currentMonth = 1;
    [SerializeField] private int currentDay = 1;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text performanceText;

    [Header("Current Contract")]
    [SerializeField] private TMP_Text insurancePremiumText;
    [SerializeField] private string insurancePremiumFormat = "{0}";
    [SerializeField] private string insurancePremiumIncreaseFormat = " <color=#FF3B30>+{0}%</color>";
    [SerializeField] private TMP_Text compensationAmountText;
    [SerializeField] private string compensationAmountFormat = "{0}";

    [Header("Visitors")]
    [SerializeField, Min(0)] private int visitorsPerMonth = 3;
    [SerializeField, Min(0)] private int visitorIncreaseStartTurn;
    [SerializeField, Min(0)] private int visitorIncreaseAmount;
    [SerializeField] private Button bellButton;

    [Header("Economy")]
    [SerializeField, Range(0f, 1f)] private float considerRejectPremiumIncreaseRate = 0.1f;
    [SerializeField, Range(0f, 1f)] private float zeroPerformanceShareRate;
    [SerializeField, Range(0f, 1f)] private float lowPerformanceShareRate = 0.05f;
    [FormerlySerializedAs("premiumIncentiveRate")]
    [SerializeField, Range(0f, 1f)] private float midPerformanceShareRate = 0.1f;
    [SerializeField, Range(0f, 1f)] private float maxPerformanceShareRate = 0.2f;
    [SerializeField] private List<MonthlySettlementDeduction> monthlyDeductions =
        new List<MonthlySettlementDeduction>();
    [SerializeField] private string accidentPayoutDeductionLabel = "보험금 지급";
    [SerializeField, Min(1f)] private float compensationToPerformanceLossRatio = 1000f;
    [SerializeField, Min(0)] private int correctRejectPerformanceReward = 10;
    [SerializeField, Min(0)] private int checkedConsiderReasonPerformanceReward = 5;
    [SerializeField, Min(0)] private int correctApprovePerformanceReward = 5;
    [SerializeField, Min(0)] private int wrongRejectPerformancePenalty = 5;
    [SerializeField] private int accidentPerformancePenalty = -2;

    [Header("Decision UI")]
    [SerializeField] private Button approveButton;
    [SerializeField] private TMP_Text approveButtonLabel;
    [SerializeField] private string approveLabel = "Approve";
    [SerializeField] private string conditionalApproveLabel = "Conditional Approval";

    [Header("Case Source")]
    [SerializeField] private MarineInsuranceCaseDefinition caseDefinition;
    [SerializeField] private List<MarineInsuranceCaseDefinition> caseDefinitions =
        new List<MarineInsuranceCaseDefinition>();
    [SerializeField] private int caseSeed;

    [Header("Newspaper")]
    [SerializeField] private NewspaperGenerator monthlyNewspaperGenerator;
    [SerializeField] private bool regenerateNewspaperOnMonthStart = true;

    [Header("UI")]
    [SerializeField] private UnderwritingCaseApplicationView applicationView;
    [SerializeField] private MarineInsuranceRejectionReasonListView rejectionReasonListView;
    [SerializeField] private ShipPanelVisualizer shipPanelVisualizer;
    [SerializeField] private Image visitorImage;
    [SerializeField] private RectTransformToggleMover[] workbenchToggleMovers;
    [SerializeField] private MonthlySettlementView settlementView;
    [SerializeField] private MonthEndContractDocumentSpawner monthEndContractDocumentSpawner;
    [SerializeField] private CrystalBallDialogueDropZone crystalBallDialogueDropZone;

    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueLogBubblePanelView dialogueLogBubblePanel;
    [SerializeField] private string approvalResponseKey = "contractor.approval";
    [SerializeField] private string declineResponseKey = "contractor.decline";
    [FormerlySerializedAs("conditionalApprovalWarningResponseKey")]
    [SerializeField] private string conditionalApprovalPatienceResponseKey = "contractor.conditionalapproval";
    [SerializeField] private string conditionalApprovalAcceptedResponseKey = "contractor.conditionalapproval.accepted";
    [SerializeField] private string workbenchEntryResponseKey;
    [SerializeField] private bool toggleWorkbenchMoveAfterIntro = true;

    [Header("Month Transition")]
    [SerializeField] private bool showMonthlySettlement = true;
    [SerializeField] private bool autoCompleteMonthTransition = true;
    [SerializeField, Min(1)] private int monthsToAdvanceAfterVisitors = 1;
    [SerializeField, Min(0f)] private float monthTransitionDuration = 0.5f;
    [SerializeField] private UnityEvent monthTransitionStarted;
    [SerializeField] private UnityEvent monthTransitionCompleted;
    [SerializeField] private UnityEvent visitorArrived;
    [SerializeField] private UnityEvent contractReviewStarted;
    [SerializeField] private UnityEvent workbenchEntered;
    [SerializeField] private UnityEvent workbenchExited;
    [SerializeField] private UnityEvent visitorResolved;

    [Header("Runtime")]
    [SerializeField] private GameLoopState state = GameLoopState.WaitingForBell;
    [SerializeField] private UnderwritingDialoguePresentationState dialoguePresentationState =
        UnderwritingDialoguePresentationState.Normal;
    [SerializeField] private int currentTurn;
    [SerializeField] private int remainingVisitorsThisMonth;
    [SerializeField] private UnderwritingCase currentCase;
    [SerializeField] private MarineInsuranceRejectionEvaluationResult currentRejectionEvaluation;
    [SerializeField] private int monthMoneyEarned;
    [SerializeField] private int monthStartPerformance;
    [SerializeField] private List<MonthlySettlementRecord> currentMonthRecords =
        new List<MonthlySettlementRecord>();

    private float monthTransitionTimer;
    private bool isCurrentReviewStarted;
    private bool enterWorkbenchAfterDialogue;
    private bool returnToWorkbenchAfterDialogue;
    private bool leaveCurrentVisitorAfterDialogue;
    private bool resolveCurrentVisitorAfterDialogue;
    private UnderwritingDecision pendingDecisionAfterDialogue;
    private DialogueSequence activeDialogueSequence;
    private DialogueManager registeredDialogueManager;
    private MarineInsuranceRejectionReasonListView registeredRejectionReasonListView;
    private int nextCaseDefinitionIndex;
    private int currentCaseBasePremium;

    public int Money => money;
    public int Performance => performance;
    public int CurrentTurn => currentTurn;
    public GameLoopState State => state;
    public UnderwritingCase CurrentCase => currentCase;
    public MarineInsuranceRejectionEvaluationResult CurrentRejectionEvaluation => currentRejectionEvaluation;
    public event System.Action VisitorArrivedEvent;
    public event System.Action VisitorResolvedEvent;
    public event System.Action<
        UnderwritingDecision,
        UnderwritingCase,
        MarineInsuranceRejectionEvaluationResult,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition>> VisitorResolvedWithDecisionEvent;

    private void Awake()
    {
        NormalizeDate();
        EnsureSettlementView();
        EnsureDialogueManager();
        RegisterDialogueEvents();
        EnsureDialogueLogBubblePanel();
        ApplyDialoguePresentationState();
        EnsureCrystalBallDialogueDropZone();
        RegisterButtonEvents();
        RegisterRejectionReasonListEvents();
    }

    private void Start()
    {
        BeginMonth();
        HideVisitorImage();
        RefreshViews();
    }

    private void Update()
    {
        if (!autoCompleteMonthTransition ||
            state != GameLoopState.MonthTransitioning)
        {
            return;
        }

        monthTransitionTimer += Time.deltaTime;

        if (monthTransitionTimer >= monthTransitionDuration)
            CompleteMonthTransition();
    }

    private void OnDestroy()
    {
        UnregisterDialogueEvents();
        UnregisterButtonEvents();
        UnregisterRejectionReasonListEvents();
    }

    public void RingBell()
    {
        if (state != GameLoopState.WaitingForBell)
            return;

        if (remainingVisitorsThisMonth <= 0)
        {
            StartMonthTransition();
            return;
        }

        OpenNextVisitor();
    }

    public void ApproveCurrentVisitor()
    {
        if (!isCurrentReviewStarted)
            return;

        if (ShouldConsumePatienceForInvalidConditionalApproval())
        {
            SetDialoguePresentationState(UnderwritingDialoguePresentationState.Normal);
            ConsumeCurrentCasePatience();
            return;
        }

        string responseKey = IsConditionalApprovalActive()
            ? conditionalApprovalAcceptedResponseKey
            : approvalResponseKey;

        if (PlayDecisionDialogueThenResolve(responseKey, UnderwritingDecision.Approved))
            return;

        ResolveCurrentVisitor(UnderwritingDecision.Approved);
    }

    public void RejectCurrentVisitor()
    {
        if (!isCurrentReviewStarted)
            return;

        SetDialoguePresentationState(UnderwritingDialoguePresentationState.Normal);

        if (PlayDecisionDialogueThenResolve(declineResponseKey, UnderwritingDecision.Rejected))
            return;

        ResolveCurrentVisitor(UnderwritingDecision.Rejected);
    }

    public void AddMoney(int value)
    {
        money += value;
        UpdatePlayerStatusTexts();
    }

    public void AddPerformance(int value)
    {
        performance += value;
        UpdatePlayerStatusTexts();
    }

    public void CompleteMonthTransition()
    {
        if (state != GameLoopState.MonthTransitioning)
            return;

        AdvanceMonth();
        BeginMonth();
        monthTransitionCompleted?.Invoke();
        RefreshViews();
    }

    private void OpenNextVisitor()
    {
        remainingVisitorsThisMonth = Mathf.Max(0, remainingVisitorsThisMonth - 1);
        currentCase = CreateNextCase();
        currentCaseBasePremium = currentCase != null ? currentCase.premium : 0;
        currentRejectionEvaluation = MarineInsuranceRejectionEvaluator.Evaluate(currentCase);
        isCurrentReviewStarted = false;
        enterWorkbenchAfterDialogue = false;
        returnToWorkbenchAfterDialogue = false;
        leaveCurrentVisitorAfterDialogue = false;
        resolveCurrentVisitorAfterDialogue = false;
        state = GameLoopState.VisitorPresent;
        SetDialoguePresentationState(UnderwritingDialoguePresentationState.Normal);

        BindVisitorImageCurrentCase();
        UpdateBellInteractable();
        visitorArrived?.Invoke();
        VisitorArrivedEvent?.Invoke();
        BindCrystalBallDialogueDropZoneCurrentCase();
        BindDialogueLogBubblePanelCurrentCase();
        BeginCurrentContractReview();
    }

    private UnderwritingCase CreateNextCase()
    {
        MarineInsuranceCaseDefinition selectedCaseDefinition =
            GetNextCaseDefinition();

        if (selectedCaseDefinition == null)
            return new UnderwritingCase();

        int seed = caseSeed;
        seed = seed * 397 ^ currentYear;
        seed = seed * 397 ^ currentMonth;
        seed = seed * 397 ^ currentDay;
        seed = seed * 397 ^ remainingVisitorsThisMonth;

        UnderwritingCase createdCase = selectedCaseDefinition.CreateCase(seed);
        return createdCase ?? new UnderwritingCase();
    }

    private MarineInsuranceCaseDefinition GetNextCaseDefinition()
    {
        if (caseDefinitions == null || caseDefinitions.Count <= 0)
            return caseDefinition;

        for (int i = 0; i < caseDefinitions.Count; i++)
        {
            int index = nextCaseDefinitionIndex % caseDefinitions.Count;
            nextCaseDefinitionIndex++;

            MarineInsuranceCaseDefinition selectedCaseDefinition =
                caseDefinitions[index];

            if (selectedCaseDefinition != null)
                return selectedCaseDefinition;
        }

        return caseDefinition;
    }

    private void ResolveCurrentVisitor(UnderwritingDecision decision)
    {
        if (state != GameLoopState.VisitorPresent)
            return;

        MonthlySettlementRecord record = new MonthlySettlementRecord();
        record.underwritingCase = currentCase;
        record.playerApproved = decision == UnderwritingDecision.Approved;
        record.caseHadRejectReason = currentRejectionEvaluation != null &&
                                     currentRejectionEvaluation.HasRejectReason;
        record.caseHadConsiderRejectReason = currentRejectionEvaluation != null &&
                                             currentRejectionEvaluation.HasConsiderRejectReason;
        record.playerDecisionMatchedCase = IsPlayerDecisionAccepted(decision);
        record.triggeredRejectionReasonNames = currentRejectionEvaluation != null
            ? currentRejectionEvaluation.GetTriggeredReasonNames()
            : new List<string>();
        record.premium = currentCase != null ? currentCase.premium : 0;
        record.compensationAmount = currentCase != null ? currentCase.compensationAmount : 0;
        record.accidentOccurrenceProbability = currentCase != null
            ? currentCase.accidentOccurrenceProbability
            : 0f;
        record.accidentOccurred = RollAccidentOccurrence(decision);
        record.accidentPayout = record.accidentOccurred ? record.compensationAmount : 0;
        record.playerCheckedAllTriggeredReasons = AreAllTriggeredReasonsChecked();
        WriteContractDecisionData(record.underwritingCase, decision);

        int performanceDelta = CalculatePerformanceDelta(decision);

        if (record.accidentOccurred)
            performanceDelta += accidentPerformancePenalty;

        record.moneyEarned = 0;
        record.performanceDelta = performanceDelta;

        performance += performanceDelta;
        UpdatePlayerStatusTexts();

        currentMonthRecords.Add(record);

        UnderwritingCase resolvedCase = currentCase;
        MarineInsuranceRejectionEvaluationResult resolvedEvaluation = currentRejectionEvaluation;
        List<MarineInsuranceRejectionReasonDefinition> uncheckedTriggeredReasons =
            rejectionReasonListView != null
                ? rejectionReasonListView.GetUncheckedTriggeredReasons(currentRejectionEvaluation, true)
                : new List<MarineInsuranceRejectionReasonDefinition>();

        NotifyVisitorResolved();
        ClearCurrentVisitorViews();
        VisitorResolvedWithDecisionEvent?.Invoke(
            decision,
            resolvedCase,
            resolvedEvaluation,
            uncheckedTriggeredReasons);

        if (remainingVisitorsThisMonth <= 0)
            OpenMonthlySettlement();
        else
            SetState(GameLoopState.WaitingForBell);
    }

    private bool ShouldConsumePatienceForInvalidConditionalApproval()
    {
        return IsConditionalApprovalActive() &&
               rejectionReasonListView != null &&
               rejectionReasonListView.HasCheckedUntriggeredReasonWithDecision(
                   currentCase,
                   MarineInsuranceRejectionDecision.ConsiderReject);
    }

    private void ConsumeCurrentCasePatience()
    {
        if (currentCase == null)
            return;

        string responseKey = GetConditionalApprovalPatienceResponseKey();
        currentCase.patience = Mathf.Max(0, currentCase.patience - 1);
        currentCase.conditionalApprovalPatienceTriggerCount++;

        Debug.Log("Conditional approval rejected by contractor patience. Remaining patience: " + currentCase.patience, this);

        if (currentCase.patience <= 0)
        {
            if (!PlayCurrentCaseKeyedDialogue(responseKey))
                CurrentVisitorLeavesWithoutContract();
            else
                leaveCurrentVisitorAfterDialogue = true;
        }
        else
        {
            if (PlayCurrentCaseKeyedDialogue(responseKey))
                returnToWorkbenchAfterDialogue = true;
        }
    }

    private string GetConditionalApprovalPatienceResponseKey()
    {
        if (currentCase == null ||
            currentCase.conditionalApprovalPatienceTriggerCount <= 0)
        {
            return conditionalApprovalPatienceResponseKey;
        }

        int responseNumber = currentCase.conditionalApprovalPatienceTriggerCount + 1;
        return conditionalApprovalPatienceResponseKey + responseNumber;
    }

    private bool PlayDecisionDialogueThenResolve(
        string responseKey,
        UnderwritingDecision decision)
    {
        SetDialoguePresentationState(UnderwritingDialoguePresentationState.Normal);

        if (!PlayCurrentCaseKeyedDialogue(responseKey))
            return false;

        pendingDecisionAfterDialogue = decision;
        resolveCurrentVisitorAfterDialogue = true;
        return true;
    }

    private void CurrentVisitorLeavesWithoutContract()
    {
        NotifyVisitorResolved();
        ClearCurrentVisitorViews();

        if (remainingVisitorsThisMonth <= 0)
            OpenMonthlySettlement();
        else
            SetState(GameLoopState.WaitingForBell);
    }

    private void NotifyVisitorResolved()
    {
        visitorResolved?.Invoke();
        VisitorResolvedEvent?.Invoke();
    }

    private int CalculateMoneyEarned(UnderwritingDecision decision)
    {
        return 0;
    }

    private bool IsPlayerDecisionAccepted(UnderwritingDecision decision)
    {
        if (currentRejectionEvaluation == null)
            return true;

        if (currentRejectionEvaluation.HasRejectReason)
            return decision == UnderwritingDecision.Rejected;

        if (currentRejectionEvaluation.HasConsiderRejectReason)
            return true;

        return decision == UnderwritingDecision.Approved;
    }

    private int CalculatePerformanceDelta(UnderwritingDecision decision)
    {
        if (currentCase == null || currentRejectionEvaluation == null)
        {
            return 0;
        }

        bool checkedAllTriggeredReasons = AreAllTriggeredReasonsChecked();
        bool hasRejectReason = currentRejectionEvaluation.HasRejectReason;
        bool hasConsiderRejectReason = currentRejectionEvaluation.HasConsiderRejectReason;

        if (hasRejectReason || hasConsiderRejectReason)
        {
            if (decision == UnderwritingDecision.Rejected)
            {
                if (!checkedAllTriggeredReasons)
                    return 0;

                return hasRejectReason
                    ? correctRejectPerformanceReward
                    : checkedConsiderReasonPerformanceReward;
            }

            return checkedAllTriggeredReasons ? 0 : -CalculateCompensationPerformanceLoss();
        }

        return decision == UnderwritingDecision.Approved
            ? correctApprovePerformanceReward
            : -wrongRejectPerformancePenalty;
    }

    private int CalculateCompensationPerformanceLoss()
    {
        int loss = Mathf.CeilToInt(currentCase.compensationAmount / compensationToPerformanceLossRatio);
        return Mathf.Max(0, loss);
    }

    private bool AreAllTriggeredReasonsChecked()
    {
        return rejectionReasonListView != null &&
               rejectionReasonListView.AreAllTriggeredReasonsChecked(currentRejectionEvaluation);
    }

    private void OpenMonthlySettlement()
    {
        MonthlySettlementSummary summary = CreateMonthlySettlementSummary();
        ApplyMonthlySettlement(summary);

        if (!showMonthlySettlement)
        {
            StartMonthTransition();
            return;
        }

        SetState(GameLoopState.SettlementOpen);

        if (settlementView == null)
        {
            StartMonthTransition();
            return;
        }

        settlementView.Show(summary, CloseSettlementAndStartMonthTransition);
        ShowMonthEndContractDocuments(summary);
    }

    private void ApplyMonthlySettlement(MonthlySettlementSummary summary)
    {
        if (summary == null)
            return;

        money += summary.netIncome;
        monthMoneyEarned = summary.netIncome;
        UpdatePlayerStatusTexts();
    }

    public void CloseSettlementAndStartMonthTransition()
    {
        if (state == GameLoopState.MonthTransitioning)
            return;

        if (settlementView != null)
            settlementView.Hide();

        HideMonthEndContractDocuments();
        StartMonthTransition();
    }

    private void ShowMonthEndContractDocuments(MonthlySettlementSummary summary)
    {
        if (monthEndContractDocumentSpawner == null)
            return;

        monthEndContractDocumentSpawner.gameObject.SetActive(true);
        monthEndContractDocumentSpawner.Bind(summary);
    }

    private void HideMonthEndContractDocuments()
    {
        if (monthEndContractDocumentSpawner == null)
            return;

        monthEndContractDocumentSpawner.Clear();
    }

    private MonthlySettlementSummary CreateMonthlySettlementSummary()
    {
        MonthlySettlementSummary summary = new MonthlySettlementSummary();
        summary.year = currentYear;
        summary.month = currentMonth;
        summary.performanceBefore = monthStartPerformance;
        summary.performanceAfter = performance;
        summary.underwrittenPremium = CalculateUnderwrittenPremium();
        summary.performanceShareRate = GetPerformanceShareRate(performance);
        summary.performanceBonus = Mathf.RoundToInt(
            summary.underwrittenPremium * summary.performanceShareRate);
        CopyMonthlyDeductions(summary);
        summary.totalDeductions = CalculateTotalDeductions(summary.deductions);
        summary.netIncome = summary.performanceBonus - summary.totalDeductions;
        summary.moneyEarned = summary.netIncome;

        if (currentMonthRecords != null)
        {
            for (int i = 0; i < currentMonthRecords.Count; i++)
                summary.records.Add(currentMonthRecords[i]);
        }

        return summary;
    }

    private int CalculateUnderwrittenPremium()
    {
        if (currentMonthRecords == null)
            return 0;

        int total = 0;

        for (int i = 0; i < currentMonthRecords.Count; i++)
        {
            MonthlySettlementRecord record = currentMonthRecords[i];

            if (record != null && record.playerApproved)
                total += Mathf.Max(0, record.premium);
        }

        return total;
    }

    private float GetPerformanceShareRate(int value)
    {
        if (value <= 0)
            return zeroPerformanceShareRate;

        if (value < 25)
            return lowPerformanceShareRate;

        if (value < 50)
            return midPerformanceShareRate;

        return maxPerformanceShareRate;
    }

    private void CopyMonthlyDeductions(MonthlySettlementSummary summary)
    {
        if (summary == null || monthlyDeductions == null)
            return;

        for (int i = 0; i < monthlyDeductions.Count; i++)
        {
            MonthlySettlementDeduction deduction = monthlyDeductions[i];

            if (deduction == null || deduction.amount <= 0)
                continue;

            summary.deductions.Add(new MonthlySettlementDeduction
            {
                label = deduction.label,
                amount = deduction.amount
            });
        }
    }

    private void AddAccidentPayoutDeduction(MonthlySettlementSummary summary)
    {
        if (summary == null)
            return;

        int accidentPayout = CalculateAccidentPayout();

        if (accidentPayout <= 0)
            return;

        summary.deductions.Add(new MonthlySettlementDeduction
        {
            label = accidentPayoutDeductionLabel,
            amount = accidentPayout
        });
    }

    private int CalculateAccidentPayout()
    {
        if (currentMonthRecords == null)
            return 0;

        int total = 0;

        for (int i = 0; i < currentMonthRecords.Count; i++)
        {
            MonthlySettlementRecord record = currentMonthRecords[i];

            if (record != null && record.accidentOccurred)
                total += Mathf.Max(0, record.accidentPayout);
        }

        return total;
    }

    private int CalculateTotalDeductions(List<MonthlySettlementDeduction> deductions)
    {
        if (deductions == null)
            return 0;

        int total = 0;

        for (int i = 0; i < deductions.Count; i++)
        {
            if (deductions[i] != null)
                total += Mathf.Max(0, deductions[i].amount);
        }

        return total;
    }

    private bool RollAccidentOccurrence(UnderwritingDecision decision)
    {
        if (decision != UnderwritingDecision.Approved || currentCase == null)
            return false;

        float probability = Mathf.Clamp01(currentCase.accidentOccurrenceProbability);
        return probability > 0f && Random.value < probability;
    }

    private void WriteContractDecisionData(
        UnderwritingCase underwritingCase,
        UnderwritingDecision decision)
    {
        if (underwritingCase == null)
            return;

        bool approved = decision == UnderwritingDecision.Approved;
        underwritingCase.SetValue(
            "contract.accepted",
            approved ? "계약 체결" : "계약 미체결",
            approved ? "approved" : "rejected");
        underwritingCase.SetValue(
            "contract.status",
            approved ? "승인" : "거절",
            approved ? "approved" : "rejected");
    }

    private void StartMonthTransition()
    {
        SetState(GameLoopState.MonthTransitioning);
        monthTransitionTimer = 0f;
        monthTransitionStarted?.Invoke();
    }

    private void BeginMonth()
    {
        currentTurn = Mathf.Max(0, currentTurn) + 1;
        remainingVisitorsThisMonth = GetVisitorsForCurrentTurn();
        monthMoneyEarned = 0;
        monthStartPerformance = performance;

        if (currentMonthRecords == null)
            currentMonthRecords = new List<MonthlySettlementRecord>();
        else
            currentMonthRecords.Clear();

        RefreshMonthlyNewspaper();
        SetState(GameLoopState.WaitingForBell);
    }

    private int GetVisitorsForCurrentTurn()
    {
        int visitorCount = Mathf.Max(0, visitorsPerMonth);

        if (visitorIncreaseStartTurn > 0 &&
            currentTurn >= visitorIncreaseStartTurn)
        {
            visitorCount += Mathf.Max(0, visitorIncreaseAmount);
        }

        return visitorCount;
    }

    private void RefreshMonthlyNewspaper()
    {
        if (!regenerateNewspaperOnMonthStart)
            return;

        EnsureMonthlyNewspaperGenerator();

        if (monthlyNewspaperGenerator == null)
        {
            if (NewspaperRuntimeContext.Current != null)
                NewspaperRuntimeContext.Current.ClearPublishedNewspapers();

            Debug.LogWarning("GameLoopController: NewspaperGenerator is not assigned. Monthly newspaper was not generated.");
            return;
        }

        if (monthlyNewspaperGenerator.RegenerateNewspaper() == null)
            Debug.LogWarning("GameLoopController: Monthly newspaper was not generated. Check prefab and root.");
    }

    private void EnsureMonthlyNewspaperGenerator()
    {
        if (monthlyNewspaperGenerator != null)
            return;

        monthlyNewspaperGenerator = Object.FindFirstObjectByType<NewspaperGenerator>();
    }

    private void AdvanceMonth()
    {
        int monthsToAdvance = Mathf.Max(1, monthsToAdvanceAfterVisitors);

        for (int i = 0; i < monthsToAdvance; i++)
        {
            currentMonth++;

            if (currentMonth > 12)
            {
                currentMonth = 1;
                currentYear++;
            }
        }
    }

    private void SetState(GameLoopState value)
    {
        state = value;
        UpdateBellInteractable();
    }

    private void BindCurrentCaseToViews()
    {
        if (applicationView != null)
            applicationView.Bind(currentCase);

        UpdateCurrentContractTexts();

        RegisterRejectionReasonListEvents();

        if (rejectionReasonListView != null)
            rejectionReasonListView.Bind(currentCase);

        if (shipPanelVisualizer != null)
            shipPanelVisualizer.Bind(currentCase);

        ApplyConsiderRejectPremiumAdjustment();
        BindCrystalBallDialogueDropZoneCurrentCase();
        BindDialogueLogBubblePanelCurrentCase();
        UpdateApproveButtonLabel();
    }

    private void ClearCurrentVisitorViews()
    {
        if (applicationView != null)
            applicationView.Bind((UnderwritingCase)null);

        if (rejectionReasonListView != null)
            rejectionReasonListView.Bind(null);

        if (shipPanelVisualizer != null)
            shipPanelVisualizer.Bind(null);

        HideVisitorImage();

        if (dialogueManager != null)
            dialogueManager.StartDialogue(null);

        if (crystalBallDialogueDropZone != null)
            crystalBallDialogueDropZone.Bind(null, GetCurrentGameDate());

        if (dialogueLogBubblePanel != null)
        {
            dialogueLogBubblePanel.Bind(null);
            dialogueLogBubblePanel.HidePanel();
        }

        currentCase = null;
        currentCaseBasePremium = 0;
        currentRejectionEvaluation = null;
        isCurrentReviewStarted = false;
        enterWorkbenchAfterDialogue = false;
        returnToWorkbenchAfterDialogue = false;
        leaveCurrentVisitorAfterDialogue = false;
        resolveCurrentVisitorAfterDialogue = false;
        SetDialoguePresentationState(UnderwritingDialoguePresentationState.Normal);
        UpdateCurrentContractTexts();
        UpdateApproveButtonLabel();
    }

    private void BeginCurrentContractReview()
    {
        if (isCurrentReviewStarted || currentCase == null)
            return;

        isCurrentReviewStarted = true;
        BindCurrentCaseToViews();

        if (PlayCurrentCaseGreetingDialogue())
        {
            enterWorkbenchAfterDialogue = true;
            return;
        }

        EnterWorkbenchAfterIntroDialogue();
    }

    private bool PlayCurrentCaseGreetingDialogue()
    {
        if (currentCase == null || currentCase.dialogueProfile == null)
        {
            return false;
        }

        EnsureDialogueManager();

        if (dialogueManager == null)
            return false;

        DialogueSequence sequence = DialogueSequenceBuilder.BuildGreetingSequence(
            currentCase.dialogueProfile,
            currentCase,
            -1,
            null,
            null);

        if (sequence != null && sequence.Count > 0)
        {
            dialogueManager.StartDialogue(sequence);
            return true;
        }

        return false;
    }

    private bool PlayCurrentCaseKeyedDialogue(string responseKey)
    {
        if (currentCase == null ||
            currentCase.dialogueProfile == null ||
            string.IsNullOrWhiteSpace(responseKey))
        {
            return false;
        }

        EnsureDialogueManager();

        if (dialogueManager == null)
            return false;

        DialogueSequence sequence = DialogueSequenceBuilder.BuildKeyedResponse(
            currentCase.dialogueProfile,
            currentCase,
            -1,
            responseKey,
            null,
            null);

        if (sequence == null || sequence.Count <= 0)
            return false;

        dialogueManager.StartDialogue(sequence);
        return true;
    }

    private void EnterWorkbenchAfterIntroDialogue()
    {
        ToggleWorkbenchMoveAfterIntro();
        EnterWorkbenchDialogueState();
        contractReviewStarted?.Invoke();

        if (!string.IsNullOrWhiteSpace(workbenchEntryResponseKey))
            PlayCurrentCaseKeyedDialogue(workbenchEntryResponseKey);
    }

    private void ToggleWorkbenchMoveAfterIntro()
    {
        if (!toggleWorkbenchMoveAfterIntro ||
            workbenchToggleMovers == null ||
            workbenchToggleMovers.Length == 0)
        {
            return;
        }

        for (int i = 0; i < workbenchToggleMovers.Length; i++)
        {
            if (workbenchToggleMovers[i] != null)
                workbenchToggleMovers[i].ToggleMove();
        }
    }

    private void EnterWorkbenchDialogueState()
    {
        SetDialoguePresentationState(UnderwritingDialoguePresentationState.Workbench);
        BindDialogueLogBubblePanelCurrentCase();
    }

    private void SetDialoguePresentationState(UnderwritingDialoguePresentationState value)
    {
        if (dialoguePresentationState == value)
        {
            ApplyDialoguePresentationState();
            return;
        }

        bool wasWorkbench = dialoguePresentationState == UnderwritingDialoguePresentationState.Workbench;
        dialoguePresentationState = value;
        ApplyDialoguePresentationState();

        bool isWorkbench = dialoguePresentationState == UnderwritingDialoguePresentationState.Workbench;

        if (!wasWorkbench && isWorkbench)
            workbenchEntered?.Invoke();
        else if (wasWorkbench && !isWorkbench)
            workbenchExited?.Invoke();
    }

    private void ApplyDialoguePresentationState()
    {
        bool isWorkbench = dialoguePresentationState == UnderwritingDialoguePresentationState.Workbench;

        if (dialogueManager != null)
            dialogueManager.SetDialogueObjectSuppressed(isWorkbench);

        if (dialogueLogBubblePanel == null)
            return;

        dialogueLogBubblePanel.SetDialogueManager(dialogueManager);
        dialogueLogBubblePanel.SetDialogueOutputEnabled(isWorkbench);

        if (!isWorkbench || currentCase == null)
        {
            dialogueLogBubblePanel.HidePanel();
            return;
        }

        dialogueLogBubblePanel.ShowPanel();
        dialogueLogBubblePanel.Bind(currentCase);
    }

    private GameDate GetCurrentGameDate()
    {
        return new GameDate(currentYear, currentMonth, currentDay);
    }

    private void HideVisitorImage()
    {
        if (visitorImage == null)
            return;

        Color color = visitorImage.color;
        color.a = 0f;
        visitorImage.color = color;
    }

    private void BindVisitorImageCurrentCase()
    {
        if (visitorImage == null)
            return;

        Sprite sprite = currentCase != null ? currentCase.npcSprite : null;
        visitorImage.sprite = sprite;

        Color color = visitorImage.color;
        color.a = sprite != null ? 1f : 0f;
        visitorImage.color = color;
        visitorImage.enabled = sprite != null;
    }

    private void EnsureSettlementView()
    {
        if (settlementView != null)
            return;

        settlementView = GetComponent<MonthlySettlementView>();

        if (settlementView == null)
            settlementView = gameObject.AddComponent<MonthlySettlementView>();
    }

    private void EnsureDialogueManager()
    {
        if (dialogueManager != null)
        {
            RegisterDialogueEvents();
            return;
        }

        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
        RegisterDialogueEvents();
    }

    private void RegisterDialogueEvents()
    {
        if (registeredDialogueManager == dialogueManager)
            return;

        UnregisterDialogueEvents();
        registeredDialogueManager = dialogueManager;

        if (registeredDialogueManager != null)
        {
            registeredDialogueManager.DialogueStarted += OnDialogueStarted;
            registeredDialogueManager.DialogueEnded += OnDialogueEnded;
        }
    }

    private void UnregisterDialogueEvents()
    {
        if (registeredDialogueManager == null)
            return;

        registeredDialogueManager.DialogueStarted -= OnDialogueStarted;
        registeredDialogueManager.DialogueEnded -= OnDialogueEnded;
        registeredDialogueManager = null;
    }

    private void OnDialogueStarted(DialogueSequence sequence)
    {
        activeDialogueSequence = sequence;

        if (sequence == null ||
            sequence.AdditionalSubmissionTiming != DialogueAdditionalSubmissionTiming.OnStart)
        {
            return;
        }

        SubmitAdditionalDocument(sequence.AdditionalSubmissionDocumentPrefab);
    }

    private void OnDialogueEnded()
    {
        DialogueSequence endedSequence = activeDialogueSequence;
        activeDialogueSequence = null;

        if (endedSequence != null &&
            endedSequence.AdditionalSubmissionTiming == DialogueAdditionalSubmissionTiming.OnEnd)
        {
            SubmitAdditionalDocument(endedSequence.AdditionalSubmissionDocumentPrefab);
        }

        if (resolveCurrentVisitorAfterDialogue)
        {
            UnderwritingDecision decision = pendingDecisionAfterDialogue;
            resolveCurrentVisitorAfterDialogue = false;
            ResolveCurrentVisitor(decision);
            return;
        }

        if (leaveCurrentVisitorAfterDialogue)
        {
            leaveCurrentVisitorAfterDialogue = false;
            CurrentVisitorLeavesWithoutContract();
            return;
        }

        if (enterWorkbenchAfterDialogue)
        {
            enterWorkbenchAfterDialogue = false;
            EnterWorkbenchAfterIntroDialogue();
            return;
        }

        if (returnToWorkbenchAfterDialogue)
        {
            returnToWorkbenchAfterDialogue = false;
            EnterWorkbenchDialogueState();
        }
    }

    private void SubmitAdditionalDocument(GameObject prefab)
    {
        if (prefab == null || applicationView == null)
            return;

        GameObject submittedDocument = applicationView.SubmitAdditionalDocument(prefab);

        if (submittedDocument != null)
            Debug.Log("Additional submission document submitted: " + prefab.name);
    }

    private void EnsureDialogueLogBubblePanel()
    {
        if (dialogueLogBubblePanel != null)
            return;

        dialogueLogBubblePanel =
            Object.FindFirstObjectByType<DialogueLogBubblePanelView>(
                FindObjectsInactive.Include);

        if (dialogueLogBubblePanel != null)
            dialogueLogBubblePanel.SetDialogueManager(dialogueManager);
    }

    private void EnsureCrystalBallDialogueDropZone()
    {
        if (crystalBallDialogueDropZone != null)
            return;

        crystalBallDialogueDropZone =
            Object.FindFirstObjectByType<CrystalBallDialogueDropZone>();
    }

    private void BindCrystalBallDialogueDropZoneCurrentCase()
    {
        if (crystalBallDialogueDropZone == null)
            return;

        crystalBallDialogueDropZone.SetDialogueManager(dialogueManager);
        crystalBallDialogueDropZone.Bind(currentCase, GetCurrentGameDate());
    }

    private void BindDialogueLogBubblePanelCurrentCase()
    {
        if (dialogueLogBubblePanel == null)
            return;

        dialogueLogBubblePanel.SetDialogueManager(dialogueManager);

        if (currentCase == null)
        {
            dialogueLogBubblePanel.Bind(null);
            dialogueLogBubblePanel.HidePanel();
            return;
        }

        dialogueLogBubblePanel.Bind(currentCase);

        if (dialoguePresentationState == UnderwritingDialoguePresentationState.Workbench)
            dialogueLogBubblePanel.ShowPanel();
        else
            dialogueLogBubblePanel.HidePanel();
    }

    private void RefreshViews()
    {
        UpdateDateText();
        UpdatePlayerStatusTexts();
        ApplyConsiderRejectPremiumAdjustment();
        UpdateCurrentContractTexts();
        UpdateBellInteractable();
        UpdateApproveButtonLabel();
    }

    private void OnCheckedReasonsChanged()
    {
        ApplyConsiderRejectPremiumAdjustment();
        UpdateCurrentContractTexts();
        UpdateApproveButtonLabel();
    }

    private void UpdateApproveButtonLabel()
    {
        if (approveButtonLabel == null && approveButton != null)
            approveButtonLabel = approveButton.GetComponentInChildren<TMP_Text>(true);

        if (approveButtonLabel == null)
            return;

        bool shouldUseConditionalLabel = IsConditionalApprovalActive();

        approveButtonLabel.text = shouldUseConditionalLabel
            ? conditionalApproveLabel
            : approveLabel;
    }

    private bool IsConditionalApprovalActive()
    {
        return rejectionReasonListView != null &&
               rejectionReasonListView.HasCheckedReasonWithDecision(
                   MarineInsuranceRejectionDecision.ConsiderReject);
    }

    private void ApplyConsiderRejectPremiumAdjustment()
    {
        if (currentCase == null)
            return;

        int checkedConsiderReasonCount = rejectionReasonListView != null
            ? rejectionReasonListView.CountCheckedReasonsWithDecision(
                MarineInsuranceRejectionDecision.ConsiderReject)
            : 0;

        float premiumMultiplier = 1f + checkedConsiderReasonCount * considerRejectPremiumIncreaseRate;
        currentCase.premium = Mathf.RoundToInt(currentCaseBasePremium * premiumMultiplier);
    }

    private void UpdatePlayerStatusTexts()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();

        if (performanceText != null)
            performanceText.text = performance.ToString();
    }

    private void UpdateCurrentContractTexts()
    {
        if (currentCase == null)
        {
            SetText(insurancePremiumText, string.Empty);
            SetText(compensationAmountText, string.Empty);
            return;
        }

        SetText(insurancePremiumText, FormatPremiumText());
        SetText(compensationAmountText, FormatAmount(compensationAmountFormat, currentCase.compensationAmount));
    }

    private string FormatPremiumText()
    {
        string premiumText = FormatAmount(insurancePremiumFormat, currentCase.premium);
        int increasePercent = GetConsiderRejectPremiumIncreasePercent();

        if (increasePercent <= 0)
            return premiumText;

        return premiumText + string.Format(insurancePremiumIncreaseFormat, increasePercent);
    }

    private int GetConsiderRejectPremiumIncreasePercent()
    {
        int checkedConsiderReasonCount = rejectionReasonListView != null
            ? rejectionReasonListView.CountCheckedReasonsWithDecision(
                MarineInsuranceRejectionDecision.ConsiderReject)
            : 0;

        return Mathf.RoundToInt(checkedConsiderReasonCount * considerRejectPremiumIncreaseRate * 100f);
    }

    private string FormatAmount(string format, int value)
    {
        return string.IsNullOrEmpty(format)
            ? value.ToString()
            : string.Format(format, value);
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }

    private void UpdateDateText()
    {
        if (dateText != null)
            dateText.text = string.Format(
                "{0:000}/{1:00}/{2:00}",
                currentYear,
                currentMonth,
                currentDay);
    }

    private void UpdateBellInteractable()
    {
        if (bellButton != null)
            bellButton.interactable = state == GameLoopState.WaitingForBell;
    }

    private void RegisterButtonEvents()
    {
        if (bellButton != null)
            bellButton.onClick.AddListener(RingBell);
    }

    private void UnregisterButtonEvents()
    {
        if (bellButton != null)
            bellButton.onClick.RemoveListener(RingBell);
    }

    private void RegisterRejectionReasonListEvents()
    {
        if (registeredRejectionReasonListView == rejectionReasonListView)
            return;

        UnregisterRejectionReasonListEvents();
        registeredRejectionReasonListView = rejectionReasonListView;

        if (registeredRejectionReasonListView != null)
            registeredRejectionReasonListView.CheckedReasonsChanged += OnCheckedReasonsChanged;
    }

    private void UnregisterRejectionReasonListEvents()
    {
        if (registeredRejectionReasonListView == null)
            return;

        registeredRejectionReasonListView.CheckedReasonsChanged -= OnCheckedReasonsChanged;
        registeredRejectionReasonListView = null;
    }

    private void NormalizeDate()
    {
        currentYear = Mathf.Max(0, currentYear);
        currentMonth = Mathf.Clamp(currentMonth, 1, 12);
        currentDay = Mathf.Clamp(currentDay, 1, 31);
    }
}
