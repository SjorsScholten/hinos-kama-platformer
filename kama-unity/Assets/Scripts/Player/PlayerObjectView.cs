using System;
using UnityEngine;

namespace hinos.player {
    public class PlayerObjectView : MonoBehaviour {
        [SerializeField] private GameObject _characterObject;

        private PlayerCharacterInstance characterInstance;
        private PlayerInputController inputController;

        // Input
        private float _moveInput;
        private bool _jumpPressed;

        public GameObject CharacterObject {
            get => _characterObject;
            set => SetCharacterObject(value);
        }

        private void Awake() {
            Initialize();
        }

        public void Update() {
            PollInput();

            ProcessMoveInput();
            ProcessJumpInput();
        }

        public void Initialize() {
            if(_characterObject == null) {
                throw new NullReferenceException("Player can not be initialize without character object");
            }
            
            characterInstance = new PlayerCharacterInstance(_characterObject);
            inputController = new PlayerInputController(characterInstance);
        }

        private void PollInput() {
            _moveInput = Input.GetAxisRaw("Horizontal");
            _jumpPressed = Input.GetKeyDown(KeyCode.Space);
        }

        private void ProcessMoveInput() {
            inputController.HandleMoveInput(_moveInput);
        }

        private void ProcessJumpInput() {
            inputController.HandleJumpInput(_jumpPressed);
        }

        public void SetCharacterObject(GameObject characterObject) {
            _characterObject = characterObject;
            Initialize();
        }
    }
}
