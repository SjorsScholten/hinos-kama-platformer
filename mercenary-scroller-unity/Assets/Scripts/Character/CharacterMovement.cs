using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hinos.character
{
    public class CharacterMovement : MonoBehaviour {
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 10f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Vector3 _groundNormal = Vector3.up;

        // Input
        private Vector2 _moveDirection;
        private float _moveAmount;
        private bool _moveRequest;

        // Components
        private Rigidbody _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            var moveSpeed = Mathf.Sign(_moveDirection.x) * _moveAmount * _speed;
            var maxSpeedChange = GetMaxSpeedChange(_moveRequest);
            ProcessMovement(_rigidbody.velocity, moveSpeed, maxSpeedChange);
        }

        private float GetMaxSpeedChange(bool moveRequest) {
            var maxSpeedChange = (moveRequest) ? _acceleration : _deceleration;
            return maxSpeedChange * Time.deltaTime;
        }

        private void ProcessMovement(Vector3 velocity, float moveSpeed, float maxSpeedChange) {
            var direction = Vector3.right - _groundNormal * Vector3.Dot(Vector3.right, _groundNormal);
            var currentSpeed = Vector3.Dot(velocity, direction);
            var newSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, maxSpeedChange);
            var velocityChange = direction * (newSpeed - currentSpeed);

            _rigidbody.velocity += velocityChange;
        }

        public void HandleMove(Vector3 direction, float amount) {
            _moveDirection = direction;
            _moveAmount = amount;
        }
    }
}
