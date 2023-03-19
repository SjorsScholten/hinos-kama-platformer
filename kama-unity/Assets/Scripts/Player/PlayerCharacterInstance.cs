using hinos.character;
using hinos.utility;
using UnityEngine;

namespace hinos.player {
    public class PlayerCharacterInstance : InstanceWrapper {

        // Components
        private CharacterMovement2D _movementComponent;
        private CharacterJump2D _jumpComponent;

        public CharacterMovement2D MovementComponent {
            get => _movementComponent;
        }

        public CharacterJump2D JumpComponent {
            get => _jumpComponent;
        }

        public PlayerCharacterInstance(GameObject sourceObject) : base(sourceObject) { }

        protected override void ProcessGetComponents() {
            _movementComponent = GetComponentOnSource<CharacterMovement2D>();
            _jumpComponent = GetComponentOnSource<CharacterJump2D>();
        }
    }
}
