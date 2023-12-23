using UnityEngine;

[System.Serializable]
public class LevelZoneSave
{
    [SerializeField] protected int _id;
    [SerializeField] protected int _placedCurrencyAmount;
    [SerializeField] protected bool _isOpened;
    
    public int ID => _id;
    public int PlacedCurrencyAmount => _placedCurrencyAmount;
    public bool IsOpened => _isOpened;

    public LevelZoneSave(int id, int placedCurrencyAmount, bool isOpened)
    {
        _id = id;
        _placedCurrencyAmount = placedCurrencyAmount;
        _isOpened = isOpened;
    }
}