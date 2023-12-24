using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelZoneLobby _lobbyZone;
    [SerializeField] private LevelZoneShelter _shelterZone;
    [SerializeField] private LevelZoneWithdraw _withdrawZone;
    [SerializeField] private LevelZone _otherZone;
    private List<LevelZone> _allLevelZones = new List<LevelZone>();
    private ISaveService _saveService;
    private NavMeshController _navMeshController;

    [Inject]
    public void Construct(ISaveService saveService, NavMeshController navMeshController)
    {
        _saveService = saveService;
        _navMeshController = navMeshController;
    }
    
    public void InitializeLevel()
    {
        _allLevelZones.Add(_lobbyZone);
        _allLevelZones.Add(_shelterZone);
        _allLevelZones.Add(_withdrawZone);
        _allLevelZones.Add(_otherZone);
        
        var levelSave = _saveService.GetSaveObject<LevelSave>("level") ?? new LevelSave();
        LoadLevelDataFromSave(levelSave);

        foreach (LevelZone levelZone in _allLevelZones)
        {
            levelZone.Initialise();
            levelZone.Lock();
        }

        _shelterZone.Unlock();

        _navMeshController.UpdateNavMesh();
    }

    private void LoadLevelDataFromSave(LevelSave levelSave)
    {
        if (levelSave == null)
        {
            Debug.Log($"LEVEL SAVE NULL");
            return;
        }

        levelSave?.LinkZoneShelter(_shelterZone);
        
        if (levelSave.ZoneShelterSave != null)
        {
            _shelterZone.LoadDataFromSave(levelSave.ZoneShelterSave);
            
            Debug.Log($"Level shelter zone save load  info. {levelSave}, " +
                      $"Zone shelter save = {levelSave.ZoneShelterSave}");
        }
    }
}