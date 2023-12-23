using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private UnitAnimator _animator;
    
    private bool _isHandEnabled;
    private int _handLayerHash;

    public UnitAnimator Animator => _animator;
    
    public void EnableHands()
    {
        if (_isHandEnabled)
            return;

        _animator.SetLayerWeight(_handLayerHash, 1.0f);
        _isHandEnabled = true;
    }

    public void DisableHands()
    {
        if (!_isHandEnabled)
            return;

        _animator.SetLayerWeight(_handLayerHash, 0.0f);
        _isHandEnabled = false;
    }
}