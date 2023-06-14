using FireSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doors
{
    public class Dynamite : MonoBehaviour
    {
        [SerializeField] private GameObject _connectedIronDoor;
        [SerializeField] private float _timeBeforeExplosion = 3f;
        [SerializeField] private Animator _explosionAnimator;

        [SerializeField] private AudioClip _fitil;
        [SerializeField] private AudioClip _boom;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private AudioSource _audioSource;

        private Coroutine _explosionCycleCor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Fire>())
            {
                if (_explosionCycleCor == null)
                {
                    _explosionCycleCor = StartCoroutine(ExplosionCycle());
                }
            }
        }

        private IEnumerator ExplosionCycle()
        {
            _animator.Play("Preparing", 0, 0f);

            _audioSource.clip = _fitil;
            _audioSource.Play();

            yield return new WaitForSeconds(_timeBeforeExplosion);

            _animator.StopPlayback();
            _explosionAnimator.Play("Explosion", 0, 0f);

            _audioSource.clip = _boom;
            _audioSource.Play();

            float waitingTime = _explosionAnimator.GetCurrentAnimatorClipInfo(0).Length / 1.5f;

            yield return new WaitForSeconds(waitingTime);

            Color color = _spriteRenderer.color;
            color.a = 0;

            if (_connectedIronDoor != null)
            {
                _connectedIronDoor.SetActive(false);

            }

            _spriteRenderer.color = color;

            yield return new WaitForSeconds(_explosionAnimator.GetCurrentAnimatorClipInfo(0).Length - waitingTime - 0.21f);

            gameObject.SetActive(false);

        }
    }
}