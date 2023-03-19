using UnityEngine;

namespace hinos.character {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement2D : MonoBehaviour {
        public float acceleration;
        public float deceleration;
        [SerializeField] private float _speed;
        public Vector2 groundNormal = Vector2.up;

        private float _moveAxis = 0f;
        private bool _moveRequest = false;
        private float _desiredSpeed;
        
        // Components
        private Rigidbody2D myRigidbody2D;

        public float Speed {
            get => _speed;
            set => SetSpeed(value);
        }

        private void Awake() {
            myRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            _desiredSpeed = _moveAxis * _speed;
        }

        private void FixedUpdate() {
            var maxSpeedChange = GetMaxSpeedChange(_moveRequest);
            ProcessMovement(myRigidbody2D.velocity, _desiredSpeed, maxSpeedChange);
        }

        private float GetMaxSpeedChange(bool moveRequest) {
            var maxSpeedChange = (moveRequest) ? acceleration : deceleration;
            return maxSpeedChange * Time.deltaTime;
        }

        private void ProcessMovement(Vector2 velocity, float moveSpeed, float maxSpeedChange) {
            var direction = Vector2.right - groundNormal * Vector2.Dot(Vector2.right, groundNormal);
            var currentSpeed = Vector2.Dot(velocity, direction);
            var newSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, maxSpeedChange);
            var velocityChange = direction * (newSpeed - currentSpeed);

            myRigidbody2D.velocity += velocityChange;
            _moveRequest = false;
        }

        public void HandleMove(float axis) {
            _moveAxis = axis;
            _moveRequest |= (axis != 0);
        }

        public void SetSpeed(float value) {
            _speed = value;
        }
    }
}
