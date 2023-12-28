using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] private AnimalCarrier _animalCarrier;
    [SerializeField] private VisitorView _view;
    private VisitorStateMachineController _stateMachineController;
    private IFactory _factory;
    
    private Coroutine _movementCoroutine;
    private Vector3 _movementTarget;
    private SimpleCallback _movementCallback;
    private bool _isRecalculationRequired;
    private bool _isMoving;
    private Vector3 _spawnPosition;
    private Vector3 _placePosition;

    public Vector3 SpawnPosition => _spawnPosition;
    public Vector3 PlacePosition => _placePosition;
    public IAnimalCarrier AnimalCarrier => _animalCarrier;

    public void Initialize(IFactory factory)
    {
        _factory = factory;
        
        _view.SetRandomMesh();
        
        _animalCarrier.Initialize(_factory);
        _animalCarrier.OnChangeCarryingState += HandleChangeCarryingState;
        
        _stateMachineController = new VisitorStateMachineController();
        _stateMachineController.Initialise(this, VisitorStateMachineController.State.Waiting);
    }

    public void Destroy()
    {
        _animalCarrier.OnChangeCarryingState -= HandleChangeCarryingState;
    }
    
    #region Movement
        
    public void SetTargetPosition(Vector3 target, SimpleCallback callback)
    {
        _isRecalculationRequired = true;
        _movementTarget = target;
        _movementCallback = callback;

        if (!_isMoving)
            _movementCoroutine = StartCoroutine(MovingCoroutine());
    }

    private IEnumerator MovingCoroutine()
    {
        _isMoving = true;
        _navMeshAgent.enabled = true;

        _view.Animator.SetRunAnimation(true);

        do
        {
            if (_isRecalculationRequired)
            {
                _navMeshAgent.SetDestination(_movementTarget);
            }

            if (!_navMeshAgent.pathPending && !_navMeshAgent.hasPath)
            {
                _navMeshAgent.SetDestination(_movementTarget);
            }

            yield return null;
        }
        while (Vector3.Distance(transform.position, _movementTarget) > _navMeshAgent.stoppingDistance);

        _movementCallback?.Invoke();

        _isMoving = false;
        _navMeshAgent.enabled = false;

        _view.Animator.SetRunAnimation(false);
    }

    public void StopMovement()
    {
        if (!_isMoving)
            return;

        _isMoving = false;
        _navMeshAgent.enabled = false;


        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);

        _movementTarget = transform.position;
        _movementCallback = null;

        _view.Animator.SetRunAnimation(false);
    }
    
    #endregion
    
    private void HandleChangeCarryingState(bool isCarrying)
    {
        if (isCarrying)
            _view.EnableHands();
        else
            _view.DisableHands();
    }
}