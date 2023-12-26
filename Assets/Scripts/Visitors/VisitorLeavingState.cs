using UnityEngine;

public class VisitorLeavingState : VisitorState
{
    private readonly VisitorStateMachineController _stateMachineController;

    public VisitorLeavingState(VisitorStateMachineController stateMachineController) : base(stateMachineController)
    {
        _stateMachineController = stateMachineController;
    }

    public override void OnStateRegistered()
    {

    }

    public override void OnStateActivated()
    {
        /*stateMachineController.ParentBehaviour.SetTargetPosition(stateMachineController.ParentBehaviour.SpawnPosition, delegate
        {
            stateMachineController.ParentBehaviour.DisableVisitor();
        });*/
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