using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalData.Type _type;
    [SerializeField] private AnimalView _view;
    [SerializeField] private AnimalCarrier _animalCarrier;

    public AnimalCarrier AnimalCarrier => _animalCarrier;

    public AnimalData.Type Type => _type;

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

    public int GetCarryingHeight()
    {
        throw new System.NotImplementedException();
    }
}