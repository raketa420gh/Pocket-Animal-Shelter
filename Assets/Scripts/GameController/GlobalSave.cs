using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalSave
{
    [SerializeField] public string _buildId;
    [SerializeField] int _levelId;
    [SerializeField] SavedDataContainer[] _saveObjects;
    [SerializeField] float _gameTime;
    [SerializeField] DateTime _lastExitTime;
    private List<SavedDataContainer> _saveObjectsList;
    
    public int LevelId
    {
        get => _levelId;
        set => _levelId = value;
    }

    public float GameTime => _gameTime + (Time - lastFlushTime);

    public DateTime LastExitTime => _lastExitTime;

    private float lastFlushTime = 0;

    public float Time { get; set; }

    public void Init(float time)
    {
        if (_saveObjects == null)
        {
            _saveObjectsList = new List<SavedDataContainer>();
        }
        else
        {
            _saveObjectsList = new List<SavedDataContainer>(_saveObjects);
        }

        for (int i = 0; i < _saveObjectsList.Count; i++)
        {
            _saveObjectsList[i].Restored = false;
        }

        Time = time;
        lastFlushTime = Time;
    }

    public void Flush()
    {
        _saveObjects = _saveObjectsList.ToArray();

        for (int i = 0; i < _saveObjectsList.Count; i++)
        {
            var saveObject = _saveObjectsList[i];

            saveObject.Flush();
        }

        _gameTime += Time - lastFlushTime;

        lastFlushTime = Time;

        _lastExitTime = DateTime.Now;
    }

    public T GetSaveObject<T>(int hash) where T : ISaveObject, new()
    {
        var container = _saveObjectsList.Find((container) => container.Hash == hash);

        if (container == null)
        {
            var saveObject = new T();
            container = new SavedDataContainer(hash, new T());

            _saveObjectsList.Add(container);
        }
        else
        {
            if (!container.Restored) container.Restore<T>();
        }

        return (T)container.SaveObject;
    }

    public T GetSaveObject<T>(string uniqueName) where T : ISaveObject, new()
    {
        return GetSaveObject<T>(uniqueName.GetHashCode());
    }

    public void Info()
    {
        foreach (var container in _saveObjectsList)
        {
            Debug.Log("Hash: " + container.Hash);
            Debug.Log("Save Object: " + container.SaveObject);
        }
    }

    public void DevSetMaxGameTime()
    {
        _gameTime = 10000;
    }

    public void DevSetMinTime()
    {
        _gameTime = -10000;
    }
}