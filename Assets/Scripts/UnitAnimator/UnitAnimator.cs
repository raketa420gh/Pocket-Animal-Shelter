using UnityEngine;

public class UnitAnimator : MonoBehaviour, IUnitAnimator
{
    [SerializeField] private Animator _animator;
    private readonly int _runHash = Animator.StringToHash("Run");
    private readonly int _movementMultilierHash = Animator.StringToHash("Movement Multiplier");

    public void SetRunAnimation(bool b)
    {
        _animator.SetBool(_runHash, b);
    }

    public void SetMoveSpeedParameter(float multiplier)
    {
        _animator.SetFloat(_movementMultilierHash, multiplier);
    }

    public void SetLayerWeight(int handLayerHash, float value)
    {
        throw new System.NotImplementedException();
    }
}