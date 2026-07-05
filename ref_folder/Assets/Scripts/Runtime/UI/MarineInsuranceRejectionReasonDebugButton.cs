using UnityEngine;
using UnityEngine.UI;

public class MarineInsuranceRejectionReasonDebugButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private bool toggleCheckOnClick = true;

    private UnderwritingCase underwritingCase;
    private MarineInsuranceRejectionReasonDefinition reason;
    private MarineInsuranceRejectionReasonItemView itemView;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        itemView = GetComponentInParent<MarineInsuranceRejectionReasonItemView>();

        if (button != null)
            button.onClick.AddListener(LogCurrentResult);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(LogCurrentResult);
    }

    public void Bind(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionReasonDefinition reason)
    {
        this.underwritingCase = underwritingCase;
        this.reason = reason;
    }

    public void LogCurrentResult()
    {
        if (reason == null)
        {
            Debug.Log("Rejection reason debug: no reason bound.", this);
            return;
        }

        bool isTriggered = underwritingCase != null && reason.IsTriggered(underwritingCase);
        string caseName = underwritingCase?.insuranceType != null
            ? underwritingCase.insuranceType.InsuranceTypeName
            : "No Case";

        Debug.Log(
            "Rejection reason debug: " +
            reason.DisplayName +
            " / Case: " +
            caseName +
            " / Triggered: " +
            isTriggered,
            this);

        if (!toggleCheckOnClick)
            return;

        if (itemView == null)
            itemView = GetComponentInParent<MarineInsuranceRejectionReasonItemView>();

        if (itemView != null)
            itemView.ToggleChecked();
    }
}
