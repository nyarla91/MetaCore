using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class Room : MonoBehaviour
    {
        private const int MinWall = 3;
        private const int MaxWall = 7;
        private const int MinSquare = 15;
        private const float FloorPieceSize = 2.5f;

        private Vector3[] AllSides
        {
            get
            {
                return new[]
                {
                    Vector3.left,
                    Vector3.right,
                    Vector3.back,
                    Vector3.forward
                };
            }
        }

        [SerializeField] private GameObject _floorPiecePrefab;
        [SerializeField] private GameObject _wallPiecePrefab;
        private Transform[,] _floor;
        private List<WallPiece>[] _walls = new List<WallPiece>[4];

        private void Awake()
        {
            for (int i = 0; i < _walls.Length; i++)
            {
                _walls[i] = new List<WallPiece>();
            }
        }

        private void Start()
        {
            Generate(Vector3.back, 5);
        }

        public void Generate(Vector3 entranceSide, int exits)
        {
            int entranceSideNumber = NumberFromSide(entranceSide);
            
            int width = Random.Range(MinWall, MaxWall + 1);
            int length = Random.Range(MinWall, MaxWall + 1);
            if (width * length < MinSquare)
            {
                width++;
                length++;
            }

            _floor = new Transform[length, width];
            for (int z = 0; z < width; z++)
            {
                for (int x = 0; x < length; x++)
                {
                    Vector3 position = transform.position + new Vector3(x, 0, z) * FloorPieceSize;
                    Transform floorPiece = Instantiate(_floorPiecePrefab, position, Quaternion.identity, transform).transform;
                    _floor[x, z] = floorPiece;
                    if (x == 0)
                        _walls[0].Add(CreateWall(position + Vector3.left * FloorPieceSize * 0.5f, -90));
                    else if (x == length - 1)
                        _walls[1].Add(CreateWall(position + Vector3.right * FloorPieceSize * 0.5f, 90));
                    if (z == 0)
                        _walls[2].Add(CreateWall(position + Vector3.back * FloorPieceSize * 0.5f, 180));
                    else if (z == width - 1)
                        _walls[3].Add(CreateWall(position + Vector3.forward * FloorPieceSize * 0.5f, 0));
                }
            }

            Transform zeroZeroPiece = _floor[0, 0];
            Transform maxMaxPiece = _floor[length - 1, width - 1];
            
            bool isEntranceAlongX = entranceSide.x == 0;
            int roomAxisOffset = Random.Range(1, (isEntranceAlongX ? width : length) - 1);
            Vector3 roomOffset = -FloorPieceSize * roomAxisOffset * (isEntranceAlongX ? Vector3.right : Vector3.forward);
            transform.position += roomOffset;

            int exitsLeft = exits;
            int[] exitsForExits = new int[4];
            do
            {
                for (int i = 0; i < exitsLeft; i++)
                {
                    exitsForExits[Random.Range(0, 4)]++;
                }
                exitsLeft = exitsForExits[entranceSideNumber];
                exitsForExits[entranceSideNumber] = 0;
            } while (exitsLeft > 0);
            for (int i = 0; i < AllSides.Length; i++)
            {
                if (i == entranceSideNumber)
                {
                    _walls[entranceSideNumber][roomAxisOffset].TurnIntoDoor();
                    continue;
                }

                if (exitsForExits[i] > 0)
                {
                    _walls[i][Random.Range(1, (isEntranceAlongX ? width : length) - 1)].TurnIntoDoor();
                }
            }

            WallPiece CreateWall(Vector3 position, float yRotation)
            {
                return Instantiate(_wallPiecePrefab, position,
                    Quaternion.Euler(0, yRotation, 0), transform).GetComponent<WallPiece>();
            }
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