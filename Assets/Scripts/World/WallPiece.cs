using System;
using NyarlaEssentials;
using Player;
using UnityEngine;

namespace World
{
    public class WallPiece : Transformer
    {
        [SerializeField] private GameObject _exitPiece;
        [SerializeField] private Room _myRoom;
        
        private Room _doorIntoRoom;
        
        public void TurnIntoDoor(Room roomInto)
        {
            _doorIntoRoom = roomInto;
            Destroy(_exitPiece);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_doorIntoRoom != null && other.GetComponent<PlayerMarker>() != null)
            {
                _myRoom.Hide();
                _doorIntoRoom.Show();
            }
        }
    }
}