using System;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] private VisitorView _view;

    private void Awake()
    {
        _view.SetRandomMesh();
    }

    public void Initialize()
    {
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