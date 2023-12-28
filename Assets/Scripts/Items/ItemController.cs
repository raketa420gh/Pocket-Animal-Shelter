using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ItemController : MonoBehaviour, IItemController
{
    [SerializeField] ItemsDatabase _itemsDatabase;

    private IFactory _factory;
    private Item[] _registeredItems;
    private Dictionary<Item.Type, Item> _itemsTypeLink;

    [Inject]
    public void Construct(IFactory factory)
    {
        _factory = factory;
    }

    public void Initialise()
    {
        List<ItemObject> itemObjects = _itemsDatabase.Items
            .Select(item => item.Model.GetComponent<ItemObject>()).ToList();

        _factory.ItemFactory.Initialize(itemObjects.ToArray());
        _registeredItems = _itemsDatabase.Items;
        _itemsTypeLink = new Dictionary<Item.Type, Item>();
        
        for (int i = 0; i < _registeredItems.Length; i++)
        {
            if (!_itemsTypeLink.ContainsKey(_registeredItems[i].ItemType))
            {
                _itemsTypeLink.Add(_registeredItems[i].ItemType, _registeredItems[i]);
            }
            else
            {
                Debug.LogError(string.Format("[Item System]: Item {0} already exists in database!", _registeredItems[i].ItemType));
            }
        }
    }

    public Item GetItem(Item.Type itemType)
    {
        if (_itemsTypeLink.ContainsKey(itemType))
            return _itemsTypeLink[itemType];

        Debug.LogError(string.Format("[Item System]: Item {0} can't be found in the database!", itemType));

        return null;
    }

    public Item[] GetItems()
    {
        return _registeredItems;
    }

    public GameObject GetItemObject(Item.Type itemType)
    {
        var itemObject = _factory.ItemFactory.CreateItem(itemType, Vector3.zero);

        return itemObject.gameObject;
    }
}