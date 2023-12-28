using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalData _data;
    [SerializeField] private AnimalView _view;

    public AnimalData.Type Type => _data.AnimalType;
    public float StorageHeight => _data.StorageHeight;

    public void Initialize()
    {
        
    }

    public void Destroy()
    {
        
    }

    public void Pick()
    {
        
    }

    public void Drop()
    {
        
    }

    public float GetCarryingHeight()
    {
        return StorageHeight;
    }
}