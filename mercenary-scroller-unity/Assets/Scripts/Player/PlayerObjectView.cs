using hinos.character;
using hinos.utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hinos.player {
    public class PlayerObjectView : MonoBehaviour {
        [SerializeField] private GameObject _characterObject;
        private PlayerCharacterInstance _characterInstance;

        // Input
        private DefaultActions _defaultActions;
        private Vector2 _moveInput;

        private PlayerInputController _inputController;

        private void Awake() {
            _defaultActions = new DefaultActions();

            Initialize();
        }

        private void OnEnable() {
            _defaultActions.Player.Enable();
        }

        private void OnDisable() {
            _defaultActions.Player.Disable();
        }

        private void Update() {
            PollInput();

            ProcessMoveInput();
            ProcessAimInput();
        }

        private void Initialize() {
            if(_characterObject == null) {
                throw new NullReferenceException("Player can not be initialize without character object");
            }

            _characterInstance = new PlayerCharacterInstance(_characterObject);
            _inputController = new PlayerInputController(_characterInstance);
        }

        private void PollInput() {
            _moveInput = _defaultActions.Player.Move.ReadValue<Vector2>();
        }

        private void ProcessMoveInput() {
            _inputController.HandleMoveInput(_moveInput);
        }

        private void ProcessAimInput() {
            _inputController.HandleAimInput(_moveInput);
        }
    }

    public class PlayerInputController {
        private PlayerCharacterInstance _source;

        public PlayerInputController(PlayerCharacterInstance source) {
            _source = source;
        }

        public void HandleMoveInput(Vector2 moveInput) {
            var direction = moveInput.normalized;
            var length = moveInput.magnitude;
            _source.Movement.HandleMove(direction, length);
        }

        public void HandleAimInput(Vector2 aimInput) {
            var direction = (Vector3)aimInput.normalized;
            _source.Weapon.HandleAim(direction);
        }
    }

    public class PlayerCharacterInstance : InstanceWrapper {
        private CharacterMovement _characterMovement;
        private CharacterWeapon _characterWeapon;

        public CharacterMovement Movement {
            get => _characterMovement;
        }

        public CharacterWeapon Weapon {
            get => _characterWeapon;
        }

        public PlayerCharacterInstance(GameObject sourceObject) : base(sourceObject) {

        }

        protected override void ProcessGetComponents() {
            _characterMovement = GetComponentOnSource<CharacterMovement>();
            _characterWeapon = GetComponentOnSource<CharacterWeapon>();
        }
    }
}
