using UnityEngine;

public class UICurrencyPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    [SerializeField] private UIPanelCurrency _prefab;

    private Pool<UIPanelCurrency> _pool;

    public void Initialize()
    {
        _pool = new Pool<UIPanelCurrency>(_prefab, _poolCount, transform)
        {
            AutoExpand = _isAutoExpand
        };
    }

    public UIPanelCurrency GetElement()
    {
        return _pool.GetFreeElement();
    }
}