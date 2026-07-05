using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    [Serializable]
    private class PanelBinding
    {
        public Button button = null;
        public GameObject panel = null;
    }

    [SerializeField] private List<PanelBinding> panels = new List<PanelBinding>();
    [SerializeField] private bool showFirstPanelOnStart = true;
    [SerializeField] private bool hideOtherPanels = true;

    private readonly List<UnityAction> registeredActions = new List<UnityAction>();

    private void Awake()
    {
        RegisterButtonEvents();
    }

    private void Start()
    {
        if (showFirstPanelOnStart)
            ShowFirstAvailablePanel();
    }

    private void OnDestroy()
    {
        UnregisterButtonEvents();
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Count)
            return;

        GameObject targetPanel = panels[index] != null ? panels[index].panel : null;

        if (targetPanel == null)
            return;

        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] == null || panels[i].panel == null)
                continue;

            bool shouldShow = panels[i].panel == targetPanel;
            panels[i].panel.SetActive(shouldShow || !hideOtherPanels);
        }
    }

    public void HideAllPanels()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null && panels[i].panel != null)
                panels[i].panel.SetActive(false);
        }
    }

    private void ShowFirstAvailablePanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null && panels[i].panel != null)
            {
                ShowPanel(i);
                return;
            }
        }
    }

    private void RegisterButtonEvents()
    {
        registeredActions.Clear();

        for (int i = 0; i < panels.Count; i++)
        {
            int index = i;
            Button button = panels[i] != null ? panels[i].button : null;

            UnityAction action = () => ShowPanel(index);
            registeredActions.Add(action);

            if (button != null)
                button.onClick.AddListener(action);
        }
    }

    private void UnregisterButtonEvents()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            Button button = panels[i] != null ? panels[i].button : null;

            if (button != null && i < registeredActions.Count)
                button.onClick.RemoveListener(registeredActions[i]);
        }

        registeredActions.Clear();
    }
}
