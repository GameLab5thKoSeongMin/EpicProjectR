using System.Collections.Generic;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonthlyEvaluationReportView : MonoBehaviour
{
    [SerializeField] private TMP_Text correctCountText;
    [SerializeField] private string correctCountFormat = "{0}/{1}";
    [SerializeField] private TMP_Text performanceText;
    [SerializeField] private string performanceFormat = "{0}";
    [SerializeField] private TMP_Text performanceChangeRateText;
    [SerializeField] private string performanceChangeRateFormat = "{0}";
    [SerializeField] private RectTransform performanceBar;
    [SerializeField] private Button closeButton;
    [SerializeField] private bool autoFindCloseButton = true;
    [SerializeField] private TMP_Text settlementBreakdownText;
    [SerializeField] private TMP_Text underwrittenPremiumText;
    [SerializeField] private TMP_Text performanceShareText;
    [SerializeField] private TMP_Text performanceBonusText;
    [SerializeField] private TMP_Text deductionsText;
    [SerializeField] private TMP_Text netIncomeText;
    [SerializeField] private string underwrittenPremiumLineFormat = "이번분기 인수 보험료 {0}두카트";
    [SerializeField] private string performanceShareLineFormat = "성과 배분율 {0}%";
    [SerializeField] private string performanceBonusLineFormat = "분기 성과급 +{0}두카트";
    [SerializeField] private string deductionLineFormat = "{0} -{1}두카트";
    [SerializeField] private string netIncomeLineFormat = "이번턴 수령액 {0}두카트";
    [SerializeField, Min(1f)] private float maxPerformance = 50f;
    [SerializeField, Min(0f)] private float maxBarHeight = 200f;

    public event Action CloseRequested;

    private Button registeredCloseButton;

    private void OnEnable()
    {
        RegisterCloseButton();
    }

    private void OnDisable()
    {
        UnregisterCloseButton();
    }

    private void OnDestroy()
    {
        UnregisterCloseButton();
    }

    public void Bind(MonthlySettlementSummary summary)
    {
        RegisterCloseButton();

        int totalCount = CountRecords(summary != null ? summary.records : null);
        int correctCount = CountCorrectRecords(summary != null ? summary.records : null);
        int performance = summary != null ? summary.performanceAfter : 0;
        int performanceDelta = summary != null ? summary.PerformanceDelta : 0;
        int performanceChangeRate = CalculatePerformanceChangeRate(performanceDelta);

        SetText(correctCountText, string.Format(correctCountFormat, correctCount, totalCount));
        SetText(performanceText, string.Format(performanceFormat, performance));
        SetText(
            performanceChangeRateText,
            string.Format(
                performanceChangeRateFormat,
                FormatSigned(performanceDelta),
                FormatSigned(performanceChangeRate)));
        BindSettlementBreakdown(summary);
        SetPerformanceBarHeight(performance);
    }

    private void SetPerformanceBarHeight(int performance)
    {
        if (performanceBar == null)
            return;

        float normalized = Mathf.Clamp01(performance / maxPerformance);
        Vector2 sizeDelta = performanceBar.sizeDelta;
        sizeDelta.y = normalized * maxBarHeight;
        performanceBar.sizeDelta = sizeDelta;
    }

    private int CountRecords(IReadOnlyList<MonthlySettlementRecord> records)
    {
        return records != null ? records.Count : 0;
    }

    private int CalculatePerformanceChangeRate(int performanceDelta)
    {
        if (maxPerformance <= 0f)
            return 0;

        return Mathf.RoundToInt(performanceDelta / maxPerformance * 100f);
    }

    private int CountCorrectRecords(IReadOnlyList<MonthlySettlementRecord> records)
    {
        if (records == null)
            return 0;

        int count = 0;

        for (int i = 0; i < records.Count; i++)
        {
            if (IsCorrectCountRecord(records[i]))
                count++;
        }

        return count;
    }

    private bool IsCorrectCountRecord(MonthlySettlementRecord record)
    {
        return record != null && record.performanceDelta > 0;
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }

    private void BindSettlementBreakdown(MonthlySettlementSummary summary)
    {
        if (summary == null)
            summary = new MonthlySettlementSummary();

        SetText(
            underwrittenPremiumText,
            string.Format(underwrittenPremiumLineFormat, summary.underwrittenPremium));
        SetText(
            performanceShareText,
            string.Format(performanceShareLineFormat, Mathf.RoundToInt(summary.performanceShareRate * 100f)));
        SetText(
            performanceBonusText,
            string.Format(performanceBonusLineFormat, summary.performanceBonus));
        SetText(deductionsText, BuildDeductionLines(summary.deductions));
        SetText(
            netIncomeText,
            string.Format(netIncomeLineFormat, FormatSigned(summary.netIncome)));
        SetText(settlementBreakdownText, BuildSettlementBreakdown(summary));
    }

    private string BuildSettlementBreakdown(MonthlySettlementSummary summary)
    {
        if (summary == null)
            summary = new MonthlySettlementSummary();

        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format(
            underwrittenPremiumLineFormat,
            summary.underwrittenPremium));
        builder.AppendLine(string.Format(
            performanceShareLineFormat,
            Mathf.RoundToInt(summary.performanceShareRate * 100f)));
        builder.AppendLine(string.Format(
            performanceBonusLineFormat,
            summary.performanceBonus));

        string deductionLines = BuildDeductionLines(summary.deductions);

        if (!string.IsNullOrEmpty(deductionLines))
            builder.AppendLine(deductionLines);

        builder.Append(string.Format(
            netIncomeLineFormat,
            FormatSigned(summary.netIncome)));

        return builder.ToString();
    }

    private string BuildDeductionLines(IReadOnlyList<MonthlySettlementDeduction> deductions)
    {
        if (deductions == null)
            return string.Empty;

        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < deductions.Count; i++)
        {
            MonthlySettlementDeduction deduction = deductions[i];

            if (deduction == null || deduction.amount <= 0)
                continue;

            if (builder.Length > 0)
                builder.AppendLine();

            builder.Append(string.Format(
                deductionLineFormat,
                string.IsNullOrWhiteSpace(deduction.label) ? "공제" : deduction.label,
                deduction.amount));
        }

        return builder.ToString();
    }

    private string FormatSigned(int value)
    {
        return value >= 0 ? "+" + value : value.ToString();
    }

    public void RequestClose()
    {
        CloseRequested?.Invoke();
    }

    private void RegisterCloseButton()
    {
        Button button = GetCloseButton();

        if (button == null)
            return;

        registeredCloseButton = button;
        registeredCloseButton.onClick.RemoveListener(RequestClose);
        registeredCloseButton.onClick.AddListener(RequestClose);
    }

    private void UnregisterCloseButton()
    {
        if (registeredCloseButton != null)
            registeredCloseButton.onClick.RemoveListener(RequestClose);

        registeredCloseButton = null;
    }

    private Button GetCloseButton()
    {
        if (closeButton != null)
            return closeButton;

        if (!autoFindCloseButton)
            return null;

        closeButton = GetComponentInChildren<Button>(true);
        return closeButton;
    }
}
