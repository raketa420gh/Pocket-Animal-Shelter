public interface ISaveService
{
    void Initialise(float time, bool useAutoSave, bool clearSave = false);
    void UpdateTime(float time);
    T GetSaveObject<T>(int hash) where T : ISaveObject, new();
    T GetSaveObject<T>(string uniqueName) where T : ISaveObject, new();
    void Save();
    void ForceSave();
    void MarkAsSaveIsRequired();
    void PresetsSave(string fullFileName);
    void GetInfo();
}