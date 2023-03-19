using System;
using UnityEngine;

namespace hinos.character {
    public class CharacterHealth : MonoBehaviour {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;

        public event Action OnHealthChangedEvent;

        public int Health {
            get => _currentHealth;
            set => SetHealth(value);
        }

        public int MaxHealth {
            get => _maxHealth;
        }

        public void SetHealth(int value) {
            _currentHealth = value;
            OnHealthChangedEvent.Invoke();
        }

        [ContextMenu("Invoke Events")]
        public void InvokeEvents() {
            OnHealthChangedEvent.Invoke();
        }
    }
}