using System;
using UnityEngine;

public class BeltPoint {
    public Vector2 position = new();
    public Vector2 velocity = new();

    public BeltPoint() {

    }
}

public class GrappleTarget : MonoBehaviour {
    [SerializeField] private float range = 10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform origin;

    [SerializeField] private float length = 1f;
    [SerializeField, Min(0)] private int detail = 5;
    [SerializeField] private float pointSmoothTime = 0.1f;
    private int pointCount;
    private float relDist;
    private Vector3[] points, prevPoints, pointVelocities;

    private Transform trans;
    private LineRenderer lineRenderer;

    private void Awake() {
        trans = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();

        pointCount = 2 + detail;
        lineRenderer.positionCount = pointCount;
        points = new Vector3[pointCount];
        prevPoints = new Vector3[pointCount];
        pointVelocities = new Vector3[pointCount];
        relDist = length / (pointCount - 1);
    }

    private void Start() {
        ResetRope();
    }

    private void Update() {
        points[0] = origin.position;

        for (var i = 1; i < pointCount; ++i) {
            var tempPosition = points[i];
            var velocity = (points[i] - prevPoints[i]) / Time.deltaTime;

            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            var diff = points[i] - points[i - 1];
            var restLength = diff - Vector3.ClampMagnitude(diff, relDist);
            if(restLength.sqrMagnitude > float.Epsilon) {
                velocity += restLength;
            }

            points[i] += velocity * Time.deltaTime;
            prevPoints[i] = tempPosition;
        }

        lineRenderer.SetPositions(points);
    }

    private void ResetRope() {
        points[0] = origin.position;
        for (var i = 1; i < pointCount; ++i) {
            points[i] = points[i - 1] + Vector3.left * relDist;
        }

        Array.Copy(points, prevPoints, pointCount);
    }
}