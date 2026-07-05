using TMPro;
using UnityEngine;

public class ShipInsuranceApplicationDocumentView : MonoBehaviour, IUnderwritingCaseDocumentView
{
    [SerializeField] private string shipNameKey = "ship.name";
    [SerializeField] private string shipOwnerNameKey = "ship.owner.name";
    [SerializeField] private string captainNameKey = "ship.captain";
    [SerializeField] private string departureDateKey = "route.departure.date";

    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private TMP_Text shipOwnerNameText;
    [SerializeField] private TMP_Text captainNameText;
    [SerializeField] private TMP_Text departureDateText;

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        SetText(shipNameText, underwritingCase?.GetText(shipNameKey));
        SetText(shipOwnerNameText, underwritingCase?.GetText(shipOwnerNameKey));
        SetText(captainNameText, underwritingCase?.GetText(captainNameKey));
        SetText(departureDateText, underwritingCase?.GetText(departureDateKey));
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }
}
