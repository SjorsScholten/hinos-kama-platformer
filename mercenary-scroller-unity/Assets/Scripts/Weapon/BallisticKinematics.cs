using System;
using UnityEngine;
using UnityEngine.Events;

namespace hinos.weapons 
{
    public class BallisticKinematics : MonoBehaviour {
        private Vector3 _velocity;
        private BallisticSnapshot _prevState;
        private Transform _transform;

        public BallisticUnityEvent OnHitEvent;

        private void Awake() {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate() {
            if(CheckForCollision(out var hit)) {
                ProcessHit(hit);
                this.gameObject.SetActive(false);
            }
            else {
                ProcessMovement();
            }
        }

        private void Initialize(Vector3 origin, Vector3 velocity) {
            _transform.position = origin;
            _velocity = velocity;
            _prevState = CreateSnapshot();
            
            ProcessMovement();
        }

        private bool CheckForCollision(out RaycastHit hit) {
            var diff = _transform.position - _prevState.position;
            var direction = diff.normalized;
            var distance = diff.magnitude;
            return Physics.Raycast(_prevState.position, direction, out hit, distance, -1, QueryTriggerInteraction.Ignore);
        }

        private BallisticSnapshot CreateSnapshot() {
            return new BallisticSnapshot(_transform.position);
        }

        private void ProcessMovement() {
            _transform.position = _velocity * Time.deltaTime;
        }

        private void ProcessHit(RaycastHit hit) {
            if(hit.collider.TryGetComponent<IHandleBallisticHit>(out var handler)) {
                handler.OnHit(hit);
            }

            OnHitEvent.Invoke(hit);
        }

        public void HandleFire(Vector3 origin, Vector3 velocity) {
            Initialize(origin, velocity);
        }
    }

    public readonly struct BallisticSnapshot {
        public readonly Vector3 position;

        public BallisticSnapshot(Vector3 position) {
            this.position = position;
        }
    }

    [Serializable]
    public class BallisticUnityEvent : UnityEvent<RaycastHit> {

    }

    public interface IHandleBallisticHit {
        void OnHit(RaycastHit hit);
    }
}