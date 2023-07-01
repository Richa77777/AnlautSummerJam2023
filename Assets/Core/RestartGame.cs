using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        FireSpace.FireController.Instance.ClearList();
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.GetPlayerShootingComponent.BigShootingAvailable = true;
        Player.Instance.GetPlayerMovementComponent.UnblockMove();
        SceneManager.LoadScene("Manual1");
    }
}
