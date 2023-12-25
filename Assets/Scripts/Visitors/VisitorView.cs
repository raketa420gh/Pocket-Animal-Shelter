using UnityEngine;

public class VisitorView : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Mesh[] _meshes;
    [Header("Animation")]
    [SerializeField] private UnitAnimator _animator;
    
    private bool _isHandEnabled;
    private int _handLayerHash = 1;

    public UnitAnimator Animator => _animator;

    public void SetRandomMesh()
    {
        int rIndex = Random.Range(0, _meshes.Length);
        Mesh rMesh = _meshes[rIndex];

        _skinnedMeshRenderer.sharedMesh = rMesh;
    }
    
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