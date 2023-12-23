using UnityEngine;

public class ItemStorageCase
{
    private int _index;
    private GameObject _itemObject;
    private Item.Type _itemType;
    private Item _item;
    private bool _isPicked;
    private GameObject _storageObject;
    
    public int Index => _index;
    public GameObject ItemObject => _itemObject;
    public Item.Type ItemType => _itemType;
    public Item Item => _item;
    public bool IsPicked => _isPicked;
    public GameObject StorageObject => _storageObject;

    public ItemStorageCase(GameObject itemObject, Item.Type itemType, Item item, GameObject storageObject)
    {
        _itemObject = itemObject;
        _itemType = itemType;
        _item = item;
        _storageObject = storageObject;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void MarkAsPicked()
    {
        _isPicked = true;
    }

    public void Reset()
    {
        _storageObject.transform.SetParent(null);
        _storageObject.SetActive(false);

        _itemObject.transform.SetParent(null);
        _itemObject.SetActive(false);
    }
}