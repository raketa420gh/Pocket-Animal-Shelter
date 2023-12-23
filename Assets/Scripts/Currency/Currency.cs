using UnityEngine;

[System.Serializable]
public class Currency
{
    [SerializeField] Type _currencyType;
    [SerializeField] Sprite _icon;
    private Save _save;
    
    public Type CurrencyType => _currencyType;
    public Sprite Icon => _icon;

    public int Amount
    {
        get => _save.Amount;
        set => _save.Amount = value;
    }

    public void SetSave(Save save)
    {
        _save = save;
    }
    
    public enum Type
    {
        Money = 0,
    }

    [System.Serializable]
    public class Save : ISaveObject
    {
        [SerializeField] int _amount;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public void Flush()
        {
        }
    }
}