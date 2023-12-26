using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalData.Type _type;
    [SerializeField] private AnimalView _view;

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
}