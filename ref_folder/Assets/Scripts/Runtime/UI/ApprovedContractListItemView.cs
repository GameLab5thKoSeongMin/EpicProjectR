using TMPro;
using UnityEngine;

public class ApprovedContractListItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text contractorNameText;
    [SerializeField] private TMP_Text routeText;
    [SerializeField] private TMP_Text premiumText;
    [SerializeField] private TMP_Text compensationAmountText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string routeFormat = "{0} -> {1}";
    [SerializeField] private string premiumFormat = "보험료 {0}";
    [SerializeField] private string compensationAmountFormat = "보상액 {0}";
    [SerializeField] private string progressFormat = "{0}%";

    public void Bind(ApprovedContractRecord contract)
    {
        SetText(contractorNameText, contract != null ? contract.ContractorName : string.Empty);

        SetText(
            routeText,
            contract != null
                ? string.Format(routeFormat, contract.DeparturePort, contract.DestinationPort)
                : string.Empty);

        SetText(
            premiumText,
            contract != null ? string.Format(premiumFormat, contract.Premium) : string.Empty);

        SetText(
            compensationAmountText,
            contract != null
                ? string.Format(compensationAmountFormat, contract.CompensationAmount)
                : string.Empty);

        SetText(
            progressText,
            contract != null
                ? string.Format(progressFormat, Mathf.RoundToInt(contract.Progress * 100f))
                : string.Empty);

        SetText(statusText, contract != null ? contract.StatusText : string.Empty);
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }
}
