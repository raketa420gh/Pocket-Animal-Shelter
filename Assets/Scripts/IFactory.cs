public interface IFactory
{
    UIFactory UI { get; }
    PurchaseAreaFactory PurchaseArea { get; }
    ItemFactory ItemFactory { get; }
    StorageFactory StorageFactory { get; }

    void Initialize();
}