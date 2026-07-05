using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TutorialButtonGate : MonoBehaviour
{
    [SerializeField] private string actionId;
    [SerializeField] private bool completeStepOnClick = true;
    [SerializeField] private UnityEvent allowedClick;
    [SerializeField] private UnityEvent blockedClick;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (string.IsNullOrEmpty(actionId))
        {
            TutorialActionMarker marker = GetComponent<TutorialActionMarker>();
            if (marker != null)
                actionId = marker.ActionId;
        }
    }

    private void OnEnable()
    {
        EnsureButton();

        if (button != null)
            button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        TimelineSequenceController controller = TimelineSequenceController.Active;

        if (controller != null && !controller.IsActionAllowed(actionId))
        {
            blockedClick?.Invoke();
            return;
        }

        allowedClick?.Invoke();

        if (completeStepOnClick && controller != null)
            controller.TryPerformAction(actionId);
    }

    private void EnsureButton()
    {
        if (button == null)
            button = GetComponent<Button>();
    }
}
