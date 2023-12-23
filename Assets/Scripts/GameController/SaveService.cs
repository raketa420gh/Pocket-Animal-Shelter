using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class SaveService : ISaveService
{
    private const Serializer.SerializeType SAVE_SERIALIZE_TYPE = Serializer.SerializeType.Binary;
    private const string SAVE_FILE_NAME = "save";
    private const int SAVE_DELAY = 30;
    private GlobalSave _globalSave;
    private bool _isSaveLoaded;
    private bool _isSaveRequired;
    private static string _tempSaveFileName;

    public int LevelId
    {
        get => _globalSave.LevelId;
        set => _globalSave.LevelId = value;
    }

    public bool IsSaveLoaded => _isSaveLoaded;
    public float GameTime => _globalSave.GameTime;
    public DateTime LastExitTime => _globalSave.LastExitTime;

    public event SimpleCallback OnSaveLoaded;

    public void Initialise(float time, bool useAutoSave, bool clearSave = false)
    {
        if (clearSave)
            InitClear(time);
        else
            Load(time);

        /*if (useAutoSave)
        {
            // Enable auto-save coroutine
            Tween.InvokeCoroutine(AutoSaveCoroutine());
        }*/
    }

    public void UpdateTime(float time)
    {
        _globalSave.Time = time;
    }

    public T GetSaveObject<T>(int hash) where T : ISaveObject, new()
    {
        if (!_isSaveLoaded)
        {
            Debug.LogError("Save controller has not been initialized");
            return default;
        }

        return _globalSave.GetSaveObject<T>(hash);
    }

    public T GetSaveObject<T>(string uniqueName) where T : ISaveObject, new()
    {
        return GetSaveObject<T>(uniqueName.GetHashCode());
    }

    public void Save()
    {
        if (!_isSaveRequired)
            return;

        _globalSave.Flush();

        var saveThread = new Thread(SaveThreadFunction);
        saveThread.Start();

        Debug.Log("[Save Controller]: Game is saved!");

        _isSaveRequired = false;
    }

    public void ForceSave()
    {
        _globalSave.Flush();

        var saveThread = new Thread(SaveThreadFunction);
        saveThread.Start();

        Debug.Log("[Save Controller]: Game is saved!");

        _isSaveRequired = false;
    }

    public void MarkAsSaveIsRequired()
    {
        _isSaveRequired = true;
    }

    public void PresetsSave(string fullFileName)
    {
        _globalSave.Flush();

        _tempSaveFileName = fullFileName;

        var saveThread = new Thread(PresetsSaveThreadFunction);
        saveThread.Start();
    }

    public void GetInfo()
    {
        _globalSave.Info();
    }

    private void InitClear(float time)
    {
        _globalSave = new GlobalSave();
        _globalSave.Init(time);

        Debug.Log("[Save Controller]: Created clear save!");

        _isSaveLoaded = true;
    }

    private void Load(float time)
    {
        if (_isSaveLoaded)
            return;

        // Try to read and deserialize file or create new one
        _globalSave =
            Serializer.DeserializeFromPDP<GlobalSave>(SAVE_FILE_NAME, SAVE_SERIALIZE_TYPE, logIfFileNotExists: false);

        _globalSave.Init(time);

        Debug.Log("[Save Controller]: Save is loaded!");

        _isSaveLoaded = true;

        OnSaveLoaded?.Invoke();
    }

    private void SaveThreadFunction()
    {
        Serializer.SerializeToPDP(_globalSave, SAVE_FILE_NAME, SAVE_SERIALIZE_TYPE);
    }

    private IEnumerator AutoSaveCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(SAVE_DELAY);

        while (true)
        {
            yield return waitForSeconds;

            Save();
        }
    }

    private void PresetsSaveThreadFunction()
    {
        Serializer.SerializeToPDP(_globalSave, _tempSaveFileName, SAVE_SERIALIZE_TYPE);
    }
}