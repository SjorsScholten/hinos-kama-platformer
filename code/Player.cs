public class PlayerAnimationHandler {
    private readonly Animator animator;

    private readonly int isGroundedAnimHash;
    private readonly int movementAnimHash;

    public PlayerAnimationHandler(Animator animator){
        this.animator = animator;

        isGroundedAnimHash = Animator.StringToHash("isGrounded");
        movementAnimHash = Animator.StringToHash("movement");
    }

    public void SetGrounded(bool value) {
        animator.SetBool(isGroundedAnimHash, value);
    }

    public void SetMovement(float value) {
        animator.SetFloat(movementAnimHash, value);
    }
}

public class PlayerInputHandler {
    private float moveInputAxis;

    public float MoveInputAxis { get => moveInputAxis; }

    public PlayerInputHandler() {

    }

    public void PollInput() {
        moveInputAxis = Input.GetAxisRaw("Horizontal");
    }
}

public class PlayerController : MonoBehaviour, IStateMachine<PlayerState> {
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;

    private Vector2 velocity;
    private Rigidbody2D body;

    private PlayerState currentState;
    private PlayerStateFactory stateFactory;

    private PlayerInputHandler inputHandler;
    private PlayerAnimationHandler animationHandler;

    public PlayerInputHandler Input { get => inputHandler; }
    public PlayerAnimationHandler Animation { get => animationHandler; }

    private void Awake() {
        animator = GetComponent<Animator>();
        animationHandler = new PlayerAnimationHandler(animator);

        stateFactory = new stateFactory(this);
        currentState = stateFactory.GetIdleState();
    }

    private void Update() {
        inputHandler.PollInput();
        currentState.Update();
    }

    private void FixedUpdate() {
        velocity = body.velocity;

        velocity.x = PlayerInputHandler.MoveInputAxis * moveSpeed;

        body.velocity = velocity;
    }

    public void SwitchState(PlayerState newState) {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}

