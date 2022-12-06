using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerController> {
    protected PlayerStateFactory factory;
    public PlayerState(PlayerController source, string name, PlayerStateFactory factory) : base(source, name) {
        this.factory = factory;
    }
}

public class PlayerStateFactory {
    private readonly IdlePlayerState idlePlayerState;
    private readonly MovingPlayerState movingPlayerState;
    public PlayerStateFactory(PlayerController source) {
        idlePlayerState = new IdlePlayerState(source, this);
        movingPlayerState = new MovingPlayerState(source, this);
    }

    public IdlePlayerState GetIdleState() => idlePlayerState;
    public MovingPlayerState GetMovingState() => movingPlayerState;
}

public abstract class GroundedPlayerState : PlayerState {

    public GroundedPlayerState(PlayerController source, string name, PlayerStateFactory factory) : base(source, name, factory) {

    }

    public override void Enter() {
        //source.AnimatorSetGrounded(true);
    }

    public override void Exit() {
        
    }

    public override void Update() {
        
    }
}

public class IdlePlayerState : GroundedPlayerState {

    public IdlePlayerState(PlayerController source, PlayerStateFactory factory) : base(source, "Idle", factory) {

    }

    public override void Enter() {
        base.Enter();
        source.AnimatorSetMoving(false);
    }

    public override void Update() {
        base.Update();

        if (source.MoveInputAxis != 0) {
            SwitchState(factory.GetMovingState());
        }
    }
}

public class MovingPlayerState : GroundedPlayerState {

    public MovingPlayerState(PlayerController source, PlayerStateFactory factory) : base(source, "Moving", factory) {

    }

    public override void Enter() {
        base.Enter();

        source.AnimatorSetMoving(true);
    }

    public override void Update() {
        base.Update();

        source.MoveTowards(source.MoveInputAxis, source.WalkSpeed);

        if(source.MoveInputAxis == 0) {
            SwitchState(factory.GetIdleState());
        }
    }
}