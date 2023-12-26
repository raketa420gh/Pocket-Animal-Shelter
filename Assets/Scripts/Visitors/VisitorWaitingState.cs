using UnityEngine;

public class VisitorWaitingState : VisitorState
{
    private readonly VisitorStateMachineController _stateMachineController;

    public VisitorWaitingState(VisitorStateMachineController stateMachineController) : base(stateMachineController)
    {
        _stateMachineController = stateMachineController;
    }

    public override void OnStateRegistered()
    {
    }

    public override void OnStateActivated()
    {
        //_stateMachineController.StateMachineParent.StopMovement();
    }

    public override void OnStateDisabled()
    {
    }

    public override void Update()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }
}