using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class DialogueBubbleClickArea : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DialogueManager dialogueManager;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = GetComponentInParent<DialogueManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
            return;

        if (dialogueManager == null)
            return;

        dialogueManager.Advance();
        eventData.Use();
    }
}
