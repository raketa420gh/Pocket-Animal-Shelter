using System.Linq;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPoolPrefab;
    private ItemPool[] _itemPools;

    public void Initialize(ItemObject[] prefabs)
    {
        _itemPools = new ItemPool[prefabs.Length];

        for (int i = 0; i < _itemPools.Length; i++)
        {
            _itemPools[i] = Instantiate(_itemPoolPrefab, transform);
            _itemPools[i].Initialize(prefabs[i]);
        }
    }
    
    public ItemObject CreateItem(Item.Type type, Vector3 position)
    {
        ItemPool itemPool = _itemPools.Where(itemPool => itemPool.ItemType == type).ToList().FirstOrDefault();

        ItemObject itemObject = itemPool.GetElement();
        itemObject.transform.position = position;

        return itemObject;
    }
}