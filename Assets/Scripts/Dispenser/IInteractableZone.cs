using System;

public interface IInteractableZone
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;
}