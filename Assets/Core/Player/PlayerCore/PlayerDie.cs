using PoolSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
    public class PlayerDie : MonoBehaviour
    {
        [SerializeField] private Animator _bodyAnimator;

        private Coroutine _dieCor;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("SpikesGrid") || collision.gameObject.GetComponent<Zombie>())
            {
                Die();
            }
        }

        private void Die()
        {
            if (_dieCor == null)
            {
                _dieCor = StartCoroutine(DieCoroutine());
            }
        }

        private IEnumerator DieCoroutine()
        {
            Player.Instance.GetPlayerShootingComponent.GetHandWithPistol.gameObject.SetActive(false);
            Player.Instance.GetPlayerMovementComponent.BlockMove();
            _bodyAnimator.Play("Die", 0, 0f);

            yield return new WaitForSeconds(_bodyAnimator.GetCurrentAnimatorClipInfo(0).Length);

            FadeController.Instance.Fade(true);

            yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

            FireSpace.FireController.Instance.ClearList();

            _dieCor = null;

            _bodyAnimator.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
