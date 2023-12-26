public class VisitorStateMachineController : StateMachineController<Visitor, VisitorStateMachineController.State>
{
    protected override void RegisterStates()
    {
        RegisterState(new VisitorWaitingState(this), State.Waiting);
        RegisterState(new VisitorDeliveringState(this), State.Delivering);
        RegisterState(new VisitorPickingAnimalState(this), State.PickingAnimal);
        RegisterState(new VisitorLeavingState(this), State.Leaving);
    }

    public enum State
    {
        Disabled = 0,
        Waiting = 1,
        Delivering = 2,
        PickingAnimal = 3,
        Leaving = 4
    }
}