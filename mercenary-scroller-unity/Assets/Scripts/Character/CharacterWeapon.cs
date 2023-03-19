using System;
using UnityEngine;

namespace hinos.character {
    public class CharacterWeapon : MonoBehaviour {
        [SerializeField] private Transform _aimOrigin;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private float timeBetweenShots;
        private float timeSinceLastShot = Mathf.Infinity;

        [SerializeField] private GameObject bulletPrefab;

        private Vector3 _aimDirection = Vector3.right;

        private void Update() {
            timeSinceLastShot += Time.deltaTime;

            var maxChange = _rotateSpeed * Time.deltaTime;
            ProcessAim(maxChange);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_aimOrigin.position + _aimDirection, 0.1f);
        }

        private void ProcessAim(float maxChange) {

        }

        private void ProcessReleaseTrigger() {
            timeSinceLastShot = 0;
        }

        private void FireShot() {

        }

        public void HandleAim(Vector3 direction) {
            _aimDirection = direction;
        }

        public void HandleFire(bool holdFire) {

        }
    }
}