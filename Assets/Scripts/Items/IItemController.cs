using UnityEngine;

public interface IItemController
{
    void Initialise();
    Item GetItem(Item.Type itemType);
    Item[] GetItems();
    GameObject GetItemObject(Item.Type itemType);
    StorageSlot CreateStorageSlot(Transform parent);
}