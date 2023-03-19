using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hinos.mercenary {
    public class DeadEyeView : MonoBehaviour {
        [SerializeField] private float modifiedTimeScale = 0.05f;
        [SerializeField] private float changeTime = 0.5f;

        private bool toggleTime;
        private float defaultTimeScale;
        private float defaultFixedTime;
        private float finalTimeScale;
        private float initialTimeScale;

        private bool keyPressed = false;

        private void Awake() {
            toggleTime = false;
            defaultTimeScale = Time.timeScale;
            defaultFixedTime = Time.fixedDeltaTime;
        }

        private void Start() {
            finalTimeScale = defaultTimeScale;
        }

        private void Update() {
            ProcessInput();
            
            if(keyPressed) {
                toggleTime = !toggleTime;
                initialTimeScale = GetTimeScale(!toggleTime);
                finalTimeScale = GetTimeScale(toggleTime);
            }

            var lerpTime = CalculateProgress(Time.timeScale, initialTimeScale, finalTimeScale) * changeTime + Time.unscaledDeltaTime;
            var lerpPercent = lerpTime / changeTime;
            
            Time.timeScale = Mathf.Lerp(initialTimeScale, finalTimeScale, lerpPercent);
            Time.fixedDeltaTime = defaultFixedTime * Time.timeScale;
        }

        private void ProcessInput() {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;
            keyPressed = keyboard.spaceKey.wasPressedThisFrame;
        }

        public float CalculateProgress(float current, float initial, float final) {
            return (current - initial) / (final - initial);
        }

        public float GetTimeScale(bool value) {
            return (value) ? modifiedTimeScale : defaultTimeScale;
        }
    }
}
