using UnityEngine;
using Zenject;

public class LevelControllerInstaller : MonoInstaller
{
    [SerializeField] private LevelController _levelController;

    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<LevelController>().FromInstance(_levelController).AsSingle().NonLazy();
    }
}