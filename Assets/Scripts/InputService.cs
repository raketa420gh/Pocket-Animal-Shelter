using System;
using UnityEngine;

public class InputService : MonoBehaviour, IInputService
{
    [SerializeField] private Joystick _joystick;

    public Vector2 Axis => _joystick.Direction;
    public Vector3 AxisFormat => new (_joystick.Horizontal, 0, _joystick.Vertical);

    public event Action OnDrop;

    public void Initialize()
    {
        if (!_joystick)
            throw new Exception("Joystick not initialized");
    }
}