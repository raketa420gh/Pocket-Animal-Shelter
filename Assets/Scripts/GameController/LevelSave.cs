using System;

[Serializable]
public class LevelSave : ISaveObject
{
    public int CurrentLevelID;
    public LevelZoneSave[] ZoneSaves;

    [NonSerialized] LevelZone[] _activeZones;

    public LevelSave()
    {
        CurrentLevelID = 0;
        ZoneSaves = null;
    }

    public void LinkZones(LevelZone[] zones)
    {
        _activeZones = zones;
    }

    public void Flush()
    {
        ZoneSaves = new LevelZoneSave[_activeZones.Length];
        
        for (int i = 0; i < _activeZones.Length; i++)
            ZoneSaves[i] = _activeZones[i].GetSaveData();
    }
}