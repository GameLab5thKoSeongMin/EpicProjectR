using UnityEngine;
using UnityEngine.UI;

public class ButtonDestroySelf : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject target;

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

    public void DestroySelf()
    {
        Destroy(target != null ? target : gameObject);
    }

    private void RegisterButton()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.RemoveListener(DestroySelf);
        button.onClick.AddListener(DestroySelf);
    }

    private void UnregisterButton()
    {
        if (button != null)
            button.onClick.RemoveListener(DestroySelf);
    }
}
