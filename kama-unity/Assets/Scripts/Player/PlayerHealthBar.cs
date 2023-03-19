using hinos.character;
using hinos.util;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace hinos.player {
    public class PlayerHealthBar : MonoBehaviour {
        [SerializeField] private CharacterHealth healthComponent;
        [SerializeField] private ProgressBar progressBar;

        private void Awake() {
            UpdateProgressBar();
        }

        private void OnEnable() {
            healthComponent.OnHealthChangedEvent += OnHealthChanged;
        }

        private void OnDisable() {
            healthComponent.OnHealthChangedEvent -= OnHealthChanged;
        }

        private void UpdateProgressBar() {
            progressBar.UpperBound = healthComponent.MaxHealth;
            progressBar.LowerBound = 0f;
            progressBar.Value = healthComponent.Health;
        }

        private void OnHealthChanged() {
            UpdateProgressBar();
        }
    }
}
