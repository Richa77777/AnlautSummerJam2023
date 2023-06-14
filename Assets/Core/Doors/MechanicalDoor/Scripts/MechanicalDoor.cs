using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doors
{
    public class MechanicalDoor : MonoBehaviour
    {
        [SerializeField] private bool _doorOpened = false;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private DoorTile _doorTileComponent;

        public bool DoorOpened => _doorOpened;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _doorTileComponent = GetComponent<DoorTile>();
        }

        public void OpenDoor()
        {
            if (_doorOpened == false)
            {
                _doorTileComponent.DisableTile();

                _doorOpened = true;
                Color color = _spriteRenderer.color;
                color.a /= 2;

                _spriteRenderer.color = color;
                _collider.enabled = false;
            }
        }

        public void CloseDoor()
        {
            if (_doorOpened == true)
            {
                _doorTileComponent.EnableTile();

                _doorOpened = false;
                Color color = _spriteRenderer.color;
                color.a *= 2;

                _spriteRenderer.color = color;
                _collider.enabled = true;
            }
        }
    }
}