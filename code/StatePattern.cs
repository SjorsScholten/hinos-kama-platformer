public interface IStateMachine<T> where T : IState{
    void SwitchState(T newState);
}

public interface IState {
    void Enter();
    void Exit();
    void Update();
}