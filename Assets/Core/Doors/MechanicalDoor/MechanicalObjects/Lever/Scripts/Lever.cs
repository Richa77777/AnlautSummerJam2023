using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doors.MechanicalObjects
{
    public class Lever : MonoBehaviour
    {
        [SerializeField] private MechanicalDoor _connectedDoor;
        [SerializeField] private Sprite _inactiveLever;
        [SerializeField] private Sprite _activeLever;

        private bool _leverActive = false;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid"))
            {
                if (_leverActive == true)
                {
                    _leverActive = false;

                    _spriteRenderer.sprite = _inactiveLever;
                    _connectedDoor.CloseDoor();
                }

                else if (_leverActive == false)
                {
                    _leverActive = true;

                    _spriteRenderer.sprite = _activeLever;
                    _connectedDoor.OpenDoor();
                }
            }
        }
    }
}