using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
  [Serializable]
  public class Gesture
  {
    public List<Vector2> Points;

    public string Name;
  }

  public List<Gesture> Gestures;

  public float DistanceError = 0.1f;

  private List<Vector2> _readPoints = new List<Vector2>();

  private bool _readingJesture;

  void Awake()
  {
    RecalcGestureSize();
  }

  private void RecalcGestureSize()
  {
    foreach (var gesture in Gestures)
    {
      var size = GetSize(gesture.Points);
      var center = GetCenter(gesture.Points);
      for (int i = 0; i < gesture.Points.Count; i++)
      {
        var p = gesture.Points[i];
        p = center - p;

        if (size.x != 0)
          p.x /= size.x;
        else
          p.x = 0.5f;

        if (size.y != 0)
          p.y /= size.y;
        else
          p.y = 0.5f;

        gesture.Points[i] = p;
      }
    }
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      _readingJesture = true;
      StartCoroutine(ReadingCoroutine());
    }

    if (Input.GetMouseButtonUp(0))
    {
      _readingJesture = true;
      StopAllCoroutines();

      RecognizeGesture();

      _readPoints.Clear();
    }
  }

  private void RecognizeGesture()
  {
    var size = GetSize(_readPoints);
    var center = GetCenter(_readPoints);
    var aspect = 1f / (size.x < size.y ? size.y / size.x : size.x / size.y);

    for (int i = 0; i < _readPoints.Count; i++)
    {
      var p = _readPoints[i];
      p = center - p;

      if (size.x != 0)
        p.x /= size.x;
      else
        p.x = 0.5f;

      if (size.y != 0)
        p.y /= size.y;
      else
        p.y = 0.5f;

      if (size.x < size.y)
      {
        p.y = 0.5f - aspect/2f + aspect * p.y;
      }
      else
      {
        p.x = 0.5f - aspect/2f + aspect * p.x;
      }

      _readPoints[i] = p;
    }


    for (int i = 0; i < _readPoints.Count - 1; i++)
    {
      var p1 = _readPoints[i];
      var p2 = _readPoints[i + 1];
      Debug.DrawLine(p1*3, p2*3, Color.red, 5f);
    }

    Gesture gesture = null;
    float maxScore = 0f;

    foreach (var g in Gestures)
    {
      for (int i = 0; i < g.Points.Count - 1; i++)
      {
        var p1 = g.Points[i];
        var p2 = g.Points[i + 1];
        Debug.DrawLine(p1 * 3, p2 * 3, Color.green, 5f);
      }

      float readScore = 0f;
      float gestureScore = 0f;

      foreach (var rp in _readPoints)
      {
        var distance = GetClosestPointDistance(rp, g.Points);
        readScore += distance;
      }

      readScore /= _readPoints.Count;

      foreach (var rp in g.Points)
      {
        var distance = GetClosestPointDistance(rp, _readPoints);
        gestureScore += distance;
      }

      gestureScore /= g.Points.Count;

      var resultScore = 1f - (gestureScore * readScore);


      Debug.Log(g.Name + " r: " + resultScore + " g: " + gestureScore + " rs: " + readScore);

      if (maxScore < resultScore)
      {
        maxScore = resultScore;
        gesture = g;
      }
    }

    Debug.Log(gesture.Name);
  }

  private float GetClosestPointDistance(Vector2 point, List<Vector2> points)
  {
    var minD = float.MaxValue;

    foreach (var p in points)
    {
      var d = Vector2.Distance(point, p);
      if (d < minD)
        minD = d;
    }

    return minD;
  }

  private Vector2 GetCenter(List<Vector2> points)
  {
    var center = new Vector2();

    foreach (var p in points)
    {
      center.x = Mathf.Max(center.x, p.x);
      center.y = Mathf.Max(center.y, p.y);
    }

    return center;
  }

  private Vector2 GetSize(List<Vector2> points)
  {
    var min = new Vector2(float.MaxValue, float.MaxValue);
    var max = new Vector2(float.MinValue, float.MinValue);

    foreach (var p in points)
    {
      min.x = Mathf.Min(min.x, p.x);
      min.y = Mathf.Min(min.y, p.y);
      max.x = Mathf.Max(max.x, p.x);
      max.y = Mathf.Max(max.y, p.y);
    }

    return max - min;
  }

  private IEnumerator ReadingCoroutine()
  {
    while (_readingJesture)
    {
      _readPoints.Add(Input.mousePosition);
      yield return null;
    }
  }
}
