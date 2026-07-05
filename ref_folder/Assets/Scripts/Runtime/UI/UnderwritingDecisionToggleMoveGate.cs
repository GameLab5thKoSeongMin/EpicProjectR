using UnityEngine;

public class UnderwritingDecisionToggleMoveGate : MonoBehaviour
{
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField] private RectTransformToggleMover[] toggleMovers;
    [SerializeField] private bool playToggleMove = true;
    [SerializeField] private bool logDebug;

    private GameLoopController pendingController;
    private bool waitingForResolution;

    public void ApproveAndToggleIfResolved()
    {
        ExecuteAndToggleIfResolved(UnderwritingDecision.Approved);
    }

    public void RejectAndToggleIfResolved()
    {
        ExecuteAndToggleIfResolved(UnderwritingDecision.Rejected);
    }

    public void ToggleIfCurrentVisitorResolved()
    {
        GameLoopController controller = GetGameLoopController();

        if (controller == null)
        {
            Log("Decision toggle ignored: GameLoopController is missing.");
            return;
        }

        if (controller.State == GameLoopState.VisitorPresent &&
            controller.CurrentCase != null)
        {
            Log("Decision toggle ignored: visitor is still present.");
            return;
        }

        ToggleMovers();
    }

    private void ExecuteAndToggleIfResolved(UnderwritingDecision decision)
    {
        GameLoopController controller = GetGameLoopController();

        if (controller == null)
        {
            Log("Decision toggle ignored: GameLoopController is missing.");
            return;
        }

        if (controller.State != GameLoopState.VisitorPresent ||
            controller.CurrentCase == null)
        {
            Log("Decision toggle ignored: no active visitor.");
            return;
        }

        UnderwritingCase previousCase = controller.CurrentCase;

        if (decision == UnderwritingDecision.Rejected)
            controller.RejectCurrentVisitor();
        else
            controller.ApproveCurrentVisitor();

        bool wasResolved = controller.CurrentCase != previousCase ||
                           controller.State != GameLoopState.VisitorPresent;

        if (!wasResolved)
        {
            WaitForVisitorResolved(controller);
            return;
        }

        ToggleMovers();
    }

    private void WaitForVisitorResolved(GameLoopController controller)
    {
        if (controller == null || waitingForResolution)
            return;

        waitingForResolution = true;
        pendingController = controller;
        pendingController.VisitorResolvedEvent += OnVisitorResolved;
        Log("Decision toggle delayed until visitor is resolved.");
    }

    private void OnVisitorResolved()
    {
        if (pendingController != null)
            pendingController.VisitorResolvedEvent -= OnVisitorResolved;

        pendingController = null;
        waitingForResolution = false;
        ToggleMovers();
    }

    private void OnDestroy()
    {
        if (pendingController != null)
            pendingController.VisitorResolvedEvent -= OnVisitorResolved;
    }

    private void ToggleMovers()
    {
        if (!playToggleMove)
        {
            Log("Decision toggle skipped: playToggleMove is disabled.");
            return;
        }

        if (toggleMovers == null || toggleMovers.Length == 0)
        {
            Log("Decision toggle ignored: no RectTransformToggleMover assigned.");
            return;
        }

        for (int i = 0; i < toggleMovers.Length; i++)
        {
            if (toggleMovers[i] != null)
                toggleMovers[i].ToggleMove();
        }
    }

    private GameLoopController GetGameLoopController()
    {
        if (gameLoopController == null)
            gameLoopController = Object.FindFirstObjectByType<GameLoopController>();

        return gameLoopController;
    }

    private void Log(string message)
    {
        if (logDebug)
            Debug.Log(message, this);
    }
}
