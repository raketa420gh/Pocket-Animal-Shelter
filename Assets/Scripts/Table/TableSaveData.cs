using UnityEngine;

[System.Serializable]
public class TableSaveData
{
    [SerializeField] bool _isOpened;
    [SerializeField] bool _isUnlocked;
    [SerializeField] int _placedCurrencyAmount;

    public bool IsOpened => _isOpened;
    public bool IsUnlocked => _isUnlocked;
    public int PlacedCurrencyAmount => _placedCurrencyAmount;

    public TableSaveData(int placedCurrencyAmount, bool isOpened, bool isLocked)
    {
        _placedCurrencyAmount = placedCurrencyAmount;
        _isOpened = isOpened;
        _isUnlocked = isLocked;
    }
}