using UnityEngine;

[System.Serializable]
public class SavedDataContainer
{
    [SerializeField] int _hash;
    [SerializeField] string _json;
    [System.NonSerialized] ISaveObject _saveObject;
    
    public int Hash => _hash;
    public bool Restored { get; set; }
    public ISaveObject SaveObject => _saveObject;

    public SavedDataContainer(int hash, ISaveObject saveObject)
    {
        _hash = hash;
        _saveObject = saveObject;
        Restored = true;
    }

    public void Flush()
    {
        if (_saveObject != null) 
            _saveObject.Flush();
        if (Restored) 
            _json = JsonUtility.ToJson(_saveObject);
    }

    public void Restore<T>() where T : ISaveObject
    {
        _saveObject = JsonUtility.FromJson<T>(_json);
        Restored = true;
    }
}