using Cinemachine;
using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private CanvasGroup _winTab;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _kletka;

    private CinemachineVirtualCamera _camera;
    private CinemachineTransposer _transposer;

    private Coroutine _endGameCor;

    private void Awake()
    {
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            if (_endGameCor == null)
            {
                _endGameCor = StartCoroutine(EndGameCor());
            }
        }
    }

    private IEnumerator EndGameCor()
    {
        Color color = _kletka.color;
        color.a = 0;
        _kletka.color = color;

        Player.Instance.gameObject.SetActive(false);

        FadeController.Instance.Fade(true);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        _camera.m_Lens.OrthographicSize = 1.85f;
        _transposer.m_FollowOffset = new Vector3(0, 0.3f, -10);

        _animator.Play("Hug", 0, 0f);

        yield return new WaitForSeconds(5f);

        FadeController.Instance.Fade(true);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        _winTab.gameObject.SetActive(true);

        float startAlpha = _winTab.alpha;
        float targetAlpha = 1f;

        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            _winTab.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / 2f);
            yield return null;
        }
    }
}
