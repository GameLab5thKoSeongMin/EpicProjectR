using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrystalBallDialogueDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private DialogueManager dialogueManager;

    private readonly HashSet<int> initializedContactKeys = new HashSet<int>();

    public void Bind(
        UnderwritingCase value,
        GameDate date)
    {
        initializedContactKeys.Clear();
    }

    public void SetDialogueManager(DialogueManager value)
    {
        dialogueManager = value;
    }

    public void SetFallbackDialogueProfile(ContractorDialogueProfile value)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        AuditDragPayload payload = GetPayload(eventData);

        if (payload == null || !payload.HasData)
        {
            Debug.Log("Crystal ball drop ignored: payload is missing.");
            return;
        }

        AddressBookContactDefinition contact = payload.definition as AddressBookContactDefinition;

        if (contact == null)
        {
            Debug.Log("Crystal ball drop ignored: payload is not an address book contact.");
            return;
        }

        SelectContact(contact);
        eventData?.Use();
    }

    private void SelectContact(AddressBookContactDefinition contact)
    {
        if (contact == null)
        {
            Debug.Log("Crystal ball could not connect this address book contact.");
            return;
        }

        bool isFirstConnection = initializedContactKeys.Add(contact.ConversationKey);
        DialogueSequence sequence = DialogueSequence.CreateRuntime(
            isFirstConnection ? contact.FirstConnectionLines : null,
            contact.ConversationKey,
            contact.TabName,
            true);

        DialogueManager manager = GetDialogueManager();
        if (manager != null)
            manager.StartDialogue(sequence);

        Debug.Log("Crystal ball connected dialogue to address book contact: " + contact.TabName);
    }

    private AuditDragPayload GetPayload(PointerEventData eventData)
    {
        AuditDragPayload payload = AuditDragSource.CurrentPayload;

        if (payload != null)
            return payload;

        AuditDragSource source = AuditDragSource.CurrentSource;

        if (source == null)
            source = GetDragSource(eventData != null ? eventData.pointerDrag : null);

        if (source == null)
            source = GetDragSource(eventData != null ? eventData.pointerPress : null);

        if (source == null)
            source = GetDragSource(eventData != null ? eventData.rawPointerPress : null);

        if (source == null && eventData != null)
            source = GetDragSource(eventData.pointerPressRaycast.gameObject);

        return source != null ? source.CreatePayload() : null;
    }

    private AuditDragSource GetDragSource(GameObject sourceObject)
    {
        return sourceObject != null
            ? sourceObject.GetComponentInParent<AuditDragSource>()
            : null;
    }

    private DialogueManager GetDialogueManager()
    {
        if (dialogueManager == null)
            dialogueManager = Object.FindFirstObjectByType<DialogueManager>();

        return dialogueManager;
    }
}
