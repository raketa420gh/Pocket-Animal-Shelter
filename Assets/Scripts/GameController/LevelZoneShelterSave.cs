using UnityEngine;

[System.Serializable]
public class LevelZoneShelterSave : LevelZoneSave
{
    [SerializeField] TableZone.SaveData[] _tableZones;
    
    public TableZone.SaveData[] TableZones => _tableZones;

    public LevelZoneShelterSave(int id, int placedCurrencyAmount, bool isOpened, TableZone.SaveData[] tableZones) : base(id, placedCurrencyAmount, isOpened)
    {
        _tableZones = tableZones;
    }
}