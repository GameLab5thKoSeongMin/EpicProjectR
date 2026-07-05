using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetRegistry : MonoBehaviour
{
    [SerializeField] private List<TutorialTargetEntry> targets =
        new List<TutorialTargetEntry>();

    public RectTransform FindRectTransform(string targetId)
    {
        if (string.IsNullOrEmpty(targetId) || targets == null)
            return null;

        for (int i = 0; i < targets.Count; i++)
        {
            TutorialTargetEntry entry = targets[i];

            if (entry != null && entry.TargetId == targetId)
                return entry.RectTransform;
        }

        return null;
    }

    public GameObject FindGameObject(string targetId)
    {
        RectTransform rectTransform = FindRectTransform(targetId);
        return rectTransform != null ? rectTransform.gameObject : null;
    }
}

[Serializable]
public class TutorialTargetEntry
{
    [SerializeField] private string targetId;
    [SerializeField] private RectTransform rectTransform;

    public string TargetId => targetId;
    public RectTransform RectTransform => rectTransform;
}
