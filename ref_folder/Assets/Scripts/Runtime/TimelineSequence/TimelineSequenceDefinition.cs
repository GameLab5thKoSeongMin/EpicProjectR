using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Timeline Sequence/Sequence Definition")]
public class TimelineSequenceDefinition : ScriptableObject
{
    [SerializeField] private List<TimelineSequenceStep> steps =
        new List<TimelineSequenceStep>();

    public IReadOnlyList<TimelineSequenceStep> Steps => steps;
    public int Count => steps != null ? steps.Count : 0;

    public TimelineSequenceStep GetStep(int index)
    {
        if (steps == null || index < 0 || index >= steps.Count)
            return null;

        return steps[index];
    }
}
