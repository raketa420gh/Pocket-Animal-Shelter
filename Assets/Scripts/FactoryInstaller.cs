using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] private Factory _factory;

    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<IFactory>().FromInstance(_factory).AsSingle().NonLazy();
    }
}