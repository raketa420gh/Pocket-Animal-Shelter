using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] private VisitorView _view;
    private VisitorStateMachineController _stateMachineController;

    private void Awake()
    {
        _view.SetRandomMesh();
    }

    public void Initialize()
    {
        _stateMachineController = new VisitorStateMachineController();
        _stateMachineController.Initialise(this, VisitorStateMachineController.State.Waiting);
    }

    public void Destroy()
    {
    }
    
    private void HandleChangeCarryingState(bool isCarrying)
    {
        if (isCarrying)
            _view.EnableHands();
        else
            _view.DisableHands();
    }
}