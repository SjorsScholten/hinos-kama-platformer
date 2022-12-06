
public interface IStateMachine<T> where T : IStateMachine<T> {
    void HandleSwitchState(State<T> oldState, State<T> newState);
}
public abstract class State<T> where T : IStateMachine<T> {
    public readonly string name;
    public readonly T source;
    public State(T source, string name) {
        this.source = source;
        this.name = name;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();

    protected void SwitchState(State<T> newState) {
        source.HandleSwitchState(this, newState);
    }
}
