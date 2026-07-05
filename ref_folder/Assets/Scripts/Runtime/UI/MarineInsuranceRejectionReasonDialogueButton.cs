using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class MarineInsuranceRejectionReasonDialogueButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField] private ContractorDialogueProfile fallbackDialogueProfile;
    [SerializeField, FormerlySerializedAs("suppressRepeatedResponseKey")]
    private bool suppressRepeatedResponseEntry = true;
    [SerializeField] private bool logDebug = true;

    private static readonly Dictionary<UnderwritingCase, HashSet<string>> PlayedResponseEntryKeysByCase =
        new Dictionary<UnderwritingCase, HashSet<string>>();

    private UnderwritingCase underwritingCase;
    private MarineInsuranceRejectionReasonDefinition reason;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        ResolveSceneReferences();

        if (button != null)
            button.onClick.AddListener(TriggerDialogue);
    }

    private void OnEnable()
    {
        ResolveSceneReferences();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(TriggerDialogue);
    }

    public void Bind(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionReasonDefinition reason)
    {
        this.underwritingCase = underwritingCase;
        this.reason = reason;
        ResolveSceneReferences();
    }

    public void TriggerDialogue()
    {
        string responseKey = reason != null ? reason.DialogueResponseKey : string.Empty;

        if (string.IsNullOrWhiteSpace(responseKey))
        {
            Log("Rejection reason dialogue ignored: response key is empty.");
            return;
        }

        DialogueManager manager = GetDialogueManager();

        if (manager == null)
        {
            Log("Rejection reason dialogue ignored: dialogue manager is missing.");
            return;
        }

        UnderwritingCase targetCase = GetUnderwritingCase();
        ContractorDialogueProfile profile = GetDialogueProfile(targetCase);

        if (profile == null)
        {
            Log("Rejection reason dialogue ignored: dialogue profile is missing.");
            return;
        }

        string responseEntryKey = profile.GetKeyedResponseEntryKey(responseKey);

        if (ShouldSuppressRepeatedResponseEntry(targetCase, responseEntryKey))
        {
            Log("Rejection reason dialogue ignored: response entry already played once: " + responseEntryKey);
            return;
        }

        DialogueSequence sequence = DialogueSequenceBuilder.BuildKeyedResponse(
            profile,
            targetCase,
            -1,
            responseKey,
            null,
            null);

        MarkResponseEntryPlayed(targetCase, responseEntryKey);
        manager.StartDialogue(sequence);
        Log("Rejection reason dialogue triggered: " + responseKey);
    }

    private bool ShouldSuppressRepeatedResponseEntry(
        UnderwritingCase targetCase,
        string responseEntryKey)
    {
        if (!ShouldUseRepeatedResponseEntryGuard(targetCase, responseEntryKey))
            return false;

        return PlayedResponseEntryKeysByCase.TryGetValue(targetCase, out HashSet<string> playedResponseEntryKeys) &&
               playedResponseEntryKeys.Contains(responseEntryKey);
    }

    private void MarkResponseEntryPlayed(
        UnderwritingCase targetCase,
        string responseEntryKey)
    {
        if (!ShouldUseRepeatedResponseEntryGuard(targetCase, responseEntryKey))
            return;

        if (!PlayedResponseEntryKeysByCase.TryGetValue(targetCase, out HashSet<string> playedResponseEntryKeys))
        {
            playedResponseEntryKeys = new HashSet<string>();
            PlayedResponseEntryKeysByCase[targetCase] = playedResponseEntryKeys;
        }

        playedResponseEntryKeys.Add(responseEntryKey);
    }

    private bool ShouldUseRepeatedResponseEntryGuard(
        UnderwritingCase targetCase,
        string responseEntryKey)
    {
        return suppressRepeatedResponseEntry &&
               targetCase != null &&
               !string.IsNullOrWhiteSpace(responseEntryKey);
    }

    private UnderwritingCase GetUnderwritingCase()
    {
        if (underwritingCase != null)
            return underwritingCase;

        GameLoopController controller = GetGameLoopController();
        return controller != null ? controller.CurrentCase : null;
    }

    private ContractorDialogueProfile GetDialogueProfile(UnderwritingCase targetCase)
    {
        if (targetCase != null && targetCase.dialogueProfile != null)
            return targetCase.dialogueProfile;

        return fallbackDialogueProfile;
    }

    private DialogueManager GetDialogueManager()
    {
        ResolveSceneReferences();
        return dialogueManager;
    }

    private GameLoopController GetGameLoopController()
    {
        ResolveSceneReferences();
        return gameLoopController;
    }

    private void ResolveSceneReferences()
    {
        if (dialogueManager == null)
            dialogueManager = Object.FindFirstObjectByType<DialogueManager>();

        if (gameLoopController == null)
            gameLoopController = Object.FindFirstObjectByType<GameLoopController>();
    }

    private void Log(string message)
    {
        if (logDebug)
            Debug.Log(message, this);
    }
}
