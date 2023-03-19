using System;
using UnityEngine;

namespace hinos.movement {
    public class Movement {

        public static Vector3 CalculateMovementOnAxis(Vector3 currentVelocity, float desiredSpeed, float maxSpeedChange, Vector3 axis, Vector3 normal) {
            var direction = axis - normal * Vector3.Dot(axis, normal);
            var currentSpeed = Vector3.Dot(currentVelocity, direction);
            var newSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, maxSpeedChange);
            return direction * (newSpeed - currentSpeed);
        }

        public static Vector2 CalculateMovementOnAxis2D(Vector2 currentVelocity, float desiredSpeed, float maxSpeedChange, Vector2 axis, Vector2 normal) {
            var direction = axis - normal * Vector2.Dot(axis, normal);
            var currentSpeed = Vector2.Dot(currentVelocity, direction);
            var newSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, maxSpeedChange);
            return direction * (newSpeed - currentSpeed);
        }
    }

    public struct SurfaceData<TVector> {
        public int contactCount;
        public TVector contactSurface;
    }

    public abstract class SurfaceDetection<TVector> : MonoBehaviour {
        [SerializeField] protected float maxGroundAngle = 25.0f;
        [SerializeField] protected float maxStairAngle = 50.0f;
        [SerializeField] protected LayerMask stairMask = -1;

        protected int stepsSinceLastGrounded;
        protected float minGroundDotProduct, minStairDotProduct;
        protected int contactCount, steepContactCount;
        protected int prevContactCount, prevSteepContactCount;
        protected TVector contactNormal = default;
        protected TVector steepNormal = default;

        public int StepsSinceLastGrounded => stepsSinceLastGrounded;
        public int ContactCount => contactCount;
        public TVector ContactNormal => contactNormal;

        private void Awake() {
            stepsSinceLastGrounded = 0;
            minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            minStairDotProduct = Mathf.Cos(maxStairAngle * Mathf.Deg2Rad);
        }

        protected float GetMinDot(int layer) {
            return (stairMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairDotProduct;
        }
    }
}
