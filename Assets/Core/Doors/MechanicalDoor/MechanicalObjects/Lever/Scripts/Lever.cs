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

        private bool _leverIsActive = false;
        private SpriteRenderer _spriteRenderer;

        public bool LeverIsActive => _leverIsActive;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid"))
            {
                if (_leverIsActive == false)
                {
                    _leverIsActive = true;

                    _spriteRenderer.sprite = _activeLever;

                    if (_connectedDoor != null)
                    {
                        _connectedDoor.OpenDoor();
                    }
                }
            }
        }
    }
}