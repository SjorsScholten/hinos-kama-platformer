using hinos.movement;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IStateMachine<PlayerController> {
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

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
    private bool jumpInputPressed;
    private bool jumpInputHeld;

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
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private int airJumps = 0;
    private float jumpTime;
    private int jumpCounter;

    private SurfaceDetection2D surface;
    private Rigidbody2D body;
    #endregion

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        surface = GetComponent<SurfaceDetection2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        mainCamera = Camera.main;

        isGroundedAnimHash = Animator.StringToHash("isGrounded");
        isMovingAnimHash = Animator.StringToHash("walking");
        movementAnimHash = Animator.StringToHash("movement");

        stateFactory = new PlayerStateFactory(this);
        currentState = stateFactory.GetIdleState();
    }

    private void Update() {
        ProcessInput();
        
        if(moveInputAxis != 0) 
            spriteRenderer.flipX = moveInputAxis < 0;

        currentState.Update();
    }

    private void FixedUpdate() {
        ProcessJump();
        ProcessMovement();
    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 100), "Player");
        GUILayout.Label($"State: {currentState.name}");
        //GUILayout.Label($"FloorNormal({groundContactCount}): {contactNormal}");
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
        jumpInputPressed |= Input.GetButtonDown("Jump");
        jumpInputHeld = Input.GetButton("Jump");
    }

    private void ProcessMovement() {
        var velocity = body.velocity;
        var desiredSpeed = moveInputAxis * walkSpeed;

        velocity += Movement.CalculateMovementOnAxis2D(velocity, desiredSpeed, acceleration, Vector2.right, surface.ContactNormal);

        body.velocity = velocity;
    }

    private void ProcessJump() {
        var velocity = body.velocity;

        if (jumpInputPressed) {
            Vector2 jumpDirection = Vector2.zero;

            if (surface.ContactCount > 0) {
                jumpDirection = surface.ContactNormal;
            }
            else {
                return;
            }

            jumpInputPressed = false;

            var jumpSpeed = MathF.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            var alignedSpeed = Vector2.Dot(velocity, jumpDirection);

            if (velocity.y > 0) {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);
            }

            velocity += jumpDirection * jumpSpeed;
        }

        body.velocity = velocity;
    }

    public void AnimatorSetGrounded(bool value) {
        animator.SetBool(isGroundedAnimHash, value);
    }

    public void AnimatorSetMoving(bool value) {
        animator.SetBool(isMovingAnimHash, value);
    }
}
