using UnityEngine;

public class CharactersFactory : MonoBehaviour
{
    [SerializeField] private AnimalsPool _animalsPool;
    [SerializeField] private VisitorsPool _visitorsPool;
    private IFactory _factory;
    
    public void Initialize(IFactory factory)
    {
        _factory = factory;
        _animalsPool.Initialize();
        _visitorsPool.Initialize();
    }

    public Animal CreateAnimal(Vector3 position, AnimalData.Type type, int sicknessType)
    {
        Animal animal = _animalsPool.GetElement(type);
        animal.Initialize();
        animal.transform.position = position;

        return animal;
    }

    public Visitor CreateVisitor(Vector3 position)
    {
        Visitor visitor = _visitorsPool.GetElement();
        visitor.Initialize(_factory);
        visitor.transform.position = position;

        return visitor;
    }
}