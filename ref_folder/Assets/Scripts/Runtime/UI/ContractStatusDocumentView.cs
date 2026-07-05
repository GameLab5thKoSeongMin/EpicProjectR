using System;
using TMPro;
using UnityEngine;

public class ContractStatusDocumentView : MonoBehaviour, IUnderwritingCaseDocumentView, IMonthEndContractDocumentView
{
    [SerializeField] private string contractStatusKey = "contract.accepted";
    [SerializeField] private string contractStatusFormat = "{0}";
    [SerializeField] private string approvedText = "계약 체결";
    [SerializeField] private string rejectedText = "계약 미체결";
    [SerializeField] private TMP_Text contractStatusText;
    [SerializeField] private string contractDescriptionFormat = "{0}";
    [SerializeField] private string approvedDescriptionText;
    [SerializeField] private string rejectedDescriptionText;
    [SerializeField] private TMP_Text contractDescriptionText;

    private string statusTextTemplate;
    private string descriptionTextTemplate;

    private void Awake()
    {
        CaptureTextTemplates();
    }

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        if (contractStatusText != null)
            contractStatusText.text = FormatText(
                GetFormat(contractStatusText, contractStatusFormat, ref statusTextTemplate),
                underwritingCase?.GetText(contractStatusKey));

        if (contractDescriptionText != null)
            contractDescriptionText.text = FormatText(
                GetFormat(contractDescriptionText, contractDescriptionFormat, ref descriptionTextTemplate),
                GetDescriptionText(underwritingCase));
    }

    void IMonthEndContractDocumentView.Bind(MonthlySettlementRecord record)
    {
        bool approved = record != null && record.playerApproved;
        string status = approved
            ? approvedText
            : rejectedText;

        string description = approved
            ? approvedDescriptionText
            : rejectedDescriptionText;

        if (contractStatusText != null)
            contractStatusText.text = FormatText(
                GetFormat(contractStatusText, contractStatusFormat, ref statusTextTemplate),
                status);

        if (contractDescriptionText != null)
            contractDescriptionText.text = FormatText(
                GetFormat(contractDescriptionText, contractDescriptionFormat, ref descriptionTextTemplate),
                description);
    }

    private void CaptureTextTemplates()
    {
        if (statusTextTemplate == null && contractStatusText != null)
            statusTextTemplate = contractStatusText.text;

        if (descriptionTextTemplate == null && contractDescriptionText != null)
            descriptionTextTemplate = contractDescriptionText.text;
    }

    private string GetFormat(
        TMP_Text target,
        string configuredFormat,
        ref string cachedTemplate)
    {
        if (cachedTemplate == null && target != null)
            cachedTemplate = target.text;

        if (!string.IsNullOrEmpty(cachedTemplate) &&
            cachedTemplate.Contains("{0}"))
        {
            return cachedTemplate;
        }

        return configuredFormat;
    }

    private string GetDescriptionText(UnderwritingCase underwritingCase)
    {
        string code = underwritingCase?.GetCode(contractStatusKey);

        if (code == "approved")
            return approvedDescriptionText;

        if (code == "rejected")
            return rejectedDescriptionText;

        return string.Empty;
    }

    private string FormatText(string format, string value)
    {
        value ??= string.Empty;

        if (string.IsNullOrEmpty(format))
            return value;

        try
        {
            return string.Format(format, value);
        }
        catch (FormatException)
        {
            return value;
        }
    }
}
