public abstract class PlayerState : IState {
    protected PlayerController source;
    protected PlayerStateFactory factory;

    public PlayerState(PlayerController source){
        this.source = source;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}

public class PlayerStateFactory {
    private readonly PlayerIdleState playerIdleState;
    
    public PlayerStateFactory(PlayerController source) {
        playerIdleState = new PlayerIdleState(source, this);
    }

    public PlayerIdleState GetIdleState() => playerIdleState;
}

public class PlayerGroundedState : PlayerIdleState {

    public PlayerGroundedState(PlayerController source, PlayerStateFactory factory) : base(source, factory){

    }

    public override void Enter() {
        source.Animation.SetGrounded(true);
    }

    public override void Exit() {

    }

    public override void Update() {

    }
}

public class PlayerAfloatState : PlayerIdleState {

    public PlayerAfloatState(PlayerController source, PlayerStateFactory factory) : base(source, factory) {

    }

    public override void Enter() {
        source.Animation.SetGrounded(false);
    }

    public override void Exit() {

    }

    public override void Update() {

    }
}

public class PlayerIdleState : PlayerGroundedState {

    public PlayerIdleState(PlayerController source) : base(source) {

    }

    public override void Enter() {
        base.Enter();
        source.Animation.SetMovement(0.0f);
    }

    public override void Exit() {

    }

    public override void Update() {

    }
}

public class PlayerMovingState : PlayerGroundedState {

    public PlayerMovingState(PlayerController source, PlayerStateFactory factory) : base(source, factory) {
        
    }

    public override void Enter() {
        base.Enter();
        source.Animation.SetMovement(0.1f);
    }

    public override void Exit() {

    }

    public override void Update() {
        source.moveSpeed = source.PlayerInputHandler.MoveInputAxis < 0.5f ? source.walkSpeed : source.runSpeed;
    }
}