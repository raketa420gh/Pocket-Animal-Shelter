using UnityEngine;
using Zenject;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private IFactory _factory;

    [Inject]
    public void Construct(IFactory factory)
    {
        _factory = factory;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnVisitor();
        }
    }

    private Visitor SpawnVisitor()
    {
        Visitor visitor = _factory.CharactersFactory.CreateVisitor(_spawnPoint.position);

        return visitor;
    }
}
