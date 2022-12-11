using System;
using UnityEngine;
public class PlayerController : MonoBehaviour, IStateMachine<PlayerController> {
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Grapple Settings")]
    [SerializeField] private float grappleRange;
    [SerializeField] private float grappleSpeed;
    [SerializeField] private Transform grappleDirectionClue;
    private Vector2 grappleDirection;
    private Vector2 grappleOrigin;
    private Camera mainCamera;

    [SerializeField] private GrappleTarget grappleTarget;

    private new Collider2D collider;

    #region Input
    private float moveInputAxis;
    private bool jumpInput;

    public float MoveInputAxis { get => moveInputAxis; }
    #endregion

    #region Animation
    private Animator animator;

    private int isGroundedAnimHash;
    private int isMovingAnimHash;
    private int movementAnimHash;
    #endregion

    private PlayerStateFactory stateFactory;
    private State<PlayerController> currentState;

    #region Physics
    [Header("Physics Settings")]
    [SerializeField] private float maxGroundAngle = 25.0f;

    private Rigidbody2D body;

    private int stepsSinceLastGrounded;
    private int groundContactCount;
    private Vector2 contactNormal;
    private float minGroundDotProduct;

    private float hMove;
    private Vector2 velocity;

    public bool OnGround => groundContactCount > 0;
    public float WalkSpeed { get => walkSpeed; }
    public Vector2 Velocity { get => velocity; }
    #endregion

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        mainCamera = Camera.main;

        isGroundedAnimHash = Animator.StringToHash("isGrounded");
        isMovingAnimHash = Animator.StringToHash("walking");
        movementAnimHash = Animator.StringToHash("movement");

        stateFactory = new PlayerStateFactory(this);
        currentState = stateFactory.GetIdleState();

        minGroundDotProduct = MathF.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Update() {
        ProcessInput();
        
        if(moveInputAxis != 0) 
            spriteRenderer.flipX = moveInputAxis < 0;

        var worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var origin = collider.bounds.center;
        grappleDirection = worldMousePos - origin;
        grappleOrigin = origin;
        if (Input.GetKeyDown(KeyCode.Z)) {
            grappleTarget.Throw(origin, grappleDirection.normalized);
        }

        currentState.Update();
    }

    private void FixedUpdate() {
        Debug.DrawRay(grappleOrigin, grappleDirection, Color.red, Time.fixedDeltaTime);
        UpdatePhysicsState();

        velocity.x = hMove;

        if(jumpInput) {
            ProcessJump();
        }

        body.velocity = velocity;

        ClearPhysicsState();
    }

    private void OnCollisionStay2D(Collision2D collision) {
        for(int i = 0; i < collision.contactCount; ++i) {
            var normal = collision.GetContact(i).normal;
            if(normal.y >= minGroundDotProduct) {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 100), "Player");
        GUILayout.Label($"State: {currentState.name}");
        GUILayout.Label($"FloorNormal({groundContactCount}): {contactNormal}");
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width - 310, 10, 300, 100), "Grapple");
        GUILayout.Space(5);
        GUILayout.Label($"Direction: {grappleDirection}");
        GUILayout.EndArea();
    }

    public void HandleSwitchState(State<PlayerController> oldState, State<PlayerController> newState) {
        oldState.Exit();
        newState.Enter();
        currentState = newState;
    }

    private void ProcessInput() {
        moveInputAxis = Input.GetAxisRaw("Horizontal");
        jumpInput |= Input.GetButtonDown("Jump");
    }

    private void UpdatePhysicsState() {
        stepsSinceLastGrounded += 1;

        if (OnGround) {
            stepsSinceLastGrounded = 0;
            if (groundContactCount > 1)
                contactNormal.Normalize();
        }
        else {
            contactNormal = Vector2.up;
        }

        velocity = body.velocity;
    }

    private void ClearPhysicsState() {
        groundContactCount = 0;
        contactNormal = Vector2.zero;
    }

    private void ProcessMovement() {
        
    }

    private void ProcessJump() {
        if (OnGround) {
            jumpInput = false;
            velocity.y += Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        }
    }

    public void AnimatorSetGrounded(bool value) {
        animator.SetBool(isGroundedAnimHash, value);
    }

    public void AnimatorSetMoving(bool value) {
        animator.SetBool(isMovingAnimHash, value);
    }

    public void MoveTowards(float direction, float speed) {
        hMove = direction * speed;
    }
}
