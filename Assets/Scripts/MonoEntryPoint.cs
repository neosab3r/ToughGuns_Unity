using OLS_HyperCasual;
using Unity.VisualScripting;

public class MonoEntryPoint : BaseEntryPoint
{
    protected override bool IsAllInited()
    {
        return true;
    }

    protected override void InitControllers()
    {
        AddController(new SaveLevelController());
        AddController(new CoroutineController());
        AddController(new TimeDelayController());
        AddController(new ResourcesController());
        AddController(new PlayerResourcesController(true));
        AddController(new PoolController());
        AddController(new GameEffectsController());
        AddController(new PrefabsController());
        AddController(new CameraMoveController());
        AddController(new PreLevelUIController());
        AddController(new PreLevelMergeController());
        AddController(new PreLevelCellController());
        AddController(new PreLevelWeaponController());
        AddController(new BulletPoolableController());
        AddController(new StartLevelWeaponController());
        AddController(new ZoneEnemyController());
        AddController(new EnemyController());

        base.InitControllers();
    }

    protected override void InitPostControllers()
    {
        Get<PreLevelMergeController>().PostInit();
        Get<PreLevelUIController>().PostInit();
        Get<StartLevelWeaponController>().PostInit();
        Get<BulletPoolableController>().PostInit();
        Get<CameraMoveController>().PostInit();
        base.InitPostControllers();
    }
}