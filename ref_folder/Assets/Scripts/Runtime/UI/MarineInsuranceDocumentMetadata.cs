using UnityEngine;

public class MarineInsuranceDocumentMetadata : MonoBehaviour
{
    [SerializeField] private string documentId;
    [SerializeField] private string documentDisplayName;
    [SerializeField] private string submittedKey;
    [SerializeField] private string submittedCode = "submitted";

    public string DocumentId => !string.IsNullOrWhiteSpace(documentId)
        ? documentId
        : name;

    public string DocumentDisplayName => !string.IsNullOrWhiteSpace(documentDisplayName)
        ? documentDisplayName
        : name;

    public string SubmittedKey => submittedKey;
    public string SubmittedCode => submittedCode;
    public bool HasSubmittedKey => !string.IsNullOrEmpty(submittedKey);

    public bool IsSubmitted(UnderwritingCase underwritingCase)
    {
        if (!HasSubmittedKey)
            return true;

        if (underwritingCase == null)
            return false;

        string code = underwritingCase.GetCode(submittedKey);

        if (string.IsNullOrEmpty(submittedCode))
            return !string.IsNullOrEmpty(code);

        return code == submittedCode;
    }

    public void SetSubmitted(UnderwritingCase underwritingCase, bool isSubmitted)
    {
        if (!HasSubmittedKey || underwritingCase == null)
            return;

        string value = isSubmitted ? "Submitted" : string.Empty;
        string code = isSubmitted
            ? (!string.IsNullOrEmpty(submittedCode) ? submittedCode : "submitted")
            : string.Empty;

        underwritingCase.SetValue(submittedKey, value, code);
    }
}
