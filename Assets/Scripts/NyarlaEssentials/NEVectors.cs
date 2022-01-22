using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NyarlaEssentials
{
    public static class NEVectors
    {
        private static Dictionary<Axis, Vector2> _axes2;
        public static Dictionary<Axis, Vector2> Axes2
        {
            get
            {
                if (_axes2 == null)
                {
                    _axes2 = new Dictionary<Axis, Vector2>();
                    _axes2.Add(Axis.X, Vector2.right);
                    _axes2.Add(Axis.Y, Vector2.up);
                }
                return _axes2;
            }
        }

        private static Dictionary<Axis, Vector3> _axes3;
        public static Dictionary<Axis, Vector3> Axes3
        {
            get
            {
                if (_axes3 == null)
                {
                    _axes3 = new Dictionary<Axis, Vector3>();
                    _axes3.Add(Axis.X, Vector3.right);
                    _axes3.Add(Axis.Y, Vector3.up);
                    _axes3.Add(Axis.Z, Vector3.forward);
                }
                return _axes3;
            }
        }
        
        public static Vector2 WithX(this Vector2 vector, float newX) => new Vector2(newX, vector.y);

        public static Vector2 WithY(this Vector2 vector, float newY) => new Vector2(vector.x, newY);
        
        public static Vector3 WithX(this Vector3 vector, float newX) => new Vector3(newX, vector.y, vector.z);
 
        public static Vector3 WithY(this Vector3 vector, float newY) => new Vector3(vector.x, newY, vector.z);

        public static Vector3 WithZ(this Vector3 vector, float newZ) => new Vector3(vector.x, vector.y, newZ);
        
        public static Vector2 DegreesToVector2(this float z) =>
            new Vector2(Mathf.Cos(z * Mathf.Deg2Rad), Mathf.Sin(z * Mathf.Deg2Rad));

        public static float ToDegrees(this Vector2 vector)
        {
            vector.Normalize();
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static void Rotate(ref Vector2 vector, float degrees)
        {
            float oldDegrees = ToDegrees(vector);
            float oldDistance = vector.magnitude;
            vector = DegreesToVector2(oldDegrees + degrees) * oldDistance;
        }

        public static Vector2 Rotated(this Vector2 vector, float degrees)
        {
            Rotate(ref vector, degrees);
            return vector;
        }

        public static void SnapToGrid(ref Vector2 point, Vector2 gridSize)
        {
            float x = point.x;
            float y = point.y;
            NEMath.Snap(ref x, gridSize.x);
            NEMath.Snap(ref y, gridSize.y);
            point = new Vector2(x, y);
        }

        public static Vector2 SnapToGrid(this Vector2 point, Vector2 gridSize)
        {
            SnapToGrid(ref point, gridSize);
            return point;
        }

        public static Vector2 Clamp(this Vector2 vector, Vector2 minValues, Vector2 maxValues)
        {
            vector.x = Mathf.Clamp(vector.x, minValues.x, maxValues.x);
            vector.y = Mathf.Clamp(vector.y, minValues.y, maxValues.y);
            return vector;
        }

        public static void Clamp(ref Vector2 vector, Vector2 minValues, Vector2 maxValues)
        {
            vector = Clamp(vector, minValues, maxValues);
        }

        public static Vector2 XZtoXY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3 XYtoXZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        public static Vector2 RandomPointInBounds2D(this Bounds bounds)
        {
            Vector2 max = bounds.max;
            Vector2 min = bounds.min;
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public static Vector3 RandomPointInBounds(this Bounds bounds)
        {
            Vector3 max = bounds.max;
            Vector3 min = bounds.min;
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        public static Vector3 LerpMulti(this Vector3[] vectors)
        {
            float[] xs = new float[vectors.Length];
            float[] ys = new float[vectors.Length];
            float[] zs = new float[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                xs[i] = vectors[i].x;
                ys[i] = vectors[i].y;
                zs[i] = vectors[i].z;
            }
            return new Vector3(NEMath.Average(xs), NEMath.Average(ys), NEMath.Average(zs));
        }

        public static Vector2 LerpMulti(this Vector2[] vectors2)
        {
            Vector3[] vectors3 = new Vector3[vectors2.Length];
            for (int i = 0; i < vectors2.Length; i++)
            {
                vectors3[i] = vectors2[i];
            }
            return LerpMulti(vectors3);
        }

        public static Vector2 Align(this Vector2 vector, float step)
        {
            float angle = NEVectors.ToDegrees(vector);
            float magnitude = vector.magnitude;
            NEMath.Snap(ref angle, step);
            return NEVectors.DegreesToVector2(angle) * magnitude;
        }

        public static void Align(ref Vector2 vector, float step) => vector = Align(vector, step);

        public static Vector2 IntToFloat(this Vector2Int intVector) => new Vector2(intVector.x, intVector.y);
        
        public static Vector3 IntToFloat(this Vector3Int intVector) => new Vector3(intVector.x, intVector.y, intVector.z);
        
        public static Vector2Int FloatToInt(this Vector2 floatVector) => new Vector2Int(
            Mathf.RoundToInt(floatVector.x),
            Mathf.RoundToInt(floatVector.y));
        
        public static Vector3Int FloatToInt(this Vector3 floatVector) => new Vector3Int(
            Mathf.RoundToInt(floatVector.x),
            Mathf.RoundToInt(floatVector.y),
            Mathf.RoundToInt(floatVector.z));

        public static void SetMagmitude(this Vector2 vector, float magnitude) => vector = vector.normalized * magnitude;
        public static void SetMagmitude(this Vector3 vector, float magnitude) => vector = vector.normalized * magnitude;
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

}