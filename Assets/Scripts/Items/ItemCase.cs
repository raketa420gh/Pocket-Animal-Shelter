using UnityEngine;

[System.Serializable]
public class ItemCase
{
    [SerializeField] Item.Type _itemType;
    [SerializeField] int _amount;
    private Item _item;

    public Item.Type ItemType => _itemType;
    public int Amount { get { return _amount; } set { _amount = value; } }
    public string AmountFormatted => CurrenciesFormatter.Format(_amount);
    public Item Item => _item;

    public ItemCase(Item item, int amount)
    {
        _itemType = item.ItemType;
        _item = item;
        _amount = amount;
    }
}