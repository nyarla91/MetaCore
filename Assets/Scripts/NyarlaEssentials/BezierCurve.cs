using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace NyarlaEssentials
{
public class BezierCurve
{
    #region Fields
    private Vector3[] _points;
    public Vector3[] Points
    {
        private get => _points;
        set
        {
            _points = value;
            RecalculatePath();
        }
    }

    private int _quality = 2;
    
    public int Quality
    {
        get => _quality;
        set
        {
            _quality = Mathf.Max(value, 0);
            RecalculatePath();
        }
    }
    public Vector3[] Path { private set; get; }

    public float Length { private set; get; }
    
    public Vector3 this[int index]
    {
        get => Points[index];
    }
    #endregion

    #region Constructors
    
    public BezierCurve() {}
    public BezierCurve(Vector3[] points)
    {
        Points = points;
    }
    public BezierCurve(Vector3[] points, int quality)
    {
        Points = points;
        Quality = quality;
    }

    #endregion

    #region Instance Methods
    
    public Vector3 SetPoint(int index, Vector3 point)
    {
        index = Mathf.Clamp(index, 0, Int32.MaxValue);
        if (Points.Length <= index)
            return point;
        Points[index] = point;
        RecalculatePath();
        return point;
    }
    
    public Vector3 Evaluate(float t) => Evaluate(Points, t);

    private void RecalculatePath()
    {
        Path = new Vector3[Quality];
        for (int i = 0; i < Path.Length; i++)
        {
            float t = (float) i / (float) (Quality - 1);
            Path[i] = Evaluate(t);
        }
        // Calculate Length
        float result = 0;
        for (int i = 1; i < Path.Length; i++)
        {
            result += Vector2.Distance(Path[i - 1], Path[i]);
        }
    }

    public Vector3[] ExtrudePath(float width)
    {
        if (Path == null)
            throw new Exception("No path has been calculated so far");
        return ExtrudePath(Path, width);
    }
    
    #endregion

    #region Static Methods

    public static Vector3 Evaluate(Vector3[] points, float t)
    {
        Vector3[] previousPoints = points;
        for (int i = previousPoints.Length - 1; i > 0; i--)
        {
            Vector3[] newPoints = new Vector3[i];
            for (int j = 0; j < i; j++)
            {
                newPoints[j] = Vector3.Lerp(previousPoints[j], previousPoints[j + 1], t);
            }
            previousPoints = newPoints;
        }
        return previousPoints[0];
    }

    public static Vector3[] EvaluatePath(Vector3[] points, int quality)
    {
        quality--;
        if (quality < 1)
            quality = 1;
        Vector3[] result = new Vector3[quality + 1];
        for (int i = 0; i < result.Length; i++)
        {
            float t = (float) i / (float) quality;
            result[i] = Evaluate(points, t);
        }
        return result;
    }

    public static Vector3[] ExtrudePath(Vector3[] path, float width)
    {
        Vector3[] result = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 direction = i < path.Length - 1 ? path[i + 1] - path[i] : path[i] - path[i - 1];
            Vector3 normal = NEVectors.Rotated(direction, 90).normalized;
            result[i] = path[i] + normal * width;
        }
        return result;
    }

    #endregion
}

}