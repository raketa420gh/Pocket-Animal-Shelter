using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] private AnimalCarrier _animalCarrier;
    [SerializeField] private VisitorView _view;
    private VisitorStateMachineController _stateMachineController;
    private IFactory _factory;

    private void Awake()
    {
        _view.SetRandomMesh();
    }

    public void Initialize(IFactory factory)
    {
        _factory = factory;
        
        _animalCarrier.Initialize(_factory);
        _animalCarrier.OnChangeCarryingState += HandleChangeCarryingState;
        
        _stateMachineController = new VisitorStateMachineController();
        _stateMachineController.Initialise(this, VisitorStateMachineController.State.Waiting);
    }

    public void Destroy()
    {
        _animalCarrier.OnChangeCarryingState -= HandleChangeCarryingState;
    }
    
    private void HandleChangeCarryingState(bool isCarrying)
    {
        if (isCarrying)
            _view.EnableHands();
        else
            _view.DisableHands();
    }
}