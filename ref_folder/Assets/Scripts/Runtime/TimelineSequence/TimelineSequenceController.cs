using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-1000)]
public class TimelineSequenceController : MonoBehaviour
{
    public static TimelineSequenceController Active { get; private set; }

    [FormerlySerializedAs("tutorialEnabled")]
    [SerializeField] private bool sequenceEnabled = true;
    [SerializeField] private bool startOnAwake = true;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private PlayableDirector playableDirector;
    [FormerlySerializedAs("sequenceDefinition")]
    [SerializeField] private TimelineSequenceDefinition sequenceDefinition;
    [SerializeField] private List<TimelineSequenceStep> steps =
        new List<TimelineSequenceStep>();
    [SerializeField] private UnityEvent<string> stepStarted;
    [SerializeField] private UnityEvent<string> stepCompleted;
    [SerializeField] private UnityEvent<string> actionBlocked;
    [SerializeField] private UnityEvent sequenceCompleted;

    private int currentStepIndex = -1;
    private DialogueManager registeredDialogueManager;
    private PlayableDirector registeredPlayableDirector;
    private bool waitingForDialogue;
    private bool waitingForTimeline;
    private bool isCompletingStep;

    public event Action StepChanged;

    public bool SequenceEnabled => sequenceEnabled;
    public bool IsRunning => sequenceEnabled && currentStepIndex >= 0 && currentStepIndex < StepCount;
    public TimelineSequenceStep CurrentStep => IsRunning ? GetStep(currentStepIndex) : null;
    public string CurrentStepId => CurrentStep != null ? CurrentStep.StepId : string.Empty;

    protected virtual void Awake()
    {
        if (Active != null && Active != this)
            Debug.LogWarning("Multiple TimelineSequenceController instances found. The latest one is now active.", this);

        Active = this;
        EnsureServices();
        RegisterEvents();

        if (startOnAwake)
            StartSequence();
    }

    protected virtual void OnDestroy()
    {
        UnregisterEvents();

        if (Active == this)
            Active = null;
    }

    public void StartSequence()
    {
        if (!sequenceEnabled || StepCount == 0)
        {
            StopSequence();
            return;
        }

        EnterStep(0);
    }

    public void StopSequence()
    {
        currentStepIndex = -1;
        waitingForDialogue = false;
        waitingForTimeline = false;

        if (playableDirector != null)
            playableDirector.Stop();

        NotifyStepChanged();
    }

    public void SetSequenceEnabled(bool value)
    {
        sequenceEnabled = value;

        if (!sequenceEnabled)
        {
            StopSequence();
            return;
        }

        if (currentStepIndex < 0)
        {
            StartSequence();
            return;
        }

        NotifyStepChanged();
    }

    public bool IsActionAllowed(string actionId)
    {
        if (!sequenceEnabled || !IsRunning)
            return true;

        TimelineSequenceStep step = CurrentStep;
        return step != null && step.Allows(actionId);
    }

    public bool TryPerformAction(string actionId)
    {
        if (!IsActionAllowed(actionId))
        {
            actionBlocked?.Invoke(actionId);
            return false;
        }

        TimelineSequenceStep step = CurrentStep;

        if (step != null && step.CompleteWhenAllowedActionPerformed)
            CompleteCurrentStep();

        return true;
    }

    public void CompleteCurrentStep()
    {
        if (!IsRunning || isCompletingStep)
            return;

        isCompletingStep = true;
        TimelineSequenceStep completedStep = CurrentStep;
        completedStep?.StepCompleted?.Invoke();
        stepCompleted?.Invoke(completedStep != null ? completedStep.StepId : string.Empty);

        int nextIndex = currentStepIndex + 1;

        if (nextIndex >= StepCount)
        {
            currentStepIndex = -1;
            waitingForDialogue = false;
            waitingForTimeline = false;
            sequenceCompleted?.Invoke();
            NotifyStepChanged();
            isCompletingStep = false;
            return;
        }

        isCompletingStep = false;
        EnterStep(nextIndex);
    }

    public void GoToStep(string stepId)
    {
        if (string.IsNullOrEmpty(stepId))
            return;

        for (int i = 0; i < StepCount; i++)
        {
            TimelineSequenceStep step = GetStep(i);

            if (step != null && step.StepId == stepId)
            {
                EnterStep(i);
                return;
            }
        }
    }

    private int StepCount
    {
        get
        {
            if (sequenceDefinition != null)
                return sequenceDefinition.Count;

            return steps != null ? steps.Count : 0;
        }
    }

