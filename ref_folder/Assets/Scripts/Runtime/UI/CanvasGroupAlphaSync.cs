using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupAlphaSync : MonoBehaviour
{
    [SerializeField] private CanvasGroup source;

    private CanvasGroup target;
    private float lastSourceAlpha = -1f;

    private void Awake()
    {
        target = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        SyncAlpha();
    }

    private void LateUpdate()
    {
        if (source == null)
            return;

        if (!Mathf.Approximately(lastSourceAlpha, source.alpha))
            SyncAlpha();
    }

    public void SetSource(CanvasGroup value)
    {
        source = value;
        SyncAlpha();
    }

    private void SyncAlpha()
    {
        if (source == null)
            return;

        if (target == null)
            target = GetComponent<CanvasGroup>();

        target.alpha = source.alpha;
        lastSourceAlpha = source.alpha;
    }
}
