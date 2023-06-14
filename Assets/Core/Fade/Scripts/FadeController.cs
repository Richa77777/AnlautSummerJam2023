using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;

    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private CanvasGroup _canvas;

    private Coroutine _fadeCor;

    public float GetFadeTime => _fadeTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Fade(bool isIn)
    {
        if (_fadeCor != null)
        {
            StopCoroutine(_fadeCor);
        }

        _fadeCor = StartCoroutine(FadeCor(_fadeTime, isIn));
    }

    private IEnumerator FadeCor(float duration, bool isIn)
    {
        float currentAlpha = isIn ? 0f : 1f;
        float targetAlpha = isIn ? 1f : 0f;

        _canvas.alpha = currentAlpha;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _canvas.alpha = Mathf.Lerp(currentAlpha, targetAlpha, t / duration);
            yield return null;
        }

        _canvas.alpha = targetAlpha;
        _fadeCor = null;
    }
}
