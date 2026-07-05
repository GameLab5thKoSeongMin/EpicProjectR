using System;
using UnityEngine;
using UnityEngine.Playables;

public enum DialogueTimelineCueTiming
{
    OnLineStart,
    OnLineCompleted,
    OnSequenceCompleted
}

[Serializable]
public class DialogueTimelineCue
{
    [SerializeField] private string lineKey;
    [SerializeField] private DialogueTimelineCueTiming timing;
    [SerializeField] private PlayableAsset timelineAsset;
    [SerializeField] private bool waitForTimeline;

    public string LineKey => lineKey;
    public DialogueTimelineCueTiming Timing => timing;
    public PlayableAsset TimelineAsset => timelineAsset;
    public bool WaitForTimeline => waitForTimeline;

    public bool Matches(string key, DialogueTimelineCueTiming targetTiming)
    {
        return timing == targetTiming &&
               !string.IsNullOrWhiteSpace(lineKey) &&
               string.Equals(lineKey, key, StringComparison.Ordinal);
    }
}
