using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireRaising
{
    public class Fire : MonoBehaviour
    {
        [SerializeField] private float _fireBurningTime = 5f;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            StartCoroutine(FireCycle());
        }

        private IEnumerator FireCycle()
        {
            _animator.Play("Start", 0, 0);

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length + 0.15f);

            _animator.Play("Firing", 0, 0);

            yield return new WaitForSeconds(_fireBurningTime);

            _animator.Play("End", 0, 0);

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length - 0.75f);

            gameObject.SetActive(false);
        }
    }
}
