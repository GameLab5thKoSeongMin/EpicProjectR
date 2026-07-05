using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public enum TimelineSequenceStepType
{
    WaitForAction,
    Dialogue,
    Timeline,
    Event
}

[Serializable]
public class TimelineSequenceStep
{
    [SerializeField] private string stepId;
    [FormerlySerializedAs("stepType")]
    [SerializeField] private TimelineSequenceStepType stepType;
    [TextArea]
    [FormerlySerializedAs("instruction")]
    [SerializeField] private string dialogueText;
    [SerializeField] private PlayableAsset timelineAsset;
    [SerializeField] private bool waitForDialogue = true;
    [SerializeField] private bool waitForTimeline = true;
    [SerializeField] private List<string> allowedActionIds = new List<string>();
    [SerializeField] private bool completeWhenAllowedActionPerformed = true;
    [SerializeField] private bool completeImmediately;
    [SerializeField] private UnityEvent stepStarted;
    [SerializeField] private UnityEvent stepCompleted;

    public string StepId => stepId;
    public TimelineSequenceStepType StepType => stepType;
    public string DialogueText => dialogueText;
    public PlayableAsset TimelineAsset => timelineAsset;
    public bool WaitForDialogue => waitForDialogue;
    public bool WaitForTimeline => waitForTimeline;
    public IReadOnlyList<string> AllowedActionIds => allowedActionIds;
    public bool CompleteWhenAllowedActionPerformed => completeWhenAllowedActionPerformed;
    public bool CompleteImmediately => completeImmediately;
    public UnityEvent StepStarted => stepStarted;
    public UnityEvent StepCompleted => stepCompleted;

    public bool Allows(string actionId)
    {
        if (string.IsNullOrEmpty(actionId))
            return false;

        if (allowedActionIds == null || allowedActionIds.Count == 0)
            return false;

        for (int i = 0; i < allowedActionIds.Count; i++)
        {
            string allowedActionId = allowedActionIds[i];

            if (allowedActionId == "*" || allowedActionId == actionId)
                return true;
        }

        return false;
    }
}
