using SSLAB;

public class AppBootstrapper : Bootstrapper
{
    protected override void Awake()
    {
        base.Awake();

        var sl = ServiceLocator.Instance;
        sl.RegisterService(new InputService());
        sl.RegisterService(new ProjectileService());
        sl.RegisterService(new ObjSpawnerService());
        sl.RegisterService(new LevelWrapService());
        sl.RegisterService(new AsteroidService());

        //TODO: StateManager to take care of this
        sl.GetService<IAsteroidService>().IsEnabled = true;
    }
}
