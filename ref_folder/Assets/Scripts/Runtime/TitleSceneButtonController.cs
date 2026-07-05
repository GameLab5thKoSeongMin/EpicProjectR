using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneButtonController : MonoBehaviour
{
    [SerializeField] private string startSceneName;
    [SerializeField] private bool loadByBuildIndex;
    [SerializeField, Min(0)] private int startSceneBuildIndex;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    public void StartGame()
    {
        if (loadByBuildIndex)
        {
            SceneManager.LoadScene(startSceneBuildIndex, loadSceneMode);
            return;
        }

        if (string.IsNullOrWhiteSpace(startSceneName))
        {
            Debug.LogWarning("TitleSceneButtonController: Start scene name is not assigned.", this);
            return;
        }

        SceneManager.LoadScene(startSceneName, loadSceneMode);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
