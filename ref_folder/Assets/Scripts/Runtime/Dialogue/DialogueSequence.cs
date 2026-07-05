using System.Collections.Generic;
using UnityEngine;

public enum DialogueAdditionalSubmissionTiming
{
    OnStart,
    OnEnd
}

[CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    [TextArea]
    [SerializeField] private List<string> lines = new List<string>();
    [SerializeField] private List<DialogueLine> dialogueLines = new List<DialogueLine>();
    [SerializeField] private List<DialogueTimelineCue> timelineCues =
        new List<DialogueTimelineCue>();
    [SerializeField] private int adventurerIndex = -1;
    [SerializeField] private string adventurerName;
    [SerializeField] private bool hasConversationOverride;
    [SerializeField] private int conversationKey;
    [SerializeField] private string conversationTitle;
    [SerializeField] private GameObject additionalSubmissionDocumentPrefab;
    [SerializeField] private DialogueAdditionalSubmissionTiming additionalSubmissionTiming =
        DialogueAdditionalSubmissionTiming.OnStart;

    public IReadOnlyList<string> Lines => lines;
    public IReadOnlyList<DialogueLine> DialogueLines => dialogueLines;
    public IReadOnlyList<DialogueTimelineCue> TimelineCues => timelineCues;
    public int Count => dialogueLines != null && dialogueLines.Count > 0
        ? dialogueLines.Count
        : lines != null ? lines.Count : 0;
    public int AdventurerIndex => adventurerIndex;
    public string AdventurerName => adventurerName;
    public bool HasConversationOverride => hasConversationOverride;
    public int ConversationKey => conversationKey;
    public string ConversationTitle => conversationTitle;
    public GameObject AdditionalSubmissionDocumentPrefab => additionalSubmissionDocumentPrefab;
    public DialogueAdditionalSubmissionTiming AdditionalSubmissionTiming => additionalSubmissionTiming;

    public string GetLine(int index)
    {
        DialogueLine dialogueLine = GetDialogueLine(index);

        if (dialogueLine != null)
            return dialogueLine.Text;

        if (lines == null || index < 0 || index >= lines.Count)
            return string.Empty;

        return lines[index] ?? string.Empty;
    }

    public string GetLineKey(int index)
    {
        DialogueLine dialogueLine = GetDialogueLine(index);
        return dialogueLine != null ? dialogueLine.Key : string.Empty;
    }

    public DialogueLine GetDialogueLine(int index)
    {
        if (dialogueLines == null || index < 0 || index >= dialogueLines.Count)
            return null;

        return dialogueLines[index];
    }

    public static DialogueSequence CreateRuntime(IEnumerable<string> sourceLines)
    {
        return CreateRuntime(sourceLines, -1, string.Empty);
    }

    public static DialogueSequence CreateRuntime(
        IEnumerable<string> sourceLines,
        int sourceAdventurerIndex,
        string sourceAdventurerName)
    {
        DialogueSequence sequence = CreateInstance<DialogueSequence>();
        sequence.SetLines(sourceLines);
        sequence.SetAdventurer(sourceAdventurerIndex, sourceAdventurerName);
        return sequence;
    }

    public static DialogueSequence CreateRuntime(
        IEnumerable<DialogueLine> sourceLines,
        int sourceAdventurerIndex,
        string sourceAdventurerName)
    {
        DialogueSequence sequence = CreateInstance<DialogueSequence>();
        sequence.SetLines(sourceLines);
        sequence.SetAdventurer(sourceAdventurerIndex, sourceAdventurerName);
        return sequence;
    }

    public static DialogueSequence CreateRuntime(
        IEnumerable<string> sourceLines,
        int sourceConversationKey,
        string sourceConversationTitle,
        bool useConversationOverride)
    {
        DialogueSequence sequence = CreateInstance<DialogueSequence>();
        sequence.SetLines(sourceLines);
        sequence.SetConversation(
            sourceConversationKey,
            sourceConversationTitle,
            useConversationOverride);
        return sequence;
    }

    public void SetAdventurer(
        int sourceAdventurerIndex,
        string sourceAdventurerName)
    {
        adventurerIndex = sourceAdventurerIndex;
        adventurerName = sourceAdventurerName ?? string.Empty;
    }

    public void SetConversation(
        int sourceConversationKey,
        string sourceConversationTitle,
        bool useConversationOverride = true)
    {
        hasConversationOverride = useConversationOverride;
        conversationKey = sourceConversationKey;
        conversationTitle = sourceConversationTitle ?? string.Empty;
    }

    public void SetAdditionalSubmissionDocument(GameObject prefab)
    {
        SetAdditionalSubmissionDocument(prefab, DialogueAdditionalSubmissionTiming.OnStart);
    }

    public void SetAdditionalSubmissionDocument(
        GameObject prefab,
        DialogueAdditionalSubmissionTiming timing)
    {
        additionalSubmissionDocumentPrefab = prefab;
        additionalSubmissionTiming = timing;
    }

    public void SetTimelineCues(IEnumerable<DialogueTimelineCue> sourceTimelineCues)
    {
        if (timelineCues == null)
            timelineCues = new List<DialogueTimelineCue>();
        else
            timelineCues.Clear();

        if (sourceTimelineCues == null)
            return;

        foreach (DialogueTimelineCue cue in sourceTimelineCues)
        {
            if (cue != null)
                timelineCues.Add(cue);
        }
    }

    public void SetLines(IEnumerable<string> sourceLines)
    {
        if (lines == null)
            lines = new List<string>();
        else
            lines.Clear();

        if (dialogueLines == null)
            dialogueLines = new List<DialogueLine>();
        else
            dialogueLines.Clear();

        if (sourceLines == null)
            return;

        foreach (string line in sourceLines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                lines.Add(line);
                dialogueLines.Add(new DialogueLine(string.Empty, line));
            }
        }
    }

    public void SetLines(IEnumerable<DialogueLine> sourceLines)
    {
        if (lines == null)
            lines = new List<string>();
        else
            lines.Clear();

        if (dialogueLines == null)
            dialogueLines = new List<DialogueLine>();
        else
            dialogueLines.Clear();

        if (sourceLines == null)
            return;

        foreach (DialogueLine line in sourceLines)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Text))
                continue;

            dialogueLines.Add(line);
            lines.Add(line.Text);
        }
    }
}
