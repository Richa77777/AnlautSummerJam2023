using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

namespace BulletsSpace
{
    public class CallBigShotFunction : MonoBehaviour
    {
        public void CallBigShot()
        {
            Player.Instance.GetPlayerShootingComponent.BigShot();
        }
    }
}
