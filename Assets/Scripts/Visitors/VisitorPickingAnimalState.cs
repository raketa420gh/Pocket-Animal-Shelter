using UnityEngine;
using UnityEngine.AI;

public class VisitorPickingAnimalState : VisitorState
{
    private readonly VisitorStateMachineController _stateMachineController;

    //private static readonly int HEART_PARTICLE_HASH = ParticlesController.GetHash("Heart");
    private const float PATH_RECALCULATION_DELAY = 0.3f;

    private AnimalBehaviour _animal;
    private NavMeshAgent navMeshAgent;
    private Transform transform;

    private float pathRecalculationTime;
    private bool isAnimalPicked = false;

    public VisitorPickingAnimalState(VisitorStateMachineController stateMachineController) : base(stateMachineController)
    {
        _stateMachineController = stateMachineController;
    }

    public override void OnStateRegistered()
    {
        //navMeshAgent = _stateMachineController.StateMachineParent.NavMeshAgent;
        transform = _stateMachineController.StateMachineParent.transform;
    }

    public override void OnStateActivated()
    {
        if (_animal == null)
        {
            Debug.LogError("Animal isn't linked!");

            _stateMachineController.SetState(VisitorStateMachineController.State.Waiting);

            return;
        }

        isAnimalPicked = false;

        RecalculatePath();
    }

    private void RecalculatePath()
    {
        /*_stateMachineController.StateMachineParent.SetTargetPosition(animalBehaviour.transform.position, delegate
        {
            isAnimalPicked = true;

            ParticlesController.PlayParticle(HEART_PARTICLE_HASH).SetPosition(transform.position + new Vector3(0, 4, 0));

            animalBehaviour.PickAnimal(stateMachineController.ParentBehaviour);
            animalBehaviour.OnAnimalCuredAndPicked();

            Tween.DelayedCall(0.5f, delegate
            {
                stateMachineController.SetState(VisitorStateMachineController.State.Leaving);
            });
        });

        pathRecalculationTime = Time.time + PATH_RECALCULATION_DELAY;*/
    }

    public override void OnStateDisabled()
    {
        _animal = null;
    }

    public override void Update()
    {
        if (isAnimalPicked)
            return;

        if (Time.time > pathRecalculationTime)
            RecalculatePath();
    }

    public void SetAnimal(AnimalBehaviour animalBehaviour)
    {
        _animal = animalBehaviour;
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }
}