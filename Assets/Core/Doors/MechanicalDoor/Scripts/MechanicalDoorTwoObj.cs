using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doors.MechanicalObjects;

namespace Doors
{
    public class MechanicalDoorTwoObj : MonoBehaviour
    {
        [SerializeField] private bool _doorOpened = false;

        [SerializeField] private MechanicalButton _connectedButton;
        [SerializeField] private Lever _connectedLever;

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

        private void Update()
        {
            if (_connectedButton.ButtonIsActive == true && _connectedLever.LeverIsActive == true)
            {
                OpenDoor();
            }

            else if (_connectedButton.ButtonIsActive == false || _connectedLever.LeverIsActive == false)
            {
                CloseDoor();
            }
        }

        public void OpenDoor()
        {
            if (_doorOpened == false)
            {
                if (_connectedButton.ButtonIsActive == true && _connectedLever.LeverIsActive == true)
                {
                    _doorTileComponent.DisableTile();

                    _doorOpened = true;
                    Color color = _spriteRenderer.color;
                    color.a /= 2;

                    _spriteRenderer.color = color;
                    _collider.enabled = false;
                }
            }
        }

        public void CloseDoor()
        {
            if (_doorOpened == true)
            {
                if (_connectedButton.ButtonIsActive == false || _connectedLever.LeverIsActive == false)
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
}