    private void EnterStep(int index)
    {
        currentStepIndex = Mathf.Clamp(index, 0, StepCount - 1);
        waitingForDialogue = false;
        waitingForTimeline = false;

        TimelineSequenceStep step = CurrentStep;
        step?.StepStarted?.Invoke();
        stepStarted?.Invoke(step != null ? step.StepId : string.Empty);
        NotifyStepChanged();
        ExecuteStep(step);
    }

    private TimelineSequenceStep GetStep(int index)
    {
        if (sequenceDefinition != null)
            return sequenceDefinition.GetStep(index);

        if (steps == null || index < 0 || index >= steps.Count)
            return null;

        return steps[index];
    }

    private void ExecuteStep(TimelineSequenceStep step)
    {
        if (step == null)
            return;

        bool startedTimeline = PlayStepTimeline(step);
        bool startedDialogue = PlayStepDialogue(step);

        waitingForTimeline = startedTimeline && step.WaitForTimeline;
        waitingForDialogue = startedDialogue && step.WaitForDialogue;

        if (step.StepType == TimelineSequenceStepType.WaitForAction)
            return;

        if (step.CompleteImmediately || (!waitingForTimeline && !waitingForDialogue))
            CompleteCurrentStep();
    }

    private bool PlayStepTimeline(TimelineSequenceStep step)
    {
        if (step.TimelineAsset == null)
            return false;

        EnsureServices();
        RegisterEvents();

        if (playableDirector == null)
        {
            Debug.LogWarning("TimelineSequenceController: PlayableDirector is not assigned.", this);
            return false;
        }

        playableDirector.Stop();
        playableDirector.playableAsset = step.TimelineAsset;
        playableDirector.time = 0;
        playableDirector.Play();
        return true;
    }

    private bool PlayStepDialogue(TimelineSequenceStep step)
    {
        if (step.StepType != TimelineSequenceStepType.Dialogue)
            return false;

        EnsureServices();
        RegisterEvents();

        if (dialogueManager == null)
        {
            Debug.LogWarning("TimelineSequenceController: DialogueManager is not assigned.", this);
            return false;
        }

        dialogueManager.StartDialogue(DialogueSequence.CreateRuntime(
            new[] { step.DialogueText }));
        return true;
    }

    private void TryCompleteAfterAsyncWait()
    {
        if (!IsRunning || waitingForDialogue || waitingForTimeline)
            return;

        TimelineSequenceStep step = CurrentStep;

        if (step != null && step.StepType != TimelineSequenceStepType.WaitForAction)
            CompleteCurrentStep();
    }

    private void EnsureServices()
    {
        if (dialogueManager == null)
            dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (playableDirector == null)
            playableDirector = FindFirstObjectByType<PlayableDirector>();
    }

    private void RegisterEvents()
    {
        RegisterDialogueEvents();
        RegisterTimelineEvents();
    }

    private void UnregisterEvents()
    {
        UnregisterDialogueEvents();
        UnregisterTimelineEvents();
    }

    private void RegisterDialogueEvents()
    {
        if (registeredDialogueManager == dialogueManager)
            return;

        UnregisterDialogueEvents();
        registeredDialogueManager = dialogueManager;

        if (registeredDialogueManager != null)
            registeredDialogueManager.DialogueEnded += OnDialogueEnded;
    }

    private void UnregisterDialogueEvents()
    {
        if (registeredDialogueManager == null)
            return;

        registeredDialogueManager.DialogueEnded -= OnDialogueEnded;
        registeredDialogueManager = null;
    }

    private void RegisterTimelineEvents()
    {
        if (registeredPlayableDirector == playableDirector)
            return;

        UnregisterTimelineEvents();
        registeredPlayableDirector = playableDirector;

        if (registeredPlayableDirector != null)
            registeredPlayableDirector.stopped += OnTimelineStopped;
    }

    private void UnregisterTimelineEvents()
    {
        if (registeredPlayableDirector == null)
            return;

        registeredPlayableDirector.stopped -= OnTimelineStopped;
        registeredPlayableDirector = null;
    }

    private void OnDialogueEnded()
    {
        if (!waitingForDialogue)
            return;

        waitingForDialogue = false;
        TryCompleteAfterAsyncWait();
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director != playableDirector || !waitingForTimeline)
            return;

        waitingForTimeline = false;
        TryCompleteAfterAsyncWait();
    }

    private void NotifyStepChanged()
    {
        StepChanged?.Invoke();
    }
}
