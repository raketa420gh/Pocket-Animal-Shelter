using System;
using UnityEngine;

public class InteractableZone : MonoBehaviour, IInteractableZone
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (!playerController)
            return;
        
        OnPlayerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (!playerController)
            return;
        
        OnPlayerExit?.Invoke();
    }
}