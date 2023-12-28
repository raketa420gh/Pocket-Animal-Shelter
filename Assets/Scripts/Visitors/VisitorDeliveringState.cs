using UnityEngine;

public class VisitorDeliveringState : VisitorState
{
    private readonly VisitorStateMachineController _stateMachineController;
    private Transform _transform;

    public VisitorDeliveringState(VisitorStateMachineController stateMachineController) : base(stateMachineController)
    {
        _stateMachineController = stateMachineController;
    }

    public override void OnStateRegistered()
    {
        _transform = _stateMachineController.StateMachineParent.transform;
    }

    public override void OnStateActivated()
    {
        _stateMachineController.StateMachineParent.SetTargetPosition(_stateMachineController.StateMachineParent.PlacePosition, delegate
        {
            _stateMachineController.StateMachineParent.StopMovement();

            /*_stateMachineController.StateMachineParent.PlaceAnimalOnGround();

            Tween.NextFrame(delegate
            {
                _stateMachineController.SetState(VisitorStateMachineController.State.Leaving);
            });*/
        });
    }

    public override void OnStateDisabled()
    {

    }

    public override void Update()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag(PhysicsHelper.TAG_PLAYER) || other.CompareTag(PhysicsHelper.TAG_NURSE))
        {
            stateMachineController.ParentBehaviour.StopMovement();

            if (rotationTweenCase != null && !rotationTweenCase.isCompleted)
                rotationTweenCase.Kill();

            rotationTweenCase = _transform.DOLookAt(other.transform.position, 0.15f);
        }*/
    }

    public override void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag(PhysicsHelper.TAG_PLAYER) || other.CompareTag(PhysicsHelper.TAG_NURSE))
        {
            stateMachineController.ParentBehaviour.SetTargetPosition(stateMachineController.ParentBehaviour.PlacePosition, delegate
            {
                stateMachineController.ParentBehaviour.PlaceAnimalOnGround();
            });
        }*/
    }
}