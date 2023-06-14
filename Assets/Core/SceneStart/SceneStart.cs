using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartSceneCor());
    }

    private IEnumerator StartSceneCor()
    {
        Player.Instance.GetPlayerMovementComponent.BlockMove();

        Player.Instance.GetPlayerMovementComponent.GetBodyAnimator.Play("Idle", 0, 0f);
        Player.Instance.GetPlayerShootingComponent.GetHandWithPistol.gameObject.SetActive(true);
        Player.Instance.transform.position = transform.position;
        FadeController.Instance.Fade(false);

        yield return new WaitForSeconds(FadeController.Instance.GetFadeTime);

        Player.Instance.GetPlayerMovementComponent.UnblockMove();
    }
}
