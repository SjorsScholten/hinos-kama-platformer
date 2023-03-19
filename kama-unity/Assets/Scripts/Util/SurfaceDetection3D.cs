using UnityEngine;

namespace hinos.movement {
    public class SurfaceDetection3D : SurfaceDetection<Vector3> {
        private void FixedUpdate() {
            contactNormal = Vector3.up;
            stepsSinceLastGrounded += 1;

            if (steepContactCount > 1) {
                steepNormal.Normalize();
                if (steepNormal.y > minGroundDotProduct) {
                    contactCount = 1;
                    contactNormal = steepNormal;
                }
            }

            if (contactCount > 0) {
                stepsSinceLastGrounded = 0;

                if (contactCount > 1) {
                    contactNormal.Normalize();
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {
            var minDot = GetMinDot(collision.gameObject.layer);

            for (var i = 0; i < collision.contactCount; ++i) {
                var normal = collision.GetContact(i).normal;

                if (normal.y >= minDot) {
                    contactCount += 1;
                    contactNormal += normal;
                }
                else if (normal.y > -0.01f) {
                    steepContactCount += 1;
                    steepNormal += normal;
                }
            }
        }
    }
}
