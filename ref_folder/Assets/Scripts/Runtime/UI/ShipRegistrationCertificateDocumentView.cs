using TMPro;
using UnityEngine;

public class ShipRegistrationCertificateDocumentView : MonoBehaviour, IUnderwritingCaseDocumentView
{
    [SerializeField] private string shipNameKey = "ship.name";
    [SerializeField] private string shipOwnerNameKey = "ship.owner.name";
    [SerializeField] private string buildYearKey = "ship.build.year";
    [SerializeField] private string expirationDateKey = "registration.expiration.date";

    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private TMP_Text shipOwnerNameText;
    [SerializeField] private TMP_Text buildYearText;
    [SerializeField] private TMP_Text expirationDateText;

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        SetText(shipNameText, underwritingCase?.GetText(shipNameKey));
        SetText(shipOwnerNameText, underwritingCase?.GetText(shipOwnerNameKey));
        SetText(buildYearText, underwritingCase?.GetText(buildYearKey));
        SetText(expirationDateText, underwritingCase?.GetText(expirationDateKey));
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }
}
