using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doors.MechanicalObjects
{
    public class MechanicalButton : MonoBehaviour
    {
        [SerializeField] private MechanicalDoor _connectedDoor;
        [SerializeField] private Sprite _inactiveButton;
        [SerializeField] private Sprite _activeButton;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid"))
            {
                _spriteRenderer.sprite = _activeButton;
                _connectedDoor.OpenDoor();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid"))
            {
                _spriteRenderer.sprite = _inactiveButton;
                _connectedDoor.CloseDoor();
            }
        }
    }
}