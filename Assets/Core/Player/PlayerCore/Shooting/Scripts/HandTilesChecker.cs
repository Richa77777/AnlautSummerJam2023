using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class HandTilesChecker : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("GroundGrid"))
            {
                Player.Instance.GetPlayerShootingComponent.ShootingAvailable = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("GroundGrid"))
            {
                Player.Instance.GetPlayerShootingComponent.ShootingAvailable = true;
            }
        }
    }
}
