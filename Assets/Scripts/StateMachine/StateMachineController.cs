using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineController<K, T> where K : MonoBehaviour
{
    private List<BaseState> _states;
    private Dictionary<T, int> _statesLink;
    private int _statesCount;
    private K _stateMachineParent;
    private T _currentState;
    private BaseState _activeState;

    public K StateMachineParent => _stateMachineParent;
    public T CurrentState => _currentState;
    public BaseState ActiveState => _activeState;

    public void Initialise(K stateMachineParent, T defaultState)
    {
        _stateMachineParent = stateMachineParent;
        _states = new List<BaseState>();
        _statesLink = new Dictionary<T, int>();
        _statesCount = 0;
        RegisterStates();
        SetState(defaultState);
    }

    public void SetState(T state)
    {
        if (!_currentState.Equals(state))
        {
            if (_activeState != null)
                _activeState.OnStateDisabled();

            _currentState = state;
            _activeState = _states[_statesLink[_currentState]];
            _activeState.OnStateActivated();
        }
    }

    public BaseState GetState(T state)
    {
        return _states[_statesLink[state]];
    }

    protected abstract void RegisterStates();

    protected void RegisterState(BaseState state, T tState)
    {
        _states.Add(state);
        _statesLink.Add(tState, _statesCount);
        _statesCount++;

        state.OnStateRegistered();
    }
}