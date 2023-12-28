public interface IAnimalCarrier
{
    void Initialize(IFactory factory);
    Animal GetAnimal(AnimalData.Type[] allowedAnimalTypes);
    AnimalStorageCase AddAnimal(Animal animal);
    void RemoveAnimal(Animal animal);
    void CarryAnimal(Animal animal);
    void DropAnimals();
    void RegroupAnimals();
    bool IsAnimalCarrying();
    bool IsAnimalPickupAllowed();
}