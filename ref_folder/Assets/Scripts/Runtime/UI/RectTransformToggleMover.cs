using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RectTransformToggleMover : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private Vector2 openedPosition;
    [SerializeField, Min(0f)] private float moveDuration = 0.25f;
    [SerializeField, FormerlySerializedAs("moveCurve")] private AnimationCurve speedCurve = DefaultSpeedCurve;

    private Vector2 closedPosition;
    private bool isOpen;
    private Coroutine moveRoutine;

    private const int SpeedCurveSampleCount = 32;

    private static AnimationCurve DefaultSpeedCurve => new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f));

    private void Awake()
    {
        EnsureSpeedCurve();

        if (targetRect == null)
            targetRect = transform as RectTransform;

        if (targetRect != null)
            closedPosition = targetRect.anchoredPosition;
    }

    public void ToggleMove()
    {
        SetOpen(!isOpen);
    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    public void SetOpen(bool value)
    {
        if (targetRect == null)
            return;

        isOpen = value;
        Vector2 targetPosition = isOpen ? openedPosition : closedPosition;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(targetPosition));
    }

    [ContextMenu("Capture Current As Opened Position")]
    public void CaptureCurrentAsOpenedPosition()
    {
        RectTransform rect = GetTargetRect();

        if (rect != null)
            openedPosition = rect.anchoredPosition;
    }

    private IEnumerator MoveTo(Vector2 targetPosition)
    {
        Vector2 startPosition = targetRect.anchoredPosition;

        if (moveDuration <= 0f)
        {
            targetRect.anchoredPosition = targetPosition;
            moveRoutine = null;
            yield break;
        }

        float elapsed = 0f;
        float positionProgress = 0f;
        float speedCurveArea = GetSpeedCurveArea();
        bool useLinearSpeed = speedCurveArea <= Mathf.Epsilon;

        while (elapsed < moveDuration)
        {
            float nextElapsed = Mathf.Min(elapsed + Time.deltaTime, moveDuration);
            float normalizedDelta = (nextElapsed - elapsed) / moveDuration;

            if (useLinearSpeed)
            {
                positionProgress = Mathf.Clamp01(nextElapsed / moveDuration);
            }
            else
            {
                float t = Mathf.Clamp01((elapsed + nextElapsed) * 0.5f / moveDuration);
                float speed = Mathf.Max(0f, speedCurve.Evaluate(t));
                positionProgress = Mathf.Clamp01(positionProgress + speed * normalizedDelta / speedCurveArea);
            }

            targetRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, positionProgress);
            elapsed = nextElapsed;

            yield return null;
        }

        targetRect.anchoredPosition = targetPosition;
        moveRoutine = null;
    }

    private RectTransform GetTargetRect()
    {
        return targetRect != null ? targetRect : transform as RectTransform;
    }

    private void EnsureSpeedCurve()
    {
        if (speedCurve == null || speedCurve.length == 0)
            speedCurve = DefaultSpeedCurve;
    }

    private float GetSpeedCurveArea()
    {
        EnsureSpeedCurve();

        float area = 0f;
        float previousSpeed = Mathf.Max(0f, speedCurve.Evaluate(0f));

        for (int i = 1; i <= SpeedCurveSampleCount; i++)
        {
            float t = i / (float)SpeedCurveSampleCount;
            float currentSpeed = Mathf.Max(0f, speedCurve.Evaluate(t));
            area += (previousSpeed + currentSpeed) * 0.5f / SpeedCurveSampleCount;
            previousSpeed = currentSpeed;
        }

        return area;
    }
}
