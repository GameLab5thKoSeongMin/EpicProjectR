using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class TutorialSelectableBlocker : MonoBehaviour
{
    [SerializeField] private string actionId;
    [SerializeField] private bool restoreOriginalInteractable = true;

    private Selectable selectable;
    private bool originalInteractable;
    private TimelineSequenceController subscribedController;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        originalInteractable = selectable != null && selectable.interactable;

        if (string.IsNullOrEmpty(actionId))
        {
            TutorialActionMarker marker = GetComponent<TutorialActionMarker>();
            if (marker != null)
                actionId = marker.ActionId;
        }
    }

    private void OnEnable()
    {
        Subscribe();
        RefreshInteractable();
    }

    private void OnDisable()
    {
        Unsubscribe();

        if (restoreOriginalInteractable && selectable != null)
            selectable.interactable = originalInteractable;
    }

    private void Update()
    {
        if (subscribedController != TimelineSequenceController.Active)
        {
            Subscribe();
            RefreshInteractable();
        }
    }

    private void Subscribe()
    {
        Unsubscribe();
        subscribedController = TimelineSequenceController.Active;

        if (subscribedController != null)
            subscribedController.StepChanged += RefreshInteractable;
    }

    private void Unsubscribe()
    {
        if (subscribedController != null)
            subscribedController.StepChanged -= RefreshInteractable;

        subscribedController = null;
    }

    private void RefreshInteractable()
    {
        if (selectable == null)
            return;

        TimelineSequenceController controller = TimelineSequenceController.Active;

        if (controller == null || !controller.SequenceEnabled || !controller.IsRunning)
        {
            selectable.interactable = originalInteractable;
            return;
        }

        selectable.interactable = originalInteractable && controller.IsActionAllowed(actionId);
    }
}
