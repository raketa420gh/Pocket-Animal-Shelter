using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    private ItemObject _prefab;
    private Pool<ItemObject> _pool;
    private Item.Type _itemType;

    public Item.Type ItemType => _itemType;

    public void Initialize(ItemObject prefab)
    {
        _pool = new Pool<ItemObject>(prefab, _poolCount, transform)
        {
            AutoExpand = _isAutoExpand
        };
        
        _itemType = prefab.Type;
    }

    public ItemObject GetElement()
    {
        return _pool.GetFreeElement();
    }
}