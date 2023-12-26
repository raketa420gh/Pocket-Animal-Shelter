using UnityEngine;

public class Factory : MonoBehaviour, IFactory
{
    [SerializeField] private UIFactory _uiFactory;
    [SerializeField] private PurchaseAreaFactory _purchaseAreaFactory;
    [SerializeField] private ItemFactory _itemFactory;
    [SerializeField] private StorageFactory _storageFactory;
    [SerializeField] private CharactersFactory _charactersFactory;

    public UIFactory UI => _uiFactory;
    public PurchaseAreaFactory PurchaseArea => _purchaseAreaFactory;
    public ItemFactory ItemFactory => _itemFactory;
    public StorageFactory StorageFactory => _storageFactory;
    public CharactersFactory CharactersFactory => _charactersFactory;

    public void Initialize()
    {
        _uiFactory.Initialize();
        _purchaseAreaFactory.Initialize();
        _storageFactory.Initialize();
    }
}