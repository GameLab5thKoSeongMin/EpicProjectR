using UnityEngine;
using UnityEngine.UI;

public class ButtonCenteredPrefabSpawner : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject prefab;
    [SerializeField] private RectTransform spawnRoot;
    [SerializeField] private bool resetScale = true;
    [SerializeField] private bool logDebug;

    private GameObject spawnedInstance;

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

    public void SpawnCenteredPrefab()
    {
        if (ShouldIgnoreClickAfterDrag())
            return;

        if (spawnedInstance != null)
        {
            Log("Centered prefab spawn ignored: instance already exists.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning("ButtonCenteredPrefabSpawner: prefab is not assigned.", this);
            return;
        }

        RectTransform root = GetSpawnRoot();

        if (root == null)
        {
            Debug.LogWarning("ButtonCenteredPrefabSpawner: spawn root or parent canvas is missing.", this);
            return;
        }

        spawnedInstance = Instantiate(prefab, root);
        spawnedInstance.transform.SetAsLastSibling();

        RectTransform spawnedRect = spawnedInstance.transform as RectTransform;

        if (spawnedRect != null)
        {
            spawnedRect.anchorMin = new Vector2(0.5f, 0.5f);
            spawnedRect.anchorMax = new Vector2(0.5f, 0.5f);
            spawnedRect.pivot = new Vector2(0.5f, 0.5f);
            spawnedRect.anchoredPosition = Vector2.zero;

            if (resetScale)
                spawnedRect.localScale = Vector3.one;
        }
        else
        {
            spawnedInstance.transform.localPosition = Vector3.zero;

            if (resetScale)
                spawnedInstance.transform.localScale = Vector3.one;
        }
    }

    private bool ShouldIgnoreClickAfterDrag()
    {
        DraggableImageObject draggable = GetComponent<DraggableImageObject>();
        return draggable != null && draggable.ConsumeRecentDrag();
    }

    public void ClearSpawnedReference()
    {
        spawnedInstance = null;
    }

    private RectTransform GetSpawnRoot()
    {
        if (spawnRoot != null)
            return spawnRoot;

        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.transform as RectTransform : null;
    }

    private void RegisterButton()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.RemoveListener(SpawnCenteredPrefab);
        button.onClick.AddListener(SpawnCenteredPrefab);
    }

    private void UnregisterButton()
    {
        if (button != null)
            button.onClick.RemoveListener(SpawnCenteredPrefab);
    }

    private void Log(string message)
    {
        if (logDebug)
            Debug.Log(message, this);
    }
}
