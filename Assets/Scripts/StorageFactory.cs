using UnityEngine;

public class StorageFactory : MonoBehaviour
{
    [SerializeField] private StorageSlotPool _storageSlotPool;
    
    public void Initialize()
    {
        _storageSlotPool.Initialize();
    }

    public StorageSlot CreateStorageSlot(Transform parent)
    {
        StorageSlot storageSlot = _storageSlotPool.GetElement();
        storageSlot.transform.SetParent(parent);

        return storageSlot;
    }
}