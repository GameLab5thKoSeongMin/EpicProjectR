using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteDeclarationDocumentView : MonoBehaviour, IUnderwritingCaseDocumentView
{
    [SerializeField] private string departureDateKey = "route.departure.date";
    [SerializeField] private string departurePortKey = "route.departure.port";
    [SerializeField] private string destinationPortKey = "route.destination.port";
    [SerializeField] private string expectedReturnDateKey = "route.expected.return.date";
    [SerializeField] private string weatherForecastKey = "route.weather.forecast";
    [SerializeField] private string routeImageKey = "route.image";

    [SerializeField] private TMP_Text departureDateText;
    [SerializeField] private TMP_Text departurePortText;
    [SerializeField] private TMP_Text destinationPortText;
    [SerializeField] private TMP_Text expectedReturnDateText;
    [SerializeField] private TMP_Text weatherForecastText;
    [SerializeField] private Image routeImage;

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        SetText(departureDateText, underwritingCase?.GetText(departureDateKey));
        SetText(departurePortText, underwritingCase?.GetText(departurePortKey));
        SetText(destinationPortText, underwritingCase?.GetText(destinationPortKey));
        SetText(expectedReturnDateText, underwritingCase?.GetText(expectedReturnDateKey));
        SetText(weatherForecastText, underwritingCase?.GetText(weatherForecastKey));
        SetImage(routeImage, underwritingCase?.GetSprite(routeImageKey));
    }

    private void SetText(TMP_Text target, string value)
    {
        if (target != null)
            target.text = value ?? string.Empty;
    }

    private void SetImage(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        target.sprite = sprite;
        target.enabled = sprite != null;
    }
}
