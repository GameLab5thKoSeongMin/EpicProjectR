using UnityEngine;

public class AdventurerQuestionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private ContractorDialogueProfile fallbackDialogueProfile;
    [SerializeField] private bool closeAfterQuestion = true;

    private UnderwritingCase underwritingCase;
    private GameDate currentDate;

    private void Awake()
    {
        SetPanelActive(false);
    }

    public void Bind(
        UnderwritingCase value,
        GameDate date)
    {
        underwritingCase = value;
        currentDate = date;
        SetPanelActive(false);
    }

    public void Open()
    {
        if (underwritingCase == null)
            return;

        SetPanelActive(true);
    }

    public void Close()
    {
        SetPanelActive(false);
    }

    public void Toggle()
    {
        GameObject target = GetPanelObject();
        SetPanelActive(target == null || !target.activeSelf);
    }

    public void Ask(AuditDragPayload payload)
    {
        if (payload == null || underwritingCase == null)
            return;

        int adventurerIndex = GetPrimaryAdventurerIndex(underwritingCase);

        if (adventurerIndex < 0)
            return;

        ContractorDialogueProfile profile = GetDialogueProfile();

        if (profile == null || dialogueManager == null)
            return;

        DialogueClaim claim = CreateClaim(adventurerIndex, payload);
        string responseKey = GetResponseKey(payload, claim);

        DialogueSequence sequence = DialogueSequenceBuilder.BuildKeyedResponse(
            profile,
            underwritingCase,
            adventurerIndex,
            responseKey,
            claim,
            null);

        dialogueManager.StartDialogue(sequence);

        if (closeAfterQuestion)
            Close();
    }

    private DialogueClaim CreateClaim(
        int adventurerIndex,
        AuditDragPayload payload)
    {
        return null;
    }

    private string GetResponseKey(
        AuditDragPayload payload,
        DialogueClaim claim)
    {
        if (payload == null)
            return string.Empty;

        if (payload.definition is SupplyDefinition)
            return "ask.supply";

        if (payload.adventurer != null)
            return "ask.adventurer";

        return "ask.unknown";
    }

    private ContractorDialogueProfile GetDialogueProfile()
    {
        if (underwritingCase != null && underwritingCase.dialogueProfile != null)
            return underwritingCase.dialogueProfile;

        return fallbackDialogueProfile;
    }

    private int GetPrimaryAdventurerIndex(UnderwritingCase underwritingCase)
    {
        if (underwritingCase == null || underwritingCase.adventurers == null)
            return -1;

        for (int i = 0; i < underwritingCase.adventurers.Count; i++)
        {
            if (underwritingCase.adventurers[i] != null)
                return i;
        }

        return -1;
    }

    private void SetPanelActive(bool value)
    {
        GameObject target = GetPanelObject();

        if (target != null)
            target.SetActive(value);
    }

    private GameObject GetPanelObject()
    {
        return panelObject != null ? panelObject : gameObject;
    }
}
