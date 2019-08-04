using UnityEngine;
[System.Serializable]
public class StateMachine<T> {
    public State<T> currentState { get; private set; }
    public T owner;
    public StateMachine(T owner) {
        this.owner = owner;
        currentState = null;
    }
    public void ChangeState(State<T> newState) {
        if (currentState != null) currentState.ExitState(owner); 
        currentState = newState;
        currentState.EnterState(owner);
    }
    public void Update() {
        if (currentState != null) {
            currentState.UpdateState(owner);
        }
    }
}
