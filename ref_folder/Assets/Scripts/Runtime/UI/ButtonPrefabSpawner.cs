using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabSpawner : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform root;

    private void Awake()
    {
        RegisterButton();
    }

    private void OnEnable()
    {
        RegisterButton();
    }

    private void OnDestroy()
    {
        UnregisterButton();
    }

    public void SpawnPrefab()
    {
        if (ShouldIgnoreClickAfterDrag())
            return;

        if (prefab == null)
        {
            Debug.LogWarning("ButtonPrefabSpawner: prefab is not assigned.", this);
            return;
        }

        Transform spawnRoot = GetSpawnRoot();

        if (spawnRoot == null)
        {
            Debug.LogWarning("ButtonPrefabSpawner: root or parent canvas is missing.", this);
            return;
        }

        GameObject instance = Instantiate(prefab, spawnRoot);
        instance.transform.SetAsLastSibling();
    }

    private bool ShouldIgnoreClickAfterDrag()
    {
        DraggableImageObject draggable = GetComponent<DraggableImageObject>();
        return draggable != null && draggable.ConsumeRecentDrag();
    }

    private Transform GetSpawnRoot()
    {
        if (root != null)
            return root;

        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.transform : null;
    }

    private void RegisterButton()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.RemoveListener(SpawnPrefab);
        button.onClick.AddListener(SpawnPrefab);
    }

    private void UnregisterButton()
    {
        if (button != null)
            button.onClick.RemoveListener(SpawnPrefab);
    }
}
