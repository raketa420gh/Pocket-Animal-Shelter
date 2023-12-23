using UnityEngine;
using Zenject;

public class ItemControllerInstaller : MonoInstaller
{
    [SerializeField] private ItemController _itemController;

    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<IItemController>().FromInstance(_itemController).AsSingle().NonLazy();
    }
}