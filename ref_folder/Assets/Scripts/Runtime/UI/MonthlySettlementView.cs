using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonthlySettlementRecord
{
    public UnderwritingCase underwritingCase;
    public bool playerApproved;
    public bool caseHadRejectReason;
    public bool caseHadConsiderRejectReason;
    public bool playerDecisionMatchedCase;
    public bool playerCheckedAllTriggeredReasons;
    public int premium;
    public int compensationAmount;
    public float accidentOccurrenceProbability;
    public bool accidentOccurred;
    public int accidentPayout;
    public int moneyEarned;
    public int performanceDelta;
    public List<string> triggeredRejectionReasonNames = new List<string>();
}

[Serializable]
public class MonthlySettlementDeduction
{
    public string label = "생활비";
    [Min(0)] public int amount;
}

[Serializable]
public class MonthlySettlementSummary
{
    public int year;
    public int month;
    public int moneyEarned;
    public int underwrittenPremium;
    public float performanceShareRate;
    public int performanceBonus;
    public int totalDeductions;
    public int netIncome;
    public int performanceBefore;
    public int performanceAfter;
    public List<MonthlySettlementRecord> records = new List<MonthlySettlementRecord>();
    public List<MonthlySettlementDeduction> deductions = new List<MonthlySettlementDeduction>();

    public int PerformanceDelta => performanceAfter - performanceBefore;
}

public class MonthlySettlementView : MonoBehaviour
{
    [SerializeField] private Transform reportRoot;
    [SerializeField] private MonthlyEvaluationReportView reportPrefab;
    [SerializeField] private MonthlyEvaluationReportView reportView;

    private Action closeCallback;
    private bool isClosing; 

    public void Show(
        MonthlySettlementSummary summary,
        Action onClose)
    {
        closeCallback = onClose;
        isClosing = false;

        if (reportRoot != null)
            reportRoot.gameObject.SetActive(true);

        MonthlyEvaluationReportView view = EnsureReportView();

        if (view == null)
        {
            Close();
            return;
        }

        view.gameObject.SetActive(true);
        view.Bind(summary);
    }

    public void CloseSettlement()
    {
        Close();
    }

    public void Hide()
    {
        HideReport();
    }

    private MonthlyEvaluationReportView EnsureReportView()
    {
        if (reportView != null)
        {
            reportView.CloseRequested -= OnReportCloseRequested;
            reportView.CloseRequested += OnReportCloseRequested;
            return reportView;
        }

        Transform parent = GetReportParent();

        if (reportPrefab != null)
            reportView = Instantiate(reportPrefab, parent);
        else
            reportView = parent.GetComponentInChildren<MonthlyEvaluationReportView>(true);

        if (reportView == null)
            Debug.LogWarning("MonthlySettlementView: Report prefab or scene report view is not assigned.", this);

        if (reportView != null)
        {
            reportView.CloseRequested -= OnReportCloseRequested;
            reportView.CloseRequested += OnReportCloseRequested;
        }

        return reportView;
    }

    private Transform GetReportParent()
    {
        if (reportRoot != null)
            return reportRoot;

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
            return parentCanvas.transform;

        Canvas sceneCanvas = FindFirstObjectByType<Canvas>();
        return sceneCanvas != null ? sceneCanvas.transform : transform;
    }

    private void HideReport()
    {
        if (reportView != null)
            reportView.CloseRequested -= OnReportCloseRequested;

        ClearReportRootChildren();
        reportView = null;
    }

    private void ClearReportRootChildren()
    {
        if (reportRoot == null)
            return;

        for (int i = reportRoot.childCount - 1; i >= 0; i--)
            Destroy(reportRoot.GetChild(i).gameObject);
    }

    private void OnReportCloseRequested()
    {
        Close();
    }

    private void Close()
    {
        if (isClosing)
            return;

        isClosing = true;
        Hide();

        Action callback = closeCallback;
        closeCallback = null;
        isClosing = false;
        callback?.Invoke();
    }
}
