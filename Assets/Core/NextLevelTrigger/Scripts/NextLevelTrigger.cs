using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private string _levelName;

    private Coroutine _nextSceneCor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            if (_nextSceneCor == null)
            {
                Player.Instance.GetPlayerMovementComponent.BlockMove();
                _nextSceneCor = StartCoroutine(NextSceneCor());
            }
        }
    }

    private IEnumerator NextSceneCor()
    {
        FadeController.Instance.Fade(true);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        SceneManager.LoadScene(_levelName);
    }
}
