using UnityEngine;
using Zenject;

public class CurrenciesControllerInstaller : MonoInstaller
{
    [SerializeField] private CurrenciesController _currenciesController;

    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<ICurrenciesController>().FromInstance(_currenciesController).AsSingle().NonLazy();
    }
}