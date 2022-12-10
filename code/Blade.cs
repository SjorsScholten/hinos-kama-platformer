public class BladeController : MonoBehaviour, IStateMachine<BladeController> {
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D body;
    private Vector2 velocity;

    private Vector2 throwingDirection;
    private bool thrown;

    private void FixedUpdate() {
        velocity = body.velocity;

        if(thrown) {
            velocity = throwingDirection
        }

        body.velocity = velocity
    }

    private void OnCollisionEnter(Collision collision) {

    }

    private void Move() {

    }

    public void ThrowTowardsDirection(Vector2 direction) {
        thrown = true;
        throwingDirection = direction;
    }

    public void Enable() {
        spriteRenderer.enabled = true;
        body.simulated = true;
    }

    public void Disable() {
        spriteRenderer.enabled = false;
        body.simulated = false;
    }
}

public class BladeState : State<BladeController> {
    protected BladeStateFactory factory;

    public BladeState(BladeController source, string name, BladeStateFactory factory) : base(source, name) {
        this.factory = factory;
    }
}

public class BladeStateFactory {
    private readonly IdleBladeState idelState;
    private readonly ThrownBladeState thrownState;

    public BladeStateFactory(BladeController source) {

    }

    public IdleBladeState GetIdleState() => idelState;

    public ThrownBladeState GetThrownState() => thrownState;
}

public class IdleBladeState : BladeState {

    public override void Enter() {

    }

    public override void Exit() {

    }

    public override void Update() {
        if(thrown) {
            SwitchState(factory.GetThrownState());
        }
    }
}

public class ThrownBladeState : BladeState {

    public override void Enter() {
        Enable();
    }

    public override void Exit() {

    }

    public override void Update() {

    }
}