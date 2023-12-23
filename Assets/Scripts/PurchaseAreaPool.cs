using UnityEngine;

public class PurchaseAreaPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    [SerializeField] private PurchaseArea _prefab;

    private Pool<PurchaseArea> _pool;

    public void Initialize()
    {
        _pool = new Pool<PurchaseArea>(_prefab, _poolCount, transform)
        {
            AutoExpand = _isAutoExpand
        };
    }

    public PurchaseArea GetElement()
    {
        return _pool.GetFreeElement();
    }
}