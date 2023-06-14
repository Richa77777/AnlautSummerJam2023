using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Sprite _wardrobe;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private CinemachineVirtualCamera _camera;
    private CinemachineTransposer _transposer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        StartCoroutine(StartGameCor());
    }

    private IEnumerator StartGameCor()
    {
        _camera.m_Lens.OrthographicSize = 1.7f;
        _transposer.m_FollowOffset = new Vector3(0, 0.35f, -10);

        FadeController.Instance.Fade(false);
        Player.Instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        _animator.Play("PlayerOut", 0, 0);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length + 2);

        FadeController.Instance.Fade(true);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        _camera.m_Lens.OrthographicSize = 5.35f;
        _transposer.m_FollowOffset = new Vector3(0, 0, -10);

        _animator.Play("Idle", 0, 0);
        _spriteRenderer.sprite = _wardrobe;
        Player.Instance.gameObject.SetActive(true);
        FadeController.Instance.Fade(false);
    }
}
