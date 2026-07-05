using UnityEngine;
using UnityEngine.Playables;

public class DialogueTimelineDirector : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private PlayableDirector playableDirector;

    private bool blockingAdvanceUntilTimelineStops;

    private void Awake()
    {
        EnsureServices();
    }

    private void OnEnable()
    {
        EnsureServices();
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
        SetAdvanceBlocked(false);
    }

    private void RegisterEvents()
    {
        if (dialogueManager != null)
        {
            dialogueManager.DialogueLineStarted += OnDialogueLineStarted;
            dialogueManager.DialogueLineCompleted += OnDialogueLineCompleted;
            dialogueManager.DialogueEnded += OnDialogueEnded;
        }

        if (playableDirector != null)
            playableDirector.stopped += OnTimelineStopped;
    }

    private void UnregisterEvents()
    {
        if (dialogueManager != null)
        {
            dialogueManager.DialogueLineStarted -= OnDialogueLineStarted;
            dialogueManager.DialogueLineCompleted -= OnDialogueLineCompleted;
            dialogueManager.DialogueEnded -= OnDialogueEnded;
        }

        if (playableDirector != null)
            playableDirector.stopped -= OnTimelineStopped;
    }

    private void OnDialogueLineStarted(int lineIndex, string line)
    {
        PlayMatchingCues(lineIndex, DialogueTimelineCueTiming.OnLineStart);
    }

    private void OnDialogueLineCompleted(int lineIndex)
    {
        PlayMatchingCues(lineIndex, DialogueTimelineCueTiming.OnLineCompleted);
    }

    private void OnDialogueEnded()
    {
        PlaySequenceCompletedCues();
    }

    private void PlayMatchingCues(int lineIndex, DialogueTimelineCueTiming timing)
    {
        if (dialogueManager == null || dialogueManager.CurrentSequence == null)
            return;

        string lineKey = dialogueManager.CurrentSequence.GetLineKey(lineIndex);

        if (string.IsNullOrEmpty(lineKey))
            return;

        PlayMatchingCues(dialogueManager.CurrentSequence, lineKey, timing);
    }

    private void PlaySequenceCompletedCues()
    {
        if (dialogueManager == null || dialogueManager.CurrentSequence == null)
            return;

        PlayMatchingCues(
            dialogueManager.CurrentSequence,
            string.Empty,
            DialogueTimelineCueTiming.OnSequenceCompleted);
    }

    private void PlayMatchingCues(
        DialogueSequence sequence,
        string lineKey,
        DialogueTimelineCueTiming timing)
    {
        if (sequence.TimelineCues == null)
            return;

        for (int i = 0; i < sequence.TimelineCues.Count; i++)
        {
            DialogueTimelineCue cue = sequence.TimelineCues[i];

            if (cue == null)
                continue;

            bool matches = timing == DialogueTimelineCueTiming.OnSequenceCompleted
                ? cue.Timing == timing
                : cue.Matches(lineKey, timing);

            if (matches)
                PlayCue(cue);
        }
    }

    private void PlayCue(DialogueTimelineCue cue)
    {
        if (cue.TimelineAsset == null)
            return;

        EnsureServices();

        if (playableDirector == null)
        {
            Debug.LogWarning("DialogueTimelineDirector: PlayableDirector is not assigned.", this);
            return;
        }

        playableDirector.Stop();
        playableDirector.playableAsset = cue.TimelineAsset;
        playableDirector.time = 0;
        playableDirector.Play();

        if (cue.WaitForTimeline)
        {
            blockingAdvanceUntilTimelineStops = true;
            SetAdvanceBlocked(true);
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director != playableDirector || !blockingAdvanceUntilTimelineStops)
            return;

        blockingAdvanceUntilTimelineStops = false;
        SetAdvanceBlocked(false);
    }

    private void SetAdvanceBlocked(bool value)
    {
        if (dialogueManager != null)
            dialogueManager.SetAdvanceBlocked(value);
    }

    private void EnsureServices()
    {
        if (dialogueManager == null)
            dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (playableDirector == null)
            playableDirector = FindFirstObjectByType<PlayableDirector>();
    }
}
