using System.Collections;
using UnityEngine;
using WiSJoy.DesignPattern;

public class SceneTransition : SingletonMonoBehaviour<SceneTransition>
{
    [SerializeField] private CanvasGroup fadeOverlay;
    public float fadeDuration = 1f;

    public void FadeOut()
    {
        StartCoroutine(Fade(1)); // Fading to opaque
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0)); // Fading to transparent
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeOverlay.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            fadeOverlay.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.alpha = targetAlpha;
    }
}
