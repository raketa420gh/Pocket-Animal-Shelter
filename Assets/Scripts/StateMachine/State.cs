public abstract class State<T> : BaseState
{
    protected T _stateMachineController;
    
    public T StateMachineController => _stateMachineController;

    public State(T stateMachineController)
    {
        _stateMachineController = stateMachineController;
    }
}