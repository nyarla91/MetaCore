using System;
using System.Collections;
using NyarlaEssentials;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace World
{
    public class FloorGenerator : MonoBehaviour
    {
        private const float RoomSize = 17.5f;
        
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private int _additionalRooms;

        private PlayerMarker _playerMarker;

        [Inject]
        private void Construct(PlayerMarker playerMarker)
        {
            _playerMarker = playerMarker;
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            int nextFloorAtWing = Random.Range(0, 4);
            int[] roomsAtWing = {1, 1, 1, 1};
            for (int i = 0; i < _additionalRooms; i++)
            {
                roomsAtWing[Random.Range(0, 4)]++;
            }
            
            Room startingRoom = CreateRoom(Vector3.zero);

            for (int wing = 0; wing < 4; wing++)
            {
                Vector3 wingSide = SideFromNumber(wing);
                Vector3 oppositeWingSide = wingSide * -1;
                int oppositeSideNumber = NumberFromSide(oppositeWingSide);
                Room previousRoom = null;
                for (int room = 0; room < roomsAtWing[wing]; room++)
                {
                    Room newRoom = CreateRoom(SideFromNumber(wing) * (room + 1) * RoomSize);
                    if (previousRoom != null)
                    {
                        newRoom.GetExitWall(oppositeSideNumber).TurnIntoDoor(previousRoom);
                        previousRoom.GetExitWall(wing).TurnIntoDoor(newRoom);
                    }
                    else
                    {
                        newRoom.GetExitWall(oppositeSideNumber).TurnIntoDoor(startingRoom);
                        startingRoom.GetExitWall(wing).TurnIntoDoor(newRoom);
                    }
                    newRoom.Hide();
                    newRoom.Generate(room == roomsAtWing[wing] - 1);
                    previousRoom = newRoom;
                }
                if (wing == nextFloorAtWing)
                    previousRoom.GetExitWall(wing).TurnIntoFloorExit();
            }
            yield return null;
            startingRoom.Show();
        }

        public Room CreateRoom(Vector3 position)
        {
            Room room = Instantiate(_roomPrefab, position.WithY(0), Quaternion.identity).GetComponent<Room>();
            room.PlayerMarker = _playerMarker;
            return room;
        }

        private Vector3 SideFromNumber(int number)
        {
            switch (number)
            {
                case 0 : return Vector3.left;
                case 1 : return Vector3.right;
                case 2 : return Vector3.back;
                case 3 : return Vector3.forward;
                default: return Vector3.zero;
            }
        }

        private int NumberFromSide(Vector3 side)
        {
            if (side.Equals(Vector3.left))
                return 0;
            if (side.Equals(Vector3.right))
                return 1;
            if (side.Equals(Vector3.back))
                return 2;
            if (side.Equals(Vector3.forward))
                return 3;
            return -1;
        }
    }
}