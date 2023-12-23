public interface IItemCarrier
{
    public void Initialize(IItemController itemController);
    public ItemStorageCase AddItem(Item.Type itemType);
    public void RemoveItem(Item.Type itemType);
    public bool HasFreeSpace();
    public bool HasItem(Item.Type itemType);
}