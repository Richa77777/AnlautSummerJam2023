using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class CallBigShotFunction : MonoBehaviour
{
    public void CallBigShot()
    {
        Player.Instance.GetPlayerShootingComponent.BigShot();
    }
}
