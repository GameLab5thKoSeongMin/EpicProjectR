using UnityEngine;
using UnityEngine.EventSystems;

public class AdventurerQuestionDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private AdventurerQuestionPanel questionPanel;

    public void OnDrop(PointerEventData eventData)
    {
        AdventurerQuestionPanel panel = GetQuestionPanel();

        if (panel == null)
            return;

        AuditDragPayload payload = AuditDragSource.CurrentPayload;

        if (payload == null && eventData != null && eventData.pointerDrag != null)
        {
            AuditDragSource source = eventData.pointerDrag.GetComponentInParent<AuditDragSource>();
            payload = source != null ? source.CreatePayload() : null;
        }

        if (payload == null || !payload.HasData)
            return;

        panel.Ask(payload);
        eventData?.Use();
    }

    private AdventurerQuestionPanel GetQuestionPanel()
    {
        if (questionPanel == null)
            questionPanel = GetComponentInParent<AdventurerQuestionPanel>();

        return questionPanel;
    }
}
