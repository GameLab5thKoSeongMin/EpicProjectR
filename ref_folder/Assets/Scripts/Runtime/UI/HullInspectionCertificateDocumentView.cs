using TMPro;
using UnityEngine;

public class HullInspectionCertificateDocumentView : MonoBehaviour, IUnderwritingCaseDocumentView
{
    [SerializeField] private string shipNameKey = "ship.name";
    [SerializeField] private string inspectionResultKey = "hull.inspection.result";
    [SerializeField] private string accidentHistoryKey = "hull.accident.history";
    [SerializeField] private string repairRecordKey = "hull.repair.record";

    [SerializeField] private TextMeshProUGUI shipNameText;
    [SerializeField] private TMP_Text inspectionResultText;
    [SerializeField] private TMP_Text accidentHistoryText;
    [SerializeField] private TMP_Text repairRecordText;

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        SetText(shipNameText, underwritingCase?.GetText(shipNameKey));
        SetText(inspectionResultText, underwritingCase?.GetText(inspectionResultKey));
        SetText(accidentHistoryText, underwritingCase?.GetText(accidentHistoryKey));
        SetText(repairRecordText, underwritingCase?.GetText(repairRecordKey));
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }
}
