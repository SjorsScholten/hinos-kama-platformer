using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour {
    [SerializeField, Min(0.01f)] private float length;
    [SerializeField, Min(0)] private int detail;
    [SerializeField] private int passes;

    private Vector3[] pointPositions;
    private Vector3[] previousPointPositions;
    private bool[] staticPoints;

    private Tuple<Vector3, Vector3>[] edges;
    private float edgeLength;

    private Coroutine LengthRoutine;

    private LineRenderer lineRenderer;

    public float Length {
        get => length;
        set {
            StopCoroutine(LengthRoutine);
            length = value;
            edgeLength = length / edges.Length;
        }
    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();

        pointPositions = new Vector3[2 + detail];
        previousPointPositions = new Vector3[pointPositions.Length];
        staticPoints = new bool[pointPositions.Length];

        edges = new Tuple<Vector3, Vector3>[pointPositions.Length - 1];
        edgeLength = length / edges.Length;

        for (var i = 0; i < edges.Length; ++i) {
            edges[i] = Tuple.Create(pointPositions[i], pointPositions[i + 1]);
        }

        lineRenderer.positionCount = pointPositions.Length;
    }

    private void Start() {
        ResetPoints();

        staticPoints[0] = true;
    }

    private void Update() {
        // Verlet Integration
        for (var i = 0; i < pointPositions.Length; ++i) {
            if (staticPoints[i]) continue;

            var cachedPosition = pointPositions[i];
            pointPositions[i] += pointPositions[i] - previousPointPositions[i];
            pointPositions[i].y += Physics.gravity.y * Time.deltaTime * Time.deltaTime;
            previousPointPositions[i] = cachedPosition;
        }

        // Jacobsen Method
        for (var i = 0; i < passes; ++i) {
            for (var j = 1; j < pointPositions.Length - 1; ++j) {
                var direction = (pointPositions[j + 1] - pointPositions[j]).normalized;
                var error = Vector2.Distance(pointPositions[j], pointPositions[j + 1]) - edgeLength;
                pointPositions[j] += direction * error * 0.5f;
                pointPositions[j + 1] -= direction * error * 0.5f;
            }
        }

        lineRenderer.SetPositions(pointPositions);
    }

    public void ResetPoints() {
        for (var i = 1; i < pointPositions.Length; ++i) {
            pointPositions[i] = pointPositions[i - 1] + Vector3.right * edgeLength;
            previousPointPositions[i] = pointPositions[i];
        }
    }

    public void SetPoint(int index, Vector3 position, bool isStatic) {
        previousPointPositions[index] = pointPositions[index] = position;
        staticPoints[index] = isStatic;
    }

    public void SetLengthOverTime(float value, float duration) {
        StopCoroutine(LengthRoutine);
        LengthRoutine = StartCoroutine(LengthOverTimeRoutine(value, duration));
    }

    private IEnumerator LengthOverTimeRoutine(float targetLength, float duration) {
        var initialLength = length;
        var initialEdgeLength = edgeLength;
        var targetEdgeLength = targetLength / edges.Length;

        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;

            var value = time / duration;
            length = Mathf.Lerp(initialLength, targetLength, value);
            edgeLength = Mathf.Lerp(initialEdgeLength, targetEdgeLength, value);

            yield return null;
        }

        length = targetLength;
        edgeLength = targetEdgeLength;
    }
}
