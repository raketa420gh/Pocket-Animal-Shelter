using System;

[Serializable]
public class LevelSave : ISaveObject
{
    public int CurrentLevelID;
    public LevelZoneShelterSave ZoneShelterSave;
    [NonSerialized] private LevelZoneShelter _activeShelterZone;

    public LevelSave()
    {
        CurrentLevelID = 0;
        ZoneShelterSave = null;
    }

    public void LinkZoneLobby(LevelZoneLobby zoneLobby)
    {
        
    }

    public void LinkZoneShelter(LevelZoneShelter zoneShelter)
    {
        _activeShelterZone = zoneShelter;
    }

    public void LinkZoneWithdraw()
    {
        
    }

    public void LinkZoneOther()
    {
        
    }

    public void Flush()
    {
        ZoneShelterSave = _activeShelterZone.GetSaveData();
    }
}