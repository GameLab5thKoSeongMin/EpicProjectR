using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarineInsuranceRejectionReasonItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text decisionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image checkedStateImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite checkedSprite;

    private MarineInsuranceRejectionReasonDefinition reason;
    private MarineInsuranceRejectionReasonListView ownerListView;
    private string decisionLabel;
    private bool isChecked;

    private void Awake()
    {
        EnsureCheckedStateImage();
        CaptureDefaultSprite();
    }

    public void Bind(MarineInsuranceRejectionReasonDefinition reason)
    {
        Bind(null, reason);
    }

    public void Bind(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionReasonDefinition reason)
    {
        Bind(underwritingCase, reason, null);
    }

    public void Bind(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionReasonDefinition reason,
        MarineInsuranceRejectionReasonListView ownerListView)
    {
        this.reason = reason;
        this.ownerListView = ownerListView;
        decisionLabel = reason != null ? reason.Decision.ToString() : string.Empty;

        SetText(titleText, reason != null ? reason.DisplayName : string.Empty);
        SetText(descriptionText, reason != null ? reason.Description : string.Empty);
        SetIcon(reason != null ? reason.IconSprite : null);
        SetChecked(ownerListView != null && ownerListView.IsReasonChecked(reason));

        MarineInsuranceRejectionReasonDebugButton debugButton =
            GetComponentInChildren<MarineInsuranceRejectionReasonDebugButton>(true);

        if (debugButton != null)
            debugButton.Bind(underwritingCase, reason);

        MarineInsuranceRejectionReasonDialogueButton dialogueButton =
            GetComponentInChildren<MarineInsuranceRejectionReasonDialogueButton>(true);

        if (dialogueButton != null)
            dialogueButton.Bind(underwritingCase, reason);
    }

    public void ToggleChecked()
    {
        if (ownerListView != null)
            ownerListView.SetReasonChecked(reason, !isChecked);
        else
            SetChecked(!isChecked);
    }

    public void SetChecked(bool value)
    {
        isChecked = value;

        UpdateCheckedStateSprite();

        string label = decisionLabel;

        if (isChecked)
            label = string.IsNullOrEmpty(label) ? "Checked" : label + " / Checked";

        SetText(decisionText, label);
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }

    private void SetIcon(Sprite sprite)
    {
        if (iconImage == null)
            return;

        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
    }

    private void EnsureCheckedStateImage()
    {
        if (checkedStateImage == null)
            checkedStateImage = GetComponent<Image>();
    }

    private void CaptureDefaultSprite()
    {
        if (defaultSprite == null && checkedStateImage != null)
            defaultSprite = checkedStateImage.sprite;
    }

    private void UpdateCheckedStateSprite()
    {
        EnsureCheckedStateImage();
        CaptureDefaultSprite();

        if (checkedStateImage == null)
            return;

        Sprite targetSprite = isChecked && checkedSprite != null
            ? checkedSprite
            : defaultSprite;

        if (targetSprite != null)
            checkedStateImage.sprite = targetSprite;
    }
}
