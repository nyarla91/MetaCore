using UnityEngine;

namespace NyarlaEssentials
{
    public static class NETransform
    {
        public static void LookAt2D(this Transform transform, Vector2 lookAtPosition)
        {
            Vector2 direction = (lookAtPosition - (Vector2) transform.position).normalized;
            transform.rotation = Quaternion.Euler(0, 0, direction.ToDegrees());
        }

        public static void LookAt2D(this Transform transform, Transform target)
        {
            LookAt2D(transform, target.position);
        }
        
        public static Transform[] AllChildren(this Transform transform)
        {
            Transform[] result = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                result[i] = transform.GetChild(i);
            }
            return result;
        }
    }
}