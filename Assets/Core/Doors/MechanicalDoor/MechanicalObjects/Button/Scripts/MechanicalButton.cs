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

        private bool _buttonIsActive = false;

        private GameObject _whoPressed = null;
        private SpriteRenderer _spriteRenderer;
       
        public bool ButtonIsActive => _buttonIsActive;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_buttonIsActive == false)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid") || collision.gameObject.layer == LayerMask.NameToLayer("Box"))
                {
                    if (_whoPressed == null)
                    {
                        _whoPressed = collision.gameObject;
                        _spriteRenderer.sprite = _activeButton;
                        _buttonIsActive = true;

                        if (_connectedDoor != null)
                        {
                            _connectedDoor.OpenDoor();
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_buttonIsActive == true)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Humanoid") || collision.gameObject.layer == LayerMask.NameToLayer("Box"))
                {
                    if (collision.gameObject == _whoPressed)
                    {
                        _spriteRenderer.sprite = _inactiveButton;
                        _buttonIsActive = false;

                        if (_connectedDoor != null)
                        {
                            _connectedDoor.CloseDoor();
                        }

                        _whoPressed = null;
                    }
                }
            }
        }
    }
}