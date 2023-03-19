namespace hinos.player {
    public class PlayerInputController {
        private PlayerCharacterInstance _source;

        public PlayerInputController(PlayerCharacterInstance source) {
            _source = source;
        }

        public void HandleMoveInput(float moveAxis) {
            _source.MovementComponent.HandleMove(moveAxis);
        }

        public void HandleJumpInput(bool jumpPressed) {
            if(jumpPressed)
                _source.JumpComponent.HandleJump();
        }
    }
}
