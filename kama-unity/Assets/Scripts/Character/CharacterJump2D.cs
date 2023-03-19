using UnityEngine;

namespace hinos.character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterJump2D : MonoBehaviour {
        [SerializeField] private int jumpBufferLength;
        private int jumpBufferCount;
        private bool jumpRequest;
        private bool grounded;
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private Vector2 groundNormal = Vector2.up;

        // Components
        private Rigidbody2D myRigidbody2D;

        private void Awake() {
            myRigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Update() {
            if(jumpRequest) {
                //ProcessJumpBuffer();
            }
        }

        public void FixedUpdate() {
            var initialVelocity = myRigidbody2D.velocity;

            if(jumpRequest) {
                ProcessJump(initialVelocity);
            }
        }

        private void ProcessJumpBuffer() {
            jumpBufferCount--;
            if(jumpBufferCount <= 0) {
                jumpRequest = false;
            }
        }

        public void ProcessJump(Vector2 velocity) {
            var jumpVelocity = Mathf.Sqrt(-2f * Physics.gravity.y * _jumpHeight);
            var alignedSpeed = Vector2.Dot(velocity, groundNormal);

            if(velocity.y > 0) {
                jumpVelocity = Mathf.Max(jumpVelocity - alignedSpeed, 0);
            }

            myRigidbody2D.velocity += groundNormal * jumpVelocity;
            jumpRequest = false;
        }

        public void HandleJump() {
            jumpRequest |= true;
        }
    }
}
