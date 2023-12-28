using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _minSpawnDelay = 1f;
    [SerializeField] private float _maxSpawnDelay = 3f;
    [SerializeField] private int _maxWaitingPatinetsCount = 5;
    [SerializeField] private bool _autoSpawn = false;
    private float _rSpawnDelay;
    private IFactory _factory;
    private List<Animal> _activeWaitingAnimals;

    [Inject]
    public void Construct(IFactory factory)
    {
        _factory = factory;
    }

    private void Awake()
    {
        _activeWaitingAnimals = new List<Animal>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnVisitor();
        }
    }

    public void Destroy()
    {
    }

    private Visitor SpawnVisitor()
    {
        if (_activeWaitingAnimals.Count < _maxWaitingPatinetsCount)
        {
            Visitor visitor = _factory.CharactersFactory.CreateVisitor(_spawnPoint.position);
            SpawnAnimal(visitor);

            return visitor;
        }

        return null;
    }

    private Animal SpawnAnimal(Visitor animalOwner)
    {
        Animal animal = _factory.CharactersFactory.CreateAnimal(_spawnPoint.position, AnimalData.Type.Cat_1, 0);
        animalOwner.AnimalCarrier.CarryAnimal(animal);
        _activeWaitingAnimals.Add(animal);

        return animal;
    }
}