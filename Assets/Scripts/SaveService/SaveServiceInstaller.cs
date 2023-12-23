using Zenject;

public class SaveServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<ISaveService>().To<SaveService>().AsSingle().NonLazy();
    }
}