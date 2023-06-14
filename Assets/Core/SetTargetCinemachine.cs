using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PlayerSpace;

public class SetTargetCinemachine : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = Player.Instance.transform;
    }
}
