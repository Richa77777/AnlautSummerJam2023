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
        SceneManager.LoadScene("Manual1");
    }
}
