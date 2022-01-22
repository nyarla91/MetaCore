using NyarlaEssentials;
using UnityEngine;

namespace World
{
    public class WallPiece : Transformer
    {
        public void TurnIntoDoor()
        {
            transform.position += Vector3.up;
        }
    }
}