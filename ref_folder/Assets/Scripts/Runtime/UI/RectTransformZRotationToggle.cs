using UnityEngine;

public class RectTransformZRotationToggle : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private float firstZRotation = 90f;
    [SerializeField] private float secondZRotation = -90f;
    [SerializeField] private bool startWithFirstRotation = true;

    private bool useFirstRotation;

    private void Awake()
    {
        if (targetRect == null)
            targetRect = transform as RectTransform;

        useFirstRotation = startWithFirstRotation;
    }

    public void ToggleRotation()
    {
        if (targetRect == null)
            return;

        float zRotation = useFirstRotation ? firstZRotation : secondZRotation;
        Vector3 eulerAngles = targetRect.localEulerAngles;
        eulerAngles.z = zRotation;
        targetRect.localEulerAngles = eulerAngles;

        useFirstRotation = !useFirstRotation;
    }
}
