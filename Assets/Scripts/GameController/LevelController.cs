using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelZone[] _levelZones;
    [SerializeField] private LevelZoneShelter _shelterZone;
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
        var levelSave = _saveService.GetSaveObject<LevelSave>("level") ?? new LevelSave();
        LoadLevelSave(levelSave);

        for (int i = 0; i < _levelZones.Length; i++)
        {
            LevelZone levelZone = _levelZones[i];
            //levelZone.Load(levelSave.ZoneSaves[i]);
            levelZone.Initialise();
            levelZone.Lock();
        }

        _shelterZone.Unlock();

        _navMeshController.UpdateNavMesh();
    }

    private void LoadLevelSave(LevelSave levelSave)
    {
        if (levelSave == null)
        {
            Debug.Log($"LEVEL SAVE NULL");
            return;
        }
        
        if (!levelSave.ZoneSaves.IsNullOrEmpty())
        {
            foreach (LevelZoneSave levelZoneSave in levelSave.ZoneSaves)
            {
                foreach (LevelZone levelZone in _levelZones)
                {
                    if (levelZone.ID == levelZoneSave.ID)
                    {
                        levelZone.LoadDataFromSave(levelZoneSave);
                        break;
                    }
                }
            }
        }

        levelSave?.LinkZones(_levelZones);

        Debug.Log($"Level save load  info. {levelSave}, ID = {levelSave.CurrentLevelID}, " +
                  $"Zone saves = {levelSave.ZoneSaves}");
    }
}