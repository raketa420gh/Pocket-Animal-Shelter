using UnityEngine;

public class AnimalStorageCase
{
    private int _index;
    private Animal _animal;
    private bool _isPicked;
    private GameObject _storageObject;

    public int Index => _index;
    public Animal Animal => _animal;
    public bool IsPicked => _isPicked;
    public GameObject StorageObject => _storageObject;
    public Transform Transform => _animal.transform;

    public AnimalStorageCase(Animal animal, GameObject storageObject)
    {
        _animal = animal;
        _storageObject = storageObject;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void MarkAsPicked()
    {
        _isPicked = true;
    }

    public void Reset()
    {
        _storageObject.transform.SetParent(null);
        _storageObject.SetActive(false);
    }
}