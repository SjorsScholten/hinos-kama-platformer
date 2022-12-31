[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour {
    [SerializeField, Min(0.01f)] private float length;
    [SerializeField, Min(0)] private int detail;
    [SerializeField] private int passes;

    private Vector3[] pointPositions;
    private Vector3[] previousPointPositions;
    private bool[] staticPoints;

    private Tuple<Vector3, Vector3>[] edges;
    private float edgeLength;

    private Coroutine LengthRoutine;

    private LineRenderer lineRenderer

    public float Length {
        get => length;
        set {
            StopCoroutine(LengthRoutine);
            length = value;
            edgeLength = length / edges.Length;
        }
    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();

        pointPositions = new Vector3[2 + detail];
        previousPoints = new Vector3[pointPositions.Length];
        static = new bool[pointPositions.Length];
        
        edges = new Tuple<Vector3, Vector3>[pointPositions.Length - 1];
        edgeLength = length / edges.Length;

        for(var i = 0; i < edges.length; ++i){
            edges[i] = Tuple.Create(pointPositions[i], pointPositions[i + 1]);
        }

        lineRenderer.pointCount = pointPositions.length;
    }

    private void Start() {
        ResetPoints();
    }

    private void Update(){
        // Verlet Integration
        for(var i = 1; i < pointPositions.length; ++i) {
            if(staticPoints[i]) continue;

            var cachedPosition = pointPositions[i];
            pointPositions[i] += pointPositions[i] - previousPointPositions[i];
            previousPointPositions[i] = cachedPosition;
        }

        // Jacobsen Method
        for(var i = 0; i < passes; ++i) {
            foreach(var edge in edges) {
                var direction = (edge.Item2 - edge.Item1).normalized;
                var error = Vector2.distance(edge.Item1, edge.Item2) - edgeLength;
                edge.Item1 += direction * error * 0.5f;
                edge.Item2 -= direction * error * 0.5f;
            }
        }

        lineRenderer.SetPositions(pointPositions);
    }

    public void ResetPoints() {
        for(var i = 1; i < pointPositions.Length; ++i) {
            pointPositions[i] = pointPositions[i - 1] + Vector3.right * edgeLength;
            previousPointPositions[i] = pointPositions[i];
        }
    }

    public void SetPoint(int index, Vector3 position, bool static) {
        previousPointPositions[index] = pointPositions[index] = position;
        staticPoints[index] = static;
    }

    public void SetLengthOverTime(float value, float duration) {
        StopCoroutine(LengthRoutine);
        LengthRoutine = StartCoroutine(LengthOverTimeRoutine(value, duration));
    }

    private IEnumerator LengthOverTimeRoutine(float targetLength, float duration) {
        var initialLength = length;
        var initialEdgeLength = edgeLength;
        var targetEdgeLength = targetLength / edges.Length;

        float time = 0;
        while(time < duration) {
            time += Time.deltaTime;

            var value = time / duration;
            length = Mathf.Lerp(initialLength, targetLength, value);
            edgeLength = Mathf.Lerp(initialEdgeLength, targetEdgeLength, value);

            yield return null;
        }

        length = targetLength;
        edgeLength = targetEdgeLength;
    }
}

[RequireComponent(typeof(Rope))]
public class BeltController : MonoBehaviour, IStateMachine<BeltController> {
    [SerializeField] private Transform origin;
    [SerializeField, Min(0.01f)] private float totalLength;

    private BeltStateFactory stateFactory;
    private IState<BeltController> currentState;

    private Transform transform;

    private void Awake() {
        transform = GetComponent<Transform>();

        point[0] = origin.position;
        static[0] = true;
    }

    private void Update() {
        currentState.Update();
    }

    private void CheckForImpact(float distance) {
        var hit = Physics2D.CircleCast(transform.position, radius, throwingDirection, distance);
        if(hit != null) {
            
        }
    }

    public void FireTowards(Vector2 direction) {
        thrown = true;
        throwingDirection = direction;
    }

    public void Retrieve() {

    }
}

public class BeltState : State<BeltController> {
    protected BeltStateFactory factory;

    public BeltState(BeltController source, string name, BeltStateFactory factory) : base(source, name) {
        this.factory = factory;
    }
}

public class BeltStateFactory {
    private readonly IdleBladeState idelState;
    private readonly ThrownBladeState thrownState;

    public BladeStateFactory(BladeController source) {

    }

    public IdleBladeState GetIdleState() => idelState;

    public ThrownBladeState GetThrownState() => thrownState;
}

public class IdleBladeState : BladeState {

    public override void Enter() {
        source.SpriteRenderer.enabled = false;
    }

    public override void Exit() {}

    public override void Update() {
        if(thrown) {
            SwitchState(factory.GetThrownState());
        }
    }
}

public class ThrownBladeState : BladeState {

    public override void Enter() {
        source.SpriteRenderer.enabled = true;
    }

    public override void Exit() {}

    public override void Update() {
        var previousPosition = source.Transform.position;

        source.Move();

        var deltaPosition = source.Transform.position - previousPosition;
        source.CheckImpact(deltaPosition.magnitude);

        if(anchor != null) {
            SwitchState(factory.GetAnchoredState());
        }
    }
}