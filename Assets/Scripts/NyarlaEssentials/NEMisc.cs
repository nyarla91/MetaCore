using UnityEngine;

namespace NyarlaEssentials
{
    public static class NEMisc
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T swapper = a;
            a = b;
            b = swapper;
        }

        public static void SelfDestruct(this GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
}