// Responsibility: Runs simple runtime UI tweens for the first playable passive view.
using System.Collections;
using UnityEngine;

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayableTweenRunner : MonoBehaviour
    {
        public void MoveTo(RectTransform target, Vector2 from, Vector2 to, float duration)
        {
            if (target == null)
            {
                return;
            }

            target.anchoredPosition = from;
            StartCoroutine(MoveRoutine(target, to, Mathf.Max(0f, duration)));
        }

        private static IEnumerator MoveRoutine(RectTransform target, Vector2 to, float duration)
        {
            if (duration <= 0f)
            {
                target.anchoredPosition = to;
                yield break;
            }

            var from = target.anchoredPosition;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed = Mathf.Min(elapsed + Time.deltaTime, duration);
                var t = elapsed / duration;
                t = t * t * (3f - 2f * t);
                target.anchoredPosition = Vector2.Lerp(from, to, t);
                yield return null;
            }

            target.anchoredPosition = to;
        }
    }
}
