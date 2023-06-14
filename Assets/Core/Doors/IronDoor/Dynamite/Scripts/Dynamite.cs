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

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private Coroutine _explosionCycleCor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
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

            yield return new WaitForSeconds(_timeBeforeExplosion);

            _animator.StopPlayback();
            _explosionAnimator.Play("Explosion", 0, 0f);

            float waitingTime = _explosionAnimator.GetCurrentAnimatorClipInfo(0).Length / 1.5f;

            yield return new WaitForSeconds(waitingTime);

            Color color = _spriteRenderer.color;
            color.a = 0;

            _connectedIronDoor.SetActive(false);
            _spriteRenderer.color = color;

            yield return new WaitForSeconds(_explosionAnimator.GetCurrentAnimatorClipInfo(0).Length - waitingTime - 0.21f);

            gameObject.SetActive(false);

        }
    }
}