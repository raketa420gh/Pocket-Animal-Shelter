using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemCarrier : MonoBehaviour, IItemCarrier
{
    [SerializeField] private Transform _storageTransform;
    private IItemController _itemController;
    private List<ItemStorageCase> _carryingItemsList;
    private int _maxItemsAmount = 3;
    private int _carryingItemsAmount;
    private float _carryingItemsHeight;
    private bool _isCarrying;
    
    public Transform StorageTransform => _storageTransform;

    public void Initialize(IItemController itemController)
    {
        _itemController = itemController;
        _carryingItemsList = new List<ItemStorageCase>();
        _carryingItemsAmount =_carryingItemsList.Count;
    }

    public ItemStorageCase AddItem(Item.Type itemType)
    {
        Item item = _itemController.GetItem(itemType);

        if (item == null)
            return null;
        
        StorageSlot storageSlot = _itemController.CreateStorageSlot(_storageTransform);
        storageSlot.transform.localPosition = new Vector3(0, _carryingItemsHeight, 0);
        storageSlot.transform.localRotation = Quaternion.identity;
        storageSlot.gameObject.SetActive(true);

        GameObject itemGameObject = _itemController.GetItemObject(itemType);
        itemGameObject.transform.position = storageSlot.transform.position;
        itemGameObject.transform.rotation = storageSlot.transform.rotation;
        itemGameObject.transform.localScale = Vector3.zero;
        itemGameObject.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutBack);
        itemGameObject.SetActive(true);

        ItemStorageCase storageCase = new ItemStorageCase(itemGameObject, itemType, item, storageSlot.gameObject);
        storageCase.SetIndex(_carryingItemsAmount);
        storageCase.MarkAsPicked();

        _carryingItemsAmount++;
        _carryingItemsList.Add(storageCase);

        _isCarrying = true;

        _carryingItemsHeight += item.ModelHeight;

        return storageCase;
    }

    public void RemoveItem(Item.Type itemType)
    {
        if (_carryingItemsAmount > 0)
        {
            for (int i = _carryingItemsAmount - 1; i >= 0; i--)
            {
                if (_carryingItemsList[i].ItemType == itemType)
                {
                    _carryingItemsList[i].Reset();

                    _carryingItemsAmount--;
                    _carryingItemsList.RemoveAt(i);

                    if (_carryingItemsAmount == 0)
                    {
                        _carryingItemsHeight = 0;
                        _isCarrying = false;
                    }
                    else
                    {
                        RegroupItems();
                    }

                    break;
                }
            }
        }
    }

    public bool HasItem(Item.Type itemType)
    {
        if (_carryingItemsAmount > 0)
        {
            for (int i = _carryingItemsAmount - 1; i >= 0; i--)
            {
                if (_carryingItemsList[i].ItemType == itemType)
                    return true;
            }
        }

        return false;
    }

    public bool HasFreeSpace()
    {
        return _carryingItemsAmount < _maxItemsAmount;
    }

    public void RecalculateItemsPositions()
    {
        if (_isCarrying)
        {
            for (int i = 0; i < _carryingItemsAmount; i++)
            {
                if (_carryingItemsList[i].IsPicked)
                {
                    _carryingItemsList[i].ItemObject.transform.position =
                        _carryingItemsList[i].StorageObject.transform.position;
                    _carryingItemsList[i].ItemObject.transform.rotation =
                        _carryingItemsList[i].StorageObject.transform.rotation;
                }
            }
        }
    }

    private void RegroupItems()
    {
        _carryingItemsHeight = 0;

        if (_carryingItemsAmount > 0)
        {
            for (int i = 0; i < _carryingItemsAmount; i++)
            {
                _carryingItemsList[i].SetIndex(i);
                _carryingItemsList[i].StorageObject.transform.localPosition = new Vector3(0, _carryingItemsHeight, 0);

                _carryingItemsHeight += _carryingItemsList[i].Item.ModelHeight;
            }
        }
    }
}