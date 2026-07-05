using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnEndingSceneLoader : MonoBehaviour
{
    [SerializeField] private GameLoopController gameLoopController;
    [SerializeField, Min(0)] private int endingAfterTurn = 3;
    [SerializeField] private string endingSceneName;
    [SerializeField] private bool loadByBuildIndex;
    [SerializeField, Min(0)] private int endingSceneBuildIndex;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    private bool hasLoaded;

    private void Awake()
    {
        if (gameLoopController == null)
            gameLoopController = FindFirstObjectByType<GameLoopController>();
    }

    private void Update()
    {
        if (hasLoaded || gameLoopController == null)
            return;

        if (gameLoopController.CurrentTurn <= endingAfterTurn)
            return;

        LoadEndingScene();
    }

    private void LoadEndingScene()
    {
        hasLoaded = true;

        if (loadByBuildIndex)
        {
            SceneManager.LoadScene(endingSceneBuildIndex, loadSceneMode);
            return;
        }

        if (string.IsNullOrWhiteSpace(endingSceneName))
        {
            Debug.LogWarning("TurnEndingSceneLoader: Ending scene name is not assigned.", this);
            hasLoaded = false;
            return;
        }

        SceneManager.LoadScene(endingSceneName, loadSceneMode);
    }
}
