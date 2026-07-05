using UnityEngine;

public class RectTransformWaveYMotion : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private float minY;
    [SerializeField] private float maxY = 10f;
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(10f, 20f);
    [SerializeField] private Vector2 pauseDurationRange = new Vector2(0.1f, 0.4f);
    [SerializeField] private bool startMovingUp = true;

    private const float StepY = 1f;

    private float targetY;
    private float currentMoveSpeed;
    private float moveAccumulator;
    private float pauseTimer;

    private RectTransform TargetRect
    {
        get
        {
            if (targetRect == null)
                targetRect = transform as RectTransform;

            return targetRect;
        }
    }

    private void Awake()
    {
        NormalizeRange();
        targetY = startMovingUp ? maxY : minY;
        PickMoveSpeed();
    }

    private void OnEnable()
    {
        moveAccumulator = 0f;
        pauseTimer = 0f;
        ClampCurrentY();
    }

    private void Update()
    {
        RectTransform rect = TargetRect;

        if (rect == null)
            return;

        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        moveAccumulator += currentMoveSpeed * Time.deltaTime;

        while (moveAccumulator >= StepY)
        {
            bool reachedTarget = MoveOneStepTowardTarget(rect);
            moveAccumulator -= StepY;

            if (reachedTarget)
            {
                moveAccumulator = 0f;
                break;
            }
        }
    }

    private bool MoveOneStepTowardTarget(RectTransform rect)
    {
        Vector2 position = rect.anchoredPosition;
        position.y = Mathf.MoveTowards(position.y, targetY, StepY);
        rect.anchoredPosition = position;

        if (!Mathf.Approximately(position.y, targetY))
            return false;

        pauseTimer = GetRandomPauseDuration();
        targetY = Mathf.Approximately(targetY, maxY) ? minY : maxY;
        PickMoveSpeed();
        return true;
    }

    private void ClampCurrentY()
    {
        RectTransform rect = TargetRect;

        if (rect == null)
            return;

        NormalizeRange();

        Vector2 position = rect.anchoredPosition;
        position.y = Mathf.Clamp(position.y, minY, maxY);
        rect.anchoredPosition = position;

        if (targetY < minY || targetY > maxY)
            targetY = startMovingUp ? maxY : minY;
    }

    private void OnValidate()
    {
        NormalizeRange();
        NormalizeVectorRange(ref moveSpeedRange);
        NormalizeVectorRange(ref pauseDurationRange);

        if (targetY < minY || targetY > maxY)
            targetY = startMovingUp ? maxY : minY;
    }

    private void NormalizeRange()
    {
        if (minY <= maxY)
            return;

        float previousMinY = minY;
        minY = maxY;
        maxY = previousMinY;
    }

    private void NormalizeVectorRange(ref Vector2 range)
    {
        range.x = Mathf.Max(0f, range.x);
        range.y = Mathf.Max(0f, range.y);

        if (range.x <= range.y)
            return;

        float previousMin = range.x;
        range.x = range.y;
        range.y = previousMin;
    }

    private void PickMoveSpeed()
    {
        NormalizeVectorRange(ref moveSpeedRange);
        currentMoveSpeed = Mathf.Max(0.001f, Random.Range(moveSpeedRange.x, moveSpeedRange.y));
    }

    private float GetRandomPauseDuration()
    {
        NormalizeVectorRange(ref pauseDurationRange);
        return Random.Range(pauseDurationRange.x, pauseDurationRange.y);
    }
}
