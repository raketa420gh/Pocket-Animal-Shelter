using UnityEngine;

public class VisitorsPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    [SerializeField] private Visitor _prefab;
    private Pool<Visitor> _pool;
    
    public void Initialize()
    {
        _pool = new Pool<Visitor>(_prefab, _poolCount, transform)
        {
            AutoExpand = _isAutoExpand
        };
    }
    
    public Visitor GetElement()
    {
        return _pool.GetFreeElement();
    }
}