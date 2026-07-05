using UnityEngine;
using UnityEngine.Events;

public class TutorialActionReporter : MonoBehaviour
{
    [SerializeField] private string actionId;
    [SerializeField] private UnityEvent actionAllowed;
    [SerializeField] private UnityEvent actionBlocked;

    public bool Report()
    {
        TimelineSequenceController controller = TimelineSequenceController.Active;

        if (controller == null)
        {
            actionAllowed?.Invoke();
            return true;
        }

        bool allowed = controller.TryPerformAction(actionId);

        if (allowed)
            actionAllowed?.Invoke();
        else
            actionBlocked?.Invoke();

        return allowed;
    }
}
