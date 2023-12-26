using UnityEngine;

public class StorageSlotPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    [SerializeField] private StorageSlot _prefab;
    private Pool<StorageSlot> _pool;

    public void Initialize()
    {
        _pool = new Pool<StorageSlot>(_prefab, _poolCount, transform)
        {
            AutoExpand = _isAutoExpand
        };
    }

    public StorageSlot GetElement()
    {
        return _pool.GetFreeElement();
    }
}