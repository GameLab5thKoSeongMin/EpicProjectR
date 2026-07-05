using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedBookPanelNavigator : MonoBehaviour
{
    [SerializeField] private Animator pageTurnAnimator;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private List<GameObject> panels = new List<GameObject>();

    [SerializeField] private string nextTriggerName = "Next";
    [SerializeField] private string previousTriggerName = "Prev";
    [SerializeField, Min(0f)] private float pageSwapDelay = 0.25f;
    [SerializeField, Min(0f)] private float fadeDuration = 0.15f;
    [SerializeField, Min(0f)] private float inputLockDuration = 0.5f;
    [SerializeField, Min(0)] private int startPageIndex = 0;
    [SerializeField] private bool showStartPageOnStart = true;
    [SerializeField] private bool swapPageByAnimationEvent = false;

    private int currentPageIndex;
    private int pendingPageIndex = -1;
    private int pendingDirection;
    private bool isTurning;
    private bool hasPendingPageSwap;
    private Coroutine turnRoutine;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        RegisterButtonEvents();
    }

    private void Start()
    {
        currentPageIndex = FindStartPageIndex();

        if (showStartPageOnStart && IsValidPageIndex(currentPageIndex))
            ShowPage(currentPageIndex);
        else
            UpdateButtonInteractable();
    }

    private void OnDestroy()
    {
        UnregisterButtonEvents();
    }

    private void OnDisable()
    {
        if (turnRoutine != null)
            StopCoroutine(turnRoutine);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        turnRoutine = null;
        fadeRoutine = null;
        isTurning = false;
        hasPendingPageSwap = false;
        pendingPageIndex = -1;
        pendingDirection = 0;
        UpdateButtonInteractable();
    }

    public void NextPage()
    {
        TryTurnPage(1);
    }

    public void PreviousPage()
    {
        TryTurnPage(-1);
    }

    public void ShowPage(int index)
    {
        if (!IsValidPageIndex(index))
            return;

        currentPageIndex = index;
        hasPendingPageSwap = false;
        pendingPageIndex = -1;
        pendingDirection = 0;
        ApplyPageVisibility();
        UpdateButtonInteractable();
    }

    public void SetPanels(List<GameObject> newPanels, int startIndex = 0)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null)
            {
                panels[i].SetActive(false);
                SetPanelAlpha(panels[i], 0f);
            }
        }

        panels = newPanels != null
            ? new List<GameObject>(newPanels)
            : new List<GameObject>();

        currentPageIndex = Mathf.Clamp(startIndex, 0, Mathf.Max(0, panels.Count - 1));
        pendingPageIndex = -1;
        pendingDirection = 0;
        hasPendingPageSwap = false;

        if (IsValidPageIndex(currentPageIndex))
            ShowPage(currentPageIndex);
        else
            UpdateButtonInteractable();
    }

    public void SwapPendingPage()
    {
        SwapPendingPage(0);
    }

    public void SwapPendingNextPage()
    {
        SwapPendingPage(1);
    }

    public void SwapPendingPreviousPage()
    {
        SwapPendingPage(-1);
    }

    public void FadeOutCurrentPage()
    {
        if (!hasPendingPageSwap || !IsValidPageIndex(currentPageIndex))
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadePanelAlpha(currentPageIndex, 0f, false));
    }

    public void FadeInPendingPage()
    {
        if (!hasPendingPageSwap || !IsValidPageIndex(pendingPageIndex))
            return;

        int previousPageIndex = currentPageIndex;
        currentPageIndex = pendingPageIndex;
        pendingPageIndex = -1;
        pendingDirection = 0;
        hasPendingPageSwap = false;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeInPage(previousPageIndex, currentPageIndex));
    }

    private void SwapPendingPage(int expectedDirection)
    {
        if (!hasPendingPageSwap || !IsValidPageIndex(pendingPageIndex))
            return;

        if (expectedDirection != 0 && pendingDirection != expectedDirection)
            return;

        int previousPageIndex = currentPageIndex;
        currentPageIndex = pendingPageIndex;
        pendingPageIndex = -1;
        pendingDirection = 0;
        hasPendingPageSwap = false;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeToPage(previousPageIndex, currentPageIndex));
    }

    private void TryTurnPage(int direction)
    {
        if (isTurning)
            return;

        int targetPageIndex = FindPageIndex(currentPageIndex, direction);

        if (!IsValidPageIndex(targetPageIndex))
            return;

        pendingPageIndex = targetPageIndex;
        pendingDirection = direction;
        hasPendingPageSwap = true;
        turnRoutine = StartCoroutine(TurnPage(direction));
    }

    private IEnumerator TurnPage(int direction)
    {
        isTurning = true;
        SetNavigationButtonsInteractable(false);
        PlayTurnAnimation(direction);

        float elapsedBeforeLockWait = 0f;

        if (!swapPageByAnimationEvent)
        {
            if (pageSwapDelay > 0f)
            {
                yield return new WaitForSeconds(pageSwapDelay);
                elapsedBeforeLockWait = pageSwapDelay;
            }

            SwapPendingPage();
        }

        float remainingLockDuration = Mathf.Max(0f, inputLockDuration - elapsedBeforeLockWait);

        if (remainingLockDuration > 0f)
            yield return new WaitForSeconds(remainingLockDuration);

        if (swapPageByAnimationEvent)
        {
            while (hasPendingPageSwap || fadeRoutine != null)
                yield return null;
        }
        else
        {
            if (hasPendingPageSwap)
                SwapPendingPage();

            if (fadeRoutine != null)
                yield return fadeRoutine;
        }

        isTurning = false;
        turnRoutine = null;
        UpdateButtonInteractable();
    }

    private void PlayTurnAnimation(int direction)
    {
        if (pageTurnAnimator == null)
            return;

        ResetTrigger(nextTriggerName);
        ResetTrigger(previousTriggerName);

        string triggerName = direction > 0 ? nextTriggerName : previousTriggerName;

        if (!string.IsNullOrWhiteSpace(triggerName))
            pageTurnAnimator.SetTrigger(triggerName);
    }

    private void ResetTrigger(string triggerName)
    {
        if (!string.IsNullOrWhiteSpace(triggerName))
            pageTurnAnimator.ResetTrigger(triggerName);
    }

    private void ApplyPageVisibility()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null)
            {
                panels[i].SetActive(i == currentPageIndex);
                SetPanelAlpha(panels[i], i == currentPageIndex ? 1f : 0f);
            }
        }
    }

    private IEnumerator FadeToPage(int previousPageIndex, int targetPageIndex)
    {
        GameObject previousPanel = IsValidPageIndex(previousPageIndex)
            ? panels[previousPageIndex]
            : null;
        GameObject targetPanel = IsValidPageIndex(targetPageIndex)
            ? panels[targetPageIndex]
            : null;

        if (targetPanel != null)
            targetPanel.SetActive(true);

        CanvasGroup previousCanvasGroup = GetCanvasGroup(previousPanel);
        CanvasGroup targetCanvasGroup = GetCanvasGroup(targetPanel);

        SetCanvasGroupAlpha(targetCanvasGroup, 0f);

        if (fadeDuration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);

                SetCanvasGroupAlpha(previousCanvasGroup, 1f - t);
                SetCanvasGroupAlpha(targetCanvasGroup, t);

                yield return null;
            }
        }

        SetCanvasGroupAlpha(previousCanvasGroup, 0f);
        SetCanvasGroupAlpha(targetCanvasGroup, 1f);

        if (previousPanel != null && previousPanel != targetPanel)
            previousPanel.SetActive(false);

        fadeRoutine = null;
    }

    private IEnumerator FadeInPage(int previousPageIndex, int targetPageIndex)
    {
        GameObject previousPanel = IsValidPageIndex(previousPageIndex)
            ? panels[previousPageIndex]
            : null;
        GameObject targetPanel = IsValidPageIndex(targetPageIndex)
            ? panels[targetPageIndex]
            : null;

        CanvasGroup previousCanvasGroup = GetCanvasGroup(previousPanel);
        CanvasGroup targetCanvasGroup = GetCanvasGroup(targetPanel);

        SetCanvasGroupAlpha(previousCanvasGroup, 0f);

        if (targetPanel != null)
            targetPanel.SetActive(true);

        SetCanvasGroupAlpha(targetCanvasGroup, 0f);

        if (fadeDuration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);

                SetCanvasGroupAlpha(targetCanvasGroup, t);

                yield return null;
            }
        }

        SetCanvasGroupAlpha(targetCanvasGroup, 1f);

        if (previousPanel != null && previousPanel != targetPanel)
            previousPanel.SetActive(false);

        fadeRoutine = null;
    }

    private IEnumerator FadePanelAlpha(int pageIndex, float targetAlpha, bool deactivateWhenDone)
    {
        GameObject panel = IsValidPageIndex(pageIndex)
            ? panels[pageIndex]
            : null;
        CanvasGroup canvasGroup = GetCanvasGroup(panel);

        if (panel != null)
            panel.SetActive(true);

        float startAlpha = canvasGroup != null ? canvasGroup.alpha : targetAlpha;

        if (fadeDuration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);

                SetCanvasGroupAlpha(canvasGroup, Mathf.Lerp(startAlpha, targetAlpha, t));

                yield return null;
            }
        }

        SetCanvasGroupAlpha(canvasGroup, targetAlpha);

        if (deactivateWhenDone && panel != null)
            panel.SetActive(false);

        fadeRoutine = null;
    }

    private void RegisterButtonEvents()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousPage);
    }

    private void UnregisterButtonEvents()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextPage);

        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousPage);
    }

    private void SetNavigationButtonsInteractable(bool value)
    {
        if (nextButton != null)
            nextButton.interactable = value;

        if (previousButton != null)
            previousButton.interactable = value;
    }

    private void UpdateButtonInteractable()
    {
        bool canInteract = !isTurning;

        if (nextButton != null)
            nextButton.interactable = canInteract && IsValidPageIndex(FindPageIndex(currentPageIndex, 1));

        if (previousButton != null)
            previousButton.interactable = canInteract && IsValidPageIndex(FindPageIndex(currentPageIndex, -1));
    }

    private int FindStartPageIndex()
    {
        if (IsValidPageIndex(startPageIndex))
            return startPageIndex;

        return FindPageIndex(-1, 1);
    }

    private int FindPageIndex(int fromIndex, int direction)
    {
        if (direction == 0 || panels.Count == 0)
            return -1;

        int index = fromIndex + direction;

        while (index >= 0 && index < panels.Count)
        {
            if (IsValidPageIndex(index))
                return index;

            index += direction;
        }

        return -1;
    }

    private bool IsValidPageIndex(int index)
    {
        return index >= 0 && index < panels.Count && panels[index] != null;
    }

    private CanvasGroup GetCanvasGroup(GameObject panel)
    {
        if (panel == null)
            return null;

        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = panel.AddComponent<CanvasGroup>();

        return canvasGroup;
    }

    private void SetPanelAlpha(GameObject panel, float alpha)
    {
        SetCanvasGroupAlpha(GetCanvasGroup(panel), alpha);
    }

    private void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = alpha;
    }
}
