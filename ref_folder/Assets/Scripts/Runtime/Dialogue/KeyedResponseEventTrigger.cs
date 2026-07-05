using UnityEngine;

public class KeyedResponseEventTrigger : MonoBehaviour
{
    [SerializeField] private string keyedResponseKey;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField] private ContractorDialogueProfile fallbackDialogueProfile;
    [SerializeField] private int adventurerIndex = -1;
    [SerializeField] private bool usePrimaryAdventurerWhenIndexUnset = true;
    [SerializeField] private bool logDebug = true;

    public string KeyedResponseKey
    {
        get => keyedResponseKey;
        set => keyedResponseKey = value;
    }

    public void Trigger()
    {
        Trigger(keyedResponseKey);
    }

    public void Trigger(string responseKey)
    {
        if (string.IsNullOrWhiteSpace(responseKey))
        {
            Log("Keyed response event ignored: response key is empty.");
            return;
        }

        DialogueManager manager = GetDialogueManager();

        if (manager == null)
        {
            Log("Keyed response event ignored: dialogue manager is missing.");
            return;
        }

        UnderwritingCase underwritingCase = GetUnderwritingCase();
        int targetAdventurerIndex = GetTargetAdventurerIndex(underwritingCase);
        ContractorDialogueProfile profile = GetDialogueProfile(underwritingCase);

        if (profile == null)
        {
            Log("Keyed response event ignored: dialogue profile is missing.");
            return;
        }

        DialogueSequence sequence = DialogueSequenceBuilder.BuildKeyedResponse(
            profile,
            underwritingCase,
            targetAdventurerIndex,
            responseKey,
            null,
            null);

        manager.StartDialogue(sequence);
        Log("Keyed response event triggered: " + responseKey);
    }

    private UnderwritingCase GetUnderwritingCase()
    {
        GameLoopController controller = GetGameLoopController();
        return controller != null ? controller.CurrentCase : null;
    }

    private ContractorDialogueProfile GetDialogueProfile(UnderwritingCase underwritingCase)
    {
        if (underwritingCase != null && underwritingCase.dialogueProfile != null)
            return underwritingCase.dialogueProfile;

        return fallbackDialogueProfile;
    }

    private int GetTargetAdventurerIndex(UnderwritingCase underwritingCase)
    {
        if (adventurerIndex >= 0 || !usePrimaryAdventurerWhenIndexUnset)
            return adventurerIndex;

        if (underwritingCase == null || underwritingCase.adventurers == null)
            return -1;

        for (int i = 0; i < underwritingCase.adventurers.Count; i++)
        {
            if (underwritingCase.adventurers[i] != null)
                return i;
        }

        return -1;
    }

    private DialogueManager GetDialogueManager()
    {
        if (dialogueManager == null)
            dialogueManager = Object.FindFirstObjectByType<DialogueManager>();

        return dialogueManager;
    }

    private GameLoopController GetGameLoopController()
    {
        if (gameLoopController == null)
            gameLoopController = Object.FindFirstObjectByType<GameLoopController>();

        return gameLoopController;
    }

    private void Log(string message)
    {
        if (logDebug)
            Debug.Log(message);
    }
}
