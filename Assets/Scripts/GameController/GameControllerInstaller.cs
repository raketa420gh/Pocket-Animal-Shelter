using Zenject;

public class GameControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Bind();
    }

    private void Bind()
    {
        Container.Bind<GameController>().AsSingle().NonLazy();
    }
}
