using System;
using UnityEngine;

namespace hinos.character {
    public class CharacterFire : MonoBehaviour {
        [SerializeField] private float timeBetweenShots;
        private float timeSinceLastShot = Mathf.Infinity;

        [SerializeField] private float chargeTime;
        private float holdTime;
        private bool triggerHeld;
        private bool isCharged;

        private void Update() {
            timeSinceLastShot += Time.deltaTime;

            if(triggerHeld) {
                holdTime += Time.deltaTime;
                isCharged = holdTime > chargeTime;
            }
        }

        private void ProcessCharge() {
            isCharged = holdTime > chargeTime;
        }

        private void ProcessReleaseTrigger() {
            if(timeSinceLastShot < timeBetweenShots) return;

            if(!isCharged) {
                FireShot();
            }
            else {
                FireChargedShot();
            }

            holdTime = 0;
            timeSinceLastShot = 0;
        }

        private void FireShot() {

        }

        private void FireChargedShot() {

        }

        public void HandleFire(bool holdFire) {
            triggerHeld |= holdFire;
        }
    }
}
