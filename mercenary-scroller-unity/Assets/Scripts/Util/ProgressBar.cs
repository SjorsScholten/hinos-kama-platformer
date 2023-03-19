using System;
using UnityEngine;
using UnityEngine.UI;

namespace hinos.util {
    [RequireComponent(typeof(Image), typeof(Mask))]
    public class ProgressBar : MonoBehaviour {
        [SerializeField] private float _value;
        [SerializeField] private float _upperBound;
        [SerializeField] private float _lowerBound;

        [SerializeField] private float _smoothTime;

        private float _smoothVel;
        private float _targetFill;
        
        private float _offset;
        private float _maxOffset;
        private bool isDirty;

        private Image imageMask;

        public float Value {
            get => _value;
            set => SetValue(value);
        }

        public float UpperBound {
            get => _upperBound;
            set => SetUpperBound(value);
        }

        public float LowerBound {
            get => _lowerBound;
            set => SetLowerBound(value);
        }

        private void Awake() {
            imageMask = GetComponent<Image>();
        }

        private void Update() {
            imageMask.fillAmount = Mathf.SmoothDamp(imageMask.fillAmount, GetTargetFill(), ref _smoothVel, _smoothTime);
        }

        public void SetValue(float value) {
            _value = value;
            SetOffset(_value, _lowerBound);
        }

        public void SetUpperBound(float value) {
            _upperBound = value;
            SetMaxOffset(_upperBound, _lowerBound);
        }

        public void SetLowerBound(float value) {
            _lowerBound = value;
            SetOffset(_value, _lowerBound);
            SetMaxOffset(_upperBound, _lowerBound);
        }

        public void SetOffset(float value, float lowerBound) {
            _offset = value - lowerBound;
            isDirty = true;
        }

        public void SetMaxOffset(float upperBound, float lowerBound) {
            _maxOffset = Mathf.Max(upperBound - lowerBound, 0.01f);
            isDirty = true;
        }

        public float GetTargetFill() {
            if(isDirty) {
                _targetFill = Mathf.Clamp01(_offset / _maxOffset);
                isDirty = false;
            }

            return _targetFill;
        }
    }
}
