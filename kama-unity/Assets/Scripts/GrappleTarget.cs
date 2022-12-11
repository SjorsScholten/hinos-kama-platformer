using UnityEngine;

public class GrappleTarget : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private float range = 10f;
    [SerializeField] private float speed = 5f;

    private Vector2 throwingDirection;
    private bool thrown;

    private Transform trans;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private Vector2 velocity;

    private void Awake() {
        trans = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void Update() {
        if(thrown) {
            var distance = Vector2.Distance(player.position, trans.position);
            if(distance > range) {
                // Return sword to player
                Disable();
                return;
            }
            DrawLine();
        }
    }

    private void FixedUpdate() {
        if (thrown)
            body.velocity = throwingDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Disable();
    }

    private void Enable() {
        thrown = true;
        spriteRenderer.enabled = true;
        body.simulated = true;
        lineRenderer.enabled = true;
    }

    private void Disable() {
        thrown = false;
        spriteRenderer.enabled = false;
        body.simulated = false;
        lineRenderer.enabled = false;
    }

    public void Throw(Vector2 origin, Vector2 direction) {
        transform.position = origin;
        throwingDirection = direction;
        Enable();
    }

    private void DrawLine() {
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, trans.position);
    }
}