using System.Collections.Generic;
using UnityEngine;

public class GameObjectToggle : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private bool startActive;
    [SerializeField] private bool applyStartStateOnStart = false;

    private bool isActive;

    private void Start()
    {
        isActive = TryGetFirstTargetActiveSelf(out bool targetActive)
            ? targetActive
            : startActive;

        if (applyStartStateOnStart)
            ApplyState();
    }

    public void Toggle()
    {
        isActive = !isActive;
        ApplyState();
    }

    public void SetActive(bool value)
    {
        isActive = value;
        ApplyState();
    }

    public void Show()
    {
        SetActive(true);
    }

    public void Hide()
    {
        SetActive(false);
    }

    private void ApplyState()
    {
        if (targets == null)
            return;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
                targets[i].SetActive(isActive);
        }
    }

    private bool TryGetFirstTargetActiveSelf(out bool value)
    {
        value = false;

        if (targets == null)
            return false;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
                continue;

            value = targets[i].activeSelf;
            return true;
        }

        return false;
    }
}
