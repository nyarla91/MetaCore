using System;
using NyarlaEssentials;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace World
{
    public class WallPiece : Transformer
    {
        [SerializeField] private MeshRenderer _doorRenderer;
        [SerializeField] private MeshRenderer _ladderRenderer;
        [SerializeField] private Collider _exitCollider;
        [SerializeField] private MeshRenderer _exitMesh;
        [SerializeField] private Collider _floorExitTrigger;
        [SerializeField] private Room _myRoom;
        
        private Room _doorIntoRoom;
        private bool _floorExit;
        
        public void TurnIntoDoor(Room roomInto)
        {
            _doorIntoRoom = roomInto;
            _exitCollider.enabled = _exitMesh.enabled = false;
            _doorRenderer.enabled = true;
        }

        public void TurnIntoFloorExit()
        {
            print(transform.position);
            _floorExit = true;
            _floorExitTrigger.enabled = true;
            _exitMesh.enabled = false;
            _doorRenderer.enabled = true;
            _ladderRenderer.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_doorIntoRoom == null || other.GetComponent<PlayerMarker>() == null)
                return;
            
            _myRoom.Hide();
            _doorIntoRoom.Show();
        }

        public void LockDoor() => _exitCollider.enabled = true;
        public void UnlockDoor()
        {
            if (_doorIntoRoom != null)
                _exitCollider.enabled = false;
        }
    }
}