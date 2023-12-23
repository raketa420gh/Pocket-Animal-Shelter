using UnityEngine;
using Zenject;

public class NavMeshControllerInstaller : MonoInstaller
{
    [SerializeField] private NavMeshController _navMeshController;
    
    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<NavMeshController>().FromInstance(_navMeshController).AsSingle().NonLazy();
    }
}