using TMPro;
using UnityEngine;

public class ApprovedContractShipMarkerView : MonoBehaviour
{
    [SerializeField] private RectTransform markerRect;
    [SerializeField] private TMP_Text labelText;

    private void Awake()
    {
        if (markerRect == null)
            markerRect = transform as RectTransform;
    }

    public void Bind(ApprovedContractRecord contract, Vector3 worldPosition)
    {
        if (markerRect == null)
            markerRect = transform as RectTransform;

        if (markerRect != null)
            markerRect.position = worldPosition;
        else
            transform.position = worldPosition;

        if (labelText != null)
            labelText.text = contract != null ? contract.ContractorName : string.Empty;
    }
}
