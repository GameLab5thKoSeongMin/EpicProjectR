// Responsibility: Runs simple runtime UI tweens for the first playable passive view.
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        public void ScaleTo(RectTransform target, Vector3 from, Vector3 to, float duration)
        {
            if (target == null)
            {
                return;
            }

            target.localScale = from;
            StartCoroutine(ScaleRoutine(target, to, Mathf.Max(0f, duration)));
        }

        public void FadeTo(CanvasGroup target, float from, float to, float duration)
        {
            if (target == null)
            {
                return;
            }

            target.alpha = from;
            StartCoroutine(FadeRoutine(target, to, Mathf.Max(0f, duration)));
        }

        public void TintTo(Graphic target, Color from, Color to, float duration)
        {
            if (target == null)
            {
                return;
            }

            target.color = from;
            StartCoroutine(TintRoutine(target, to, Mathf.Max(0f, duration)));
        }

        public void InvokeAfter(float delaySeconds, Action action)
        {
            if (action == null)
            {
                return;
            }

            StartCoroutine(InvokeAfterRoutine(Mathf.Max(0f, delaySeconds), action));
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

        private static IEnumerator ScaleRoutine(RectTransform target, Vector3 to, float duration)
        {
            if (duration <= 0f)
            {
                target.localScale = to;
                yield break;
            }

            var from = target.localScale;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed = Mathf.Min(elapsed + Time.deltaTime, duration);
                var t = elapsed / duration;
                t = t * t * (3f - 2f * t);
                target.localScale = Vector3.Lerp(from, to, t);
                yield return null;
            }

            target.localScale = to;
        }

        private static IEnumerator FadeRoutine(CanvasGroup target, float to, float duration)
        {
            if (duration <= 0f)
            {
                target.alpha = to;
                yield break;
            }

            var from = target.alpha;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed = Mathf.Min(elapsed + Time.deltaTime, duration);
                var t = elapsed / duration;
                t = t * t * (3f - 2f * t);
                target.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            target.alpha = to;
        }

        private static IEnumerator TintRoutine(Graphic target, Color to, float duration)
        {
            if (duration <= 0f)
            {
                target.color = to;
                yield break;
            }

            var from = target.color;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed = Mathf.Min(elapsed + Time.deltaTime, duration);
                var t = elapsed / duration;
                t = t * t * (3f - 2f * t);
                target.color = Color.Lerp(from, to, t);
                yield return null;
            }

            target.color = to;
        }

        private static IEnumerator InvokeAfterRoutine(float delaySeconds, Action action)
        {
            if (delaySeconds > 0f)
            {
                yield return new WaitForSeconds(delaySeconds);
            }

            action();
        }
    }
}
