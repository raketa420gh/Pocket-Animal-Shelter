using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalsPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _isAutoExpand = true;
    [SerializeField] private Animal[] _animalPrefabs;
    private List<Pool<Animal>> _animalPools;

    public void Initialize()
    {
        _animalPools = new List<Pool<Animal>>();

        foreach (Animal animal in _animalPrefabs)
        {
            Pool<Animal> animalPool = new Pool<Animal>(animal, _poolCount, transform) 
                { AutoExpand = _isAutoExpand };
            
            _animalPools.Add(animalPool);
        }
    }

    public Animal GetElement(AnimalData.Type type)
    {
        Animal animal = (from animalPool in _animalPools where animalPool.Prefab.Type == type select animalPool
            .GetFreeElement())
            .FirstOrDefault();

        return animal;
    }
}