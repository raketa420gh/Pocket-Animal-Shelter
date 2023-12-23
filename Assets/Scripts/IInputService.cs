using System;
using UnityEngine;

public interface IInputService
{
    event Action OnDrop;
    
    Vector2 Axis { get; }
    Vector3 AxisFormat { get; }

    void Initialize();
}