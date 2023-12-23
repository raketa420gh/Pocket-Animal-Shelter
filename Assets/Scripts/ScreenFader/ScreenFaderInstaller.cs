using UnityEngine;
using Zenject;

public class ScreenFaderInstaller : MonoInstaller
{
    [SerializeField] private ScreenFader _screenFaderPrefab;
    
    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<IScreenFader>().FromComponentInNewPrefab(_screenFaderPrefab).AsSingle().NonLazy();
    }
}