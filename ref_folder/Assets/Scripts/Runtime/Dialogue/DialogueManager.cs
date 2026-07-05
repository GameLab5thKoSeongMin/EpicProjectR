using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogueSequence;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float charactersPerSecond = 30f;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private UnityEvent dialogueCompleted;

    private Coroutine typingCoroutine;
    private int currentLineIndex = -1;
    private int currentLineCharacterCount;
    private bool isTyping;
    private bool advanceBlocked;
    private bool dialogueObjectSuppressed;

    public bool IsTyping => isTyping;
    public bool HasNextLine => dialogueSequence != null && currentLineIndex + 1 < dialogueSequence.Count;
    public DialogueSequence CurrentSequence => dialogueSequence;
    public int CurrentLineIndex => currentLineIndex;
    public string CurrentLineKey => dialogueSequence != null
        ? dialogueSequence.GetLineKey(currentLineIndex)
        : string.Empty;
    public UnityEvent DialogueCompleted => dialogueCompleted;
    public event Action<DialogueSequence> DialogueStarted;
    public event Action<int, string> DialogueLineStarted;
    public event Action<int, int> DialogueLineVisibleCharactersChanged;
    public event Action<int> DialogueLineCompleted;
    public event Action DialogueEnded;

    private void Start()
    {
        if (playOnStart)
            StartDialogue(dialogueSequence);
        else
            SetDialogueObjectActive(false);
    }

    private void OnDisable()
    {
        StopTyping();
    }

    public void StartDialogue(DialogueSequence sequence)
    {
        StopTyping();

        dialogueSequence = sequence;
        currentLineIndex = -1;
        currentLineCharacterCount = 0;
        DialogueStarted?.Invoke(dialogueSequence);

        if (dialogueSequence == null || dialogueSequence.Count <= 0)
        {
            SetDialogueObjectActive(false);
            DialogueEnded?.Invoke();
            return;
        }

        SetDialogueObjectActive(true);

        if (dialogueText != null)
        {
            dialogueText.text = string.Empty;
            dialogueText.maxVisibleCharacters = int.MaxValue;
        }

        if (!ShowNextLine())
            CompleteDialogue();
    }

    public void Advance()
    {
        if (advanceBlocked)
            return;

        if (isTyping)
        {
            CompleteCurrentLine();
            return;
        }

        if (!ShowNextLine())
            CompleteDialogue();
    }

    public void SetAdvanceBlocked(bool value)
    {
        advanceBlocked = value;
    }

    public void SetDialogueObjectSuppressed(bool value)
    {
        if (dialogueObjectSuppressed == value)
            return;

        dialogueObjectSuppressed = value;

        if (dialogueObjectSuppressed)
            SetDialogueObjectActive(false);
        else if (dialogueSequence != null && dialogueSequence.Count > 0)
            SetDialogueObjectActive(true);
    }

    private bool ShowNextLine()
    {
        if (dialogueSequence == null)
            return false;

        int nextLineIndex = currentLineIndex + 1;

        if (nextLineIndex >= dialogueSequence.Count)
            return false;

        currentLineIndex = nextLineIndex;
        string line = dialogueSequence.GetLine(currentLineIndex);

        StopTyping();
        typingCoroutine = StartCoroutine(TypeLine(line));
        return true;
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        int characterCount = !string.IsNullOrEmpty(line) ? line.Length : 0;

        if (dialogueText != null)
        {
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();
            characterCount = dialogueText.textInfo.characterCount;
        }

        currentLineCharacterCount = characterCount;
        DialogueLineStarted?.Invoke(currentLineIndex, line);
        DialogueLineVisibleCharactersChanged?.Invoke(currentLineIndex, 0);

        if (characterCount <= 0)
        {
            if (dialogueText != null)
                dialogueText.maxVisibleCharacters = int.MaxValue;

            isTyping = false;
            typingCoroutine = null;
            DialogueLineCompleted?.Invoke(currentLineIndex);
            yield break;
        }

        float delay = charactersPerSecond > 0f ? 1f / charactersPerSecond : 0f;

        for (int i = 1; i <= characterCount; i++)
        {
            if (dialogueText != null)
                dialogueText.maxVisibleCharacters = i;

            DialogueLineVisibleCharactersChanged?.Invoke(currentLineIndex, i);

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            else
                yield return null;
        }

        isTyping = false;
        typingCoroutine = null;
        DialogueLineCompleted?.Invoke(currentLineIndex);
    }

    private void CompleteCurrentLine()
    {
        StopTyping();

        if (dialogueText != null)
            dialogueText.maxVisibleCharacters = int.MaxValue;

        DialogueLineVisibleCharactersChanged?.Invoke(
            currentLineIndex,
            currentLineCharacterCount);
        DialogueLineCompleted?.Invoke(currentLineIndex);
    }

    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
    }

    private void SetDialogueObjectActive(bool value)
    {
        if (dialogueObject != null)
            dialogueObject.SetActive(value && !dialogueObjectSuppressed);
    }

    private void CompleteDialogue()
    {
        SetDialogueObjectActive(false);
        DialogueEnded?.Invoke();
        dialogueCompleted?.Invoke();
    }
}
