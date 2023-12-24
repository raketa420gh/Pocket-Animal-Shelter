using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelZoneShelter : LevelZone
{
    [SerializeField] private TableZone[] _tableZones;
    [SerializeField] private ItemDispenser[] _itemDispensers;

    public override void Initialise()
    {
        base.Initialise();
        
        if (_isOpened || _isPermanentOpened)
        {
            foreach (var tableZone in _tableZones)
                tableZone.Initialize(this);
            
            foreach (var itemDispenser in _itemDispensers)
                itemDispenser.Initialise();
        }
    }

    protected override void ActivateZone()
    {
        base.ActivateZone();

        if (_tableZones != null)
        {
            for (int i = 0; i < _tableZones.Length; i++)
                _tableZones[i].Initialize(this);
        }

        if (_itemDispensers != null)
        {
            foreach (ItemDispenser itemDispenser in _itemDispensers)
            {
                itemDispenser.Initialise();
                itemDispenser.SetUnlockState(true);
            }
        }
    }

    public bool HasFreeTable()
    {
        for (int i = 0; i < _tableZones.Length; i++)
        {
            for (int t = 0; t < _tableZones[i].Tables.Length; t++)
            {
                if (_tableZones[i].Tables[t].IsOpened && !_tableZones[i].Tables[t].IsAnimalPlaced &&
                    !_tableZones[i].Tables[t].IsBusy)
                    return true;
            }
        }

        return false;
    }

    public Table GetFreeTable()
    {
        for (int i = 0; i < _tableZones.Length; i++)
        {
            for (int t = 0; t < _tableZones[i].Tables.Length; t++)
            {
                if (_tableZones[i].Tables[t].IsOpened && !_tableZones[i].Tables[t].IsAnimalPlaced &&
                    !_tableZones[i].Tables[t].IsBusy)
                    return _tableZones[i].Tables[t];
            }
        }

        return null;
    }

    public Table GetRandomFreeTable()
    {
        List<Table> availableTableBehaviours = new List<Table>();

        for (int i = 0; i < _tableZones.Length; i++)
        {
            availableTableBehaviours.AddRange(_tableZones[i].Tables
                .Where(t1 => t1.IsOpened && !t1.IsAnimalPlaced && !t1.IsBusy));
        }

        if (availableTableBehaviours.Count > 0)
            return availableTableBehaviours.GetRandomItem();

        return null;
    }

    public int GetFreeTablesAmount()
    {
        int freeTablesAmount = 0;

        for (int i = 0; i < _tableZones.Length; i++)
        {
            for (int t = 0; t < _tableZones[i].Tables.Length; t++)
            {
                if (_tableZones[i].Tables[t].IsOpened && !_tableZones[i].Tables[t].IsAnimalPlaced &&
                    !_tableZones[i].Tables[t].IsBusy)
                {
                    freeTablesAmount++;
                }
            }
        }

        return freeTablesAmount;
    }

    #region Load/Save

    public void LoadDataFromSave(LevelZoneShelterSave levelZoneShelterSave)
    {
        _placedCurrencyAmount = levelZoneShelterSave.PlacedCurrencyAmount;
        _isOpened = levelZoneShelterSave.IsOpened;
        
        if (levelZoneShelterSave.TableZones != null)
        {
            foreach (TableZone.SaveData saveData in levelZoneShelterSave.TableZones)
            {
                foreach (TableZone tableZone in _tableZones)
                    if (saveData.ID == tableZone.TableZoneID)
                        tableZone.Load(saveData);
            }
        }
    }

    public LevelZoneShelterSave GetSaveData()
    {
        TableZone.SaveData[] tableZonesSave = new TableZone.SaveData[_tableZones.Length];
        for (int i = 0; i < tableZonesSave.Length; i++)
            tableZonesSave[i] = _tableZones[i].Save();

        return new LevelZoneShelterSave(_id, _placedCurrencyAmount, _isOpened, tableZonesSave);
    }

    #endregion
}