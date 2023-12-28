using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalCarrier : MonoBehaviour, IAnimalCarrier
{
    [SerializeField] private Transform _storagePoint;
    private List<AnimalStorageCase> _carryingAnimalsList;
    private Animal _carryingAnimal;
    private int _maxAnimalsAmount = 1;
    private bool _isAnimalCarrying;
    private int _carryingAnimalsAmount;
    private float _carryingAnimalsHeight;
    private IFactory _factory;

    public event Action<bool> OnChangeCarryingState;

    public Animal Animal => _carryingAnimal;

    public void Initialize(IFactory factory)
    {
        _factory = factory;
        _carryingAnimalsList = new List<AnimalStorageCase>();
        _carryingAnimalsAmount = _carryingAnimalsList.Count;
    }

    public Animal GetAnimal(AnimalData.Type[] allowedAnimalTypes)
    {
        if (_isAnimalCarrying)
        {
            for (int i = 0; i < allowedAnimalTypes.Length; i++)
            {
                if (allowedAnimalTypes[i] == _carryingAnimal.Type)
                {
                    return _carryingAnimal;
                }
            }
        }

        return null;
    }

    public AnimalStorageCase AddAnimal(Animal animal)
    {
        StorageSlot storageSlot = _factory.StorageFactory.CreateStorageSlot(_storagePoint);
        storageSlot.transform.localPosition = new Vector3(0, _carryingAnimalsHeight, 0);
        storageSlot.transform.localRotation = Quaternion.identity;
        storageSlot.transform.localScale = Vector3.one;
        storageSlot.gameObject.SetActive(true);

        AnimalStorageCase storageCase = new AnimalStorageCase(animal, storageSlot.gameObject);
        animal.transform.localPosition = storageSlot.transform.position;
        animal.transform.localRotation = storageSlot.transform.rotation;
        storageCase.SetIndex(_carryingAnimalsAmount);
        storageCase.MarkAsPicked();

        _carryingAnimal = animal;
        _carryingAnimalsAmount++;
        _carryingAnimalsList.Add(storageCase);
        _carryingAnimalsHeight += animal.GetCarryingHeight();
        _isAnimalCarrying = true;
        
        OnChangeCarryingState?.Invoke(true);

        return storageCase;
    }

    public void RemoveAnimal(Animal animal)
    {
        if (_carryingAnimalsAmount > 0)
        {
            for (int i = _carryingAnimalsAmount - 1; i >= 0; i--)
            {
                if (_carryingAnimalsList[i].IsPicked && _carryingAnimalsList[i].Animal == animal)
                {
                    _carryingAnimalsList[i].Animal.transform.SetParent(null);
                    _carryingAnimalsList[i].Reset();

                    _carryingAnimalsAmount--;
                    _carryingAnimalsList.RemoveAt(i);

                    if (_carryingAnimalsAmount == 0)
                    {
                        _carryingAnimalsHeight = 0;
                        _isAnimalCarrying = false;
                        
                        OnChangeCarryingState?.Invoke(false);
                    }

                    break;
                }
            }

            RegroupAnimals();
        }
    }

    public void CarryAnimal(Animal animal)
    {
        AddAnimal(animal);
    }

    public void DropAnimals()
    {
        if (_carryingAnimalsAmount > 0)
        {
            for (int i = _carryingAnimalsAmount - 1; i >= 0; i--)
            {
                Vector3 dropPosition = transform.position + (Random.insideUnitSphere.SetY(0) * 5);

                Animal storedAnimal = _carryingAnimalsList[i].Animal;
                storedAnimal.transform.SetParent(null);
                storedAnimal.Drop();
                _carryingAnimalsList[i].Reset();
                _carryingAnimalsAmount--;
                _carryingAnimalsList.RemoveAt(i);
            }
        }

        _carryingAnimalsHeight = 0;
        _isAnimalCarrying = false;
    }

    public void RegroupAnimals()
    {
        _carryingAnimalsHeight = 0; 

        if (_carryingAnimalsAmount > 0)
        {
            for (int i = 0; i < _carryingAnimalsAmount; i++)
            {
                _carryingAnimalsList[i].SetIndex(i);
                _carryingAnimalsList[i].StorageObject.transform.localPosition = new Vector3(0, _carryingAnimalsHeight, 0);

                _carryingAnimalsHeight += _carryingAnimalsList[i].Animal.GetCarryingHeight();
            }

            _carryingAnimal = _carryingAnimalsList[0].Animal;
        }
    }

    public bool IsAnimalCarrying()
    {
        return _isAnimalCarrying;
    }

    public bool IsAnimalPickupAllowed()
    {
        return _carryingAnimalsAmount < _maxAnimalsAmount;
    }
}